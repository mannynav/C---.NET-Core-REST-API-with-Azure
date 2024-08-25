
using AutoMapper;
using DotnetAPI.Data;
using DotnetAPI.Dtos;
using DotnetAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace DotnetAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class UserEFController : ControllerBase
{

    IUserRepository _userRepository;

    IMapper _mapper;

    public UserEFController(IConfiguration config, IUserRepository userRepository)
    {
        _userRepository = userRepository;

        _mapper = new Mapper(new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<UserToAddDto, User>();
            cfg.CreateMap<UserSalary, UserSalary>(); //dont need dto!
            cfg.CreateMap<UserJobInfo, UserJobInfo>();
        }));

    }


    /// <summary>
    /// Get endpoint to get all users
    /// </summary>
    [HttpGet("GetUsers")]
    public IEnumerable<User> GetUsers()
    {
        IEnumerable<User> users = _userRepository.GetUsers();
        return users;
    }



    /// <summary>
    /// Get endpoint for a single user. The userId field is unique in the database, so this return
    /// will only be one user.
    /// </summary>
    [HttpGet("GetSingleUser/{userId}")]
    public User GetSingleUser(int userId)
    {
        return _userRepository.GetSingleUser(userId);
    }


    ///// <summary>
    ///Put endpoint to edit a user
    ///
    [HttpPut("EditUser")]
    public IActionResult EditUser(User user)
    {

        User? userDb = _userRepository.GetSingleUser(user.UserId);

        if (userDb != null)
        {
            userDb.FirstName = user.FirstName;
            userDb.LastName = user.LastName;
            userDb.Email = user.Email;
            userDb.Active = user.Active;
            userDb.Gender = user.Gender;

            if (_userRepository.SaveChanges())
            {
                return Ok();
            }
            throw new Exception("Failed to update user");
        }
        throw new Exception("Failed to update User");
    }


    /// <summary>
    /// Post endpoint to add a user
    ///
    [HttpPost("AddUser")]
    public IActionResult AddUser(UserToAddDto user)
    {
        //User userDb = new User();
        // userDb.FirstName = user.FirstName;
        // userDb.LastName = user.LastName;
        // userDb.Email = user.Email;
        // userDb.Active = user.Active;
        // userDb.Gender = user.Gender;


        User userDb = _mapper.Map<User>(user); //Mapping from UserToAddDto to User

        _userRepository.AddEntity<User>(userDb);

        if (_userRepository.SaveChanges())
        {
            return Ok();
        }

        throw new Exception("Failed to add user");
    }



    /// <summary>
    /// Delete endpoint to delete a user
    ///
    [HttpDelete("DeleteUser/{userId}")]
    public IActionResult DeleteUser(int userId)
    {
        User? userDb = _userRepository.GetSingleUser(userId);


        if (userDb != null)
        {
            _userRepository.RemoveEntity<User>(userDb);

            if (_userRepository.SaveChanges())
            {
                return Ok();
            }
            throw new Exception("Failed to delete user");
        }
        throw new Exception("Failed to delete User");

    }


    //////////////////////////// UserSalary //////////////////////////////////////


    [HttpGet("GetSingleUserSalary/{userId}")]
    public UserSalary GetSingleUserSalary(int userId)
    {
        return _userRepository.GetSingleUserSalary(userId);
    }



    /// <summary>
    /// Post endpoint to add a user salary
    ///
    [HttpPost("AddUserSalary")]
    public IActionResult AddUserSalary(UserSalary user)
    {
        //User userDb = new User();
        // userDb.FirstName = user.FirstName;
        // userDb.LastName = user.LastName;
        // userDb.Email = user.Email;
        // userDb.Active = user.Active;
        // userDb.Gender = user.Gender;

        _userRepository.AddEntity<UserSalary>(user);

        if (_userRepository.SaveChanges())
        {
            return Ok();
        }

        throw new Exception("Failed to add user salary");
    }



    ///// <summary>
    ///Put endpoint to edit a user
    ///
    [HttpPut("EditUserSalary")]
    public IActionResult EditUserSalary(UserSalary user)
    {

        UserSalary? userDb = _userRepository.GetSingleUserSalary(user.UserId);

        if (userDb != null)
        {
            _mapper.Map(userDb, user);

            if (_userRepository.SaveChanges())
            {
                return Ok();
            }
            throw new Exception("Failed to update user salary");
        }
        throw new Exception("Failed to update User salary");
    }


    /// <summary>
    /// Delete endpoint to delete a user from UserSalary
    ///
    [HttpDelete("DeleteUserSalary/{userId}")]
    public IActionResult DeleteUserSalary(int userId)
    {
        UserSalary? userDb = _userRepository.GetSingleUserSalary(userId);

        if (userDb != null)
        {
            _userRepository.RemoveEntity<UserSalary>(userDb);

            if (_userRepository.SaveChanges())
            {
                return Ok();
            }
            throw new Exception("Failed to delete user");
        }
        throw new Exception("Failed to delete User");
    }


    //////////////////////////////// UserJobInfo //////////////////////////////////////

    [HttpGet("GetSingleUserJobInfo/{userId}")]
    public UserJobInfo GetSingleUserJobInfo(int userId)
    {
        return _userRepository.GetSingleUserJobInfo(userId);
    }



    /// <summary>
    /// Post endpoint to add a user job info
    ///
    [HttpPost("AddUserJobInfo")]
    public IActionResult AddUserJobInfo(UserJobInfo user)
    {
        //User userDb = new User();
        // userDb.FirstName = user.FirstName;
        // userDb.LastName = user.LastName;
        // userDb.Email = user.Email;
        // userDb.Active = user.Active;
        // userDb.Gender = user.Gender;

        _userRepository.AddEntity<UserJobInfo>(user);

        if (_userRepository.SaveChanges())
        {
            return Ok();
        }

        throw new Exception("Failed to add user job info");
    }



    ///// <summary>
    ///Put endpoint to edit a user job info
    ///
    [HttpPut("EditUserJobInfo")]
    public IActionResult EditUserJobInfo(UserJobInfo user)
    {
        UserJobInfo? userDb = _userRepository.GetSingleUserJobInfo(user.UserId);

        if (userDb != null)
        {
            //userDb.JobTitle = user.JobTitle;
            //userDb.Department = user.Department;
            _mapper.Map(user, userDb);

            if (_userRepository.SaveChanges())
            {
                return Ok();
            }
            throw new Exception("Failed to update user job info");
        }
        throw new Exception("Failed to update User job info");
    }


    /// <summary>
    /// Delete endpoint to delete a user from UserJobInfo
    ///
    [HttpDelete("DeleteUserJobInfo/{userId}")]
    public IActionResult DeleteUserJobInfo(int userId)
    {
        UserJobInfo? userDb = _userRepository.GetSingleUserJobInfo(userId);

        if (userDb != null)
        {
            _userRepository.RemoveEntity<UserJobInfo>(userDb);

            if (_userRepository.SaveChanges())
            {
                return Ok();
            }
            throw new Exception("Failed to delete user");
        }
        throw new Exception("Failed to delete User");
    }

}
