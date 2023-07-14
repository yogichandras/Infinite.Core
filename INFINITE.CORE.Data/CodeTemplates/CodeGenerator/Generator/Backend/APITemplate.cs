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
        #region API 
        public static (bool success, string key, object val) GenerateAPICQRS(string current_namespace, IModel model, Microsoft.Extensions.Options.IOptions<HandlebarsScaffoldingOptions> _options, List<SettingExcludeTableCodeGeneratorObject> exclude)
        {
            var sb = new Microsoft.EntityFrameworkCore.Infrastructure.IndentedStringBuilder();
            string project_name = current_namespace + "Data";
            string project_path = Directory.GetCurrentDirectory().Replace(project_name, "");

            string template_path = Path.Combine(project_path, current_namespace + @"Data\CodeTemplates\CodeGenerator\Template\Backend\API.hbs");
            if (!File.Exists(template_path))
            {
                return (false, null, null);
            }
           
            var request_template = File.ReadAllText(template_path);
            request_template = request_template.Replace("{{namespace}}", current_namespace);
            using (sb.Indent())
            using (sb.Indent())
            {
                foreach (var entityType in model.GetScaffoldEntityTypes(_options.Value))
                {
                    string name = RemovePrefix(entityType.Name);
                    string model_name = entityType.Name;
                    var list_properties = entityType.GetProperties();

                    bool create_code = true;
                    if (exclude != null && exclude.Count() > 0)
                    {
                        var exclude_table = exclude.Where(d => d.TableName.ToLower() == entityType.Name.ToLower()).FirstOrDefault();
                        if (exclude_table != null && !exclude_table.ApiController)
                            create_code = false;
                    }
                    if (create_code)
                    {
                        //string target_path = Path.Combine(project_path, "Presentation", current_namespace + $@"API\Controllers\v1\{GetPrefix(entityType.Name)}");
                        string target_path = Path.Combine(project_path, current_namespace + $@"Data\Generated\Backend\API\{GetPrefix(entityType.Name)}");

                        if (!Directory.Exists(target_path))
                        {
                            Directory.CreateDirectory(target_path);
                        }
                        string service_file = Path.Combine(target_path, $"{name}Controller.cs");

                        var request = request_template;

                        request = request.Replace("{{name}}", name);
                        request = request.Replace("{{model}}", model_name);
                        request = request.Replace("{{schema}}", GetPrefix(entityType.Name));

                        bool is_master = list_properties.Any(d => d.Name.ToLower() == ("active"));
                        if (is_master)
                            request = request.Replace("{{>master}}", "").Replace("{{<master}}", "");
                        else
                            request = RemoveText(request, "{{>master}}", "{{<master}}");


                        string primary_type = list_properties.Where(d => d.IsPrimaryKey() == true).Select(d => ParseType(d.ClrType)).FirstOrDefault()!;
                        if (string.IsNullOrWhiteSpace(primary_type))
                            primary_type = list_properties.Where(d => d.Name.ToLower() == ("id")).Select(d => ParseType(d.ClrType)).FirstOrDefault()!;
                        request = request.Replace("{{primary_key_type}}", primary_type);
                        using (StreamWriter outputFile = new StreamWriter(service_file))
                        {
                            outputFile.WriteLine(request);
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
