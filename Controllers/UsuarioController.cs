using AppAndroid.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AppAndroid.Controllers;

[ApiController]
[Route("[controller]")]
public class UsuarioController : ControllerBase
{
    private readonly AppContext _context;
    private readonly UserManager<ApplicationUser> _userManager;

    public UsuarioController(AppContext context, UserManager<ApplicationUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }
    
    [HttpPost("cria-usuario")]
    public async Task<ActionResult<ApplicationUser>> CriaUsuario(
        ApplicationUserInputModel userInputModel)
    {
        var usuario = ObtemUsuario(userInputModel);
        
        var resultado = await _userManager.CreateAsync(usuario, userInputModel.Senha);

        if (resultado.Succeeded)
        {
            usuario = await _userManager.FindByEmailAsync(usuario.Email);
            return Ok(usuario);
        }

        return BadRequest();
    }

    private ApplicationUser ObtemUsuario(ApplicationUserInputModel inputModel)
    {
        return new ApplicationUser
        {
            Email = inputModel.Email,
            UserName = inputModel.Email
        };
    }
}