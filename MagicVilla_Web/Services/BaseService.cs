using MagicVilla_Utility;
using MagicVilla_Web.Models;
using MagicVilla_Web.Services.IServices;
using Newtonsoft.Json;
using System.Text;

namespace MagicVilla_Web.Services
{
    public class BaseService : IBaseService
    {
        public APIResponse ResponseModel  { get; set; }

        // will be used to call the API
        public IHttpClientFactory httpClient { get; set; }

        public BaseService(IHttpClientFactory httpClient)
        {
            this.ResponseModel = new APIResponse();
            this.httpClient = httpClient;
        }

        public async Task<T> SendAsync<T>(APIRequest aPIRequest)
        {
            try
            {
                var client = httpClient.CreateClient("MagicAPI");
                
                HttpRequestMessage message = new HttpRequestMessage();
                message.Headers.Add("Accept", "application/json");
                message.RequestUri = new Uri(aPIRequest.Url);

                // if there are some data to pass, we need to SERIALIZE that
                // Data will have to be passed when making POST / PUT HTTP calls to the API
                if(aPIRequest.Data != null)
                {
                    message.Content = 
                        new StringContent(JsonConvert.SerializeObject(aPIRequest.Data),
                        Encoding.UTF8, "application/json");

                }

                switch (aPIRequest.APIType) {

                    case SD.APIType.POST:
                        message.Method = HttpMethod.Post;
                        break;

                    case SD.APIType.PUT:
                        message.Method = HttpMethod.Put;
                        break;

                    case SD.APIType.DELETE:
                        message.Method = HttpMethod.Delete;
                        break;

                    default:
                        message.Method = HttpMethod.Get;
                        break;


                }

                HttpResponseMessage apiResponse = null;

                apiResponse = await client.SendAsync(message);

                // when we receive the apiResponse, we need to READ the API content 
                var apiContent = await apiResponse.Content.ReadAsStringAsync();

                // and DESERIALIZE the content
                var deserializedAPIResponse = JsonConvert.DeserializeObject<T>(apiContent);

                return deserializedAPIResponse;

            }
            catch(Exception ex)
            {
                var dto = new APIResponse
                {
                    ErrorMessages = new List<string> { Convert.ToString(ex.Message) },
                    IsSuccess = false
                };


                // here, we first have to serialize DTO and then de-serialize it
                // you cannot return DTO directly because we need to convert DTO to
                // type T before returning it via a call to DeserializeObject<T>.
                // This deserialize expects a serialized data hence we had to Serialize
                // the DTO first before calling deserialize to type T.
                var res = JsonConvert.SerializeObject(dto);
                var deserializedAPIResponse = JsonConvert.DeserializeObject<T>(res);

                return deserializedAPIResponse;

            }
        }
    }
}
