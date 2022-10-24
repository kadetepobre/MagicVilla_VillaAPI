using AutoMapper;
using MagicVilla_Web.Models;
using MagicVilla_Web.Models.Dto;
using MagicVilla_Web.Services;
using MagicVilla_Web.Services.IServices;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace MagicVilla_Web.Controllers
{
    public class VillaNumberController : Controller
    {
        private IVillaNumberService mVillaNumberService;
        private IVillaNumberService mVillaService;

        private readonly IMapper mMapper;

        public VillaNumberController(IVillaNumberService mVillaNumberService, IMapper mMapper)
        {
            this.mVillaNumberService = mVillaNumberService;

            this.mMapper = mMapper;
        }

        public async Task<IActionResult> IndexVillaNumber()
        {
            List<VillaNumberDTO> list = new List<VillaNumberDTO>();

            var response = await mVillaNumberService.GetAllAsync<APIResponse>();

            if (response != null && response.IsSuccess)
            {
                list = JsonConvert.DeserializeObject<List<VillaNumberDTO>>(Convert.ToString(response.Result));
            }

            return View(list);

        }

       


    }
}
