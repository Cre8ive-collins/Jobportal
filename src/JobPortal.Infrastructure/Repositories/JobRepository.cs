using JobPortal.Application.Jobs;
using JobPortal.Domain.Entities;
using JobPortal.Domain.Enums;
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

    public async Task<(List<Job> Items, int TotalCount)> SearchPublishedAsync(
        JobSearchRequest request
    )
    {
        var query = _dbContext.Jobs
            .AsNoTracking()
            .Include(job => job.Category)
            .Where(job => job.Status == JobStatus.Published);

        if (!string.IsNullOrWhiteSpace(request.Search))
        {
            var pattern = $"%{request.Search}%";
            query = query.Where(job =>
                EF.Functions.ILike(job.Title, pattern) ||
                EF.Functions.ILike(job.Description, pattern) ||
                EF.Functions.ILike(job.Requirements, pattern) ||
                EF.Functions.ILike(job.Location, pattern)
            );
        }

        if (request.CategoryId.HasValue)
        {
            query = query.Where(job =>
                job.CategoryId == request.CategoryId.Value
            );
        }

        if (request.EmploymentType.HasValue)
        {
            query = query.Where(job =>
                job.EmploymentType == request.EmploymentType.Value
            );
        }

        if (request.ExperienceLevel.HasValue)
        {
            query = query.Where(job =>
                job.ExperienceLevel == request.ExperienceLevel.Value
            );
        }

        var totalCount = await query.CountAsync();
        var items = await query
            .OrderByDescending(job => job.CreatedAtUtc)
            .Skip((request.Page - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToListAsync();

        return (items, totalCount);
    }

    public Task<Job?> GetByIdAndEmployerAsync(Guid id, Guid employerId)
    {
        return _dbContext.Jobs
            .Include(job => job.Category)
            .FirstOrDefaultAsync(job =>
                job.Id == id && job.EmployerId == employerId
            );
    }

    public Task<Job?> GetByIdAsync(Guid id)
    {
        return _dbContext.Jobs
            .AsNoTracking()
            .FirstOrDefaultAsync(job => job.Id == id);
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
