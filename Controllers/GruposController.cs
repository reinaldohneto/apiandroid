using System.Security.Claims;
using AppAndroid.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AppAndroid.Controllers;

[ApiController]
[Authorize]
[Route("[controller]")]
public class GruposController : ControllerBase
{
    private readonly AppContext _context;
    private readonly UserManager<ApplicationUser> _userManager;


    public GruposController(AppContext context, UserManager<ApplicationUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }
    
    
    [HttpPost]
    public async Task<ActionResult<Grupo?>> CriarGrupo(
        GrupoInputViewModel? grupo)
    {
        var grupoDomain = new Grupo
        {
            Nome = grupo.Nome
        };

        await _context.Grupos.AddAsync(grupoDomain);
        await _context.SaveChangesAsync();
        return Ok(grupoDomain);   
    }
    
    [HttpGet]
    public async Task<ActionResult<ICollection<Grupo?>>> ListarGrupos()
        => Ok(await _context.Grupos.ToListAsync());
    
    [HttpPost("{id:guid}")]
    public async Task<ActionResult<Grupo?>> InscreverGrupo(Guid id)
    {
        var value = User.FindFirst(i => i.Type == ClaimTypes.NameIdentifier)?.Value;

        var usuario = await _userManager.FindByIdAsync(value);

        usuario.GrupoId = id;
        await _userManager.UpdateAsync(usuario);
        
        await _context.SaveChangesAsync();
        return Ok();   
    }
}