using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.Models.DTO;
using NZWalks.API.Repositories;

namespace NZWalks.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WalkDifficultiesController : Controller
    {
        private readonly IWalkDifficultyRepository walkDifficultyRepository;
        private readonly IMapper mapper;

        public WalkDifficultiesController(IWalkDifficultyRepository walkDifficultyRepository,IMapper mapper)
        {
            this.walkDifficultyRepository = walkDifficultyRepository;
            this.mapper = mapper;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllWalkDifficulties()
        {
            //return Ok(await walkDifficultyRepository.GetAllAsync());
            var walkDifficultiesDomain = await walkDifficultyRepository.GetAllAsync();
            //convert Domain to DTO
            var walkDifficultiesDTO=mapper.Map<List<Models.DTO.WalkDifficulty>>(walkDifficultiesDomain);
            return Ok(walkDifficultiesDTO);
        }
        [HttpGet]
        [Route("{id:guid}")]
        [ActionName("GetWalkDifficultyById")]
        public async Task<IActionResult>GetWalkDifficultyById(Guid id)
        {
            var walkDifficultiesDomain= await walkDifficultyRepository.GetAsync(id);
            if(walkDifficultiesDomain==null)
            {
                return NotFound();
            }
            //convert domain to dto
            var walkDifficultiesDTO=mapper.Map<Models.DTO.WalkDifficulty>(walkDifficultiesDomain);
            return Ok(walkDifficultiesDTO);
        }
        [HttpPost]
        public async Task<IActionResult>AddWalkDifficultAsync(Models.DTO.AddWalkDifficultyRequest addWalkDifficultyRequest)
        {
            //validate incoming response
            //if(!ValidateAddWalkDifficultAsync(addWalkDifficultyRequest))
            //{
            //    return BadRequest(ModelState);
            //}
            //convert DTO tp domain
            var walkDifficultyDomain = new Models.Domain.WalkDifficulty
            {
                Code = addWalkDifficultyRequest.Code
            };
            walkDifficultyDomain=await walkDifficultyRepository.AddAsync(walkDifficultyDomain);
            //convert domain to DTO
            var walkDifficultyDTO=mapper.Map<Models.DTO.WalkDifficulty> (walkDifficultyDomain);
            return CreatedAtAction(nameof(GetWalkDifficultyById), new { id = walkDifficultyDTO.Id }, walkDifficultyDTO);
        }
        [HttpPut]
        [Route("{id:guid}")]
        public async Task<IActionResult>UpdateWalkDifficultyAsync(Guid id,Models.DTO.UpdateWalkDifficultyRequest updateWalkDifficultyRequest)
        {
            ////validate incoming response
            //if (!ValidateUpdateWalkDifficultAsync(updateWalkDifficultyRequest))
            //{
            //    return BadRequest(ModelState);
            //}
            //convert DTO to domain
            var walkDifficultyDomain = new Models.Domain.WalkDifficulty
            {
                Code = updateWalkDifficultyRequest.Code
            };
            //call to repository to update
            walkDifficultyDomain=await walkDifficultyRepository.UpdateAsync(id, walkDifficultyDomain);
            //check if null
            if(walkDifficultyDomain==null) {
                return NotFound();
            }
            //convert Domain to DTO
            var walkDifficultyDTO = mapper.Map<Models.DTO.WalkDifficulty>(walkDifficultyDomain);
            //return response
            return Ok(walkDifficultyDTO);
        }
        [HttpDelete]
        [Route("{id:guid}")]
        public async Task<IActionResult>DeleteWalkDifficulty(Guid id)
        {
            //call to repository
            var walkDifficultyDomain=await walkDifficultyRepository.DeleteAsync(id);
            if(walkDifficultyDomain==null)
            {
                return NotFound();
            }
            //convert to DTO
            var walkDifficultyDTO = mapper.Map<Models.DTO.WalkDifficulty>(walkDifficultyDomain);
            //return response
            return Ok(walkDifficultyDTO);
        }
        #region Private methods
        private bool ValidateAddWalkDifficultAsync(Models.DTO.AddWalkDifficultyRequest addWalkDifficultyRequest)
        {
            if (addWalkDifficultyRequest == null)
            {
                ModelState.AddModelError(nameof(addWalkDifficultyRequest), $"Add WalkDifficulty data is required.");
                return false;
            }
            if (string.IsNullOrWhiteSpace(addWalkDifficultyRequest.Code))
            {
                //i want to tell client what went wrong in the request
                //so I use ModelState attribute provided by ASP.Net core.
                ModelState.AddModelError(nameof(addWalkDifficultyRequest.Code), $"{nameof(addWalkDifficultyRequest.Code)} cannot be Null or empty or White spaces. ");
                //client will get indication of what went wrong in the request why we got 400 bad request.

            }
            if (ModelState.ErrorCount > 0)
            {
                return false;
            }
            return true;
        }
        private bool ValidateUpdateWalkDifficultAsync(Models.DTO.UpdateWalkDifficultyRequest updateWalkDifficultyRequest)
        {
            if (updateWalkDifficultyRequest == null)
            {
                ModelState.AddModelError(nameof(updateWalkDifficultyRequest), $"Update WalkDifficulty data is required.");
                return false;
            }
            if (string.IsNullOrWhiteSpace(updateWalkDifficultyRequest.Code))
            {
                //i want to tell client what went wrong in the request
                //so I use ModelState attribute provided by ASP.Net core.
                ModelState.AddModelError(nameof(updateWalkDifficultyRequest.Code), $"{nameof(updateWalkDifficultyRequest.Code)} cannot be Null or empty or White spaces. ");
                //client will get indication of what went wrong in the request why we got 400 bad request.

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
