using AppAndroid.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AppAndroid.Controllers;

[ApiController]
[Authorize]
[Route("[controller]")]
public class GruposController : ControllerBase
{
    private readonly AppContext _context;


    public GruposController(AppContext context)
    {
        _context = context;
    }
    
    
    [HttpPost]
    public async Task<ActionResult<Grupo?>> CriarLocalizacao(
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


}