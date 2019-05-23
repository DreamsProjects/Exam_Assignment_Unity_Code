namespace Assets.Models
{
    public class Player
    {
        public int Speed { get; set; }
        public string CurrentMap { get; set; }
        public string SpawnPoint { get; set; }
        public int CurrentHp { get; set; }
        public int MaximumHp { get; set; }
        public int Damage { get; set; }
    }
}