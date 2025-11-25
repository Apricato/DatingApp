using API.Entities;
using API.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using API.Mappers;
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

}