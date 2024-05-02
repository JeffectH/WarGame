using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using System.Runtime.CompilerServices;

namespace WarGame
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Battlefield battlefield = new Battlefield(55);
        }
    }

    class Battlefield
    {
        private int _numberSoldierPlatoon;
        private int _minNumberSoldierPlatoon = 15;
        private int _maxNumberSoldierPlatoon = 60;
        private bool _isFight = true;

        public Battlefield(int numberSoldierPlatoon)
        {
            _numberSoldierPlatoon = (numberSoldierPlatoon < _minNumberSoldierPlatoon) ? _minNumberSoldierPlatoon : (numberSoldierPlatoon > _maxNumberSoldierPlatoon) ? _maxNumberSoldierPlatoon : numberSoldierPlatoon;
        }

        Soldier SoldierTypeA = new Soldier("Иван “Простой”", 100, 15, 50);
        Soldier SoldierTypeB = new SingleTargetHighDamageSoldier("Алексей “Убийца”", 100, 15, 50, 2.5f);
        Soldier SoldierTypeC = new MultiTargetSoldier("Наталья “Множитель”", 100, 15, 50, 2, false);
        Soldier SoldierTypeD = new MultiTargetSoldier("Дмитрий “Многократный”", 100, 15, 50, 3, true);

        public void Fight()
        {
            while (_isFight)
            {
                Console.WriteLine("" +
                    "Иван Простой - обычный солдат, но надежный и преданный своему долгу.\r\n" +
                    "Алексей “Убийца” - атакует одного противника, но с огромным уроном. Его враги дрожат при упоминании его имени.\r\n" +
                    "Наталья “Множитель” - атакует группу противников одновременно, не повторяя свои атаки. Она как вихрь смерти на поле боя.\r\n" +
                    "Дмитрий “Многократный” - атакует несколько противников одновременно, и его атакованные солдаты могут вновь вступать в бой. Он создает цепную реакцию разрушения.");

                Console.WriteLine("Введите колличество человек в взводе от 15 до 60");

                if (int.TryParse(Console.ReadLine(), out int userInput))
                {
                    Console.WriteLine("Происходит формирование взвода...");
                }
                else
                {
                    Console.WriteLine("Введены некорректные данные!");
                }
            }
        }

        private void FormationPlatoon()
        {

        }
    }

    class Soldier
    {
        protected string Name;
        protected float Health;
        protected float Damage;
        protected float Armor;

        public Soldier(string name, float health, float damage, float armor)
        {
            Name = name;
            Health = health;
            Damage = damage;
            Armor = armor;
        }

        public void TakeDamage(float damage)
        {
            float damageDealt = Math.Max(0, damage - Armor);
            Armor -= Damage;
            Health -= damageDealt;
        }
    }

    class SingleTargetHighDamageSoldier : Soldier
    {
        public SingleTargetHighDamageSoldier(string name, int health, int damage, int armor, float damageMultiplier) : base(name, health, damage, armor)
        {
            Damage *= damageMultiplier;
        }
    }

    class MultiTargetSoldier : Soldier
    {
        private int _numberSoldiersPerStrike;
        private bool _isPossibleHitAgain;
        private List<int> _attackedSoldierNumbers = new List<int>();

        public MultiTargetSoldier(string name, int health, int damage, int armor, int numberSoldiersPerStrike, bool isPossibleHitAgain) : base(name, health, damage, armor)
        {
            _numberSoldiersPerStrike = numberSoldiersPerStrike;
            _isPossibleHitAgain = isPossibleHitAgain;
        }

        public void Attack(List<Soldier> soldiers)
        {
            for (int i = 0; i < _numberSoldiersPerStrike; i++)
            {


                // Console.WriteLine($"{Name} атакует {solder.Name} и наносит {damageDealt} ед. урона.");
            }
        }

        private Soldier SelectionSoldiers()
        {
            int randomIndexSoldier = UserUtils.GenerateRandomNumber(0, _numberSoldiersPerStrike);

            if (_isPossibleHitAgain == false)
            {

            }


        }

    }

    class UserUtils
    {
        private static Random s_random = new Random();

        public static int GenerateRandomNumber(int min, int max) =>
            s_random.Next(min, max);
    }
}

