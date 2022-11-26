using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;
using NZWalks.API.Repositories;

namespace NZWalks.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RegionsController : Controller
    {
        private readonly IRegionRepository regionRepository;
        private readonly IMapper mapper;

        public RegionsController(IRegionRepository regionRepository,IMapper mapper)
        {
            this.regionRepository = regionRepository;
            this.mapper = mapper;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllRegionsAsync()
        {
           var regions= await regionRepository.GetAllAsync();
            //Return DTO Region
            //var regionsDTO = new List<Models.DTO.Region>();
            //regions.ToList().ForEach(region =>
            //{
            //    var regionDTO = new Models.DTO.Region()
            //    {
            //        Id = region.Id,
            //        Code = region.Code,
            //        Name = region.Name,
            //        Area = region.Area,
            //        Lat = region.Lat,
            //        Long = region.Long,
            //        Population = region.Population,
            //    };
            //    regionsDTO.Add(regionDTO);
            //});
          var regionDTO=  mapper.Map<List<Models.DTO.Region>>(regions);
            return Ok(regionDTO);
        }
        [HttpGet]
        [Route("{id:guid}")]
        [ActionName("GetRegionAsync")]
        public async Task<IActionResult>GetRegionAsync(Guid id)
        {
            var region=await regionRepository.GetAsync(id);
            if(region==null)
            {
                return NotFound();
            }
            var regionDTO=mapper.Map<Models.DTO.Region>(region);
            return Ok(regionDTO);
        }
        [HttpPost]
        public async Task<IActionResult>AddRegionAsync(Models.DTO.AddRegionRequest addRegionRequest)
        {
            // first validate addRegionRequest Model and check if it is correct or not
            //validate the request
            if (!ValidateAddRegionAsync(addRegionRequest))
            {
                return BadRequest(ModelState);
            }


            //request(DTO) into domain model
            var region = new Models.Domain.Region()
            {
                Code = addRegionRequest.Code,
                Area = addRegionRequest.Area,
                Lat = addRegionRequest.Lat,
                Long = addRegionRequest.Long,
                Name = addRegionRequest.Name,
                Population = addRegionRequest.Population
            };
            //pass details to repository
            region=await regionRepository.AddAsync(region);
            //convert data back to the DTO
            var regionDTO = new Models.DTO.Region()
            {
                Code= region.Code,
                Area= region.Area,
                Lat = region.Lat,
                Long  = region.Long,
                Name = region.Name,
                Population = region.Population,
                Id= region.Id
            };
            return CreatedAtAction(nameof(GetRegionAsync), new { id = regionDTO.Id }, regionDTO);
        }
        [HttpDelete]
        [Route("{id:guid}")]
        public async Task<IActionResult>DeleteRegionAsync(Guid id)
        {
            //we need to get region from database
            var region=await regionRepository.DeleteAsync(id);
            //If null (not Found)
            if (region == null)
            {
                return NotFound();
            }
            //convert response back to DTO
            var regionDTO = new Models.DTO.Region
            {
                Code = region.Code,
                Area = region.Area,
                Lat = region.Lat,
                Long = region.Long,
                Name = region.Name,
                Population = region.Population,
                Id = region.Id

            };
            //return ok response
            return Ok(regionDTO);
        }
        [HttpPut]
        [Route("{id:guid}")]
        public async Task<IActionResult> UpdateRegionAsync([FromRoute]Guid id, [FromBody]Models.DTO.UpdateRegionRequest updateRegionRequest)
        {
            //validate incoming request
            if(!ValidateUpdateRegionAsync(updateRegionRequest))
            {
                return BadRequest(ModelState);
            }
            //convert DTO to Domain Model
            var region = new Models.Domain.Region
            {
                Code = updateRegionRequest.Code,
                Area = updateRegionRequest.Area,
                Lat = updateRegionRequest.Lat,
                Long = updateRegionRequest.Long,
                Name = updateRegionRequest.Name,
                Population = updateRegionRequest.Population,

            };
            //now we will update region using repository (now pass above region to repository)
            region = await regionRepository.UpdateAsync(id,region);
            //If it is null give not found
            if(region==null)
            {
                return NotFound();
            }
            //if it is not null convert domain to dto
            var regionDTO = new Models.DTO.Region
            {
                Id = region.Id,
                Name = region.Name,
                Code = region.Code,
                Area = region.Area,
                Lat = region.Lat,
                Long = region.Long,
                Population = region.Population
            };
            //return ok response
            return Ok(regionDTO);
        }
        #region Private methods
        private bool ValidateAddRegionAsync(Models.DTO.AddRegionRequest addRegionRequest)
        {
            if (addRegionRequest == null)
            {
                ModelState.AddModelError(nameof(addRegionRequest), $"Add region data is required.");
                return false;
            }
            if (string.IsNullOrWhiteSpace(addRegionRequest.Code))
            {
                //i want to tell client what went wrong in the request
                //so I use ModelState attribute provided by ASP.Net core.
                ModelState.AddModelError(nameof(addRegionRequest.Code), $"{nameof(addRegionRequest.Code)} cannot be Null or empty or White spaces. ");
                //client will get indication of what went wrong in the request why we got 400 bad request.

            }
            if (string.IsNullOrWhiteSpace(addRegionRequest.Name))
            {
                //i want to tell client what went wrong in the request
                //so I use ModelState attribute provided by ASP.Net core.
                ModelState.AddModelError(nameof(addRegionRequest.Name), $"{nameof(addRegionRequest.Name)} cannot be Null or empty or White spaces. ");
                //client will get indication of what went wrong in the request why we got 400 bad request.

            }
            if (addRegionRequest.Area <= 0)
            {
                ModelState.AddModelError(nameof(addRegionRequest.Area), $"{nameof(addRegionRequest.Area)} cannot be lessthan or equal to zero. ");
            }

           
            if (addRegionRequest.Population < 0)
            {
                ModelState.AddModelError(nameof(addRegionRequest.Population), $"{nameof(addRegionRequest.Population)} cannot be lessthan  zero. ");
            }
            if (ModelState.ErrorCount > 0)
            {
                return false;
            }
            return true;
        }
        private bool ValidateUpdateRegionAsync(Models.DTO.UpdateRegionRequest updateRegionRequest)
        {
            if (updateRegionRequest == null)
            {
                ModelState.AddModelError(nameof(updateRegionRequest), $"Update region data is required.");
                return false;
            }
            if (string.IsNullOrWhiteSpace(updateRegionRequest.Code))
            {
                //i want to tell client what went wrong in the request
                //so I use ModelState attribute provided by ASP.Net core.
                ModelState.AddModelError(nameof(updateRegionRequest.Code), $"{nameof(updateRegionRequest.Code)} cannot be Null or empty or White spaces. ");
                //client will get indication of what went wrong in the request why we got 400 bad request.

            }
            if (string.IsNullOrWhiteSpace(updateRegionRequest.Name))
            {
                //i want to tell client what went wrong in the request
                //so I use ModelState attribute provided by ASP.Net core.
                ModelState.AddModelError(nameof(updateRegionRequest.Name), $"{nameof(updateRegionRequest.Name)} cannot be Null or empty or White spaces. ");
                //client will get indication of what went wrong in the request why we got 400 bad request.

            }
            if (updateRegionRequest.Area <= 0)
            {
                ModelState.AddModelError(nameof(updateRegionRequest.Area), $"{nameof(updateRegionRequest.Area)} cannot be lessthan or equal to zero. ");
            }


            if (updateRegionRequest.Population < 0)
            {
                ModelState.AddModelError(nameof(updateRegionRequest.Population), $"{nameof(updateRegionRequest.Population)} cannot be lessthan  zero. ");
            }
            if (ModelState.ErrorCount > 0)
            {
                return false;
            }
            return true;
        }

            #endregion
    }
}
