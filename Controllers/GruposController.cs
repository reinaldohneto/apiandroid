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
    
    [HttpPut("{id:guid}")]
    public async Task<ActionResult<Grupo?>> AtualizaGrupo(
        Guid id, GrupoInputViewModel? grupo)
    {
        var grupoDomain = await _context.Grupos.FindAsync(id);

        if (grupoDomain == null)
            return NotFound();
        
        grupoDomain.Nome = grupo.Nome;

        _context.Grupos.Update(grupoDomain);
        await _context.SaveChangesAsync();
        return Ok(grupoDomain);   
    }
    
    [HttpGet]
    public async Task<ActionResult<ICollection<Grupo?>>> ListarGrupos()
        => Ok(await _context.Grupos.ToListAsync());
    
    [HttpPost("{id:guid}")]
    public async Task<ActionResult<Grupo?>> InscreverGrupo(Guid id, GrupoViewModel viewModel)
    {
        var value = User.FindFirst(i => i.Type == ClaimTypes.NameIdentifier)?.Value;

        var usuario = await _userManager.FindByIdAsync(value);

        usuario.GrupoId = id;
        await _userManager.UpdateAsync(usuario);
        
        await _context.SaveChangesAsync();
        return Ok(new
        {
            Id = id
        });   
    }

    [HttpGet("ranking")]
    public async Task<ActionResult<ICollection<GrupoRankingViewModel>>> ObterRanking()
    {
        var grupos = await _context.Grupos
            .Include(g => g.Usuarios)!
                .ThenInclude(u => u.Localizacoes)
            .AsNoTracking()
            .OrderByDescending(t => t.Usuarios!.Sum(t => t.Localizacoes.Count))
            .ToListAsync();

        var listRanking = new List<GrupoRankingViewModel>();
        int i = 1;
        grupos.ForEach(t =>
        {
            listRanking.Add(new GrupoRankingViewModel
            {
                Colocacao = i,
                NomeGrupo = t.Nome,
                Quantidade = t.Usuarios!.Sum(t => t.Localizacoes.Count)
            });
            i += 1;
        });

        return listRanking;
    }
}