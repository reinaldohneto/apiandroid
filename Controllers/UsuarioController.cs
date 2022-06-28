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
        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.UniqueName, usuario.UserName)
        };

        var jwtKey = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")!.Equals("Production")
            ? Environment.GetEnvironmentVariable("TOKEN_CONFIGURATION_KEY")
            : _configuration.GetValue<string>("JwtKey");

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey ?? throw new ArgumentNullException("Key")));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);

        var expiration = DateTime.UtcNow.AddDays(14);

        JwtSecurityToken token = new JwtSecurityToken(
            issuer: Environment.GetEnvironmentVariable("TOKEN_ISSUER"),
            audience: null,
            claims: claims,
            expires: expiration,
            signingCredentials: creds);

        return new TokenViewModel
        {
            Token = new JwtSecurityTokenHandler().WriteToken(token),
            Duracao = expiration
        };
    }
}