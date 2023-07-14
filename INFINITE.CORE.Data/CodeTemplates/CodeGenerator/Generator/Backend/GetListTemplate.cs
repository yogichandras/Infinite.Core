using INFINITE.CORE.Data.CodeGenerator;
using EntityFrameworkCore.Scaffolding.Handlebars;
using Microsoft.EntityFrameworkCore.Metadata;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace INFINITE.CORE.Data.CodeGenerator.Generator
{
    public partial class CodeTemplate
    {
        #region Template
        public static (bool success, string key, object val) GenerateGetList(string current_namespace, IModel model, Microsoft.Extensions.Options.IOptions<HandlebarsScaffoldingOptions> _options, List<SettingExcludeTableCodeGeneratorObject> exclude)
        {
            var sb = new Microsoft.EntityFrameworkCore.Infrastructure.IndentedStringBuilder();
            string project_name = current_namespace + "Data";
            string project_path = Directory.GetCurrentDirectory().Replace(project_name, "");
            string template_path = Path.Combine(project_path, current_namespace + @"Data\CodeTemplates\CodeGenerator\Template\Backend\GetList.hbs");
            if (!File.Exists(template_path))
            {
                return (false, null, null);
            }
            var code_template = File.ReadAllText(template_path);
            code_template = code_template.Replace("{{namespace}}", current_namespace);

            using (sb.Indent())
            using (sb.Indent())
            {
                foreach (var entityType in model.GetScaffoldEntityTypes(_options.Value))
                {

                    bool create_code = true;
                    if (exclude != null && exclude.Count() > 0)
                    {
                        var exclude_table = exclude.Where(d => d.TableName.ToLower() == entityType.Name.ToLower()).FirstOrDefault();
                        if (exclude_table != null && !exclude_table.Service)
                            create_code = false;
                    }
                    if (create_code)
                    {
                        string name = RemovePrefix(entityType.Name);
                        string model_name = entityType.Name;
                        var list_properties = entityType.GetProperties();

                        string target_path = Path.Combine(project_path, current_namespace + $@"Data\Generated\Backend\Core\{GetPrefix(entityType.Name)}\{name}\Query");

                        if (!Directory.Exists(target_path))
                            Directory.CreateDirectory(target_path);

                        var code = code_template;
                        string code_file = Path.Combine(target_path, $"Get{name}ListHandler.cs");

                        string primary_name = list_properties.Where(d => d.IsPrimaryKey()).Select(d => d.Name).FirstOrDefault()!;
                        if (string.IsNullOrWhiteSpace(primary_name))
                            primary_name = "Id";

                        code = code.Replace("{{name}}", name);
                        code = code.Replace("{{model}}", model_name);
                        code = code.Replace("{{primary_key_name}}", primary_name);
                        code = code.Replace("{{schema}}", GetPrefix(entityType.Name));

                        string list_template = "";
                        foreach (var d in list_properties)
                        {
                            string type = ParseType(d.ClrType);
                            if (type.ToLower().Contains("point") || type.ToLower().Contains("geometry"))
                                continue;
                            bool isnullable = d.IsNullable;
                            string tabular_default = "\t\t\t\t\t";
                            string case_template = "";
                            case_template += $"{tabular_default}case \"{d.Name.ToLower()}\" : " + Environment.NewLine;
                            case_template += tabular_default + "\tif(is_where){" + Environment.NewLine;
                            switch (type)
                            {
                                case "string":
                                    case_template += $"{tabular_default}\t\tresult_where = (d=>d.{d.Name}.Trim().ToLower().Contains(search));" + Environment.NewLine;
                                    break;
                                case "byte[]":
                                    case_template += $"{tabular_default}\t\tresult_where = (d=>d.{d.Name} == System.Text.Encoding.UTF8.GetBytes(search));" + Environment.NewLine;
                                    break;
                                case "Guid":
                                    case_template += $"{tabular_default}\t\tif ({type}.TryParse(search, out var _{d.Name}))" + Environment.NewLine;
                                    case_template += $"{tabular_default}\t\t\tresult_where = (d=>d.{d.Name} == _{d.Name});" + Environment.NewLine;
                                    case_template += $"{tabular_default}\t\t\telse" + Environment.NewLine;
                                    case_template += $"{tabular_default}\t\t\tresult_where = (d=>d.{d.Name} == Guid.Empty);" + Environment.NewLine;
                                    break;
                                default:
                                    case_template += $"{tabular_default}\t\tif ({type}.TryParse(search, out var _{d.Name}))" + Environment.NewLine;
                                    case_template += $"{tabular_default}\t\t\tresult_where = (d=>d.{d.Name} == _{d.Name});" + Environment.NewLine;
                                    break;
                            }
                            case_template += tabular_default + "\t}" + Environment.NewLine;
                            case_template += tabular_default + "\telse" + Environment.NewLine;
                            case_template += $"{tabular_default}\t\tresult_order = (d => d.{d.Name});" + Environment.NewLine;
                            case_template += $"{tabular_default}break;";
                            list_template += case_template + Environment.NewLine;
                        }
                        code = code.Replace("{{list_expression}}", list_template);

                        using (StreamWriter outputFile = new StreamWriter(code_file))
                        {
                            outputFile.WriteLine(code);
                        }
                    }
                }
            }
            var onDTOGenerate = sb.ToString();
            return (true, "on-Services-Generate", onDTOGenerate);
        }
        #endregion
    }
}
