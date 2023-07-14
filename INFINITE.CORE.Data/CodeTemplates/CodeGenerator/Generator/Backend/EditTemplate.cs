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
        public static (bool success, string key, object val) GenerateEdit(string current_namespace, IModel model, Microsoft.Extensions.Options.IOptions<HandlebarsScaffoldingOptions> _options, List<SettingExcludeTableCodeGeneratorObject> exclude)
        {
            var sb = new Microsoft.EntityFrameworkCore.Infrastructure.IndentedStringBuilder();
            string project_name = current_namespace + "Data";
            string project_path = Directory.GetCurrentDirectory().Replace(project_name, "");
            string template_path = Path.Combine(project_path, current_namespace + @"Data\CodeTemplates\CodeGenerator\Template\Backend\Edit.hbs");
            if (!File.Exists(template_path))
            {
                return (false, null, null);
            }
            var code_template = File.ReadAllText(template_path);
            code_template = code_template.Replace("{{namespace}}", current_namespace);

            List<string> exclude_attributes = new List<string>()
            {
                "id",
                "createby",
                "create_by",
                "createdby",
                "created_by",
                "createdate",
                "create_date",
                "updateby",
                "update_by",
                "updatedby",
                "updated_by",
                "updatedate",
                "update_date"
            };
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

                        string target_path = Path.Combine(project_path, current_namespace + $@"Data\Generated\Backend\Core\{GetPrefix(entityType.Name)}\{name}\Command");

                        if (!Directory.Exists(target_path))
                            Directory.CreateDirectory(target_path);

                        var code = code_template;
                        string code_file = Path.Combine(target_path, $"Edit{name}Handler.cs");

                        string primary_type = list_properties.Where(d => d.IsPrimaryKey()).Select(d => ParseType(d.ClrType)).FirstOrDefault()!;
                        if (string.IsNullOrWhiteSpace(primary_type))
                            primary_type = list_properties.Where(d => d.Name.ToLower() == ("id")).Select(d => ParseType(d.ClrType)).FirstOrDefault()!;

                        string primary_name = list_properties.Where(d => d.IsPrimaryKey()).Select(d => d.Name).FirstOrDefault()!;
                        if (string.IsNullOrWhiteSpace(primary_name))
                            primary_name = "Id";

                        code = code.Replace("{{primary_key_type}}", primary_type);
                        code = code.Replace("{{primary_key_name}}", primary_name);
                        code = code.Replace("{{name}}", name);
                        code = code.Replace("{{model}}", model_name);
                        code = code.Replace("{{schema}}", GetPrefix(entityType.Name));

                        var update_by = CheckUpdateBy(list_properties);
                        var update_date = CheckUpdateDate(list_properties);

                        if (update_by.have)
                        {
                            code = code.Replace("{{>update_by}}", "").Replace("{{<update_by}}", "");
                            code = code.Replace("{{update_by_name}}", update_by.value);
                        }
                        else
                            code = RemoveText(code, "{{>update_by}}", "{{<update_by}}");

                        if (update_date.have)
                        {
                            code = code.Replace("{{>update_date}}", "").Replace("{{<update_date}}", "");
                            code = code.Replace("{{update_date_name}}", update_date.value);
                        }
                        else
                            code = RemoveText(code, "{{>update_date}}", "{{<update_date}}");

                        string attributes = "";
                        foreach (var d in list_properties)
                        {
                            if (d.IsPrimaryKey())
                                continue;
                            if (!exclude_attributes.Contains(d.Name.ToLower()))
                            {
                                string type = ParseType(d.ClrType);
                                string isnullable = d.IsNullable && type != "string" ? "?" : "";
                                string attribute = "";
                                if (!d.IsNullable)
                                    attribute = "\t\t[Required]" + Environment.NewLine;

                                string attribute_name = d.Name;
                                attribute += $"\t\tpublic {type}{isnullable} {attribute_name}";
                                attribute += "{ get; set; }";
                                attributes += attribute + Environment.NewLine;
                            }
                        }
                        code = code.Replace("{{attributes}}", attributes);

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
