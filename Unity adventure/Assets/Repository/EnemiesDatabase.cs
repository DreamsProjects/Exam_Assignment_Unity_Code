using Assets.Classes;
using Mono.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Repository
{
   public static class EnemiesDatabase
    {
        private static IDbConnection dbcon = new SqliteConnection("URI=file:" + Application.dataPath + @"\StreamingAssets\SqlDatabaseConnection.db");

        private static List<InventoryAndKilledEnemies> enemies = new List<InventoryAndKilledEnemies>();

        public static List<InventoryAndKilledEnemies> ReadKilledEnemies()
        {
            EnemiesConnection("","");
            return enemies;
        }

        public static void AddEnemies(string name)
        {
            EnemiesConnection("add", name);
        }

        public static void EnemiesConnection(string method, string name)
        {
            dbcon.Close();
            dbcon.Open();

            //Lägg till
            if (method == "add")
            {
                //Connection
                IDbCommand cmnd = dbcon.CreateCommand();
                cmnd.CommandText = $"INSERT INTO Enemies (Name, PlayerID) VALUES (\'{name}\', {AccountDatabase.PlayerID})";
                cmnd.ExecuteNonQuery();
            }

            //Hämta
            else
            {
                IDbCommand cmnd_read = dbcon.CreateCommand();
                IDataReader reader;
                string query = $"SELECT * FROM Enemies WHERE PlayerID == {AccountDatabase.PlayerID}";
                cmnd_read.CommandText = query;
                reader = cmnd_read.ExecuteReader();
                while (reader.Read())
                {
                    var enemy = new InventoryAndKilledEnemies {
                        Name = reader["Name"].ToString()
                    };

                    enemies.Add(enemy);
                }
            }

            dbcon.Close();
        }
    }
}