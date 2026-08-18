using JobPortal.Application.Jobs;
using JobPortal.Domain.Entities;
using JobPortal.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace JobPortal.Infrastructure.Repositories;

public class JobRepository : IJobRepository
{
    private readonly AppDbContext _dbContext;

    public JobRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public Task<List<Job>> GetAllByEmployerAsync(Guid employerId)
    {
        return _dbContext.Jobs
            .AsNoTracking()
            .Include(job => job.Category)
            .Where(job => job.EmployerId == employerId)
            .OrderByDescending(job => job.CreatedAtUtc)
            .ToListAsync();
    }

    public Task<Job?> GetByIdAndEmployerAsync(Guid id, Guid employerId)
    {
        return _dbContext.Jobs
            .Include(job => job.Category)
            .FirstOrDefaultAsync(job =>
                job.Id == id && job.EmployerId == employerId
            );
    }

    public async Task AddAsync(Job job)
    {
        _dbContext.Jobs.Add(job);
        await _dbContext.SaveChangesAsync();
    }

    public async Task UpdateAsync(Job job)
    {
        _dbContext.Jobs.Update(job);
        await _dbContext.SaveChangesAsync();
    }

    public async Task DeleteAsync(Job job)
    {
        _dbContext.Jobs.Remove(job);
        await _dbContext.SaveChangesAsync();
    }
}
