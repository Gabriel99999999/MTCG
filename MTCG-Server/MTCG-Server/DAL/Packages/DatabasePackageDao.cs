using MTCGServer.DAL.Exceptions;
using MTCGServer.Models;
using Npgsql;
using SWE1.MessageServer.DAL;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Dynamic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace MTCGServer.DAL.Packages
{
    internal class DatabasePackageDao : DatabaseDao, IPackageDao
    {
        public DatabasePackageDao(string connectionString) : base(connectionString) { }

        public bool AddPackage(Package package)
        {
            bool worksFine = true;
            try
            {
                //insert cards into database
                worksFine = ExecuteWithDbConnection((connection2) =>
                {
                    //check if id are unique
                    foreach (Card card in package.PackageOfCards)
                    {
                        string query1 = "SELECT name FROM cards WHERE cardid=@cardid";
                        using NpgsqlCommand cmd1 = new NpgsqlCommand(query1, connection2);
                        cmd1.Parameters.AddWithValue("cardid", card.Id);
                        cmd1.Prepare();
                        using NpgsqlDataReader reader = cmd1.ExecuteReader();

                        if (reader.Read())
                        {
                            return worksFine = false;
                        }
                    }
                    //insert the cards
                    string query2;
                    foreach (Card card in package.PackageOfCards)
                    {
                        query2 = $"INSERT INTO cards (cardid, fightable, element, name, damage, owner) VALUES (@cardid, @fightable, @element, @name, @damage, '')";
                        using NpgsqlCommand cmd2 = new NpgsqlCommand(query2, connection2);
                        cmd2.Parameters.AddWithValue("cardid", card.Id);
                        cmd2.Parameters.AddWithValue("fightable", card.Fightable);
                        cmd2.Parameters.AddWithValue("element", card.Element.ToString());
                        cmd2.Parameters.AddWithValue("name", card.Name.ToString());
                        cmd2.Parameters.AddWithValue("damage", card.Damage);
                        cmd2.Prepare();
                        if (cmd2.ExecuteNonQuery() != 1)
                        {
                            return worksFine = false;
                        }
                    }
                    return worksFine;
                });
                if (worksFine)
                {

                    //add a new package id into the table package
                    worksFine = ExecuteWithDbConnection((connection1) =>
                    {
                        int indexOfNextPackage = 1;
                        string query = "INSERT INTO packages (bought) VALUES (@bought) RETURNING packageid";
                        using NpgsqlCommand cmd = new NpgsqlCommand(query, connection1);
                        cmd.Parameters.AddWithValue("bought", false);
                        cmd.Prepare();

                        var result = cmd.ExecuteScalar();
                        indexOfNextPackage = Convert.ToInt32(result);

                        string? query3 = null;
                        foreach (Card card in package.PackageOfCards)
                        {
                            query3 = $"INSERT INTO match_cards_to_package (packageid, cardid) VALUES (@packageid, @cardid)";
                            using NpgsqlCommand cmd3 = new NpgsqlCommand(query3, connection1);
                            cmd3.Parameters.AddWithValue("packageid", indexOfNextPackage);
                            cmd3.Parameters.AddWithValue("cardid", card.Id);
                            cmd3.Prepare();
                            if (cmd3.ExecuteNonQuery() != 1)
                            {
                                return worksFine = false;
                            }
                        }
                        return worksFine;
                    });
                }

                return worksFine;
            }
            catch (Exception ex)
            {
                if (ex is DataAccessFailedException)
                {
                    throw;
                }
                if (ex is PostgresException)
                {
                    Console.WriteLine(ex.Message);
                    throw;
                }
                else
                {
                    Console.WriteLine(ex.Message);
                    throw;
                }

            }

        }
        public List<Card>? BuyPackage(User user)
        {
            List<Card> package = new List<Card>();
            try
            {
                if (user.Money >= 5)
                {
                    package = ExecuteWithDbConnection((connection) =>
                    {
                        //Create the list of Cards the user buy 
                        string query = "SELECT cardid, name, damage FROM cards WHERE cardid = ANY (SELECT cardid FROM match_cards_to_package WHERE packageid = (SELECT MIN(packageid) FROM packages WHERE bought = false))";
                        using NpgsqlCommand cmd1 = new NpgsqlCommand(query, connection);
                        cmd1.Prepare();
                        using NpgsqlDataReader reader = cmd1.ExecuteReader();
                        while (reader.Read())
                        {
                            package.Add(new Card(reader.GetGuid("cardid"), reader.GetString("name"), reader.GetDecimal("damage")));
                        }
                        return package;
                    });

                    //update: the bougt value in packages, owner of the bought card, money of the user 
                    if (package.Any())
                    {
                        if (updateOwner(user) && updateMoney2(user) && updateBoughtValue())
                        {
                            return package;
                        }
                    }
                    return null;
                }
                else
                {
                    throw new NotEnoughMoneyException();
                }

            }
            catch (Exception ex)
            {
                if (ex is NotEnoughMoneyException)
                {
                    throw;
                }
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

        private bool updateMoney2(User user)
        {
            try
            {
                return ExecuteWithDbConnection((connection) =>
                {
                    int currentMoney = user.Money - 5;
                    string updateMoney = "UPDATE users SET money = @money WHERE username = @username";
                    using NpgsqlCommand cmd4 = new NpgsqlCommand(updateMoney, connection);
                    cmd4.Parameters.AddWithValue("money", currentMoney);
                    cmd4.Parameters.AddWithValue("username", user.Credentials.Username);
                    cmd4.Prepare();
                    return cmd4.ExecuteNonQuery() == 1;
                });
            }
            catch (DataAccessFailedException)
            {
                throw;
            }
        }

        private bool updateOwner(User user)
        {
            try
            {
                return ExecuteWithDbConnection((connection) =>
                {
                    //Change owner of the cards
                    string changeOwner = "UPDATE cards SET owner = @username WHERE cardid = ANY (SELECT cardid FROM match_cards_to_package WHERE packageid = (SELECT MIN(packageid) FROM packages WHERE bought = false))";
                    using NpgsqlCommand cmd3 = new NpgsqlCommand(changeOwner, connection);
                    cmd3.Parameters.AddWithValue("username", user.Credentials.Username);
                    return cmd3.ExecuteNonQuery() == 5;
                });
            }
            catch (DataAccessFailedException)
            {
                throw;
            }
        }

        private bool updateBoughtValue()
        {
            try
            {
                return ExecuteWithDbConnection((connection) =>
                {
                    string updateBoughtValue = "UPDATE packages SET bought = true WHERE packageid = (SELECT MIN(packageid) FROM packages WHERE bought = false)";
                    using NpgsqlCommand cmd2 = new NpgsqlCommand(updateBoughtValue, connection);
                    return cmd2.ExecuteNonQuery() == 1;
                });
            }
            catch (DataAccessFailedException)
            {
                throw;
            }
        }
    }
}
