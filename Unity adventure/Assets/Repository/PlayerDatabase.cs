using Assets.Models;
using Mono.Data.Sqlite;
using System.Data;
using UnityEngine;

namespace Assets.Repository
{
    public static class PlayerDatabase
    {
        private static IDbConnection dbcon = new SqliteConnection("URI=file:" + Application.dataPath + @"\StreamingAssets\SqlDatabaseConnection.db");
        public static string Username;
        private static Player player;

        //Hämta spelaren
        public static Player PlayerStats()
        {
            PlayerConnection("player", 0, "", "");
            return player;
        }

        public static void SetHealth(int currentHP)
        {
            PlayerConnection("updateHP", currentHP, "", "");
        }

        public static void AddPlayer()
        {
            PlayerConnection("add", 100, "start", "spawn");
        }

        public static void PlayerConnection(string method, int currentHP, string scene, string spawnPoint)
        {
            dbcon.Close();
            dbcon.Open();

            if (method == "updateHP")
            {
                IDbCommand cmnd = dbcon.CreateCommand();

                cmnd.CommandText = $"Update [Player] set  CurrentHP = '{currentHP}' where PlayerID == {AccountDatabase.PlayerID}";
                cmnd.ExecuteNonQuery();
            }

            else if (method == "add")
            {
                IDbCommand cmnd = dbcon.CreateCommand();

                cmnd.CommandText = $"Insert into Player(Speed, CurrentMap, StartPoint, CurrentHP," +
                    $" MaximumHP,Damage, Username) " +
                    $"values(5, 'start', 'spawn', 100, 100, 5, '{Username}')";

                cmnd.ExecuteNonQuery();
            }

            else
            {
                IDbCommand cmnd_read = dbcon.CreateCommand();
                IDataReader reader;
                string query = $"SELECT * FROM [Player] WHERE PlayerID == {AccountDatabase.PlayerID}";
                cmnd_read.CommandText = query;
                reader = cmnd_read.ExecuteReader();
                while (reader.Read())
                {
                    player = new Player
                    {
                        Speed = int.Parse(reader["Speed"].ToString()),
                        CurrentHp = int.Parse(reader["CurrentHP"].ToString()),
                        MaximumHp = int.Parse(reader["MaximumHP"].ToString()),
                        Damage = int.Parse(reader["Damage"].ToString()),
                        CurrentMap = reader["CurrentMap"].ToString(),
                        SpawnPoint = reader["StartPoint"].ToString()
                    };
                }
            }
            dbcon.Close();
        }
    }
}