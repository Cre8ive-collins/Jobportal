using JobPortal.Application.Common.Exceptions;
using JobPortal.Application.Users;
using JobPortal.Domain.Entities;

namespace JobPortal.Application.Profiles;

public class JobSeekerProfileService
{
    private readonly IJobSeekerProfileRepository _profileRepository;
    private readonly IUserRepository _userRepository;

    public JobSeekerProfileService(
        IJobSeekerProfileRepository profileRepository,
        IUserRepository userRepository
    )
    {
        _profileRepository = profileRepository;
        _userRepository = userRepository;
    }

    public async Task<JobSeekerProfileResponse> GetAsync(Guid userId)
    {
        var user = await GetUserAsync(userId);
        var profile = await _profileRepository.GetByUserIdAsync(userId);

        return MapToResponse(user, profile);
    }

    public async Task<JobSeekerProfileResponse> UpdateAsync(
        Guid userId,
        UpdateJobSeekerProfileRequest request
    )
    {
        var user = await GetUserAsync(userId);
        var profile = await _profileRepository.GetByUserIdAsync(userId);
        var now = DateTime.UtcNow;

        if (profile is null)
        {
            profile = new JobSeekerProfile
            {
                UserId = userId,
                Skills = request.Skills.Trim(),
                Education = request.Education.Trim(),
                Experience = request.Experience.Trim(),
                Headline = request.Headline.Trim(),
                UpdatedAtUtc = now
            };

            await _profileRepository.AddAsync(profile);
        }
        else
        {
            profile.Skills = request.Skills.Trim();
            profile.Education = request.Education.Trim();
            profile.Experience = request.Experience.Trim();
            profile.Headline = request.Headline.Trim();
            profile.UpdatedAtUtc = now;

            await _profileRepository.UpdateAsync(profile);
        }

        return MapToResponse(user, profile);
    }

    private async Task<User> GetUserAsync(Guid userId)
    {
        return await _userRepository.GetByIdAsync(userId)
            ?? throw new NotFoundException("User not found.");
    }

    private static JobSeekerProfileResponse MapToResponse(
        User user,
        JobSeekerProfile? profile
    )
    {
        return new JobSeekerProfileResponse
        {
            UserId = user.Id,
            FullName = user.FullName,
            Email = user.Email,
            Skills = profile?.Skills ?? string.Empty,
            Education = profile?.Education ?? string.Empty,
            Experience = profile?.Experience ?? string.Empty,
            Headline = profile?.Headline ?? string.Empty,
            CvUrl = profile?.CvUrl,
            UpdatedAtUtc = profile?.UpdatedAtUtc
        };
    }
}
