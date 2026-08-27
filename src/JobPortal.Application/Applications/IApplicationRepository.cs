using JobPortal.Domain.Entities;

namespace JobPortal.Application.Applications;

public interface IApplicationRepository
{
    Task<bool> ExistsAsync(Guid jobId, Guid applicantId);

    Task AddAsync(JobApplication application);

    Task<JobApplication?> GetByIdAsync(Guid jobId, Guid applicantId);

    Task<List<JobApplication>> GetAllByEmployerAsync(Guid employerId);

    Task<List<JobApplication>> GetAllByApplicantAsync(Guid applicantId);

    Task<JobApplication?> GetByIdAndEmployerAsync(
        Guid jobId,
        Guid applicantId,
        Guid employerId
    );

    Task UpdateAsync(JobApplication application);
}
