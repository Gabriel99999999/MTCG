using MTCGServer.BLL;
using MTCGServer.Models;
using Newtonsoft.Json.Linq;
using Npgsql;
using Npgsql.Replication.PgOutput.Messages;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace MTCGServer.DAL
{
    internal class InMemoryUserDao : IUserDao
    {
        private readonly List<User> _users = new();

        public User? GetUserByAuthToken(string authToken)
        {
            User? user = null;
            using (NpgsqlConnection con = GetConnection())
            {
                con.Open();

                string query = @"SELECT * FROM users WHERE token = @Token";
                NpgsqlCommand cmd = new NpgsqlCommand(query, con);
                cmd.Parameters.AddWithValue("token", authToken);
                cmd.Prepare();

                if (con.State == System.Data.ConnectionState.Open)
                {
                    Console.WriteLine("Connected");
                    using NpgsqlDataReader reader = cmd.ExecuteReader();
                    user = createUser(reader);
                }
                else
                {
                    Console.WriteLine("connection not possible");
                }
                con.Close();
            }
            return user;
        }

        public User? GetUserByCredentials(string username, string password)
        {
            User? user;
            user = GetUserByUsername(username);
            
            if(user != null)
            {
                user.Token = $"{username}-msgToken";
                using (NpgsqlConnection con = GetConnection())
                {
                    con.Open();

                    string query = @"UPDATE users SET token = @token WHERE username = @username";
                    NpgsqlCommand cmd = new NpgsqlCommand(query, con);
                    cmd.Parameters.AddWithValue("token", user.Token);
                    cmd.Parameters.AddWithValue("username", username);
                    cmd.Prepare();

                    if (con.State == System.Data.ConnectionState.Open)
                    {
                        Console.WriteLine("Connected");
                        cmd.ExecuteNonQuery();
                        Console.WriteLine("Token updated");

                    }
                    else
                    {
                        Console.WriteLine("connection not possible so token was not updated in database");
                    }
                }
            }
            return user;
            
        }

        public bool InsertUser(User user)
        {
            var inserted = false;

            if (GetUserByUsername(user.Credentials.Username) == null)
            {
                using (NpgsqlConnection con = GetConnection())
                {
                    con.Open();

                    //these colums are not allowed to be null
                    string query = @"INSERT INTO users (username, password, money, name, bio, image, win, loss, elo, token) VALUES (@Username, @Password, @Money, @Name, @Bio, @Image, @Win, @Loss, @Elo, @token)";
                    NpgsqlCommand cmd = new NpgsqlCommand(query, con);
                    cmd.Parameters.AddWithValue("username", user.Credentials.Username);
                    cmd.Parameters.AddWithValue("password", user.Credentials.Password);
                    cmd.Parameters.AddWithValue("money", user.Money);
                    cmd.Parameters.AddWithValue("name", user.UserData.Name);
                    cmd.Parameters.AddWithValue("bio", user.UserData.Bio);
                    cmd.Parameters.AddWithValue("image", user.UserData.Image);
                    cmd.Parameters.AddWithValue("win", user.ScoreboardData.Win);
                    cmd.Parameters.AddWithValue("loss", user.ScoreboardData.Loss);
                    cmd.Parameters.AddWithValue("elo", user.ScoreboardData.Elo);
                    cmd.Parameters.AddWithValue("token", user.Token);
                    cmd.Prepare();
                    
                    if (con.State == System.Data.ConnectionState.Open)
                    {   
                        Console.WriteLine("Connected");
                        cmd.ExecuteNonQuery();
                        con.Close();
                    }
                    else
                    {
                        Console.WriteLine("connection not possible");
                    }
                }

                inserted = true;
            }

           

            return inserted;
        }

        private User? GetUserByUsername(string username)
        {
            //return _users.SingleOrDefault(u => u.Username == username);
            User? user = null;
            using (NpgsqlConnection con = GetConnection())
            {
                con.Open();

                string query = @"SELECT * FROM users WHERE username = @Username";
                NpgsqlCommand cmd = new NpgsqlCommand(query, con);
                cmd.Parameters.AddWithValue("username",username);
                cmd.Prepare();
                
                if (con.State == System.Data.ConnectionState.Open)
                {
                    Console.WriteLine("Connected");
                    using NpgsqlDataReader reader = cmd.ExecuteReader();
                    user = createUser(reader);
                }
                else
                {
                    Console.WriteLine("connection not possible");
                }
                con.Close();
            }
            return user;
        }
  
        private User? createUser(NpgsqlDataReader reader)
        {
            User? user = null;
            
            if (reader.Read())
            {
                Credentials credentials = new Credentials(reader.GetString(0), reader.GetString(1));
                UserData userdata = new UserData(reader.GetString(3), reader.GetString(4), reader.GetString(5));
                ScoreboardData scoreboardData = new ScoreboardData(reader.GetInt16(6), reader.GetInt16(7), reader.GetInt16(8));
                user = new User(credentials, reader.GetInt16(2), userdata, reader.GetString(9), scoreboardData);
            }

            return user;
        }

        public string? GetToken(string username)
        {
            string? token = null;
            using (NpgsqlConnection con = GetConnection())
            {
                con.Open();

                string query = @"SELECT token FROM users WHERE username = @Username";
                NpgsqlCommand cmd = new NpgsqlCommand(query, con);
                cmd.Parameters.AddWithValue("username", username);
                cmd.Prepare();

                if (con.State == System.Data.ConnectionState.Open)
                {
                    Console.WriteLine(username);
                    Console.WriteLine("Connected");
                    using NpgsqlDataReader reader = cmd.ExecuteReader();
                    if(reader.Read()) 
                    { 
                        token = reader.GetString(0); 
                    }
                    else 
                    { 
                        
                    }

                }
                else
                {
                    Console.WriteLine("connection not possible");
                }
                con.Close();
            }
            return token;
        }

        public UserData? GetUserData(string username)
        {
            UserData? userdata = null;
            using (NpgsqlConnection con = GetConnection())
            {
                con.Open();

                string query = @"SELECT name, bio, image FROM users WHERE username = @Username";
                NpgsqlCommand cmd = new NpgsqlCommand(query, con);
                cmd.Parameters.AddWithValue("username", username);
                cmd.Prepare();

                if (con.State == System.Data.ConnectionState.Open)
                {
                    Console.WriteLine("Connected");
                    using NpgsqlDataReader reader = cmd.ExecuteReader();
                    if(reader.Read())
                    {
                        userdata = new UserData(reader.GetString(0), reader.GetString(1), reader.GetString(2));
                    }
                    else
                    {
                        throw new UserNotFoundException();
                    }
                    
                }
                else
                {
                    Console.WriteLine("connection not possible");
                }
                con.Close();

                return userdata;
            }
        }

        public NpgsqlConnection GetConnection()
        {
            return new NpgsqlConnection(@"Server=localhost;Port=10001;User Id=postgres;Password=123;Database=MTCGDB;");
        }
    }
}
