using AutoMapper;
using Microsoft.AspNetCore.Identity;
using TO_DO___API.Data.Dto;
using TO_DO___API.Data;
using TO_DO___API.Models;

namespace TO_DO___API.Services;

public class UsuariosService
{
    private const int MAX_IMAGE_SIZE_BYTES = 3145728; // 3 MB - Tamanho máximo da imagem 
    private readonly IMapper _mapper;
    private readonly UserManager<Usuario> _userManager;
    private readonly SignInManager<Usuario> _signInManager;
    private readonly TokenService _tokenService;
    private readonly EmailService _emailService;
    private readonly UsuariosContext _context;

    public UsuariosService(IMapper mapper,
        UserManager<Usuario> userManager,
        SignInManager<Usuario> signInManager,
        TokenService tokenService,
        EmailService emailService,
        UsuariosContext usuariosContext)
    {
        _mapper = mapper;
        _userManager = userManager;
        _signInManager = signInManager;
        _tokenService = tokenService;
        _emailService = emailService;
        _context = usuariosContext;
    }

    public async Task RegisterUser(CreateUsuarioDto createUsuarioDto)
    {
        // Verificar o tamanho da imagem
        if (createUsuarioDto.ImageProfile != null && createUsuarioDto.ImageProfile.Length > MAX_IMAGE_SIZE_BYTES)
        {
            throw new ApplicationException("Tamanho da imagem excede o limite permitido.");
        }

        // Mapeando a classe de usuário
        Usuario user = _mapper.Map<Usuario>(createUsuarioDto);

        // Cadastrando um usuário no banco
        IdentityResult result = await _userManager.CreateAsync(user, createUsuarioDto.Password);

        // Se o resultado tiver sucesso
        if (result.Succeeded)
        {
            // Envie o e-mail de boas-vindas
            await _emailService.PasswordResetEmail(createUsuarioDto.Email, createUsuarioDto.Username);
        }
        else
        {
            throw new ApplicationException("Erro ao cadastrar usuário.");
        }
    }

    public async Task<string> LoginUser(LoginUsuarioDto loginUsuarioDto)
    {
        var user = await _signInManager.UserManager.FindByEmailAsync(loginUsuarioDto.Email);

        if (user == null)
        {
            throw new ApplicationException("Erro ao acessar sua conta, verifique seus dados de email e senha.");
        }

        // Verificando se o usuário é válido antes de gerar o token
        if (_userManager.SupportsUserLockout && await _userManager.IsLockedOutAsync(user))
        {
            throw new ApplicationException("A conta está bloqueada.");
        }

        // Verificando a senha do usuário
        var result = await _signInManager.CheckPasswordSignInAsync(user, loginUsuarioDto.Password, lockoutOnFailure: true);

        if (!result.Succeeded)
        {
            throw new ApplicationException("Erro ao acessar sua conta, verifique seus dados de email e senha.");
        }

        // Gerando o token apenas se o usuário for autenticado com sucesso
        var token = _tokenService.GenerateToken(user);

        return token;
    }

    public async Task<string> PasswordReset(string email)
    {
        // verificando email do usuário
        var user = await _userManager.FindByEmailAsync(email.ToLower());

        // se o email do user for nulo/não existir
        if (user == null)
        {
            throw new ApplicationException("Usuário não encontrado para o email fornecido.");
        }

        // gerando um token para reset de senha
        var token = await _userManager.GeneratePasswordResetTokenAsync(user);
        Console.WriteLine(token); // log para verificar o token gerado


        await _emailService.PasswordResetEmail(email, token);

        return token;
    }


    public async Task ChangePassword(string email, string token, string newPassword)
    {
        var user = await _userManager.FindByEmailAsync(email.ToLower());

        if (user == null)
        {
            throw new ApplicationException("Usuário não encontrado");
        }

        // Verifica se o token é válido antes de redefinir a senha
        var isTokenValid = await _userManager.VerifyUserTokenAsync(user, _userManager.Options.Tokens.PasswordResetTokenProvider, UserManager<Usuario>.ResetPasswordTokenPurpose, token);

        if (!isTokenValid)
        {
            throw new ApplicationException("Token inválido");
        }

        // Agora podemos redefinir a senha
        var result = await _userManager.ResetPasswordAsync(user, token, newPassword);

        if (!result.Succeeded)
        {
            throw new ApplicationException("Erro ao redefinir a senha");
        }
    }
}