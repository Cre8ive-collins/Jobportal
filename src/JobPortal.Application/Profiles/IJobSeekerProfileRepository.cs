using JobPortal.Domain.Entities;

namespace JobPortal.Application.Profiles;

public interface IJobSeekerProfileRepository
{
    Task<JobSeekerProfile?> GetByUserIdAsync(Guid userId);

    Task AddAsync(JobSeekerProfile profile);

    Task UpdateAsync(JobSeekerProfile profile);
}
