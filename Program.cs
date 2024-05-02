using System;

namespace WarGame
{
    internal class Program
    {
        static void Main(string[] args)
        {

        }
    }

    class Battlefield
    {
        
    }

    class Soldier
    {
        protected string Name;
        protected int Health;
        protected int Damage;
        protected int Armor;

        public Soldier(string name, int health, int damage, int armor)
        {
            Name = name;
            Health = health;
            Damage = damage;
            Armor = armor;
        }

        public void Attack(Soldier solder)
        {
            int damageDealt = Math.Max(0, Damage - solder.Armor);
            solder.Health -= damageDealt;
            Console.WriteLine($"{Name} атакует {solder.Name} и наносит {damageDealt} ед. урона.");
        }
    }

    class SingleTargetHighDamageSoldier : Soldier
    {
        public SingleTargetHighDamageSoldier(string name, int health, int damage, int armor) : base(name, health, damage, armor)
        {

        }
    }

    class MultiTargetSoldier : Soldier
    {
        public MultiTargetSoldier(string name, int health, int damage, int armor) : base(name, health, damage, armor)
        {

        }
    }

}
