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
    // this is another way of route  ->
    // [Route("api/[controller]")]
    [Route("api/VillaAPI")]

    // APICONTROLLER's BUILT IN Validation gets executed first before ModelState Validation.
    // This also supports BUILTIN VALIDATIONS.  Comment out if you need to use STEP/ DEBUG ModelState
    // Otherwise, BREAKPOINT will not be hit.
    [ApiController] 



    public class VillaAPIController : ControllerBase
    {

        private APIResponse _APIResponse;

        //private readonly ApplicationDbContext mDbContext;
        private readonly IVillaRepository _villaRepository;

        private readonly IMapper mMapper;

        private readonly ILogger<VillaAPIController> mLogger; // BUILT-IN LOGGER
        //private readonly ILogging mLogger;  // CUSTOM LOGGER

     
        public VillaAPIController(ApplicationDbContext dbContext, ILogger<VillaAPIController> logger,
            IMapper mapper, IVillaRepository villaRepository)
        //public VillaAPIController(ILogging logger)
        {
            //mDbContext = dbContext;

            _villaRepository = villaRepository;
            mLogger = logger;
            mMapper = mapper;
            this._APIResponse = new APIResponse();

        }


        // instead of defining the specific return type which is VillaDTO here, 
        // we can use ACTIONRESULT. We can use this for ANY RETURN TYPE that we want.
        //[HttpGet]
        //public IEnumerable<VillaDTO> GetVillas()
        //{
        //    return VillaStore.villaList;

        //}

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        //public async Task<ActionResult<IEnumerable<VillaDTO>>> GetVillas()
        public async Task<ActionResult<APIResponse>> GetVillas()
        {
            try
            {

                mLogger.LogInformation("Getting all villas...");
                //mLogger.Log("Getting all villas...", "warning");

                //return Ok(VillaStore.villaList);
                IEnumerable<Villa> villaList = await _villaRepository.GetAllAsync();

                _APIResponse.Result = mMapper.Map<List<VillaDTO>>(villaList);
                _APIResponse.IsSuccess = true;
                _APIResponse.StatusCode = HttpStatusCode.OK;

                // convert from a list of Villa to a list of VillaDTO
                return Ok(_APIResponse);

            }
            catch (Exception ex)
            {
                _APIResponse.IsSuccess = false;
                _APIResponse.ErrorMessages = new List<string>() { ex.ToString() };

            }

            return _APIResponse;

        }


        //[HttpGet("id")] // state that this HTTP get REQUIRES an ID
        ////[HttpGet("{id:int}")] // same as above but explicitly says that id is an INT
        //public VillaDTO GetVilla(int id)
        //{
        //    return VillaStore.villaList.FirstOrDefault(m => m.Id == id);

        //}


        // GET
        [HttpGet("{id:int}", Name = "GetVilla")] // state that this http get requires an id. Optionally, we can give this route a name
        [ProducesResponseType(StatusCodes.Status200OK)] // This is optional.
        [ProducesResponseType(StatusCodes.Status400BadRequest)] //    SPECIFY ALL POSSIBLE RESULTS
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        //[ProducesResponseType(200)] 
        //[ProducesResponseType(400)] 
        //[ProducesResponseType(404)] 
        public async Task<ActionResult<APIResponse>> GetVilla(int id)
        {
            try
            {

                if (id == 0)
                {
                    mLogger.LogError("Get Villa Error with id = " + id);
                    //mLogger.Log("Get Villa Error with id = " + id, "error");

                    _APIResponse.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_APIResponse); // Return Error 400
                }

                //var villa = VillaStore.villaList.FirstOrDefault(m => m.Id == id);
                //var villa = await mDbContext.Villas.FirstOrDefaultAsync(m => m.Id == id);
                var villa = await _villaRepository.GetAsync(m => m.Id == id, false);

                if (villa == null)
                {
                    _APIResponse.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_APIResponse); // Return Error 404
                }

                _APIResponse.Result = mMapper.Map<VillaDTO>(villa);
                _APIResponse.IsSuccess = true;
                _APIResponse.StatusCode = HttpStatusCode.OK;

                //return Ok(villa);       // Return OK ( 200 )
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

        // typically, we receive the object that will be created 
        // FROM THE BODY. So we need to add that attribute
        public async Task<ActionResult<APIResponse>> CreateVilla ([FromBody] VillaCreateDTO createDTO)
        {
            // Use ModelState for checking if you have CUSTOM VALIDATIONS to be done
            // Comment out the [APICONTROLLER] above attribute if you want to use ModelState checking
            //if (!ModelState.IsValid)
            //{
            //    return BadRequest(ModelState);
            //}

            try
            {

                // check if record already exists
                if (await _villaRepository.GetAsync(m => m.Name.ToLower() == createDTO.Name.ToLower()) != null)
                {
                    // add custom error message

                    ModelState.AddModelError("My Custom Error", "Villa already exists !");
                    return BadRequest(ModelState);
                }


                if (createDTO == null)
                {
                    return BadRequest(createDTO);
                }

                Villa villa = mMapper.Map<Villa>(createDTO);

                //Villa model = new()
                //{
                //    Amenity = createDTO.Amenity,
                //    Details = createDTO.Details,
                //    ImageURL = createDTO.ImageURL,
                //    Name = createDTO.Name,
                //    Occupancy = createDTO.Occupancy,
                //    Rate = createDTO.Rate,
                //    AreaInSqFt = createDTO.AreaInSqFt
                //};

                await _villaRepository.CreateAsync(villa);

                _APIResponse.IsSuccess = true;
                _APIResponse.Result = mMapper.Map<VillaDTO>(villa);
                _APIResponse.StatusCode = HttpStatusCode.Created;


                //return Ok(villa);       // Return OK ( 200 )
                //return Ok(_APIResponse);


                // return the details of the DTO we CREATED.
                //return Ok(villaDTO);

                // Send a Created Response
                // return the details by passing the ID to a named route above (GetVilla)
                // so the caller / user would know the details of the newly-added record
                // Here, CreatedAtRoute requires us 2x things:
                //    1. ID of the villa which it will use when it calls "GetVilla" endpoint
                //    2. The created object which it needs for its own purpose
                return CreatedAtRoute("GetVilla", new { id = villa.Id }, _APIResponse);

            }
            catch (Exception ex)
            {
                _APIResponse.IsSuccess = false;
                _APIResponse.ErrorMessages = new List<string>() { ex.ToString() };

            }

            return _APIResponse;

        }


        // DELETE
        [HttpDelete("{id:int}", Name = "DeleteVilla")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        // IActionResult here does not have any type because
        // we dont want to return any data after delete
        //public async Task<IActionResult> DeleteVilla(int id)
        public async Task<ActionResult<APIResponse>> DeleteVilla(int id)
        {

            try
            {

                // check if record already exists
                if (id == 0)
                {

                    return BadRequest();
                }

                var villa = await _villaRepository.GetAsync(m => m.Id == id);

                if (villa == null)
                {
                    return NotFound();
                }

                await _villaRepository.RemoveAsync(villa);

                _APIResponse.StatusCode = HttpStatusCode.NoContent;
                _APIResponse.IsSuccess = true;

                // when we delete, normally we dont want to return anything
                // but you can also return OK if you want
                //return NoContent();

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
        [HttpPut("{id:int}", Name = "UpdateVilla")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<APIResponse>> UpdateVilla(int id, [FromBody] VillaUpdateDTO updateDTO)
        {
            try
            {

                if (updateDTO == null || id != updateDTO.Id)
                {
                    return BadRequest();
                }


                // villaDTO is an object which we populated with our DESIRED properties
                // villa is the object instance connected to the database which will be updated
                //var villa = VillaStore.villaList.FirstOrDefault(m => m.Id == id);
                //villa.Name = villaDTO.Name;
                //villa.AreaInSqFt = villaDTO.AreaInSqFt;
                //villa.Occupancy = villaDTO.Occupancy;


                Villa model = mMapper.Map<Villa>(updateDTO);


                // JUST CONVERT THE DTO TO VILLA
                // AND EF WILL KNOW WHICH TO UPDATE BASED ON THE ID
                //Villa model = new()
                //{
                //    Amenity = updateDTO.Amenity,
                //    Details = updateDTO.Details,
                //    Id = updateDTO.Id,
                //    ImageURL = updateDTO.ImageURL,
                //    Name = updateDTO.Name,
                //    Occupancy = updateDTO.Occupancy,
                //    Rate = updateDTO.Rate,
                //    AreaInSqFt = updateDTO.AreaInSqFt
                //};

                await _villaRepository.UpdateAsync(model);

                _APIResponse.StatusCode = HttpStatusCode.NoContent;
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


        // PATCH ( Update only one or a few of the properties in the model )
        // This required NugetPackages JSONPATCH and NEWTONSOFTJSON
        [HttpPatch("{id:int}", Name = "UpdateVilla")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]

        // SINGLE field update CALL
        //[
        //  {

        //    "path": "/name",
        //    "op": "replace",
        //    "value": "New Villa"
        //  }
        //]

        // MULTIPLE Field Update CALL
        //[
        //  {
    
        //    "path": "/name",
        //    "op": "replace",
        //    "value": "The Big Block"
        //  },

        //  {
    
        //    "path": "/Details",
        //    "op": "replace",
        //    "value": "This is the Big Block."
        //  }
        //]
        //
        // When we are working with PATCH, we will receive something called a JSONPatchDocument
        // This PATCH DTO Document ONLY CONTAINS THE FIELD WE WANT TO UPDATE, NOT THE WHOLE OBJECT
        public async Task<IActionResult> UpdatePartialVilla(int id, JsonPatchDocument<VillaUpdateDTO> patchDTO)
        {
            if(patchDTO == null || id == 0)
            {
                return BadRequest();
            }

            // when this line is executed, EF will keep tracking this villa record / object instance.
            // Any change you make in this villa object will be saved
            // to DB immediately after SaveChanges call.
            // In this case though, we are dealing with PATCH DOCUMENT so we dont want this
            // normal behaviour. What we want is after retieving the Villa object here,
            // we will CREATE a NEW VILLA DTO OBJECT and populate this new villaDTO object  
            // with the properties of the retrieved villa object. (After this, we DONT want EF to
            // keep tracking this retrieved Villa object (using the ID) because we will submit a totally new 
            // Villa object soon. Thus we retrieve it with a call to AsNoTracking())
            //
            // At this point, we  will apply ANY CHANGE WE LIKE TO DO TO THE VillaDTO object.
            // Then we Apply the change to the PatchDocument with a call to patchDTO.applyTo.
            //
            //After our patchDTO update is applied, we would like to save our change back to the DB.
            // But our DB expects a VILLA object so we need to CREATE A NEW VILLA OBJECT and then
            // populate it with the properties of the patchDTO object.
            
            var villa = await _villaRepository.GetAsync(m => m.Id == id, tracked:false);

            // at this point, if villa is not null, then our patchDTO document will have 
            // what needs to be updated. 

            


            // EF sends us a PatchDocument which IS of type VillaDTO.
            // It contains the field the user wants updated.
            // But since it is only partial object, we need to create a FULL DTO object
            // and apply the field update contained in the patchDTO document
            // to this FULL DTO object (because we set the patchDTO object to be of type VillaDTO).
            // Now, we need to COMBINE THE CONTENTS OF the retrieved Villa object with the
            // UPDATED FIELD which is contained by the patchDTO patch.
            //
            // We do this by copying the properties from the VILLA object we have retrieved into a
            // villa DTO object and then calling the patchDTO.APPLYTO() method to transfer the
            // updated field into this new VillaDTO object. After this, our VillaDTO object will
            // containg the updated information of the object we want to save to the database.


            VillaUpdateDTO villaDTO = mMapper.Map<VillaUpdateDTO>(villa);

            if (villa == null)
            {
                return BadRequest(); // or can also return Not Found
            }

            //VillaUpdateDTO villaDTO = new()
            //{
            //    Amenity = villa.Amenity,
            //    Details = villa.Details,
            //    Id = villa.Id,
            //    ImageURL = villa.ImageURL,
            //    Name = villa.Name,
            //    Occupancy = villa.Occupancy,
            //    Rate = villa.Rate,
            //    AreaInSqFt = villa.AreaInSqFt
            //};


            // Apply or COPY the patchDTO contents from the patchDocument 
            // to a 
            patchDTO.ApplyTo(villaDTO, ModelState); // ModelState -> used if there are any errors

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }


            // now that patchDTO document is updated, we need to save to EF Database
            // which expects a Villa so we will have to convert from VillaDTO back to villa object

            Villa model = mMapper.Map<Villa>(villaDTO);

            //Villa model = new()
            //{
            //    Amenity = villaDTO.Amenity,
            //    Details = villaDTO.Details,
            //    Id = villaDTO.Id,
            //    ImageURL = villaDTO.ImageURL,
            //    Name = villaDTO.Name,
            //    Occupancy = villaDTO.Occupancy,
            //    Rate = villaDTO.Rate,
            //    AreaInSqFt = villaDTO.AreaInSqFt
            //};

            await _villaRepository.UpdateAsync(model);
            
            return NoContent();
        
        
        }

    }

}
