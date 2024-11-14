using SmallConsoleJRPG;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmallConsoleJRPG
{
    public class Enemy
    {
        public string Key { get; set; }
        public string Name { get; set; }
        public int level { get; set; }
        public bool IsDead { get; set; }
        public int MaxHP { get; set; }
        public int CurrentHP { get; set; }
        public int Strength { get; set; }
        public int Defense { get; set; }
        public string size { get; set; }
        public int position { get; set; }
        public Animations animation { get; set; }

        public Enemy(string key, string name, int level, bool isDead, int maxHP, int currentHP, int strength, int defense, string size, int position, Animations anim)
        {
            Key = key;
            Name = name;
            this.level = level;
            IsDead = isDead;
            MaxHP = maxHP;
            CurrentHP = currentHP;
            Strength = strength;
            Defense = defense;
            this.size = size;
            this.position = position;
            animation = anim;
        }
    }
}
