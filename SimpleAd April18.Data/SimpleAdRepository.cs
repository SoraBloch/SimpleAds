using System.Data.SqlClient;

namespace SimpleAd_April18.Data
{
    public class SimpleAdRepository
    {
        private string _connectionString;
        public SimpleAdRepository(string connectionString)
        {
            _connectionString = connectionString;
        }
        public void AddUser(User user, String password)
        {
            using var connection = new SqlConnection(_connectionString);
            using var command = connection.CreateCommand();
            command.CommandText = "INSERT INTO Users " +
                "VALUES (@name, @email, @passwordHash)";
            string passwordHash = BCrypt.Net.BCrypt.HashPassword(password);
            command.Parameters.AddWithValue("@name", user.Name);
            command.Parameters.AddWithValue("@email", user.Email);
            command.Parameters.AddWithValue("@passwordHash", passwordHash);
            connection.Open();
            command.ExecuteNonQuery();
        }
        public void AddAd(Ad ad)
        {
            using var connection = new SqlConnection(_connectionString);
            using var command = connection.CreateCommand();
            command.CommandText = "INSERT INTO Ads " +
                "VALUES (@title, @phoneNumber, @description, @userId, @datePosted)";
            command.Parameters.AddWithValue("@title", ad.Title);
            command.Parameters.AddWithValue("@phoneNumber", ad.PhoneNumber);
            command.Parameters.AddWithValue("@description", ad.Description);
            command.Parameters.AddWithValue("@userId", ad.UserId);
            command.Parameters.AddWithValue("@datePosted", ad.DatePosted);
            connection.Open();
            command.ExecuteNonQuery();
        }
        public User Login(string email, string password)
        {
            var user = GetByEmail(email);
            if (user == null)
            {
                return null;
            }

            var isValid = BCrypt.Net.BCrypt.Verify(password, user.PasswordHash);
            if (!isValid)
            {
                return null;
            }

            return user;

        }
        public User GetByEmail(string email)
        {
            using var connection = new SqlConnection(_connectionString);
            using var cmd = connection.CreateCommand();
            cmd.CommandText = "SELECT * FROM Users WHERE Email = @email";
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
                Name = (string)reader["Name"],
                Email = (string)reader["Email"],
                PasswordHash = (string)reader["PasswordHash"],
            };
        }
        public List<Ad> GetAllAds()
        {
            using var connection = new SqlConnection(_connectionString);
            using var command = connection.CreateCommand();
            command.CommandText = "SELECT * FROM Ads a " +
                "JOIN Users u " +
                "ON a.UserId = u.Id";
            connection.Open();
            var reader = command.ExecuteReader();
            List<Ad> ads = new List<Ad>();
            while (reader.Read())
            {
                ads.Add(new Ad
                {
                    Id = (int)reader["Id"],
                    Title = (string)reader["Title"],
                    PhoneNumber = (string)reader["PhoneNumber"],
                    DatePosted = (DateTime)reader["DatePosted"],
                    Description = (string)reader["Description"],
                    UserId = (int)reader["UserId"],
                    UserName = (string)reader["Name"]
                });
            }
            return ads;
        }
        public void DeleteAd(int id)
        {
            using var connection = new SqlConnection(_connectionString);
            using var command = connection.CreateCommand();
            command.CommandText = "DELETE FROM Ads WHERE Id = @id";
            command.Parameters.AddWithValue("@id", id);
            connection.Open();
            command.ExecuteNonQuery();
        }
        public List<Ad> GetAdsForUser(int id)
        {
            using var connection = new SqlConnection(_connectionString);
            using var command = connection.CreateCommand();
            command.CommandText = "SELECT * FROM Ads a " +
                "JOIN Users u " +
                "ON a.UserId = u.Id " +
                "WHERE a.UserId = @id";
            command.Parameters.AddWithValue("@id", id);
            connection.Open();
            var reader = command.ExecuteReader();
            List<Ad> ads = new List<Ad>();
            while (reader.Read())
            {
                ads.Add(new Ad
                {
                    Id = (int)reader["Id"],
                    Title = (string)reader["Title"],
                    PhoneNumber = (string)reader["PhoneNumber"],
                    DatePosted = (DateTime)reader["DatePosted"],
                    Description = (string)reader["Description"],
                    UserId = (int)reader["UserId"],
                    UserName = (string)reader["Name"]
                });
            }
            return ads;
        }
    }
}