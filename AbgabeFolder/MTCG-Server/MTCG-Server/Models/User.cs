﻿using NpgsqlTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCGServer.Models
{
    public class User
    {
        public Credentials Credentials { get; set; }
        public int Money { get; }
        public ScoreboardData ScoreboardData { get; set; }
        public string Token { get; set; }   
        public UserData UserData { get; set; }
        public List<Card> Stack { get; set; } = new List<Card>();
        public List<Card> Deck { get; set; } = new List<Card>();

        //Konstruktor für das neue erstellen eines Users
        public User(Credentials credentials)
        {
            Credentials = credentials;
            UserData = new UserData();  
            ScoreboardData = new ScoreboardData();
            Money = 20;
            Token = String.Empty;
        }

        //Konstruktor um aus den werten der datenbank den user anzulegen
        public User(Credentials credentials, int money, UserData userdata, string token, ScoreboardData scoreboardData)
        {
            Credentials= credentials;
            Money = money;
            UserData = userdata;
            Token = token;
            ScoreboardData = scoreboardData;
        }


    }
}
