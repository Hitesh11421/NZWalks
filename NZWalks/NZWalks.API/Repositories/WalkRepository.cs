using Microsoft.EntityFrameworkCore;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;

namespace NZWalks.API.Repositories
{
    public class WalkRepository : IWalkRepository
    {
        private readonly NZWalksDbContext nZWalksDbContext;

        public WalkRepository(NZWalksDbContext nZWalksDbContext)
        {
            this.nZWalksDbContext = nZWalksDbContext;
        }

        public async Task<Walk> AddAsync(Walk walk)
        {
            //assign new id
            walk.Id=Guid.NewGuid();
            await nZWalksDbContext.Walks.AddAsync(walk);
            await nZWalksDbContext.SaveChangesAsync();
            return walk;
        }

        public async Task<Walk> DeleteAsync(Guid id)
        {
            //first checks if walks exists if it is delete that else sends null back
            var existingWalk=await nZWalksDbContext.Walks.FindAsync(id);
            if (existingWalk==null)
            {
                return null;
            }
            //if not null we need to delete it
            nZWalksDbContext.Walks.Remove(existingWalk);
            await nZWalksDbContext.SaveChangesAsync();
            return existingWalk;
        }

        public async Task<IEnumerable<Walk>> GetAllAsync()
        {
            return await nZWalksDbContext.Walks
                .Include(x=>x.Region)
                .Include(x=>x.WalkDifficulty)
                .ToListAsync();
        }

        public  Task<Walk> GetAsync(Guid id)
        {
            return  nZWalksDbContext.Walks
                .Include(x=>x.Region)
                .Include(x=>x.WalkDifficulty) 
                .FirstOrDefaultAsync(x=>x.Id==id);
        }

        public async Task<Walk> UpdateAsync(Guid id, Walk walk)
        {
            //now we use nZDbcontext to find walk inside database, if we find update details
            var existingWalk = await nZWalksDbContext.Walks.FindAsync(id);
            if (existingWalk!= null)
            {
                existingWalk.Length = walk.Length;
                existingWalk.Name= walk.Name;
                existingWalk.WalkDifficultyId = walk.WalkDifficultyId;
                existingWalk.RegionId = walk.RegionId;
                await nZWalksDbContext.SaveChangesAsync();
                return existingWalk;
            };
            return null;
        }
    }
}
