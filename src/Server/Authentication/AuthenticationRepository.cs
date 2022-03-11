using TradingJournal.Infrastructure.Server.Persistence;
using TradingJournal.Application.Common.Interfaces;
using TradingJournal.Application.Common.Models;
using TradingJournal.Domain.Entities;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Security.Claims;
using System.Text;

namespace TradingJournal.Server.Authentication;

internal class AuthenticationRepository : IAuthenticationRepository
{
    private readonly ApplicationDbContext _context;
    private readonly IConfiguration _configuration;

    public AuthenticationRepository(ApplicationDbContext context, IConfiguration configuration)
    {
        _context = context;
        _configuration = configuration;
    }

    public async Task<ServiceResponse<string>> Login(UserLogin userLogin)
    {
        var user = await _context.Users.FirstOrDefaultAsync(x => x.Email.ToLower() == userLogin.Email.ToLower());

        // if user was found and password is correct
        if (user != null && VerifyPasswordHash(userLogin.Password, user.PasswordHash, user.PasswordSalt))
        {
            return new ServiceResponse<string>()
            {
                Data = CreateToken(user),
            };
        }

        // if user was not found OR password is wrong
        return new ServiceResponse<string>()
        {
            Success = false,
            Message = "Invalid credentials.",
        };
    }

    public async Task<ServiceResponse<int>> Register(UserRegistration registration)
    {
        if (await UserExists(registration.Email))
        {
            return new ServiceResponse<int>()
            {
                Success = false,
                Data = -1,
                Message = $"User with the email address '{registration.Email}' already exists.",
            };
        }

        CreatePasswordHash(registration.Password, out byte[] passwordHash, out byte[] passwordSalt);

        User user = new User
        {
            DisplayName = registration.DisplayName,
            Email = registration.Email,
            PasswordHash = passwordHash,
            PasswordSalt = passwordSalt,
        };

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        return new ServiceResponse<int>()
        {
            Data = user.Id
        };
    }

    public async Task<bool> UserExists(string email) => await _context.Users.AnyAsync(user => user.Email.ToLower() == email.ToLower());


    private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
    {
        using (var hmac = new HMACSHA512())
        {
            passwordSalt = hmac.Key;
            passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
        }
    }

    private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
    {
        using (var hmac = new HMACSHA512(passwordSalt))
        {
            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            for (int i = 0; i < computedHash.Length; i++)
            {
                if (computedHash[i] != passwordHash[i])
                    return false;
            }
            return true;
        }
    }

    private string CreateToken(User user)
    {
        List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.DisplayName)
            };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
            _configuration.GetSection("JavaWebTokenSettings:EncryptionKey").Value));

        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

        double daysToExpire = double.Parse(_configuration.GetSection("JavaWebTokenSettings:DaysToExpire").Value);

        var token = new JwtSecurityToken(
            claims: claims,
            expires: DateTime.Now.AddDays(daysToExpire),
            signingCredentials: creds);

        var jwt = new JwtSecurityTokenHandler().WriteToken(token);

        return jwt;
    }
}