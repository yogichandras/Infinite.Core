using EntityFrameworkCore.Scaffolding.Handlebars;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore.Scaffolding;
using Microsoft.Extensions.Options;
using INFINITE.CORE.Data.CodeGenerator.Generator;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace INFINITE.CORE.Data.CodeGenerator
{
    public class CodeGenerator : HbsCSharpDbContextGenerator
    {
        private readonly IOptions<HandlebarsScaffoldingOptions> _options;
        private string _modelNamespace = "";
        public CodeGenerator(
            IProviderConfigurationCodeGenerator providerConfigurationCodeGenerator,
            IAnnotationCodeGenerator annotationCodeGenerator,
            IDbContextTemplateService dbContextTemplateService,
            IEntityTypeTransformationService entityTypeTransformationService,
            ICSharpHelper cSharpHelper,
            IOptions<HandlebarsScaffoldingOptions> options)
            : base(providerConfigurationCodeGenerator, annotationCodeGenerator, dbContextTemplateService, entityTypeTransformationService, cSharpHelper, options)
        {
            _options = options;
        }

        protected override void GenerateClass(IModel model, string contextName, string connectionString, bool suppressConnectionStringWarning, bool suppressOnConfiguring)
        {
            IOptions<HandlebarsScaffoldingOptions> options = _options;
            if (options == null || options.Value?.EnableSchemaFolders != true)
            {
                TemplateData.Add("model-namespace", _modelNamespace);
            }
            else
            {
                GenerateModelImports(model);
            }

            TemplateData.Add("class", contextName);
            GenerateDbSets(model);
            GenerateEntityTypeErrors(model);
            if (suppressOnConfiguring)
            {
                TemplateData.Add("suppress-on-configuring", true);
            }
            else
            {
                GenerateOnConfiguring(connectionString, suppressConnectionStringWarning);
            }
            string setting_path = Path.Combine(Environment.CurrentDirectory, @"CodeTemplates\CodeGenerator\settings.json");
            if (File.Exists(setting_path))
            {
                var settings_txt = File.ReadAllText(setting_path);
                var setting = Newtonsoft.Json.JsonConvert.DeserializeObject<SettingCodeGeneratorObject>(settings_txt);

                //BACKEND
                CodeTemplate.GenerateGetByID(setting.PrefixNamespace, model, _options, setting.Exclude);
                CodeTemplate.GenerateGetList(setting.PrefixNamespace, model, _options, setting.Exclude);
                CodeTemplate.GenerateResponseCQRS(setting.PrefixNamespace, model, _options, setting.Exclude);
                CodeTemplate.GenerateRequestCQRS(setting.PrefixNamespace, model, _options, setting.Exclude);
                CodeTemplate.GenerateAdd(setting.PrefixNamespace, model, _options, setting.Exclude);
                CodeTemplate.GenerateDelete(setting.PrefixNamespace, model, _options, setting.Exclude);
                CodeTemplate.GenerateEdit(setting.PrefixNamespace, model, _options, setting.Exclude);
                CodeTemplate.GenerateActive(setting.PrefixNamespace, model, _options, setting.Exclude);
                CodeTemplate.GenerateAPICQRS(setting.PrefixNamespace, model, _options, setting.Exclude);

                //FRONTEND
                //CodeTemplate.GenerateViewIndex(setting.PrefixNamespace, model, _options, setting.Exclude);
                //CodeTemplate.GenerateViewAdd(setting.PrefixNamespace, model, _options, setting.Exclude);
                //CodeTemplate.GenerateViewEdit(setting.PrefixNamespace, model, _options, setting.Exclude);
                //CodeTemplate.GenerateViewDetail(setting.PrefixNamespace, model, _options, setting.Exclude);
                //CodeTemplate.GenerateScriptIndex(setting.PrefixNamespace, model, _options, setting.Exclude);
                //CodeTemplate.GenerateScriptAdd(setting.PrefixNamespace, model, _options, setting.Exclude);
                //CodeTemplate.GenerateScriptEdit(setting.PrefixNamespace, model, _options, setting.Exclude);
                //CodeTemplate.GenerateScriptDelete(setting.PrefixNamespace, model, _options, setting.Exclude);
                //CodeTemplate.GenerateScriptDetail(setting.PrefixNamespace, model, _options, setting.Exclude);

                //INFRASTRUCTURE
                //CodeTemplate.GenerateInfrastructureInterface(setting.PrefixNamespace, model, _options, setting.Exclude);
                //CodeTemplate.GenerateInfrastructureRequest(setting.PrefixNamespace, model, _options, setting.Exclude);
                //CodeTemplate.GenerateInfrastructureResponse(setting.PrefixNamespace, model, _options, setting.Exclude);
                //CodeTemplate.GenerateInfrastructureService(setting.PrefixNamespace, model, _options, setting.Exclude);
            }
            GenerateOnModelCreating(model);
        }
        private IReadOnlyDictionary<string, string> GetEntityTypeErrors(IReadOnlyModel model)
            => (IReadOnlyDictionary<string, string>?)model["Scaffolding:EntityTypeErrors"] ?? new Dictionary<string, string>();

        private void GenerateEntityTypeErrors(IModel model)
        {
            List<Dictionary<string, object>> list = new List<Dictionary<string, object>>();
#pragma warning disable EF1001 // Internal EF Core API usage.
            foreach (KeyValuePair<string, string> entityTypeError in GetEntityTypeErrors(model))
            {
                list.Add(new Dictionary<string, object>
                {
                    {
                        "entity-type-error",
                        "// " + entityTypeError.Value + " Please see the warning messages."
                    }
                });
            }
#pragma warning restore EF1001 // Internal EF Core API usage.

            TemplateData.Add("entity-type-errors", list);
        }

        private void GenerateDbSets(IModel model)
        {
            List<Dictionary<string, object>> list = new List<Dictionary<string, object>>();
            foreach (IEntityType scaffoldEntityType in model.GetScaffoldEntityTypes(_options.Value))
            {
                if (!IsManyToManyJoinEntityType(scaffoldEntityType))
                {
                    string entityTypeName = GetEntityTypeName(scaffoldEntityType, EntityTypeTransformationService.TransformTypeEntityName(scaffoldEntityType.Name));
#pragma warning disable EF1001 // Internal EF Core API usage.
                    list.Add(new Dictionary<string, object>
                    {
                        {
                            "set-property-type",
                            entityTypeName
                        },
                        {
                            "set-property-name",
                            scaffoldEntityType.GetDbSetName()
                        },
                        {
                            "nullable-reference-types",
                            UseNullableReferenceTypes
                        }
                    });
#pragma warning restore EF1001 // Internal EF Core API usage.
                }
            }

            TemplateData.Add("dbsets", list);
        }


        private void GenerateModelImports(IModel model)
        {
            List<Dictionary<string, object>> list = new List<Dictionary<string, object>>();
            foreach (string item in (from e in model.GetScaffoldEntityTypes(_options.Value)
                                     select e.GetSchema() into s
                                     orderby s
                                     select s).Distinct())
            {
                Dictionary<string, object> dictionary = new Dictionary<string, object>();
                DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(4, 3);
                defaultInterpolatedStringHandler.AppendFormatted(item);
                defaultInterpolatedStringHandler.AppendLiteral(" = ");
                defaultInterpolatedStringHandler.AppendFormatted(_modelNamespace);
                defaultInterpolatedStringHandler.AppendLiteral(".");
                defaultInterpolatedStringHandler.AppendFormatted(item);
                dictionary.Add("model-import", defaultInterpolatedStringHandler.ToStringAndClear());
                list.Add(dictionary);
            }

            TemplateData.Add("model-imports", list);
        }

        private string GetEntityTypeName(IEntityType entityType, string entityTypeName)
        {
            string str = !string.IsNullOrWhiteSpace(entityType.GetTableName()) ? entityType.GetSchema() : entityType.GetViewSchema();
            IOptions<HandlebarsScaffoldingOptions> options = _options;
            if (options == null || options.Value?.EnableSchemaFolders != true)
            {
                return entityTypeName;
            }

            return str + "." + entityTypeName;
        }

        private static bool IsManyToManyJoinEntityType(IEntityType entityType)
        {
            if (!entityType.GetNavigations().Any() && !entityType.GetSkipNavigations().Any())
            {
                IKey key = entityType.FindPrimaryKey();
                List<IProperty> list = entityType.GetProperties().ToList();
                List<IForeignKey> list2 = entityType.GetForeignKeys().ToList();
                if (key != null && key.Properties.Count > 1 && list2.Count == 2 && key.Properties.Count == list.Count && list2[0].Properties.Count + list2[1].Properties.Count == list.Count && !list2[0].Properties.Intersect(list2[1].Properties).Any() && list2[0].IsRequired && list2[1].IsRequired && !list2[0].IsUnique && !list2[1].IsUnique)
                {
                    return true;
                }
            }

            return false;
        }

    }


    internal class SettingCodeGeneratorObject
    {
        public string PrefixNamespace { get; set; }
        public List<SettingExcludeTableCodeGeneratorObject> Exclude { get; set; }
    }
    public class SettingExcludeTableCodeGeneratorObject
    {
        public string TableName { get; set; }
        public bool ApiController { get; set; }
        public bool RequestObj { get; set; }
        public bool ResponseObj { get; set; }
        public bool Service { get; set; }
    }
}