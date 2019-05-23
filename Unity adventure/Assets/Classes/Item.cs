using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Classes
{
    public class Item
    {
        public string Name { get; set; }
        public string IconPath { get; set; }
        public GameObject Object { get; set; }
        public int Amount { get; set; }
        public string Tag { get; set; }

        public List<Potion> Potion = new List<Potion>();
        public List<Food> Food = new List<Food>();
    }

    public class ItemOnGround
    {
        public string Name { get; set; }
        public int Amount { get; set; }
        public string Map { get; set; }
        public Vector2 Position { get; set; }
        public string TagName { get; set; }
    }
}