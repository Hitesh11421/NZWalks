using AutoMapper;
using Microsoft.AspNetCore.Mvc;
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
    }
}
