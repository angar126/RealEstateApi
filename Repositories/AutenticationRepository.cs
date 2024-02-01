using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RealEstateApi.Enumeratori;
using RealEstateApi.Models;
using RealEstateApi.RealEstateDbContext;
using RealEstateApi.Models;
using RealEstateApi.Models.Interfaces;
using RealEstateApi.Models.ModelsDTO;
using RealEstateApi.Repositories.Interfaces;
using System;
using System.Threading.Tasks;
using RealEstateApi.Models.RequestModelsDTO;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Linq;

namespace RealEstateApi.Repositories
{
    public class AutenticationRepository : IAutentication
    {
        public readonly UserAirbnbDataContext _context;
        public readonly ILogger <AutenticationRepository> _logger;
        public readonly IMailTemplate _mailTemplate;
        public readonly INotifier _notifier;
        public AutenticationRepository(UserAirbnbDataContext context, ILogger<AutenticationRepository> Logger, 
                                       IMailTemplate MailTemplate, INotifier Notifier)
        {
            _context = context;
            _logger = Logger;
            _mailTemplate = MailTemplate;
            _notifier = Notifier;
        }  

        public async Task<string> Login(LoginDTO credentials)
        {
            try
            {
                var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == credentials.Email);

                if (user == null || !VerifyPasswordHash(credentials.Password, user.PasswordHash, user.PasswordSalt))
                {
                    _logger.LogInformation("Login Fallito");
                    return null;
                }
                // o lascio user
                UserDTO userDTO = new UserDTO(user);
                return GenerateJwtToken(userDTO);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Errore durante login");
                throw;
            }
        }

        public async Task<UserDTO> Logout(LoginDTO credentials)
        {
            try
            {
                var user = await _context.Users
                    .FirstOrDefaultAsync(u => u.Email == credentials.Email);// && u.Password == credentials.Password);

                if(user == null)
                {
                    _logger.LogInformation("Logout Fallito");
                    return null;
                }

                return new UserDTO(user);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Errore durante logout");
                throw;
            }
        }

        public async Task<UserDTO> Register(RequestRegisterDTO requestRegister)
        {
            try
            {
                if (await _context.Users.AnyAsync(u => u.Email == requestRegister.Credentials.Email))
                {
                    _logger.LogInformation("Utente gia registrato");

                    return null;
                }

                byte[] passwordHash, passwordSalt;
                CreatePasswordHash(requestRegister.Credentials.Password, out passwordHash, out passwordSalt);

                User user = new User()
                {
                    Id = Guid.NewGuid(),
                    Email = requestRegister.Credentials.Email,
                    PasswordHash = passwordHash,
                    PasswordSalt = passwordSalt,
                    Name = requestRegister.DataAccount.Name,
                    LastName = requestRegister.DataAccount.LastName,
                    Role = requestRegister.DataAccount.Role
                };
               
                await _context.Users.AddAsync(user);
                var changes = await _context.SaveChangesAsync();
                if (changes > 0)
                {
                    _logger.LogInformation("Nuovo utente registrato");
                    
                    var emailTemplate = _mailTemplate.GetMailTemplateForNewUser(user.Name);
                    _notifier.SendNotification(user.Email, "Conferma registrazione", emailTemplate);

                    return new UserDTO(user);
                }
                else
                {
                    _logger.LogWarning("Errore durante il salvataggio nuovo utente");
                    return null;
                }

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Errore nella registrazione");
                throw;
            }
        }

        public string GenerateJwtToken(UserDTO user)  //qua nel suo codice sono tutti sync
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes("CreateSomeRandomStringForSecretKey"); //Encoding.ASCII.GetBytes(user.PasswordSalt());

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                new Claim(ClaimTypes.Name, user.Id.ToString())
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha512Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
        public void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key; // Save the generated salt
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        public bool VerifyPasswordHash(string password, byte[] storedHash, byte[] storedSalt)
        {
            using var hmac = new System.Security.Cryptography.HMACSHA512(storedSalt); // Use the stored salt


            var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            return computedHash.SequenceEqual(storedHash); // Compare the computed hash with the stored hash
        }
    }
}
