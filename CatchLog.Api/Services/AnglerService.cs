using CatchLog.Api.Data;
using CatchLog.Api.DTOs;
using CatchLog.Api.Exceptions;
using CatchLog.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace CatchLog.Api.Services;

public class AnglerService
{
    private readonly CatchLogDbContext _context;

    public AnglerService(CatchLogDbContext context)
    {
        _context = context;
    }

    public async Task<List<AnglerResponseDto>> GetAllAsync()
    {
        return await _context.Anglers
            .Select(a => ToResponseDto(a))
            .ToListAsync();
    }

    public async Task<AnglerResponseDto?> GetByIdAsync(int id)
    {
        var angler = await _context.Anglers.FindAsync(id);
        return angler is null ? null : ToResponseDto(angler);
    }

    public async Task<AnglerResponseDto> CreateAsync(CreateAnglerDto dto)
    {
        var angler = new Angler
        {
            Name = dto.Name,
            Email = dto.Email,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
            CreatedAt = DateTime.UtcNow
        };

        _context.Anglers.Add(angler);
        await _context.SaveChangesAsync();

        return ToResponseDto(angler);
    }

    public async Task<AnglerResponseDto?> UpdateAsync(int id, UpdateAnglerDto dto)
    {
        var angler = await _context.Anglers.FindAsync(id);
        if (angler is null)
        {
            return null;
        }

        var emailTaken = await _context.Anglers
            .AnyAsync(a => a.Email == dto.Email && a.Id != id);
        if (emailTaken)
        {
            throw new ConflictException($"Email {dto.Email} is already in use.");
        }

        angler.Name = dto.Name;
        angler.Email = dto.Email;
        await _context.SaveChangesAsync();

        return ToResponseDto(angler);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var angler = await _context.Anglers.FindAsync(id);
        if (angler is null)
        {
            return false;
        }

        _context.Anglers.Remove(angler);
        await _context.SaveChangesAsync();
        return true;
    }

    private static AnglerResponseDto ToResponseDto(Angler angler)
    {
        return new AnglerResponseDto
        {
            Id = angler.Id,
            Name = angler.Name,
            Email = angler.Email,
            CreatedAt = angler.CreatedAt
        };
    }
}