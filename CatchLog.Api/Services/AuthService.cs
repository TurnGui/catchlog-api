using CatchLog.Api.Data;
using CatchLog.Api.DTOs;
using Microsoft.EntityFrameworkCore;

namespace CatchLog.Api.Services;

public class AuthService
{
    private readonly CatchLogDbContext _context;
    private readonly TokenService _tokenService;

    public AuthService(CatchLogDbContext context, TokenService tokenService)
    {
        _context = context;
        _tokenService = tokenService;
    }

    public async Task<string?> LoginAsync(LoginDto dto)
    {
        var angler = await _context.Anglers
            .FirstOrDefaultAsync(a => a.Email == dto.Email);

        if (angler is null || !BCrypt.Net.BCrypt.Verify(dto.Password, angler.PasswordHash))
        {
            return null;
        }

        return _tokenService.GenerateToken(angler);
    }
}