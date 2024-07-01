using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TO_DO___API.Data.Dto;
using TO_DO___API.Data;
using TO_DO___API.Services;

namespace TO_DO___API.Controllers;

[ApiController]
[Route("[controller]")]
public class UsuariosController : ControllerBase
{
    private UsuariosService _usuarioService;
    private IMapper _mapper;
    private UsuariosContext _context;


    public UsuariosController(UsuariosService usuarioService,
        IMapper mapper, UsuariosContext context)
    {
        _usuarioService = usuarioService;
        _mapper = mapper;
        _context = context;
    }

    [HttpPost("cadastro")]
    public async Task<IActionResult> CadastroAsync(CreateUsuarioDto cadastroDto)
    {
        try
        {
            await _usuarioService.RegisterUser(cadastroDto);
            return Ok("Usuário cadastrado com sucesso!");
        }
        catch (Exception ex)
        {
            return BadRequest($"Erro 400.. verifique seus dados..{ex}");
        }

    }


    [HttpPost("login")]
    public async Task<IActionResult> LoginAsync(LoginUsuarioDto loginDto)
    {
        try
        {
            var token = await _usuarioService.LoginUser(loginDto);

            if (token == null)
            {
                Unauthorized("Token nulo ou inválido..");
            }

            return Ok(token);

        }
        catch (Exception ex)
        {
            return Unauthorized($"Acesso não autorizado, seus dados de acesso..{ex}");
        }


    }

    [HttpPost("reset-password")]
    public async Task<IActionResult> ResetPasswordAsync(string email)
    {
        try
        {
            email = email.ToLower();
            await _usuarioService.PasswordReset(email);
            return Ok("Email de recuperação de senha enviado com sucesso!");
        }
        catch (Exception ex)
        {
            return BadRequest($"Erro 400.. verifique se seu email foi digitado corretamente..{ex}");
        }
    }

    [HttpPost("reset-password-confirm")]
    public async Task<IActionResult> ResetPasswordConfirmAsync(ResetPasswordDto resetPasswordDto)
    {
        await _usuarioService.ChangePassword(resetPasswordDto.Email.ToLower(),
           resetPasswordDto.Token, resetPasswordDto.NewPassword);
        _context.SaveChanges();
        return Ok("Senha alterada com sucesso!");
    }

    [HttpGet]
    [Authorize("AuthenticationUser")]
    public IEnumerable<ReadUsuariosDto> GetUsuarios()
    {
        return _mapper.Map<List<ReadUsuariosDto>>(
          _context.Users.ToList());
    }

    [HttpGet("{id}")]
    public IActionResult GetUsuariosId(string id)
    {
        var usuario = _context.Users.FirstOrDefault(user =>
        user.Id == id);
        if (usuario == null) return NotFound();
        var usuariosDto = _mapper.Map<ReadUsuariosDto>(usuario);
        return Ok(usuariosDto);
    }

    [HttpGet("perfil")]
    [Authorize("AuthenticationUser")]
    public IActionResult GetPerfilUsuario()
    {
        // Obtenha o ID do usuário autenticado
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        // Obtenha os dados do usuário do banco de dados usando o ID
        var usuario = _context.Users.FirstOrDefault(u => u.Id == userId);

        if (usuario == null)
        {
            return NotFound();
        }

        // Mapeie os dados do usuário para o DTO de leitura
        var usuarioDto = _mapper.Map<ReadUsuariosDto>(usuario);

        return Ok(usuarioDto);
    }


    [HttpPut("perfil")]
    [Authorize("AuthenticationUser")]
    public IActionResult UpdateProfile([FromBody] UpdateUsuarioDto updateUsuarioDto)
    {
        // Obtenha o ID do usuário autenticado
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        // Obtenha o usuário do banco de dados usando o ID
        var user = _context.Users.FirstOrDefault(u => u.Id == userId);
        if (user == null)
            return NotFound();

        _mapper.Map(updateUsuarioDto, user);
        _context.SaveChanges();

        return NoContent();
    }


    [HttpPut("{id}")]
    [Authorize("AuthenticationUser")]
    public IActionResult UpdateCadastro(string id,
        [FromBody] UpdateUsuarioDto updateUsuarioDto)
    {
        var user = _context.Users.FirstOrDefault(
            user => user.Id == id);
        if (user == null) NotFound();

        _mapper.Map(updateUsuarioDto, user);
        _context.SaveChanges();
        return NoContent();
    }


    [HttpDelete("{id}")]
    [Authorize("AuthenticationUser")]
    public IActionResult DeleteUsuario(string id)
    {
        var user = _context.Users.FirstOrDefault(user => user.Id == id);
        if (user == null) return NotFound();

        _context.Remove(user);
        _context.SaveChanges();
        return NoContent();
    }
}