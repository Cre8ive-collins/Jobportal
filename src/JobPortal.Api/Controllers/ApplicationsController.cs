using System.IdentityModel.Tokens.Jwt;
using JobPortal.Application.Applications;
using JobPortal.Application.Common.Exceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JobPortal.Api.Controllers;

[ApiController]
[Authorize]
[Route("api/applications")]
public class ApplicationsController : ControllerBase
{
    private readonly ApplicationService _applicationService;

    public ApplicationsController(ApplicationService applicationService)
    {
        _applicationService = applicationService;
    }

    [HttpPost("/api/jobs/{jobId:guid}/applications")]
    [Authorize(Policy = "JobSeekerOnly")]
    [EndpointSummary("Apply for a job")]
    [EndpointDescription(
        "Creates an Open application for the authenticated job seeker. The " +
        "job must be published, its deadline must not have passed, and the " +
        "job seeker must not have already applied."
    )]
    [ProducesResponseType(
        typeof(ApplicationResponse),
        StatusCodes.Status201Created
    )]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<ActionResult<ApplicationResponse>> Apply(Guid jobId)
    {
        var application = await _applicationService.ApplyAsync(
            jobId,
            GetUserId()
        );

        return StatusCode(StatusCodes.Status201Created, application);
    }

    [HttpGet]
    [Authorize(Policy = "EmployerOnly")]
    [EndpointSummary("Get all applicants")]
    [EndpointDescription(
        "Returns applications and applicant details across every job owned by " +
        "the authenticated employer, ordered by most recent application."
    )]
    [ProducesResponseType(
        typeof(List<ApplicationResponse>),
        StatusCodes.Status200OK
    )]
    public async Task<ActionResult<List<ApplicationResponse>>> GetAll()
    {
        return Ok(
            await _applicationService.GetAllByEmployerAsync(GetUserId())
        );
    }

    [HttpGet("my-applications")]
    [Authorize(Policy = "JobSeekerOnly")]
    [EndpointSummary("Get my applications")]
    [EndpointDescription(
        "Returns every application submitted by the authenticated job seeker, " +
        "ordered by most recent application."
    )]
    [ProducesResponseType(
        typeof(List<ApplicationResponse>),
        StatusCodes.Status200OK
    )]
    public async Task<ActionResult<List<ApplicationResponse>>> GetMine()
    {
        return Ok(
            await _applicationService.GetAllByApplicantAsync(GetUserId())
        );
    }

    [HttpPatch("{jobId:guid}/{applicantId:guid}/status")]
    [Authorize(Policy = "EmployerOnly")]
    [EndpointSummary("Update an application status")]
    [EndpointDescription(
        "Updates an application for a job owned by the authenticated employer. " +
        "Allowed statuses are Open, Hired, Rejected, Reviewed, Interviewing, " +
        "and Withdrawn."
    )]
    [ProducesResponseType(
        typeof(ApplicationResponse),
        StatusCodes.Status200OK
    )]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApplicationResponse>> UpdateStatus(
        Guid jobId,
        Guid applicantId,
        UpdateApplicationStatusRequest request
    )
    {
        return Ok(
            await _applicationService.UpdateStatusAsync(
                jobId,
                applicantId,
                request,
                GetUserId()
            )
        );
    }

    private Guid GetUserId()
    {
        var subject = User.FindFirst(JwtRegisteredClaimNames.Sub)?.Value;

        if (!Guid.TryParse(subject, out var userId))
        {
            throw new UnauthorizedException("The token subject is invalid.");
        }

        return userId;
    }
}
