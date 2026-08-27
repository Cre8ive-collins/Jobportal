using JobPortal.Application.Profiles;
using JobPortal.Domain.Entities;
using JobPortal.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace JobPortal.Infrastructure.Repositories;

public class JobSeekerProfileRepository : IJobSeekerProfileRepository
{
    private readonly AppDbContext _dbContext;

    public JobSeekerProfileRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public Task<JobSeekerProfile?> GetByUserIdAsync(Guid userId)
    {
        return _dbContext.JobSeekerProfiles.FirstOrDefaultAsync(profile =>
            profile.UserId == userId
        );
    }

    public async Task AddAsync(JobSeekerProfile profile)
    {
        _dbContext.JobSeekerProfiles.Add(profile);
        await _dbContext.SaveChangesAsync();
    }

    public async Task UpdateAsync(JobSeekerProfile profile)
    {
        _dbContext.JobSeekerProfiles.Update(profile);
        await _dbContext.SaveChangesAsync();
    }
}
