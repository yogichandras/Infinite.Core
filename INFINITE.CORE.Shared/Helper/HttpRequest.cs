using INFINITE.CORE.Shared.Interface;
using Newtonsoft.Json;
using System.Text;

namespace INFINITE.CORE.Shared.Helper
{
    public class HttpRequest : IHttpRequest
    {
        public async Task<(bool IsSuccess, string ErrorMessage, T Result, Exception ex)> DoRequestData<T>(HttpMethod httpMethod, string token, string url, object paramBody, Dictionary<string, string> additionalHeaders = null) where T : class
        {
            try
            {
                using var client = new HttpClient();
                client.Timeout = TimeSpan.FromMinutes(30);
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                if (additionalHeaders != null)
                {
                    foreach (var item in additionalHeaders)
                    {
                        client.DefaultRequestHeaders.Add(item.Key, item.Value);
                    }
                }

                if (!string.IsNullOrEmpty(token))
                    client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

                //var request = new HttpRequestMessage(httpMethod, pathUrl);
                HttpContent httpContent = null;
                if ((httpMethod == HttpMethod.Post || httpMethod == HttpMethod.Put) && paramBody != null)
                {
                    string jsonString = JsonConvert.SerializeObject(paramBody);
                    httpContent = new StringContent(jsonString, Encoding.UTF8, "application/json");
                }

                HttpResponseMessage response = null;
                switch (httpMethod.Method)
                {
                    case "GET":
                        response = await client.GetAsync($"{url}{paramBody}");
                        break;

                    case "POST":
                        response = await client.PostAsync(url, httpContent);
                        break;

                    case "PUT":
                        response = await client.PutAsync(url, httpContent);
                        break;

                    case "DELETE":
                        response = await client.DeleteAsync($"{url}{paramBody}");
                        break;
                }

                if (response == null)
                    throw new Exception("Something Went Wrong, response is null!");

                //var response = client.SendAsync(request).Result; // dikomen karena => https://stackoverflow.com/questions/63211539/blazor-startup-error-system-threading-synchronizationlockexception-cannot-wait

                var content = await response.Content.ReadAsStringAsync();
                if (string.IsNullOrEmpty(content))
                    throw new Exception("Something Went Wrong! content is null");

                var result = JsonConvert.DeserializeObject<T>(content);

                return (response.IsSuccessStatusCode, "", result, null);
            }
            catch (Exception ex)
            {
                return (false, ex.Message, null, ex);
            }
        }
        
        public async Task<(bool IsSuccess, string ErrorMessage, T Result, Exception ex)> DoRequestDataWithoutContent<T>(HttpMethod httpMethod, string token, string url, object paramBody, Dictionary<string, string> additionalHeaders = null) where T : class
        {
            try
            {
                using var client = new HttpClient();
                client.Timeout = TimeSpan.FromHours(1);
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                if (additionalHeaders != null)
                {
                    foreach (var item in additionalHeaders)
                    {
                        client.DefaultRequestHeaders.Add(item.Key, item.Value);
                    }
                }

                if (!string.IsNullOrEmpty(token))
                    client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

                //var request = new HttpRequestMessage(httpMethod, pathUrl);
                HttpContent httpContent = null;
                if ((httpMethod == HttpMethod.Post || httpMethod == HttpMethod.Put) && paramBody != null)
                {
                    string jsonString = JsonConvert.SerializeObject(paramBody);
                    httpContent = new StringContent(jsonString, Encoding.UTF8, "application/json");
                }

                HttpResponseMessage response = null;
                switch (httpMethod.Method)
                {
                    case "GET":
                        response = await client.GetAsync($"{url}{paramBody}");
                        break;

                    case "POST":
                        response = await client.PostAsync(url, httpContent);
                        break;

                    case "PUT":
                        response = await client.PutAsync(url, httpContent);
                        break;

                    case "DELETE":
                        response = await client.DeleteAsync($"{url}{paramBody}");
                        break;
                }

                if (response == null)
                    throw new Exception("Something Went Wrong, response is null!");

                var content = await response.Content.ReadAsStringAsync();

                var result = JsonConvert.DeserializeObject<T>(content);

                return (response.IsSuccessStatusCode, "", result, null);
            }
            catch (Exception ex)
            {
                return (false, ex.Message, null, ex);
            }
        }
    }

}
