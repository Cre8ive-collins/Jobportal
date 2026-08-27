using JobPortal.Application.Applications;
using JobPortal.Application.Common.Exceptions;
using JobPortal.Domain.Entities;
using JobPortal.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace JobPortal.Infrastructure.Repositories;

public class ApplicationRepository : IApplicationRepository
{
    private readonly AppDbContext _dbContext;

    public ApplicationRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public Task<bool> ExistsAsync(Guid jobId, Guid applicantId)
    {
        return _dbContext.Applications.AnyAsync(application =>
            application.JobId == jobId &&
            application.ApplicantId == applicantId
        );
    }

    public async Task AddAsync(JobApplication application)
    {
        _dbContext.Applications.Add(application);

        try
        {
            await _dbContext.SaveChangesAsync();
        }
        catch (DbUpdateException exception)
            when (exception.InnerException is PostgresException
            {
                SqlState: PostgresErrorCodes.UniqueViolation
            })
        {
            throw new ConflictException(
                "You have already applied for this job."
            );
        }
    }

    public Task<JobApplication?> GetByIdAsync(Guid jobId, Guid applicantId)
    {
        return _dbContext.Applications
            .AsNoTracking()
            .Include(application => application.Job)
            .Include(application => application.Applicant)
            .FirstOrDefaultAsync(application =>
                application.JobId == jobId &&
                application.ApplicantId == applicantId
            );
    }

    public Task<List<JobApplication>> GetAllByEmployerAsync(Guid employerId)
    {
        return _dbContext.Applications
            .AsNoTracking()
            .Include(application => application.Job)
            .Include(application => application.Applicant)
            .Where(application => application.Job.EmployerId == employerId)
            .OrderByDescending(application => application.AppliedAtUtc)
            .ToListAsync();
    }

    public Task<JobApplication?> GetByIdAndEmployerAsync(
        Guid jobId,
        Guid applicantId,
        Guid employerId
    )
    {
        return _dbContext.Applications
            .Include(application => application.Job)
            .Include(application => application.Applicant)
            .FirstOrDefaultAsync(application =>
                application.JobId == jobId &&
                application.ApplicantId == applicantId &&
                application.Job.EmployerId == employerId
            );
    }

    public async Task UpdateAsync(JobApplication application)
    {
        _dbContext.Applications.Update(application);
        await _dbContext.SaveChangesAsync();
    }
}
