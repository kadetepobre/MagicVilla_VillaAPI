using AutoMapper;
using MagicVilla_Web.Models;
using MagicVilla_Web.Models.Dto;
using MagicVilla_Web.Services.IServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Reflection;
using System.Text.Json.Serialization.Metadata;

namespace MagicVilla_Web.Controllers
{
    public class VillaController : Controller
    {
        private readonly IVillaService mVillaService;
        private readonly IMapper mMapper;

        public VillaController(IVillaService mVillaService, IMapper mMapper)
        {
            this.mVillaService = mVillaService;
            this.mMapper = mMapper;
        }

        public async Task<IActionResult> IndexVilla()
        {
            List<VillaDTO> list = new List<VillaDTO>();

            var response = await mVillaService.GetAllAsync<APIResponse>();

            if(response != null && response.IsSuccess)
            {
                list = JsonConvert.DeserializeObject<List<VillaDTO>>(Convert.ToString(response.Result));
            }

            return View(list);

        }

        public async Task<IActionResult> CreateVilla()
        {
            

            return View();

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateVilla(VillaCreateDTO model)
        {
            if (ModelState.IsValid)
            {
                var response = await mVillaService.CreateAsync<APIResponse>(model);

                if (response != null && response.IsSuccess)
                {
                    return RedirectToAction(nameof(IndexVilla));
                }

            }

            return View(model);

        }

        public async Task<IActionResult> UpdateVilla(int villaId)
        {

            // need to show user what they are about to update
            var response = await mVillaService.GetAsync<APIResponse>(villaId);

            if (response != null && response.IsSuccess)
            {
                // deserialize the received response

                VillaDTO model = JsonConvert.DeserializeObject<VillaDTO>(Convert.ToString(response.Result));

                // we need to convert to VillaUpdateDTO when user submits later on for saving to DB.
                return View(mMapper.Map<VillaUpdateDTO>(model));
            }


            return NotFound();

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateVilla(VillaUpdateDTO model)
        {
            if (ModelState.IsValid)
            {
                var response = await mVillaService.UpdateAsync<APIResponse>(model);

                if (response != null && response.IsSuccess)
                {
                    return RedirectToAction(nameof(IndexVilla));
                }

            }

            return View(model);

        }


        public async Task<IActionResult> DeleteVilla(int villaId)
        {

            // need to show user what they are about to update
            var response = await mVillaService.GetAsync<APIResponse>(villaId);

            if (response != null && response.IsSuccess)
            {
                // deserialize the received response

                VillaDTO model = JsonConvert.DeserializeObject<VillaDTO>(Convert.ToString(response.Result));

                // no need to convert
                return View(model);
            }


            return NotFound();

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteVilla(VillaDTO model)
        {
            // When we are deleting, we dont need validation and we just need the ID
            var response = await mVillaService.DeleteAsync<APIResponse>(model.Id);

            if (response != null && response.IsSuccess)
            {
                return RedirectToAction(nameof(IndexVilla));
            }

           

            return View(model);

        }

    }
}
