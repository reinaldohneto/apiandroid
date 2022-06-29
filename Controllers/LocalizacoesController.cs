using System.Security.Claims;
using AppAndroid.Model;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AppAndroid.Controllers;

[ApiController]
[Route("[controller]")]
[Authorize]
public class LocalizacoesController : ControllerBase
{
    private readonly AppContext _context;


    public LocalizacoesController(AppContext context)
    {
        _context = context;
    }

    [HttpPost]
    public async Task<ActionResult<Localizacao?>> CriarLocalizacao(
        LocalizacaoInputModel? localizacao)
    {
        var value = User.FindFirst(i => i.Type == ClaimTypes.NameIdentifier)?.Value;
        var localizacaoDomain = new Localizacao
        {
            Base64 = localizacao.Base64,
            Descricao = localizacao.Descricao,
            Latitude = localizacao.Latitude,
            Longitude = localizacao.Longitude,
            Titulo = localizacao.Titulo,
            UsuarioId = value
        };

        await _context.Localizacoes.AddAsync(localizacaoDomain);
        await _context.SaveChangesAsync();
        return Ok(localizacaoDomain);   
    }

    [HttpGet]
    public async Task<ActionResult<ICollection<Localizacao?>>> ListarLocalizacoes()
        => Ok(await _context.Localizacoes.ToListAsync());

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<Localizacao?>> BuscarLocalizacaoPorId(Guid id)
    {
        var registro = await _context.Localizacoes.FirstOrDefaultAsync(l => l.Id.Equals(id));

        if (registro != null)
            return Ok(registro);

        return NotFound();
    }

    [HttpDelete("{id:guid}")]
    public async Task<ActionResult> DeletarLocalizacao(Guid id)
    {
        var registro = await _context.Localizacoes.FirstOrDefaultAsync(l => l.Id.Equals(id));

        if (registro == null)
            return NotFound();

        _context.Localizacoes.Remove(registro);

        await _context.SaveChangesAsync();
        
        return NoContent();
    }
    
    [HttpPut("{id:guid}")]
    public async Task<ActionResult<Localizacao>> AtualizarLocalizacao(Guid id, [FromBody] Localizacao localizacaoApp)
    {
        var registro = await _context.Localizacoes.FirstOrDefaultAsync(l => l.Id.Equals(id));

        if (registro == null)
            return NotFound();

        registro.AtualizarLocalizacao(localizacaoApp);

        _context.Localizacoes.Update(registro);
        await _context.SaveChangesAsync();
        
        return Ok(registro);
    }
}