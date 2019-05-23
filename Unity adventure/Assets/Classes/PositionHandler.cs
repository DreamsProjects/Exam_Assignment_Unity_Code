using UnityEngine;

namespace Assets.Classes
{
    public class Position
    {
        public string Scene { get; set; }
        public Vector2 PositionOnMap { get; set; }
        public Collider2D BoxCollider { get; set; }
    }
}