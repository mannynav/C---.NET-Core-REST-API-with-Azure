
using DotnetAPI.Models;

namespace DotnetAPI.Data
{

    public interface IUserRepository
    {
        /// <summary>
        /// Calls to these methods, called from EF Controller
        /// </summary>
        public bool SaveChanges();
        public void AddEntity<T>(T entity);
        public void RemoveEntity<T>(T entity);


        public IEnumerable<User> GetUsers();

        public User GetSingleUser(int userId);
        public UserSalary GetSingleUserSalary(int userId);
        public UserJobInfo GetSingleUserJobInfo(int userId);
















    }

}