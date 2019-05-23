using Assets.Classes;
using Mono.Data.Sqlite;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

namespace Assets.Repository
{
    public static class InventoryDatabase
    {
        private static IDbConnection dbcon = new SqliteConnection("URI=file:" + Application.dataPath + @"/StreamingAssets/SqlDatabaseConnection.db");
        private static List<ItemOnGround> onGround = new List<ItemOnGround>();
        private static List<Inventory> playersBackpackInventory = new List<Inventory>();

        //Läser in items från databasen
        public static List<Inventory> PlayersInventory()
        {
            InventoryConnection("get", "", "", "", "");
            return playersBackpackInventory;
        }

        public static void RemoveFromInventory(string nameOnItem)
        {
            InventoryConnection("remove", nameOnItem, "", "", "");
        }

        public static void AddToInventory(string nameOnItem, string path, string category, string tagname)
        {
            InventoryConnection("add", nameOnItem, path, category, tagname);
        }

        //Föremål på kartan måste läsas in med!
        public static List<ItemOnGround> ItemsOnMap()
        {
            ItemConnection("get", "", "", "");

            return onGround;
        }

        //Ta bort föremål från table om den plockas upp
        public static void RemoveItemsFromMap(string nameOnItem, string map, string position)
        {
            ItemConnection("remove", nameOnItem, map, position);
        }

        public static void DefaultItems(string nameOnItem, string map, string position)
        {
            ItemConnection("startItems", nameOnItem, map, position);
        }

        public static void AddItemsToMap(string nameOnItem, string map, string position)
        {
            ItemConnection("add", nameOnItem, map, position);
        }

        public static void InventoryConnection(string method, string item, string path, string category, string tagname)
        {
            if (tagname == "") tagname = " ";

            dbcon.Close();
            dbcon.Open();

            if (method == "add")
            {
                //Connection
                IDbCommand cmnd = dbcon.CreateCommand();
                cmnd.CommandText = $"INSERT INTO [Inventory] (PlayerID, Path, Name, ReplacedName, CategoryName,TagName) VALUES ({AccountDatabase.PlayerID}, \'{path}\', \'{item}\', \'{item}\', \'{category}\', \'{tagname}\')";
                cmnd.ExecuteNonQuery();
            }

            else if (method == "remove")
            {
                //Connection
                IDbCommand cmnd = dbcon.CreateCommand();
                cmnd.CommandText = $"DELETE from [Inventory] WHERE RowID IN(SELECT RowID FROM [Inventory] WHERE PlayerID == {AccountDatabase.PlayerID} AND Name == \'{item}\')";
                cmnd.ExecuteNonQuery();
            }

            //Ge spelaren inventory
            else
            {
                IDbCommand cmnd_read = dbcon.CreateCommand();
                IDataReader reader;
                string query = $"SELECT * FROM [Inventory] WHERE PlayerID == {AccountDatabase.PlayerID} ";
                cmnd_read.CommandText = query;
                reader = cmnd_read.ExecuteReader();
                while (reader.Read())
                {
                    var invent = new Inventory
                    {
                        replacedName = reader["Name"].ToString(),
                        nameOnItem = reader["Name"].ToString(),
                        howMany = 1,
                        tagName = reader["TagName"].ToString(),
                        background = reader["Path"].ToString(),
                        CategoryName = reader["CategoryName"].ToString()
                    };

                    playersBackpackInventory.Add(invent);
                }
            }
            dbcon.Close();
        }

