using System;
using System.Collections.Generic;

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
            FormationPlatoon();
            Console.WriteLine("Все готово! Можно начинать битву!");
            Console.ReadKey();
            Console.Clear();

            while (_platoonFirst.Count != 0 || _platoonSecond.Count != 0)
            {
                int randomValue;

                for (int i = 0; i < _platoonFirst.Count; i++)
                {

                    randomValue = UserUtils.GenerateRandomNumber(0, _platoonSecond.Count);

                    if (_platoonFirst[i] is MultiTargetSoldier)
                    {
                        for (int j = 0; j < _platoonFirst[i].NumberSoldiersPerStrike; j++)
                        {

                        }

                        continue;
                    }


                }
            }
        }

        private void RemoveIncapacitatedSoldiers()
        {

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
            float damageDealt = Math.Max(0, damage - Armor);
            Armor -= Damage;
            Health -= damageDealt;

            if (Health <= 0)
                _isDeath = true;
        }

        public virtual void Attack(Soldier soldier)
        {
            soldier.TakeDamage(Damage);
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
        public int NumberSoldiersPerStrike { get; private set; }
        public bool _isPossibleHitAgain;
        private List<int> _attackedSoldierNumbers = new List<int>();

        public MultiTargetSoldier(string name, int health, int damage, int armor, int numberSoldiersPerStrike, bool isPossibleHitAgain) : base(name, health, damage, armor)
        {
            NumberSoldiersPerStrike = numberSoldiersPerStrike;
            _isPossibleHitAgain = isPossibleHitAgain;
        }

        // Console.WriteLine($"{Name} атакует {SelectionSoldiers.Name} и наносит {damageDealt} ед. урона.");




    }

    class UserUtils
    {
        private static Random s_random = new Random();

        public static int GenerateRandomNumber(int min, int max) =>
            s_random.Next(min, max);
    }
}
