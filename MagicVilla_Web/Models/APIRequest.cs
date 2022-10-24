using static MagicVilla_Utility.SD;

namespace MagicVilla_Web.Models
{
    public class APIRequest
    {
        public APIType APIType { get; set; } = APIType.GET;

        // URL of API we want to call
        public string Url { get; set; }

        // Data - need to pass to API during call
        public object Data { get; set; }    


    }
}
