
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using AutoMapper;
using Dapper;
using DotnetAPI.Data;
using DotnetAPI.Dtos;
using DotnetAPI.Helpers;
using DotnetAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using Microsoft.IdentityModel.Tokens;

namespace DotnetAPI.Controllers
{
    [Authorize]                 //Authorize users to access this controller
    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly DataContextDapper _dapper;
        private readonly AuthHelper _authHelper;
        private readonly ReusableSql _sqlHelper;
        private readonly IMapper _mapper;

        public AuthController(IConfiguration config)
        {
            _dapper = new DataContextDapper(config);
            _authHelper = new AuthHelper(config);
            _sqlHelper = new ReusableSql(config);
            _mapper = new Mapper(new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<UserForRegistrationDto, UserComplete>();
            }));
        }


        [AllowAnonymous]            //Allow users to access this endpoint without being authorized
        [HttpPost("Register")]
        public IActionResult Register(UserForRegistrationDto userForRegistrationDto)
        {
            //Make sure password and password confirmation match
            if (userForRegistrationDto.Password == userForRegistrationDto.PasswordConfirmation)
            {

                //check if there is already a user
                string sqlCheckUserExists = "select Email from TutorialAppSchema.Auth where Email='" +
                userForRegistrationDto.Email + "'";

                IEnumerable<string> existingUsers = _dapper.LoadData<string>(sqlCheckUserExists);

                if (existingUsers.Count() == 0)
                {
                    UserForLoginDto userForSetPassword = new UserForLoginDto
                    {
                        Email = userForRegistrationDto.Email,
                        Password = userForRegistrationDto.Password
                    };

                    if (_authHelper.SetPassword(userForSetPassword))
                    {
                        UserComplete userComplete = _mapper.Map<UserComplete>(userForRegistrationDto);
                        userComplete.Active = true;

                        if (_sqlHelper.UpsertUser(userComplete))
                        {
                            return Ok();
                        }

                        throw new Exception("Failed to add user");
                    }
                    throw new Exception("Failed to register user");
                }
                throw new Exception("User already exists with this email");
            }
            throw new Exception("Passwords do not match");
        }



        [HttpPut("ResetPassword")]
        public IActionResult ResetPassword(UserForLoginDto userForSetPassword)
        {
            if (_authHelper.SetPassword(userForSetPassword))
            {
                return Ok();
            }
            throw new Exception("Failed to reset password");

        }


        [AllowAnonymous]
        [HttpPost("Login")]
        public IActionResult Login(UserForLoginDto userForLoginDto)
        {

            string sqlForHashAndSalt = @"EXEC TutorialAppSchema.spLoginConfirmation_Get
                    @Email = @EmailParam";

            DynamicParameters sqlParameters = new DynamicParameters();

            sqlParameters.Add("@EmailParam", userForLoginDto.Email, DbType.String);

            UserForLoginConfirmationDto userForLoginConfirmationDto = _dapper
            .LoadDataSingleWithParameters<UserForLoginConfirmationDto>(sqlForHashAndSalt, sqlParameters);


            byte[] passwordHash = _authHelper.GetPasswordHash(userForLoginDto.Password, userForLoginConfirmationDto.PasswordSalt);

            for (int index = 0; index < passwordHash.Length; index++)
            {
                if (passwordHash[index] != userForLoginConfirmationDto.PasswordHash[index])
                {
                    return StatusCode(401, "Invalid password");
                }

            }
            int userId = _dapper.LoadDataSingle<int>("select UserId from TutorialAppSchema.Users where Email='" + userForLoginDto.Email + "'");
            return Ok(new Dictionary<string, string> { { "token", _authHelper.CreateToken(userId) } });
        }



        [HttpGet("RefreshToken")]
        public IActionResult RefreshToken()
        {
            string userId = User.FindFirst("userId")?.Value + "";

            //check if user exists
            string userIdSql = @"
                Select userId from TutorialAppSchema.Users where UserId = "
                + userId;

            int userIdFromDb = _dapper.LoadDataSingle<int>(userIdSql);

            return Ok(new Dictionary<string, string> { { "token", _authHelper.CreateToken(userIdFromDb) } });
        }
    }
}