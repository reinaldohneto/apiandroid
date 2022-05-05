using AppAndroid.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AppAndroid.Controllers;

[ApiController]
[Route("[controller]")]
public class LocalizacoesController : ControllerBase
{
    private readonly AppContext _context;


    public LocalizacoesController(AppContext context)
    {
        _context = context;
    }

    [HttpPost]
    public async Task<ActionResult<Localizacao?>> CriarLocalizacao(
        Localizacao? localizacao)
        => Ok((await _context.Localizacoes.AddAsync(localizacao)).Entity);

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
        
        return Ok(registro);
    }
}