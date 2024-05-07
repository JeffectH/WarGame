using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace WarGame
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Battlefield battlefield = new Battlefield(10);
            battlefield.Fight();
        }
    }

    class Battlefield
    {
        private int _numberSoldierPlatoon;
        private int _minNumberSoldierPlatoon = 15;
        private int _maxNumberSoldierPlatoon = 60;
        private int _minNumberSoldiersAttacked = 2;
        private int _maxNumberSoldiersAttacked = 4;
        private Soldier _soldierTypeA;
        private Soldier _soldierTypeB;
        private Soldier _soldierTypeC;
        private Soldier _soldierTypeD;
        private List<Soldier> _platoonFirst = new List<Soldier>();
        private List<Soldier> _platoonSecond = new List<Soldier>();

        public Battlefield(int numberSoldierPlatoon)
        {
            _numberSoldierPlatoon = (numberSoldierPlatoon < _minNumberSoldierPlatoon) ? _minNumberSoldierPlatoon : (numberSoldierPlatoon > _maxNumberSoldierPlatoon) ? _maxNumberSoldierPlatoon : numberSoldierPlatoon;

            _soldierTypeA = new Soldier("Иван “Простой”", 400, 15, 50);
            _soldierTypeB = new SingleTargetHighDamageSoldier("Алексей “Убийца”", 100, 15, 50, 2.5f);
            _soldierTypeC = new MultiTargetSoldier("Наталья “Множитель”", 200, 15, 50, false);
            _soldierTypeD = new MultiTargetSoldier("Дмитрий “Многократный”", 200, 15, 50, true);
        }

        public void Fight()
        {
            Console.WriteLine("Сейчас будет битва. 2 взвода. В каждом взводе по " + _numberSoldierPlatoon + " солдат\r\n");
            Console.WriteLine("В каждом взводе присутствуют 4 типа солдат.\r\n");
            Console.WriteLine("" +
                "Иван “Простой” - обычный солдат, но надежный и преданный своему долгу.\r\n" +
                "Алексей “Убийца” - атакует одного противника, но с огромным уроном. Его враги дрожат при упоминании его имени.\r\n" +
                "Наталья “Множитель” - атакует группу противников одновременно, не повторяя свои атаки. Она как вихрь смерти на поле боя.\r\n" +
                "Дмитрий “Многократный” - атакует несколько противников одновременно, и его атакованные солдаты могут вновь вступать в бой.");

            Console.WriteLine("\nПроисходит формирование взвода...");
            FormationPlatoon();
            Console.WriteLine("\nВсе готово! Можно начинать битву!");
            Console.ReadKey();
            Console.Clear();

            while (_platoonFirst.Count != 0 && _platoonSecond.Count != 0)
            {
                Console.WriteLine("\nАттакует взвод №1 \n");
                AttackPlatoon(_platoonFirst, _platoonSecond);
                Console.WriteLine("\nАттакует взвод №2 \n");
                AttackPlatoon(_platoonSecond, _platoonFirst);

                Console.WriteLine($"\nКолличество человек в взводе №1: {_platoonFirst.Count}." + "\n" +
                    $"Колличество человек в взводе №2: {_platoonSecond.Count}.");

                Console.ReadKey();
            }

            if (_platoonFirst.Count == 0 && _platoonSecond.Count == 0)
            {
                Console.WriteLine("\nНичья");
            }
            else if (_platoonFirst.Count == 0)
            {
                Console.WriteLine("\nПобеда взвода 2");
            }
            else if (_platoonSecond.Count == 0)
            {
                Console.WriteLine("\nПобеда взвода 1");
            }

            Console.ReadKey();
        }

        private void AttackPlatoon(List<Soldier> platoonFirst, List<Soldier> platoonSecond)
        {
            foreach (Soldier attacker in platoonFirst)
            {
                if (platoonSecond.Count == 0)
                    break;

                if (attacker is MultiTargetSoldier multiTargetSoldier)
                {
                    int numberSoldiersPerStrike = UserUtils.GenerateRandomNumber(_minNumberSoldiersAttacked, _maxNumberSoldiersAttacked);
                    multiTargetSoldier.AttackSeveralSoldiers(platoonSecond, numberSoldiersPerStrike);
                }
                else
                {
                    int targetIndex = platoonFirst.IndexOf(attacker);

                    if (targetIndex >= 0 && targetIndex < platoonSecond.Count)
                    {
                        attacker.Attack(platoonSecond[targetIndex]);
                    }
                }

                UpdateListPlatoon(platoonSecond);
            }
        }

        private void UpdateListPlatoon(List<Soldier> platoon)
        {
            List<Soldier> deadSoldiers = new List<Soldier>();

            foreach (Soldier soldier in platoon)
            {
                if (soldier.IsDeath)
                    deadSoldiers.Add(soldier);
            }

            foreach (Soldier deadSoldier in deadSoldiers)
            {
                platoon.Remove(deadSoldier);
            }
        }

        private void FormationPlatoon()
        {
            Soldier[] soldierTypes = { _soldierTypeA, _soldierTypeB, _soldierTypeC, _soldierTypeD };

            for (int i = 0; i < _numberSoldierPlatoon; i++)
            {
                AddSoldierToPlatoon(_platoonFirst, soldierTypes);
                AddSoldierToPlatoon(_platoonSecond, soldierTypes);
            }
        }

        private void AddSoldierToPlatoon(List<Soldier> platoon, Soldier[] soldierTypes)
        {
            int randomValue = UserUtils.GenerateRandomNumber(0, soldierTypes.Length);
            platoon.Add(soldierTypes[randomValue]);
        }
    }

    class Soldier
    {
        protected float Damage;

        private bool _isDeath;

        public Soldier(string name, float health, float damage, float armor)
        {
            Name = name;
            Health = health;
            Damage = damage;
            Armor = armor;
        }

        public string Name { get; private set; }
        public float Health { get; private set; }
        public float Armor { get; private set; }
        public bool IsDeath => _isDeath;

        public void TakeDamage(float damage)
        {
            if(IsDeath)
                return;

            float damageDealt = damage - Armor;

            Armor -= Damage;

            if (Armor < 0)
                Armor = 0;

            Health -= damageDealt;

            if (Health <= 0)
                _isDeath = true;
        }

        public void Attack(Soldier soldier)
        {
            soldier.TakeDamage(Damage);
            Console.WriteLine("Аттаку произвел " + Name + " по " + soldier.Name + " с силой " + Damage + " жизнями " + Health + " и бронёй " + Armor);
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
        private bool _isPossibleHitAgain;

        public MultiTargetSoldier(string name, int health, int damage, int armor, bool isPossibleHitAgain) : base(name, health, damage, armor)
        {
            _isPossibleHitAgain = isPossibleHitAgain;
        }

        public void AttackSeveralSoldiers(List<Soldier> platoon, int numberSoldiersPerStrike)
        {
            List<int> numbers = GenerateRandomIndices(platoon.Count, numberSoldiersPerStrike, _isPossibleHitAgain);

            foreach (int index in numbers)
            {
                Attack(platoon[index]);
            }
        }

        private List<int> GenerateRandomIndices(int maxIndex, int count, bool allowRepeats)
        {
            List<int> indices = new List<int>();

            while (indices.Count < count)
            {
                int index = UserUtils.GenerateRandomNumber(0, maxIndex);

                if (allowRepeats || indices.Contains(index) == false)
                {
                    indices.Add(index);
                }
            }

            return indices;
        }
    }

    class UserUtils
    {
        private static Random s_random = new Random();

        public static int GenerateRandomNumber(int min, int max) =>
            s_random.Next(min, max);
    }
}
