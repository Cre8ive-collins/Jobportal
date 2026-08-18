using System.IdentityModel.Tokens.Jwt;
using JobPortal.Application.Common.Exceptions;
using JobPortal.Application.Jobs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JobPortal.Api.Controllers;

[ApiController]
[Authorize(Policy = "EmployerOnly")]
[Route("api/jobs")]
public class JobsController : ControllerBase
{
    private readonly JobService _jobService;

    public JobsController(JobService jobService)
    {
        _jobService = jobService;
    }

    [HttpGet]
    [ProducesResponseType(typeof(List<JobResponse>), StatusCodes.Status200OK)]
    public async Task<ActionResult<List<JobResponse>>> GetAll()
    {
        return Ok(await _jobService.GetAllAsync(GetEmployerId()));
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(JobResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<JobResponse>> GetById(Guid id)
    {
        return Ok(await _jobService.GetByIdAsync(id, GetEmployerId()));
    }

    [HttpPost]
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
