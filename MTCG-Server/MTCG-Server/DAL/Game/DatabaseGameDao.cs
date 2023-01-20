using MTCGServer.DAL.Exceptions;
using MTCGServer.Models;
using Npgsql;
using SWE1.MessageServer.DAL;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCGServer.DAL.Game
{
    internal class DatabaseGameDao : DatabaseDao, IGameDao
    {
        public DatabaseGameDao(string connectionString) : base(connectionString) { }

        public List<ScoreboardData> GetScoreboard()
        {
            List<ScoreboardData> scoreboard = new List<ScoreboardData>();
            try
            {
                return ExecuteWithDbConnection((connection) =>
                {
                    //Create the list of ScoreboardData in descending order
                    string query = "SELECT name, elo, wins, losses FROM users ORDER BY elo DESC";
                    using NpgsqlCommand cmd = new NpgsqlCommand(query, connection);
                    cmd.Prepare();
                    using NpgsqlDataReader reader = cmd.ExecuteReader();
                    while(reader.Read())
                    {
                        //bereits sortiert
                        scoreboard.Add(new ScoreboardData(reader.GetString("name"), reader.GetInt16("elo"), reader.GetInt16("wins"), reader.GetInt16("losses")));
                    }
                    return scoreboard;
                });
            }
            catch (Exception ex)
            {
                if (ex is DataAccessFailedException)
                {
                    throw;
                }
                else
                {
                    throw;
                }
            }
        }

        /*public ScoreboardData? GetIndividuelScoreboardData(User user)
        {
            ScoreboardData? stats = null;
            try
            {
                return ExecuteWithDbConnection((connection) =>
                {
                    string query = "SELECT name, elo, wins, losses FROM users WHERE username = @username";
                    using NpgsqlCommand cmd = new NpgsqlCommand(query, connection);
                    cmd.Parameters.AddWithValue("username", user.Credentials.Username);
                    cmd.Prepare();
                    using NpgsqlDataReader reader = cmd.ExecuteReader();
                    if(reader.Read())
                    {
                        stats = new ScoreboardData(reader.GetString("name"), reader.GetInt16("elo"), reader.GetInt16("wins"), reader.GetInt16("losses"));
                    }
                    return stats;
                });
            }
            catch (Exception ex)
            {
                if (ex is DataAccessFailedException)
                {
                    throw;
                }
                else
                {
                    throw;
                }
            }
        }*/
    }
}
