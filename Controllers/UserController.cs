
using DotnetAPI.Data;
using DotnetAPI.Dtos;
using DotnetAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace DotnetAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class UserController : ControllerBase
{
    //Bring in dapper data context
    DataContextDapper _dapper;

    public UserController(IConfiguration config)
    {
        _dapper = new DataContextDapper(config);
        //Console.WriteLine(config.GetConnectionString("DefaultConnection"));
    }


    /// <summary>
    /// Get endpoint to get all users
    /// </summary>
    [HttpGet("GetUsers")]
    public IEnumerable<User> GetUsers()
    {
        string sql = @"
        SELECT  [UserId]
            , [FirstName]
            , [LastName]
            , [Email]
            , [Gender]
            , [Active]
        FROM  TutorialAppSchema.Users";
        IEnumerable<User> users = _dapper.LoadData<User>(sql);
        return users;
    }



    /// <summary>
    /// Get endpoint for a single user. The userId field is unique in the database, so this return
    /// will only be one user.
    /// </summary>
    [HttpGet("GetSingleUser/{userId}")]
    public User GetSingleUser(int userId)
    {
        string sql = @"
        SELECT  [UserId]
            , [FirstName]
            , [LastName]
            , [Email]
            , [Gender]
            , [Active]
        FROM  TutorialAppSchema.Users
        WHERE UserId = " + userId.ToString();
        User user = _dapper.LoadDataSingle<User>(sql);
        return user;
    }


    ///// <summary>
    ///Put endpoint to edit a user
    ///
    [HttpPut("EditUser")]
    public IActionResult EditUser(User user)
    {
        //string for udpdate statement
        string sql = @"
        Update TutorialAppSchema.Users
            set [FirstName] = '" + user.FirstName +
            "', [LastName] = '" + user.LastName +
            "', [Email] = '" + user.Email +
            "', [Gender] = '" + user.Gender +
            "', [Active] = '" + user.Active +
       "' WHERE UserId = " + user.UserId;
        Console.WriteLine(sql);
        if (_dapper.ExecuteSql(sql))
        {
            return Ok();
        }

        throw new Exception("Failed to update user");
    }


    /// <summary>
    /// Post endpoint to add a user
    ///
    [HttpPost("AddUser")]
    public IActionResult AddUser(UserToAddDto user)
    {
        string sql = @"
        Insert into TutorialAppSchema.Users(
                [FirstName],
                [LastName],
                [Email],
                [Gender],
                [Active]
            ) values (" +
            "'" + user.FirstName +
            "', '" + user.LastName +
            "', '" + user.Email +
            "', '" + user.Gender +
            "', '" + user.Active +
       "' )";

        Console.WriteLine(sql);
        if (_dapper.ExecuteSql(sql))
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
        string sql = @"
        DELETE FROM TutorialAppSchema.Users
        WHERE UserId = " + userId.ToString();
        if (_dapper.ExecuteSql(sql))
        {
            return Ok();
        }

        throw new Exception("Failed to delete user");
    }


    /////////////////////////////// User Salary Endpoints ///////////////////////////////////////////////



    /// <summary>
    /// Get endpoint to get all user salaries
    /// </summary>
    [HttpGet("UserSalaries")]
    public IEnumerable<UserSalary> GetUserSalaries()
    {
        string sql = @"
        Select [UserId]
            , [Salary]
            FROM  TutorialAppSchema.UserSalary;
        ";
        IEnumerable<UserSalary> userSalaries = _dapper.LoadData<UserSalary>(sql);
        return userSalaries;
    }


    /// <summary>
    /// Get endpoint to get a single salary from User.Salary
    /// </summary>
    [HttpGet("UserSalary/{userId}")]
    public IEnumerable<UserSalary> GetUserSalary(int userId)
    {
        string sql = @"
        Select [UserId],
                [Salary]
            FROM TutorialAppSchema.UserSalary
            WHERE UserId = " + userId.ToString();

        IEnumerable<UserSalary> userSalary = _dapper.LoadData<UserSalary>(sql);
        return userSalary;

    }


    ///// <summary>
    ///Put endpoint to edit a user salary
    ///
    [HttpPut("EditUserSalary")]
    public IActionResult PutUserSalary(UserSalary userSalary)
    {
        string sql = @"
        UPDATE TutorialAppSchema.UserSalary
        SET [Salary]= " + userSalary.Salary
         + " WHERE UserId= " + userSalary.UserId;

        if (_dapper.ExecuteSql(sql))
        {
            return Ok();
        }
        throw new Exception("User Salary not updated");
    }


    /// <summary>
    /// Post endpoint to add a user salary
    ///
    [HttpPost("AddUserSalary")]
    public IActionResult PostUserSalary(UserSalary userSalary)
    {
        //the database will give the user an id

        string sql = @"
        INSERT INTO TutorialAppSchema.UserSalary (
            UserId,
            Salary
        ) VALUES (" + userSalary.UserId
        + ", " + userSalary.Salary
        + ")";

        if (_dapper.ExecuteSql(sql))
        {
            return Ok();
        }
        throw new Exception("Adding UserSalary failed");
    }


    /// <summary>
    /// Delete endpoint to delete a user salary
    ///
    [HttpDelete("DeleteSalary/{userId}")]
    public IActionResult DeleteUserSalary(int userId)
    {
        string sql = @"
        DELETE FROM TutorialAppSchema.UserSalary
        WHERE UserId = " + userId;

        if (_dapper.ExecuteSql(sql))
        {
            return Ok();
        }
        throw new Exception("User Salary not deleted");
    }





    /////////////////////////////////Job Info Endpoints///////////////////////////////////////////////

    [HttpGet("UserJobInfo/{userId}")]
    public IEnumerable<UserJobInfo> GetUserJobInfo(int userId)
    {
        string sql = @"
        SELECT [UserId]
            , [JobTitle]
            , [Department]
        FROM TutorialAppSchema.UserJobInfo
        WHERE UserId = " + userId;

        IEnumerable<UserJobInfo> userJobInfo = _dapper.LoadData<UserJobInfo>(sql);
        return userJobInfo;
    }

    [HttpPost("AddUserJobInfo")]
    public IActionResult PostUserJobInfo(UserJobInfo userJobInfo)
    {
        string sql = @"
        INSERT INTO TutorialAppSchema.UserJobInfo (
            UserId,
            JobTitle,
            Department
        ) VALUES (" + userJobInfo.UserId
        + ", '" + userJobInfo.JobTitle
        + "', '" + userJobInfo.Department
        + "')";

        if (_dapper.ExecuteSql(sql))
        {
            return Ok();
        }
        throw new Exception("Adding UserJobInfo failed");
    }

    [HttpPut("EditUserJobInfo")]
    public IActionResult PutUserJobInfo(UserJobInfo userJobInfo)
    {
        string sql = @"
        UPDATE TutorialAppSchema.UserJobInfo
        SET [JobTitle]= '" + userJobInfo.JobTitle
        + "', [Department]= '" + userJobInfo.Department
        + "' WHERE UserId= " + userJobInfo.UserId.ToString();

        if (_dapper.ExecuteSql(sql))
        {
            return Ok();
        }
        throw new Exception("User Job Info not updated");
    }



    [HttpDelete("DeleteJobInfo/{userId}")]
    public IActionResult DeleteUserJobInfo(int userId)
    {
        string sql = @"
        DELETE FROM TutorialAppSchema.UserJobInfo
        WHERE UserId = " + userId;

        if (_dapper.ExecuteSql(sql))
        {
            return Ok();
        }
        throw new Exception("User Job Info not deleted");
    }
















}
