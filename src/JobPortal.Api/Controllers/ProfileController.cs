using System.IdentityModel.Tokens.Jwt;
using JobPortal.Application.Common.Exceptions;
using JobPortal.Application.Profiles;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JobPortal.Api.Controllers;

[ApiController]
[Authorize(Policy = "JobSeekerOnly")]
[Route("api/profile")]
public class ProfileController : ControllerBase
{
    private readonly JobSeekerProfileService _profileService;

    public ProfileController(JobSeekerProfileService profileService)
    {
        _profileService = profileService;
    }

    [HttpGet]
    [EndpointSummary("Get my job-seeker profile")]
    [EndpointDescription(
        "Returns the authenticated job seeker's profile. Profile fields are " +
        "empty until the profile is updated for the first time."
    )]
    [ProducesResponseType(
        typeof(JobSeekerProfileResponse),
        StatusCodes.Status200OK
    )]
    public async Task<ActionResult<JobSeekerProfileResponse>> Get()
    {
        return Ok(await _profileService.GetAsync(GetUserId()));
    }

    [HttpPut]
    [EndpointSummary("Update my job-seeker profile")]
    [EndpointDescription(
        "Creates or replaces the authenticated job seeker's skills, " +
        "education, experience, and headline. CV upload is handled separately."
    )]
    [ProducesResponseType(
        typeof(JobSeekerProfileResponse),
        StatusCodes.Status200OK
    )]
    public async Task<ActionResult<JobSeekerProfileResponse>> Update(
        UpdateJobSeekerProfileRequest request
    )
    {
        return Ok(
            await _profileService.UpdateAsync(GetUserId(), request)
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
