using JobPortal.Application.Common.Exceptions;
using JobPortal.Application.Jobs;
using JobPortal.Domain.Entities;
using JobPortal.Domain.Enums;

namespace JobPortal.Application.Applications;

public class ApplicationService
{
    private readonly IApplicationRepository _applicationRepository;
    private readonly IJobRepository _jobRepository;

    public ApplicationService(
        IApplicationRepository applicationRepository,
        IJobRepository jobRepository
    )
    {
        _applicationRepository = applicationRepository;
        _jobRepository = jobRepository;
    }

    public async Task<ApplicationResponse> ApplyAsync(
        Guid jobId,
        Guid applicantId
    )
    {
        var job = await _jobRepository.GetByIdAsync(jobId)
            ?? throw new NotFoundException("Job not found.");

        if (job.Status != JobStatus.Published || job.Deadline <= DateTime.UtcNow)
        {
            throw new ConflictException(
                "This job is not accepting applications."
            );
        }

        if (await _applicationRepository.ExistsAsync(jobId, applicantId))
        {
            throw new ConflictException(
                "You have already applied for this job."
            );
        }

        var now = DateTime.UtcNow;
        var application = new JobApplication
        {
            JobId = jobId,
            ApplicantId = applicantId,
            Status = ApplicationStatus.Open,
            AppliedAtUtc = now,
            UpdatedAtUtc = now
        };

        await _applicationRepository.AddAsync(application);

        var savedApplication = await _applicationRepository.GetByIdAsync(
            jobId,
            applicantId
        );

        return MapToResponse(savedApplication!);
    }

    public async Task<List<ApplicationResponse>> GetAllByEmployerAsync(
        Guid employerId
    )
    {
        var applications = await _applicationRepository
            .GetAllByEmployerAsync(employerId);

        return applications.Select(MapToResponse).ToList();
    }

    public async Task<ApplicationResponse> UpdateStatusAsync(
        Guid jobId,
        Guid applicantId,
        UpdateApplicationStatusRequest request,
        Guid employerId
    )
    {
        var application = await _applicationRepository
            .GetByIdAndEmployerAsync(jobId, applicantId, employerId)
            ?? throw new NotFoundException("Application not found.");

        application.Status = request.Status!.Value;
        application.UpdatedAtUtc = DateTime.UtcNow;

        await _applicationRepository.UpdateAsync(application);
        return MapToResponse(application);
    }

    private static ApplicationResponse MapToResponse(
        JobApplication application
    )
    {
        return new ApplicationResponse
        {
            JobId = application.JobId,
            JobTitle = application.Job.Title,
            ApplicantId = application.ApplicantId,
            ApplicantName = application.Applicant.FullName,
            ApplicantEmail = application.Applicant.Email,
            Status = application.Status,
            AppliedAtUtc = application.AppliedAtUtc,
            UpdatedAtUtc = application.UpdatedAtUtc
        };
    }
}
