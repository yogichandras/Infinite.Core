using INFINITE.CORE.Shared.Attributes;
using Jint;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;
using System.Text.RegularExpressions;

namespace INFINITE.CORE.API.Proxy
{
    public static class DynamicAjaxProxy
    {
        public static void Up(Microsoft.AspNetCore.Hosting.IHostingEnvironment _hostingEnvironment)
        {
            const string templateFileName = "Proxy/template-proxy.js";
            const string templateDataTableFileName = "Proxy/template-datatable-proxy.js";
            string templateContent = File.ReadAllText(Path.Combine(_hostingEnvironment.ContentRootPath, templateFileName));
            string templateDataTableContent = File.ReadAllText(Path.Combine(_hostingEnvironment.ContentRootPath, templateDataTableFileName));

            var controllers = Assembly.GetExecutingAssembly()
            .GetTypes()
            .Where(type => typeof(Controller).IsAssignableFrom(type));
            var generatedContent = "var service = {@";
            foreach (var controller in controllers)
            {
                var controllerName = controller.Name;
                if (!controllerName.ToLower().Contains("base"))
                {
                    var baseUrl = controllerName.ToLower().Replace("controller", "");
                    var content = $"{baseUrl}: "+"{@";
                    var methods = controller.GetMethods().Where(x => x.GetCustomAttribute(typeof(HttpGetAttribute)) != null || x.GetCustomAttribute(typeof(HttpPostAttribute)) != null || x.GetCustomAttribute(typeof(HttpPutAttribute)) != null || x.GetCustomAttribute(typeof(HttpPatchAttribute)) != null || x.GetCustomAttribute(typeof(HttpDeleteAttribute)) != null);

                    foreach (var method in methods)
                    {
                        var routing = string.Empty;
                        var parameters = method.GetParameters();
                        var httpMethod = string.Empty;
                        var template = parameters.Any(x => x.ParameterType == typeof(ListRequest)) ? templateDataTableContent : templateContent;

                        var getAttributes = method.GetCustomAttribute(typeof(HttpGetAttribute));
                        if (getAttributes != null)
                        {
                            httpMethod = "GET";
                            routing = ConvertToRouteName(((HttpGetAttribute)getAttributes)?.Template, parameters.Select(x => x.Name));
                        }

                        var postAttributes = method.GetCustomAttribute(typeof(HttpPostAttribute));
                        if (postAttributes != null)
                        {
                            httpMethod = "POST";
                            routing = ConvertToRouteName(((HttpPostAttribute)postAttributes)?.Template, parameters.Select(x => x.Name));
                        }

                        var putAttributes = method.GetCustomAttribute(typeof(HttpPutAttribute));
                        if (putAttributes != null)
                        {
                            httpMethod = "PUT";
                            routing = ConvertToRouteName(((HttpPutAttribute)putAttributes)?.Template, parameters.Select(x => x.Name));
                        }

                        var patchAttributes = method.GetCustomAttribute(typeof(HttpPatchAttribute));
                        if (patchAttributes != null)
                        {
                            httpMethod = "PATCH";
                            routing = ConvertToRouteName(((HttpPatchAttribute)patchAttributes)?.Template, parameters.Select(x => x.Name));
                        }

                        var deleteAttributes = method.GetCustomAttribute(typeof(HttpDeleteAttribute));
                        if (deleteAttributes != null)
                        {
                            httpMethod = "DELETE";
                            routing = ConvertToRouteName(((HttpDeleteAttribute)deleteAttributes)?.Template, parameters.Select(x => x.Name));
                        }

                        template = template.Replace("path_name", routing.Replace("-","_")).Replace("path_url", $"'/{baseUrl}/{routing}'")
                            .Replace("method", "'"+httpMethod+"'");

                        var endContent = method == methods.LastOrDefault() ? "" : ",";
                        content += $"@{template}{endContent}@";
                    }

                    var endControllerContent = controller == controllers.LastOrDefault() ? "" : ",";
                    content += "}" + $"{endControllerContent}@";
                    content = content.Replace("@", System.Environment.NewLine);
                    generatedContent += content;
                }
            }
            generatedContent += "@}";
            generatedContent = generatedContent.Replace("@", System.Environment.NewLine);

            const string generatedTemplate = "Proxy/Generated/service.proxy.js";
            File.WriteAllText(Path.Combine(_hostingEnvironment.ContentRootPath, generatedTemplate), generatedContent);
        }

        private static string ConvertToRouteName(string routeString, IEnumerable<string> parameters)
        {
            if (!string.IsNullOrEmpty(routeString))
            {
                routeString = routeString.Replace("{", string.Empty).Replace("}", string.Empty);
                var listRoute = routeString.Split("/");

                var routeStringResult = string.Empty;
                foreach (var item in listRoute)
                {
                    if (!parameters.Any(x => x == item))
                    {
                        routeStringResult += item + "/";
                    }
                }

                string[] parts = routeStringResult.Trim('/').Split('/');

                if (parts.Length == 1)
                {
                    return parts[0];
                }

                string[] cleanedParts = new string[parts.Length];
                for (int i = 0; i < parts.Length; i++)
                {
                    cleanedParts[i] = Regex.Replace(parts[i], "[^a-zA-Z0-9]", "");
                }

                string routeName = string.Join(".", cleanedParts);

                return routeName;
            }
            return routeString;
        }
    }
}