        public static void ItemConnection(string method, string item, string map, string position)
        {
            dbcon.Close();
            dbcon.Open();

            //OM SPELAREN PLOCKAR UPP FÖREMÅL SÅ LÄGGS DEN I HÄR
            if (method == "remove")
            {
                //Connection
                IDbCommand cmnd = dbcon.CreateCommand();

                cmnd.CommandText = $"Delete from [OnMap] where PlayerID == {AccountDatabase.PlayerID} AND Name == \'{item}\' AND Map == \'{map}\' AND Position == \'{position}\'";
                cmnd.ExecuteNonQuery();
            }

            //lägger till på kartan
            else if (method == "add")
            {
                //Connection
                IDbCommand cmnd = dbcon.CreateCommand();

                cmnd.CommandText = $"INSERT INTO [OnMap]  (PlayerID, Name , Map, Position) VALUES ({AccountDatabase.PlayerID}, \'{item}\', \'{map}\', \'{position}\')";
                cmnd.ExecuteNonQuery();
            }

            else if (method == "startItems")
            {
                IDbCommand cmnd = dbcon.CreateCommand();

                cmnd.CommandText = $"INSERT INTO [OnMap]  (PlayerID, Name, Map, Position) VALUES ({AccountDatabase.PlayerID}, 'Ball', 'start' , '(18,5, 13,1)')";
                cmnd.ExecuteNonQuery();
                cmnd.CommandText = $"INSERT INTO [OnMap]  (PlayerID, Name, Map, Position) VALUES ({AccountDatabase.PlayerID}, 'Stronger','start', '(1,5, 4,5)')";
                cmnd.ExecuteNonQuery();
                cmnd.CommandText = $"INSERT INTO [OnMap]  (PlayerID, Name, Map, Position) VALUES ({AccountDatabase.PlayerID}, 'Love','start', '(0,4, 4,5)')";
                cmnd.ExecuteNonQuery();
                cmnd.CommandText = $"INSERT INTO [OnMap]  (PlayerID, Name, Map, Position) VALUES ({AccountDatabase.PlayerID}, 'Healing', 'house', '(22,7, 13,6)')";
                cmnd.ExecuteNonQuery();
                cmnd.CommandText = $"INSERT INTO [OnMap]  (PlayerID, Name, Map, Position) VALUES ({AccountDatabase.PlayerID}, 'TilTheEnd', 'castle', '(562,4, 660,5)')";
                cmnd.ExecuteNonQuery();
                cmnd.CommandText = $"INSERT INTO [OnMap]  (PlayerID, Name, Map, Position) VALUES ({AccountDatabase.PlayerID}, 'TilTheEnd', 'castle', '(563,6, 660,5)')";
                cmnd.ExecuteNonQuery();
                cmnd.CommandText = $"INSERT INTO [OnMap]  (PlayerID, Name, Map, Position) VALUES ({AccountDatabase.PlayerID}, 'Dragon Seed', 'wonderlandQuestOne', '(564,5, 670,0)')";
                cmnd.ExecuteNonQuery();
                cmnd.CommandText = $"INSERT INTO [OnMap]  (PlayerID, Name, Map, Position) VALUES ({AccountDatabase.PlayerID}, 'Stronger', 'wonderlandQuestTwo', '(561,5, 673,5)')";
                cmnd.ExecuteNonQuery();
            }

            //HÄMTA ALLA ITEMS OCH LÄGG IN I GLOBALA LISTAN
            else
            {
                IDbCommand cmnd_read = dbcon.CreateCommand();
                IDataReader reader;
                string query = $"SELECT * FROM [OnMap] WHERE PlayerID == {AccountDatabase.PlayerID}";
                cmnd_read.CommandText = query;
                reader = cmnd_read.ExecuteReader();
                while (reader.Read())
                {
                    var positionOnMap = reader["Position"].ToString();
                    var removeSigns = positionOnMap.Replace("(", "").Replace(")", "").Replace(" ", "");

                    string[] positions = removeSigns.Split(',');

                    var items = new ItemOnGround
                    {
                        //Name, Map, Position
                        Name = reader["Name"].ToString(),
                        Map = reader["Map"].ToString(),
                        Position = new Vector2(
                            float.Parse(positions[0] + "," + positions[1]),
                            float.Parse(positions[2] + "," + positions[3]))
                    };

                    onGround.Add(items);
                }
            }
        }
    }
}