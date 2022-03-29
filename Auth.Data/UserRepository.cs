using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Auth.Data
{
    public class UserRepository
    {
        private string _connectionString;

        public UserRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public void AddUser(User user, string password)
        {
            using var connection = new SqlConnection(_connectionString);
            using var cmd = connection.CreateCommand();
            cmd.CommandText = "INSERT INTO Logins (Name, Email, PasswordHash) " +
                "VALUES (@name, @email, @hash)";
            cmd.Parameters.AddWithValue("@name", user.Name);
            cmd.Parameters.AddWithValue("@email", user.Email);
            cmd.Parameters.AddWithValue("@hash", BCrypt.Net.BCrypt.HashPassword(password));

            connection.Open();
            cmd.ExecuteNonQuery();
        }

        public User Login(string email, string password)
        {
            var user = GetByEmail(email);
            if (user == null)
            {
                return null;
            }

            bool isValid = BCrypt.Net.BCrypt.Verify(password, user.PasswordHash);
            return isValid ? user : null;

        }

        public User GetByEmail(string email)
        {
            using var connection = new SqlConnection(_connectionString);
            using var cmd = connection.CreateCommand();
            cmd.CommandText = "SELECT TOP 1 * FROM Logins WHERE Email = @email";
            cmd.Parameters.AddWithValue("@email", email);
            connection.Open();
            var reader = cmd.ExecuteReader();
            if (!reader.Read())
            {
                return null;
            }

            return new User
            {
                Id = (int)reader["Id"],
                Email = (string)reader["Email"],
                Name = (string)reader["Name"],
                PasswordHash = (string)reader["PasswordHash"]
            };
        }
        public List<Add> GetAllAdds()
        {
            using var connection = new SqlConnection(_connectionString);
            using var cmd = connection.CreateCommand();
            cmd.CommandText = "select a.id, a.LoginId,a.Description,a.PhoneNumber,l.Name,l.Email,a.Date from Adds a join Logins l on a.LoginId = l.Id";
            List<Add> adds = new List<Add>();
            connection.Open();
            var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                adds.Add(new Add
                {
                    Name = (string)reader["Name"],
                    Description = (string)reader["Description"],
                    Date = (DateTime)reader["Date"],
                    LoginId = (int)reader["LoginId"],
                    Number = (int)reader["PhoneNumber"],
                    Email = (string)reader["Email"],
                    id=(int)reader["id"]
                });
            }
            return adds;

        }
        public void DeleteAdd(int id)
        {
            using var connection = new SqlConnection(_connectionString);
            using var cmd = connection.CreateCommand();
            cmd.CommandText = "Delete from adds where id=@id";
            cmd.Parameters.AddWithValue("@id", id);

            connection.Open();
            cmd.ExecuteNonQuery();
        }
        public void NewAdd(Add add)
        {
            using var connection = new SqlConnection(_connectionString);
            using var cmd = connection.CreateCommand();
            cmd.CommandText = "INSERT INTO Adds VALUES (@PhoneNumber, @Description, @LoginId, GetDate())";
            cmd.Parameters.AddWithValue("@PhoneNumber", add.Number);
            cmd.Parameters.AddWithValue("@Description", add.Description);
            cmd.Parameters.AddWithValue("@LoginId", add.LoginId);
            connection.Open();
            cmd.ExecuteNonQuery();

        }
        public List<Add> GetAllAdds(int id)
        {
            using var connection = new SqlConnection(_connectionString);
            using var cmd = connection.CreateCommand();
            cmd.CommandText = "select a.id, a.LoginId,a.Description,a.PhoneNumber,l.Name,l.Email,a.Date from Adds a join Logins l on a.LoginId = l.Id where LoginId=@id";
            cmd.Parameters.AddWithValue("@id", id);
            List<Add> adds = new List<Add>();
            connection.Open();
            var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                adds.Add(new Add
                {
                    Name = (string)reader["Name"],
                    Description = (string)reader["Description"],
                    Date = (DateTime)reader["Date"],
                    LoginId = (int)reader["LoginId"],
                    Number = (int)reader["PhoneNumber"],
                    Email = (string)reader["Email"],
                    id=(int)reader["id"]
                });
            }
            return adds;
        }
    }
}
