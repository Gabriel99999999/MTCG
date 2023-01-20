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

namespace MTCGServer.DAL.Cards
{
    internal class DatabaseCardDao : DatabaseDao, ICardDao
    {
        public DatabaseCardDao(string connectionString) : base(connectionString) { }

        public bool ConfigureDeck(User user, List<Guid> guids)
        {
            bool possibleToAdd = true;
            bool updatingWorks = true;
            try
            {
                //remove old cards in deck 
                updatingWorks = ExecuteWithDbConnection((connection) =>
                {
                    //Create the list of Cards the user buy 
                    string query = "UPDATE cards SET deck = false WHERE owner = @username AND deck = true";
                    using NpgsqlCommand cmd = new NpgsqlCommand(query, connection);
                    cmd.Parameters.AddWithValue("username", user.Credentials.Username);
                    cmd.Prepare();
                    
                    if(cmd.ExecuteNonQuery() != -1)
                    {
                        return true;
                    }
                    return false;
                    
                });

                if (!updatingWorks)
                {
                    throw new DataUpdateException();
                }

                foreach (Guid id in guids)
                {
                    possibleToAdd = ExecuteWithDbConnection((connection) =>
                    {
                        //Create the list of Cards the user buy 
                        string query = "SELECT cardid FROM cards WHERE owner = @username AND cardid = @id";
                        using NpgsqlCommand cmd = new NpgsqlCommand(query, connection);
                        cmd.Parameters.AddWithValue("username", user.Credentials.Username);
                        cmd.Parameters.AddWithValue("id", id);
                        cmd.Prepare();
                        using NpgsqlDataReader reader = cmd.ExecuteReader();
                        if (!reader.Read())
                        {
                            return false;
                        }
                        return true; ;
                    });
                    //if owner does not own one id  its not possible to configure the deck
                    if (!possibleToAdd)
                    {
                        return false;
                    }
                }

                //Configuring deck is possible => set bool deck = true
                foreach (Guid id in guids)
                {
                    updatingWorks = ExecuteWithDbConnection((connection) =>
                    {
                        //Create the list of Cards the user buy 
                        string query = "UPDATE cards SET deck = true WHERE cardid = @id";
                        using NpgsqlCommand cmd = new NpgsqlCommand(query, connection);
                        cmd.Parameters.AddWithValue("id", id);
                        cmd.Prepare();
                        if (cmd.ExecuteNonQuery() != 1)
                        {
                            return false;
                        }
                        return true; ;
                    });
                    if (!updatingWorks)
                    {
                        throw new DataUpdateException();
                    }
                }
                return updatingWorks;
            }
            catch (Exception ex)
            {
                if (ex is DataUpdateException) { throw; }
                else if (ex is DataAccessFailedException) { throw; }
                else if (ex is NpgsqlException) { throw; }
                else { throw; }
            }
        }

        public List<Card> GetDeck(User user)
        {
            List<Card> stack = new List<Card>();
            try
            {
                return ExecuteWithDbConnection((connection) =>
                {
                    //Create the list of Cards the user buy 
                    string query = "SELECT cardid, name, damage FROM cards WHERE owner = @username AND deck = true";
                    using NpgsqlCommand cmd = new NpgsqlCommand(query, connection);
                    cmd.Parameters.AddWithValue("username", user.Credentials.Username);
                    cmd.Prepare();
                    using NpgsqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        stack.Add(new Card(reader.GetGuid("cardid"), reader.GetString("name"), reader.GetDecimal("damage")));
                    }
                    return stack;
                });
            }
            catch (DataAccessFailedException)
            {
                throw;
            }
        }

        public List<Card> GetStack(User user)
        {
            List<Card> stack = new List<Card>();
            try
            {
                return ExecuteWithDbConnection((connection) =>
                {
                    //Create the list of Cards the user buy 
                    string query = "SELECT cardid, name, damage FROM cards WHERE owner = @username";
                    using NpgsqlCommand cmd = new NpgsqlCommand(query, connection);
                    cmd.Parameters.AddWithValue("username", user.Credentials.Username);
                    cmd.Prepare();
                    using NpgsqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        stack.Add(new Card(reader.GetGuid("cardid"), reader.GetString("name"), reader.GetDecimal("damage")));
                    }
                    return stack;
                });
            }
            catch (DataAccessFailedException)
            {
                throw;
            }
        }
    }
}
