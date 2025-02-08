using Backend.DTOs;
using Backend.Models;
using Dapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Services
{
    public class AuthService
    {
        private readonly IDbConnection _dbConnection;
        private readonly IConfiguration _configuration;
        private readonly IPasswordHasher<User> _passwordHasher;

        public AuthService(IDbConnection dbConnection, IConfiguration configuration, IPasswordHasher<User> passwordHasher)
        {
            _dbConnection = dbConnection;
            _configuration = configuration;
            _passwordHasher = passwordHasher;
        }

        public async Task<IdentityResult> Register(RegisterDTO registerDTO)
        {
            var user = new User
            {
                FullName = registerDTO.FullName,
                Email = registerDTO.Email,
                UserName = registerDTO.Email
            };

            var existingUser = await _dbConnection.QueryFirstOrDefaultAsync<User>("SELECT * FROM AspNetUsers WHERE Email = @Email", new { Email = registerDTO.Email });
            if (existingUser != null)
            {
                return IdentityResult.Failed(new IdentityError { Description = "Email is already registered." });
            }

            user.PasswordHash = _passwordHasher.HashPassword(user, registerDTO.Password);

            var sql = "INSERT INTO AspNetUsers (Id, UserName, NormalizedUserName, Email, NormalizedEmail, EmailConfirmed, PasswordHash, SecurityStamp, ConcurrencyStamp, PhoneNumber, PhoneNumberConfirmed, TwoFactorEnabled, LockoutEnd, LockoutEnabled, AccessFailedCount, FullName) VALUES (@Id, @UserName, @NormalizedUserName, @Email, @NormalizedEmail, @EmailConfirmed, @PasswordHash, @SecurityStamp, @ConcurrencyStamp, @PhoneNumber, @PhoneNumberConfirmed, @TwoFactorEnabled, @LockoutEnd, @LockoutEnabled, @AccessFailedCount, @FullName)";
            await _dbConnection.ExecuteAsync(sql, user);

            return IdentityResult.Success;
        }

        public async Task<string> Login(LoginDTO loginDTO)
        {
            var user = await _dbConnection.QueryFirstOrDefaultAsync<User>("SELECT * FROM AspNetUsers WHERE Email = @Email", new { Email = loginDTO.Email });

            if (user == null)
            {
                throw new Exception("Email not found.");
            }

            var result = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, loginDTO.Password);

            if (result == PasswordVerificationResult.Failed)
            {
                throw new Exception("Incorrect password.");
            }

            return GenerateJwtToken(user);
        }

        public Task Logout()
        {
            return Task.CompletedTask;
        }

        public async Task<UserDTO> GetLoggedInUser(ClaimsPrincipal claimsPrincipal)
        {
            var userId = claimsPrincipal.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await _dbConnection.QueryFirstOrDefaultAsync<User>("SELECT * FROM AspNetUsers WHERE Id = @Id", new { Id = userId });

            if (user == null)
            {
                throw new Exception("User not found");
            }

            return new UserDTO
            {
                Id = user.Id,
                UserName = user.UserName,
                FullName = user.FullName,
                Email = user.Email
            };
        }

        private string GenerateJwtToken(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id ?? string.Empty),
                new Claim(ClaimTypes.Email, user.Email ?? string.Empty)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"] ?? string.Empty));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(24),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}