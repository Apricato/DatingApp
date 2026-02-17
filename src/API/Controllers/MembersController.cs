using API.Entities;
using API.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using API.Mappers;
using System.Security.Claims;
using API.DTOs;

namespace API.Controllers;

[Authorize]
public class MembersController(IMembersRepository membersRepository) : BaseApiController
{
    [HttpGet]
    public async Task<IReadOnlyList<Member>> GetMembers()
    {
        return await membersRepository.GetMembersAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Member>> GetMember(string id)
    {
        var member = await membersRepository.GetMemberAsync(id);

        if (member == null)
        {
            return NotFound();
        }
        return member.ToResponse();
    }

    [HttpGet("{id}/photos")]
    public async Task<ActionResult<IReadOnlyList<Member>>> GetPhotos(string id)
    {
        return Ok(await membersRepository.GetPhotosAsync(id));
    }

    [HttpPut]
    public async Task<ActionResult> UpdateMember(MemberUpdateRequest request)
    {
        var memberId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (memberId == null)
        {
            return BadRequest("No id found in token");
        }

        var member = await membersRepository.GetMemberAsync(memberId);

        if (member == null)
        {
            return BadRequest("Failed to get member");
        }

        member.DisplayName = request.DisplayName ?? member.DisplayName;
        member.DisplayName = string.IsNullOrEmpty(request.DisplayName) ? member.DisplayName : request.DisplayName;
        member.Description = request.Description ?? member.Description;
        member.City = request.City ?? member.City;
        member.Country = request.Country ?? member.Country;

        membersRepository.Update(member);

        if (await membersRepository.SaveAllAsync())
        {
            return NoContent();
        }

        return BadRequest("Failed to update profile");
    }
}