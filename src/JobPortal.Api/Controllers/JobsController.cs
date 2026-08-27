using System.IdentityModel.Tokens.Jwt;
using JobPortal.Application.Common.Exceptions;
using JobPortal.Application.Jobs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JobPortal.Api.Controllers;

[ApiController]
[Authorize]
[Route("api/jobs")]
public class JobsController : ControllerBase
{
    private readonly JobService _jobService;

    public JobsController(JobService jobService)
    {
        _jobService = jobService;
    }

    [HttpGet("search")]
    [EndpointSummary("Search published jobs")]
    [EndpointDescription(
        "Returns a paginated list of published jobs. Results can be filtered " +
        "by search text, category, employment type, and experience level."
    )]
    [ProducesResponseType(
        typeof(PaginatedJobsResponse),
        StatusCodes.Status200OK
    )]
    public async Task<ActionResult<PaginatedJobsResponse>> Search(
        [FromQuery] JobSearchRequest request
    )
    {
        return Ok(await _jobService.SearchAsync(request));
    }

    [HttpGet]
    [Authorize(Policy = "EmployerOnly")]
    [EndpointSummary("Get the employer's jobs")]
    [EndpointDescription(
        "Returns every job created by the authenticated employer, including " +
        "draft, published, and closed jobs."
    )]
    [ProducesResponseType(typeof(List<JobResponse>), StatusCodes.Status200OK)]
    public async Task<ActionResult<List<JobResponse>>> GetAll()
    {
        return Ok(await _jobService.GetAllAsync(GetEmployerId()));
    }

    [HttpGet("{id:guid}")]
    [Authorize(Policy = "EmployerOnly")]
    [EndpointSummary("Get an employer job")]
    [EndpointDescription(
        "Returns one job when it belongs to the authenticated employer."
    )]
    [ProducesResponseType(typeof(JobResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<JobResponse>> GetById(Guid id)
    {
        return Ok(await _jobService.GetByIdAsync(id, GetEmployerId()));
    }

    [HttpPost]
    [Authorize(Policy = "EmployerOnly")]
    [EndpointSummary("Create a job")]
    [EndpointDescription(
        "Creates a job owned by the authenticated employer using the supplied " +
        "details, category, status, and application deadline."
    )]
    [ProducesResponseType(typeof(JobResponse), StatusCodes.Status201Created)]
    public async Task<ActionResult<JobResponse>> Create(CreateJobRequest request)
    {
        var job = await _jobService.CreateAsync(request, GetEmployerId());

        return CreatedAtAction(
            nameof(GetById),
            new { id = job.Id },
            job
        );
    }

    [HttpPut("{id:guid}")]
    [Authorize(Policy = "EmployerOnly")]
    [EndpointSummary("Update a job")]
    [EndpointDescription(
        "Replaces the editable details of a job owned by the authenticated " +
        "employer, including its publication status."
    )]
    [ProducesResponseType(typeof(JobResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<JobResponse>> Update(
        Guid id,
        UpdateJobRequest request
    )
    {
        return Ok(
            await _jobService.UpdateAsync(id, request, GetEmployerId())
        );
    }

    [HttpDelete("{id:guid}")]
    [Authorize(Policy = "EmployerOnly")]
    [EndpointSummary("Delete a job")]
    [EndpointDescription(
        "Permanently deletes a job owned by the authenticated employer."
    )]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _jobService.DeleteAsync(id, GetEmployerId());
        return NoContent();
    }

    private Guid GetEmployerId()
    {
        var subject = User.FindFirst(JwtRegisteredClaimNames.Sub)?.Value;

        if (!Guid.TryParse(subject, out var employerId))
        {
            throw new UnauthorizedException("The token subject is invalid.");
        }

        return employerId;
    }
}
