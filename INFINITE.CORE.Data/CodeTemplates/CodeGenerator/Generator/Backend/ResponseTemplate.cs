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
        public static (bool success, string key, object val) GenerateResponseCQRS(string current_namespace, IModel model, Microsoft.Extensions.Options.IOptions<HandlebarsScaffoldingOptions> _options, List<SettingExcludeTableCodeGeneratorObject> exclude)
        {
            var sb = new Microsoft.EntityFrameworkCore.Infrastructure.IndentedStringBuilder();
            string project_name = current_namespace + "Data";
            string project_path = Directory.GetCurrentDirectory().Replace(project_name, "");
            string template_path = Path.Combine(project_path, current_namespace + @"Data\CodeTemplates\CodeGenerator\Template\Backend\Response.hbs");
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
                        if (exclude_table != null && !exclude_table.ResponseObj)
                            create_code = false;
                    }
                    if (create_code)
                    {
                        string name = RemovePrefix(entityType.Name);
                        string model_name = entityType.Name;
                        var list_properties = entityType.GetProperties();

                        string target_path = Path.Combine(project_path, current_namespace + $@"Data\Generated\Backend\Core\{GetPrefix(entityType.Name)}\{name}\Object");

                        if (!Directory.Exists(target_path))
                            Directory.CreateDirectory(target_path);

                        var code = code_template;
                        string code_file = Path.Combine(target_path, $"{name}Response.cs");
                        code = code.Replace("{{name}}", name);
                        code = code.Replace("{{model}}", model_name);
                        code = code.Replace("{{schema}}", GetPrefix(entityType.Name));

                        string primary_type = list_properties.Where(d => d.IsPrimaryKey()).Select(d => ParseType(d.ClrType)).FirstOrDefault()!;
                        if (string.IsNullOrWhiteSpace(primary_type))
                            primary_type = list_properties.Where(d => d.Name.ToLower() == ("id")).Select(d => ParseType(d.ClrType)).FirstOrDefault()!;

                        string attributes = "";
                        List<string> mapper = new List<string>();
                        foreach (var d in list_properties)
                        {
                            string type = ParseType(d.ClrType);
                            string isnullable = d.IsNullable && type != "string" ? "?" : "";
                            string attribute_name = d.Name;
                            string attribute = $"\t\tpublic {type}{isnullable} {attribute_name}";
                            attribute += "{ get; set; }";
                            attributes += attribute + Environment.NewLine;

                        }
                        code = code.Replace("{{attributes}}", attributes);
                        string map = "";
                        if (mapper.Count > 0)
                            map = $"\t\t\tmap.{string.Join(".\n\t\t\t", mapper)};";
                        code = code.Replace("{{mapper}}", map);

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
    }
}
