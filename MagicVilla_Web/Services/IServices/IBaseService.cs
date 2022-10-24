using MagicVilla_Web.Models;

namespace MagicVilla_Web.Services.IServices
{
    public interface IBaseService
    {

        APIResponse ResponseModel { get; set; }

        // will be used to send API request / calls
        Task<T> SendAsync<T>(APIRequest aPIRequest);



    }

}
