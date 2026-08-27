using JobPortal.Domain.Entities;

namespace JobPortal.Application.Jobs;

public interface IJobRepository
{
    Task<List<Job>> GetAllByEmployerAsync(Guid employerId);

    Task<(List<Job> Items, int TotalCount)> SearchPublishedAsync(
        JobSearchRequest request
    );

    Task<Job?> GetByIdAndEmployerAsync(Guid id, Guid employerId);

    Task<Job?> GetByIdAsync(Guid id);

    Task AddAsync(Job job);

    Task UpdateAsync(Job job);

    Task DeleteAsync(Job job);
}
