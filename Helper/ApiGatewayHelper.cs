/*********************************************************
 * 
 * Author : Ujjwal R. Poudel
 * Date:    July 7th 2022
 * Project: Coastal Credit Job Interview Mini Project
 * 
 * Description of the project : 
 * Create a single-page mobile-first web application that allows the user to control their credit card.  
 * The application will allow the user to lock/freeze their card to disable transactions or unlock/unfreeze it to enable transactions again.  
 * The current state of the card should be maintained within the “session”.  The user should get a confirmation that their submission was successful.  
 * The user should be able to submit messages or report an issue with their card (i.e. lost/damaged/stolen).  
 * If a card is reported lost/damaged/stolen then it should also be frozen.
 *
 * 
 ********************************************************/
using System;
using System.Text;
using System.Net.Http;
using System.Configuration;
using Newtonsoft.Json;
using System.Threading.Tasks;
using System.Threading;
using System.Net.Http.Headers;

namespace APIDemo.Helper
{
    public class ApiGatewayHelper<TModel> where TModel:class
    {
        private static Uri APIGatewayUrl = new Uri("https://anypoint.mulesoft.com/mocking/api/v1/sources/exchange/assets/1a662d35-8008-4343-b811-226e2284646b/appdeveloperinterview/1.0.0/m");
        private static readonly HttpClient client = new HttpClient();
        private static string api_key = "C5F5A63C-E604-47AA-A7CC-B01F95FFBF09";
        public static async Task<TModel> GetSingleItemRequest(string controllerName,string actionName)
        {
            client.DefaultRequestHeaders.Add("API-Key", api_key);
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            try
            {
                string apiUrl = $"{APIGatewayUrl}/{controllerName}/{actionName}";
                var result = default(TModel);

                var response = await client.GetAsync(apiUrl).ConfigureAwait(false);
                if (response.IsSuccessStatusCode)
                {
                    await response.Content.ReadAsStringAsync().ContinueWith(responseString =>
                    {
                        if (typeof(TModel).Namespace != "System")
                        {
                            result = JsonConvert.DeserializeObject<TModel>(responseString?.Result);
                        }
                        else
                        {
                            result = (TModel)Convert.ChangeType(responseString?.Result, typeof(TModel));
                        }
                    });
                }
                else
                {
                    var content = await response.Content.ReadAsStringAsync();
                    response.Content?.Dispose();
                    throw new HttpRequestException($"{response.StatusCode},{content}");
                }
                return result;
            }
            catch(Exception e)
            {
                throw new Exception("System encountered error ",e);
            }
        }

        public static async Task<TModel> PostRequest(string controllerName, string actionName, object postObject, CancellationToken cancellationToken)
        {
            client.DefaultRequestHeaders.Add("API-Key", api_key);
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            TModel result = default;
            string apiUrl = $"{APIGatewayUrl}/{controllerName}/{actionName}";
            string jsonStringObject = JsonConvert.SerializeObject(postObject);
            var response = await client.PostAsync(apiUrl, new StringContent(jsonStringObject ,Encoding.UTF8,"application/json"), cancellationToken).ConfigureAwait(false);
            if (response.IsSuccessStatusCode)
            {
                await response.Content.ReadAsStringAsync().ContinueWith((Task<string> task) =>
                {
                    result = JsonConvert.DeserializeObject<TModel>(task.Result);
                }, cancellationToken);
            }
            else
            {
                var content = await response.Content.ReadAsStringAsync();
                response.Content?.Dispose();
                throw new HttpRequestException($"{response.StatusCode}:{content}");
            }
            return result;
        }
    }
}