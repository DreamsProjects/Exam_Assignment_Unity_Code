using System.Collections.Generic;
using System.Linq;


namespace Assets.Classes
{
    public static class QuestRelated
    {
        public static List<QuestNumber> CompletedQuest = new List<QuestNumber>();
        public static int? ActiveQuest;

        public static void AddToQuestList(int questNumber)
        {
            var quest = new QuestNumber { Number = questNumber };
            CompletedQuest.Add(quest);
        }

        public static bool ActivateQuest(int questNumber)
        {
            if (ActiveQuest == null)
            {
                ActiveQuest = questNumber;
                return true;
            }

            else
                return false; //finns redan en aktiverad
        }

        public static bool IsFinished(int number)
        {
            return CompletedQuest.Any(x => x.Number == number);
        }
    }

    public class QuestNumber
    {
        public int Number { get; set; }
    }
}