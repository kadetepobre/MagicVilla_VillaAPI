using AutoMapper;
using MagicVilla_VillaAPI.Data;
using MagicVilla_VillaAPI.Logging;
using MagicVilla_VillaAPI.Models;
using MagicVilla_VillaAPI.Models.Dto;
using MagicVilla_VillaAPI.Repository.IRepository;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace MagicVilla_VillaAPI.Controllers
{
    [Route("api/VillaNumberAPI")]
    [ApiController] 



    public class VillaNumberAPIController : ControllerBase
    {

        private APIResponse _APIResponse;

        private readonly IVillaNumberRepository _villaNumberRepository;
        private readonly IVillaRepository _villaRepository;

        private readonly IMapper mMapper;

        private readonly ILogger<VillaNumberAPIController> mLogger;

     
        public VillaNumberAPIController(ApplicationDbContext dbContext, ILogger<VillaNumberAPIController> logger,
            IMapper mapper, IVillaNumberRepository villaNumberRepository, IVillaRepository villaRepository)
        
        {

            _villaNumberRepository = villaNumberRepository;
            _villaRepository = villaRepository;

            mLogger = logger;
            mMapper = mapper;
            this._APIResponse = new APIResponse();

        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<APIResponse>> GetVillaNumber()
        {
            try
            {

                mLogger.LogInformation("Getting all villas...");

                // Include Villa Property
                IEnumerable<VillaNumber> villaNumberList = await _villaNumberRepository.GetAllAsync(includeProperties:"Villa");

                _APIResponse.Result = mMapper.Map<List<VillaNumberDTO>>(villaNumberList);
                _APIResponse.IsSuccess = true;
                _APIResponse.StatusCode = HttpStatusCode.OK;

                return Ok(_APIResponse);

            }
            catch (Exception ex)
            {
                _APIResponse.IsSuccess = false;
                _APIResponse.ErrorMessages = new List<string>() { ex.ToString() };

            }

            return _APIResponse;

        }


        [HttpGet("{id:int}", Name = "GetVillaNumber")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> GetVillaNumber(int id)
        {
            try
            {

                if (id == 0)
                {
                    mLogger.LogError("Get Villa Error with id = " + id);

                    _APIResponse.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_APIResponse);
                }

                var villaNumber = await _villaNumberRepository.GetAsync(m => m.VillaNo == id);

                if (villaNumber == null)
                {
                    _APIResponse.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_APIResponse);
                }

                _APIResponse.Result = mMapper.Map<VillaNumberDTO>(villaNumber);
                _APIResponse.IsSuccess = true;
                _APIResponse.StatusCode = HttpStatusCode.OK;

                return Ok(_APIResponse);

            }
            catch (Exception ex)
            {
                _APIResponse.IsSuccess = false;
                _APIResponse.ErrorMessages = new List<string>() { ex.ToString() };

            }

            return _APIResponse;

        }

        // CREATE
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status201Created)]

        public async Task<ActionResult<APIResponse>> CreateVillaNumber([FromBody] VillaNumberCreateDTO createDTO)
        {

            try
            {

                if (await _villaNumberRepository.GetAsync(m => m.VillaNo == createDTO.VillaNo) != null)
                {

                    ModelState.AddModelError("My Custom Error", "Villa number already exists !");
                    return BadRequest(ModelState);
                }

                // if villadId is invalid (there Villa record being pointed does not exist or is invalid)
                if (await _villaRepository.GetAsync(m => m.Id == createDTO.VillaId) == null)
                {
                    ModelState.AddModelError("My Custom Error", "Villa Id is Invalid !");
                    return BadRequest(ModelState);
                }


                if (createDTO == null)
                {
                    return BadRequest(createDTO);
                }

                VillaNumber villaNumber = mMapper.Map<VillaNumber>(createDTO);

                await _villaNumberRepository.CreateAsync(villaNumber);

                _APIResponse.IsSuccess = true;
                _APIResponse.Result = mMapper.Map<VillaNumberDTO>(villaNumber);
                _APIResponse.StatusCode = HttpStatusCode.Created;

                //return Ok(_APIResponse);


                return CreatedAtRoute("GetVilla", new { id = villaNumber.VillaNo }, _APIResponse);


            }
            catch (Exception ex)
            {
                _APIResponse.IsSuccess = false;
                _APIResponse.ErrorMessages = new List<string>() { ex.ToString() };

            }

            return _APIResponse;

        }


        // DELETE
        [HttpDelete("{id:int}", Name = "DeleteVillaNumber")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]

        public async Task<ActionResult<APIResponse>> DeleteVillaNumber(int id)
        {

            try
            {

                // check if record already exists
                if (id == 0)
                {

                    return BadRequest();
                }

                var villaNumber = await _villaNumberRepository.GetAsync(m => m.VillaNo == id);

                if (villaNumber == null)
                {
                    return NotFound();
                }

                await _villaNumberRepository.RemoveAsync(villaNumber);

                _APIResponse.StatusCode = HttpStatusCode.OK;
                _APIResponse.IsSuccess = true;

                return Ok(_APIResponse);

            }
            catch (Exception ex)
            {
                _APIResponse.IsSuccess = false;
                _APIResponse.ErrorMessages = new List<string>() { ex.ToString() };

            }

            return _APIResponse;

        }

        // UPDATE ( Update all of the properties in the model )
        [HttpPut("{id:int}", Name = "UpdateVillaNumber")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<APIResponse>> UpdateVillaNumber(int id, [FromBody] VillaNumberUpdateDTO updateDTO)
        {
            try
            {

                if (updateDTO == null || id != updateDTO.VillaNo)
                {
                    return BadRequest();
                }

                // if villadId is invalid (there Villa record being pointed does not exist or is invalid)
                if (await _villaRepository.GetAsync(m => m.Id == updateDTO.VillaId) == null)
                {
                    ModelState.AddModelError("My Custom Error", "Villa Id is Invalid !");
                    return BadRequest(ModelState);
                }


                VillaNumber model = mMapper.Map<VillaNumber>(updateDTO);

                await _villaNumberRepository.UpdateAsync(model);

                _APIResponse.StatusCode = HttpStatusCode.OK;
                _APIResponse.IsSuccess = true;

                return _APIResponse;

            }

            catch (Exception ex)
            {
                _APIResponse.IsSuccess = false;
                _APIResponse.ErrorMessages = new List<string>() { ex.ToString() };


            }

            return _APIResponse;

        }


    }

}
