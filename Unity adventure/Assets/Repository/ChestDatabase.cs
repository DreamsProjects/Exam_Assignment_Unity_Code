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
   public static class ChestDatabase
    {
        //private static IDbConnection dbcon = new SqliteConnection(@"Data Source= .\SqlDatabaseConnection.db;");
        //private static IDbConnection dbcon = new SqliteConnection(@"C:\Users\emma\Desktop\New game\Unity adventure\Assets\Builds\Database\SqlDatabaseConnection.db");
        // private static IDbConnection dbcon = new SqliteConnection("URI=file:" + Application.dataPath + "/SqlDatabaseConnection.db");
        private static IDbConnection dbcon = new SqliteConnection("URI=file:" + Application.dataPath + @"\StreamingAssets\SqlDatabaseConnection.db");

        private static List<Chest> chests = new List<Chest>();

        public static List<Chest> OpenedChests()
        {
            DatabaseChest("", "read");

            return chests;
        }

        public static void AddChest(string map)
        {
            DatabaseChest(map, "add");

            var chest = new Chest
            {
                Scene = map
            };

            chests.Add(chest);
        }

        public static void DatabaseChest(string map, string method)
        {
            dbcon.Close();
            dbcon.Open();

            //Lägg till som öppnad
            if (method == "add")
            {
                //Connection
                IDbCommand cmnd = dbcon.CreateCommand();

                cmnd.CommandText = $"INSERT INTO [OpenedChest] (PlayerID, Map) VALUES ({AccountDatabase.PlayerID}, \'{map}\')";
                cmnd.ExecuteNonQuery();
            }

            //Hämta
            else
            {
                IDbCommand cmnd_read = dbcon.CreateCommand();
                IDataReader reader;
                string query = $"SELECT * FROM [OpenedChest] WHERE PlayerID == {AccountDatabase.PlayerID}";
                cmnd_read.CommandText = query;
                reader = cmnd_read.ExecuteReader();
                while (reader.Read())
                {
                    var chest = new Chest
                    {
                        Scene = reader["Map"].ToString()
                    };

                    chests.Add(chest);
                }
            }

            dbcon.Close();
        }
    }
}