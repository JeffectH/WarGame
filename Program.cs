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
            battlefield.Fight();
        }
    }

    class Battlefield
    {
        private int _numberSoldierPlatoon;
        private int _minNumberSoldierPlatoon = 15;
        private int _maxNumberSoldierPlatoon = 60;
        private bool _isFight = true;
        private Soldier _soldierTypeA;
        private Soldier _soldierTypeB;
        private Soldier _soldierTypeC;
        private Soldier _soldierTypeD;
        private List<Soldier> _platoonFirst = new List<Soldier>();
        private List<Soldier> _platoonSecond = new List<Soldier>();

        public Battlefield(int numberSoldierPlatoon)
        {
            _numberSoldierPlatoon = (numberSoldierPlatoon < _minNumberSoldierPlatoon) ? _minNumberSoldierPlatoon : (numberSoldierPlatoon > _maxNumberSoldierPlatoon) ? _maxNumberSoldierPlatoon : numberSoldierPlatoon;

            _soldierTypeA = new Soldier("Иван “Простой”", 100, 15, 50);
            _soldierTypeB = new SingleTargetHighDamageSoldier("Алексей “Убийца”", 100, 15, 50, 2.5f);
            _soldierTypeC = new MultiTargetSoldier("Наталья “Множитель”", 100, 15, 50, 2, false);
            _soldierTypeD = new MultiTargetSoldier("Дмитрий “Многократный”", 100, 15, 50, 3, true);
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

            Console.WriteLine("Происходит формирование взвода...");

            Console.WriteLine("Все готово! Можно начинать битву!");
            Console.ReadKey();
            Console.Clear();

            while (_isFight)
            {






                _isFight = false;
                Console.ReadKey();
            }


        }

        private void FormationPlatoon()
        {
            Soldier[] soldierTypes = { _soldierTypeA, _soldierTypeB, _soldierTypeC, _soldierTypeD };
            int randomValue;
            for (int i = 0; i < _numberSoldierPlatoon; i++)
            {
                randomValue = UserUtils.GenerateRandomNumber(0, soldierTypes.Length);
                _platoonFirst.Add(soldierTypes[randomValue]);
                randomValue = UserUtils.GenerateRandomNumber(0, soldierTypes.Length);
                _platoonSecond.Add(soldierTypes[randomValue]);
            }
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

        public void Attack()
        {
            for (int i = 0; i < _numberSoldiersPerStrike; i++)
            {

                //  SelectionSoldiers.TakeDamage(Damage);

                // Console.WriteLine($"{Name} атакует {SelectionSoldiers.Name} и наносит {damageDealt} ед. урона.");
            }
        }

        private Soldier SelectionSoldiers(List<Soldier> soldiers)
        {
            int randomIndexSoldier = UserUtils.GenerateRandomNumber(0, _numberSoldiersPerStrike);

            if (_isPossibleHitAgain == false)
            {
                while (true)
                {
                    for (int i = 0; i < _attackedSoldierNumbers.Count; i++)
                    {
                        _attackedSoldierNumbers.Add(randomIndexSoldier);
                    }

                    for (int i = 0; i < _attackedSoldierNumbers.Count; i++)
                    {
                        _attackedSoldierNumbers.Add(randomIndexSoldier);

                    }
                }
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
