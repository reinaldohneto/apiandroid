using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AppAndroid.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using JwtRegisteredClaimNames = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames;

namespace AppAndroid.Controllers;

[ApiController]
[Route("[controller]")]
public class UsuarioController : ControllerBase
{
    private readonly AppContext _context;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly IConfiguration _configuration;
    public UsuarioController(AppContext context, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IConfiguration configuration)
    {
        _context = context;
        _userManager = userManager;
        _signInManager = signInManager;
        _configuration = configuration;
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

    [HttpPost("login")]
    public async Task<ActionResult<TokenViewModel>> EfetuaLogin(
        ApplicationUserLoginInputModel userInputModel)
    {
        var resultado = await _signInManager.PasswordSignInAsync(userInputModel.Email, userInputModel.Senha, 
            false, false);

        if (resultado.Succeeded)
        {
            var usuario = await _userManager.FindByEmailAsync(userInputModel.Email);

            if (usuario != null)
                return GeraToken(usuario);

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
    
    private TokenViewModel GeraToken(ApplicationUser usuario)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(Environment.GetEnvironmentVariable("TOKEN_CONFIGURATION_KEY") ?? string.Empty);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new Claim[]
            {
                new(ClaimTypes.NameIdentifier, usuario.Id)
            }),
            Expires = DateTime.UtcNow.AddDays(14),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };

         var security = tokenHandler.CreateToken(tokenDescriptor);
         var token = tokenHandler.WriteToken(security);
         
        return new TokenViewModel
        {
            Token = token,
            Duracao = tokenDescriptor.Expires
        };
    }
}