using Assets.Classes;
using Mono.Data.Sqlite;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

namespace Assets.Repository
{
    public static class QuestDatabase
    {
        //private static IDbConnection dbcon = new SqliteConnection(@"Data Source= .\SqlDatabaseConnection.db;");
        //private static IDbConnection dbcon = new SqliteConnection(@"C:\Users\emma\Desktop\New game\Unity adventure\Assets\Builds\Database\SqlDatabaseConnection.db");
        private static IDbConnection dbcon = new SqliteConnection("URI=file:" + Application.dataPath + @"\StreamingAssets\SqlDatabaseConnection.db");

        private static List<Quests> finishedQuests = new List<Quests>();

        public static void DoneQuests()
        {
            QuestConnection("select", 0);
        }

        public static void AnyActivated()
        {
            QuestConnection("activated", 0);
        }

        public static void AddQuest(int quest)
        {
            QuestConnection("add", quest);
        }

        public static void ActiveQuest(int Activequest)
        {
            QuestConnection("active", Activequest);
        }

        public static void NoActiveQuest(int removeActiveQuest)
        {
            QuestConnection("remove", removeActiveQuest);
        }

        public static void QuestConnection(string method, int questNumber)
        {
            dbcon.Close();
            dbcon.Open();

            //LÄGG TILL SOM AKTIVERAD QUEST
            if (method == "active")
            {
                //Connection
                IDbCommand cmnd = dbcon.CreateCommand();

                cmnd.CommandText = $"Insert into ActiveQuest(PlayerID, QuestNumber) values({AccountDatabase.PlayerID}, {questNumber})";
                cmnd.ExecuteNonQuery();
            }

            //TA BORT AKTIVERAD QUEST --Finns alltid bara en som är aktiverad
            else if (method == "remove")
            {
                //Connection
                IDbCommand cmnd = dbcon.CreateCommand();
                cmnd.CommandText = $"Delete from ActiveQuest where PlayerID == {AccountDatabase.PlayerID}";
                cmnd.ExecuteNonQuery();
            }

            //Ta fram aktiverad quest om det finns
            else if (method == "activated")
            {
                IDbCommand cmnd_read = dbcon.CreateCommand();
                IDataReader reader;
                string query = $"SELECT * FROM ActiveQuest WHERE PlayerID == {AccountDatabase.PlayerID}";
                cmnd_read.CommandText = query;
                reader = cmnd_read.ExecuteReader();
                while (reader.Read())
                {
                   var hasValue = reader["QuestID"].ToString();

                    var quest = new QuestNumber
                    {
                        Number = int.Parse(reader["QuestID"].ToString())
                    };

                    QuestRelated.ActiveQuest = quest.Number;
                }
            }

            //LÄGG TILL SOM AVKLARAD QUEST
            else if (method == "add")
            {
                //Connection
                IDbCommand cmnd = dbcon.CreateCommand();

                cmnd.CommandText = $"Insert into FinishedQuest(PlayerID, QuestID) values ({AccountDatabase.PlayerID}, {questNumber})";
                cmnd.ExecuteNonQuery();
            }

            //TA FRAM ALLA AVKLARADE QUEST --select
            else
            {
                IDbCommand cmnd_read = dbcon.CreateCommand();
                IDataReader reader;
                string query = $"SELECT * FROM FinishedQuest WHERE PlayerID == {AccountDatabase.PlayerID}";
                cmnd_read.CommandText = query;
                reader = cmnd_read.ExecuteReader();
                while (reader.Read())
                {
                    var quest = new QuestNumber
                    {
                        Number = int.Parse(reader["QuestID"].ToString())
                    };
                    QuestRelated.CompletedQuest.Add(quest);
                }
            }

            dbcon.Close();
        }
    }
}