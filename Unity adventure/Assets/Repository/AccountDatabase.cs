using UnityEngine;
using System.Data;
using Mono.Data.Sqlite;
using System;

namespace Assets.Repository
{
    public static class AccountDatabase
    {
        public static int PlayerID;
        private static IDbConnection dbcon = new SqliteConnection("URI=file:" + Application.dataPath + @"/StreamingAssets/SqlDatabaseConnection.db");

        public static string Register(string username, string password)
        {
            var returned = ConnectionToDb(username, password, "reg");
            return returned;
        }

        public static string Login(string username, string password)
        {
            var returned = ConnectionToDb(username, password, "login");
            return returned;
        }

        public static string ConnectionToDb(string user, string pass, string method)
        {
            var value = "";
            bool hasAlready = false;

            //Kolla om användaren redan finns -- Annars gå vidare
            if (method == "reg")
            {
                hasAlready = HasPlayer(user);

                if (hasAlready)
                    return "error";
            }

            dbcon.Close();
            dbcon.Open();

            if (method == "reg")
            {
                IDbCommand cmnd = dbcon.CreateCommand();
                cmnd.CommandText = $"INSERT INTO [AspNetUser] (Username, Password) VALUES (\'{user}\', \'{pass}\')";
                PlayerDatabase.Username = user;
                cmnd.ExecuteNonQuery();

                IDbCommand cmnd_read = dbcon.CreateCommand();
                IDataReader reader;
                string query = $"SELECT * FROM [AspNetUser] ";
                cmnd_read.CommandText = query;
                reader = cmnd_read.ExecuteReader();
                while (reader.Read())
                {
                    var firstValue = reader["Username"].ToString();
                    var passValue = reader["Password"].ToString();

                    if (firstValue.ToLower() == user.ToLower() && passValue == pass)
                    {
                        var getUserId = reader["PlayerID"].ToString();
                        PlayerID = Convert.ToInt32(getUserId);
                    }
                }
            }

            else
            {

                IDbCommand cmnd_read = dbcon.CreateCommand();
                IDataReader reader;
                string query = $"SELECT * FROM [AspNetUser] ";
                cmnd_read.CommandText = query;
                reader = cmnd_read.ExecuteReader();
                while (reader.Read())
                {
                    var firstValue = reader["Username"].ToString();
                    var passValue = reader["Password"].ToString();

                    if (firstValue.ToLower() == user.ToLower() && passValue == pass)
                    {
                        var getUserId = reader["PlayerID"].ToString();
                        PlayerID = Convert.ToInt32(getUserId);
                    }
                }
            }

            dbcon.Close();

            return value;
        }

        public static bool HasPlayer(string user)
        {
            dbcon.Open();

            IDbCommand createCommand = dbcon.CreateCommand();

            string query = $"SELECT * FROM [AspNetUser]";

            createCommand.CommandText = query;

            IDataReader reader = createCommand.ExecuteReader();

            while (reader.Read())
            {
                var firstValue = reader["Username"].ToString();

                if (firstValue.ToLower() == user.ToLower())
                {
                    return true;
                }
            }

            dbcon.Close();

            return false;
        }
    }
}