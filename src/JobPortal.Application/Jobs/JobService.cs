using JobPortal.Application.Categories;
using JobPortal.Application.Common.Exceptions;
using JobPortal.Domain.Entities;

namespace JobPortal.Application.Jobs;

public class JobService
{
    private readonly IJobRepository _jobRepository;
    private readonly ICategoryRepository _categoryRepository;

    public JobService(
        IJobRepository jobRepository,
        ICategoryRepository categoryRepository
    )
    {
        _jobRepository = jobRepository;
        _categoryRepository = categoryRepository;
    }

    public async Task<List<JobResponse>> GetAllAsync(Guid employerId)
    {
        var jobs = await _jobRepository.GetAllByEmployerAsync(employerId);
        return jobs.Select(MapToResponse).ToList();
    }

    public async Task<JobResponse> GetByIdAsync(Guid id, Guid employerId)
    {
        var job = await GetOwnedJobAsync(id, employerId);
        return MapToResponse(job);
    }

    public async Task<JobResponse> CreateAsync(
        CreateJobRequest request,
        Guid employerId
    )
    {
        await EnsureCategoryExistsAsync(request.CategoryId!.Value);

        var now = DateTime.UtcNow;
        var job = new Job
        {
            Id = Guid.NewGuid(),
            EmployerId = employerId,
            CreatedAtUtc = now,
            UpdatedAtUtc = now
        };

        ApplyRequest(job, request);
        await _jobRepository.AddAsync(job);

        return await GetByIdAsync(job.Id, employerId);
    }

    public async Task<JobResponse> UpdateAsync(
        Guid id,
        UpdateJobRequest request,
        Guid employerId
    )
    {
        var job = await GetOwnedJobAsync(id, employerId);
        await EnsureCategoryExistsAsync(request.CategoryId!.Value);

        ApplyRequest(job, request);
        job.UpdatedAtUtc = DateTime.UtcNow;

        await _jobRepository.UpdateAsync(job);
        return await GetByIdAsync(job.Id, employerId);
    }

    public async Task DeleteAsync(Guid id, Guid employerId)
    {
        var job = await GetOwnedJobAsync(id, employerId);
        await _jobRepository.DeleteAsync(job);
    }

    private async Task<Job> GetOwnedJobAsync(Guid id, Guid employerId)
    {
        return await _jobRepository.GetByIdAndEmployerAsync(id, employerId)
            ?? throw new NotFoundException("Job not found.");
    }

    private async Task EnsureCategoryExistsAsync(Guid categoryId)
    {
        if (!await _categoryRepository.ExistsAsync(categoryId))
        {
            throw new NotFoundException("Category not found.");
        }
    }

    private static void ApplyRequest(Job job, JobRequest request)
    {
        job.Title = request.Title.Trim();
        job.Description = request.Description.Trim();
        job.Requirements = request.Requirements.Trim();
        job.Location = request.Location.Trim();
        job.CategoryId = request.CategoryId!.Value;
        job.EmploymentType = request.EmploymentType!.Value;
        job.ExperienceLevel = request.ExperienceLevel!.Value;
        job.Salary = request.Salary;
        job.Deadline = NormalizeToUtc(request.Deadline!.Value);
        job.Status = request.Status!.Value;
    }

    private static DateTime NormalizeToUtc(DateTime value)
    {
        return value.Kind switch
        {
            DateTimeKind.Utc => value,
            DateTimeKind.Local => value.ToUniversalTime(),
            _ => DateTime.SpecifyKind(value, DateTimeKind.Utc)
        };
    }

    private static JobResponse MapToResponse(Job job)
    {
        return new JobResponse
        {
            Id = job.Id,
            Title = job.Title,
            Description = job.Description,
            Requirements = job.Requirements,
            Location = job.Location,
            CategoryId = job.CategoryId,
            Category = new CategoryResponse
            {
                Id = job.Category.Id,
                Name = job.Category.Name
            },
            EmploymentType = job.EmploymentType,
            ExperienceLevel = job.ExperienceLevel,
            Salary = job.Salary,
            Deadline = job.Deadline,
            Status = job.Status,
            EmployerId = job.EmployerId,
            CreatedAtUtc = job.CreatedAtUtc,
            UpdatedAtUtc = job.UpdatedAtUtc
        };
    }
}
