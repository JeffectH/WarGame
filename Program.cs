using System;
using System.Collections.Generic;

namespace WarGame
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Battlefield battlefield = new Battlefield();
            battlefield.Fight();
        }
    }

    class Battlefield
    {
        private Platoon _firstPlatoon;
        private Platoon _secondPlatoon;
        private int _numberSoldierPlatoon = 10;

        public Battlefield()
        {
            _firstPlatoon = new Platoon(_numberSoldierPlatoon);
            _secondPlatoon = new Platoon(_numberSoldierPlatoon);
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
            _firstPlatoon.Formation();
            _secondPlatoon.Formation();
            Console.WriteLine("\nВсе готово! Можно начинать битву!");
            Console.ReadKey();
            Console.Clear();

            while (_firstPlatoon.NumberSolders != 0 && _secondPlatoon.NumberSolders != 0)
            {
                Console.WriteLine("\nАттакует взвод №1 \n");
                _firstPlatoon.Attack(_secondPlatoon);

                Console.WriteLine("\nАттакует взвод №2 \n");
                _secondPlatoon.Attack(_firstPlatoon);

                _firstPlatoon.UpdateList();
                _secondPlatoon.UpdateList();

                Console.WriteLine($"\nКолличество человек в взводе №1: {_firstPlatoon.NumberSolders}." + "\n" +
                    $"Колличество человек в взводе №2: {_secondPlatoon.NumberSolders}.");

                Console.ReadKey();
            }

            if (_firstPlatoon.NumberSolders == 0 && _secondPlatoon.NumberSolders == 0)
            {
                Console.WriteLine("\nНичья");
            }
            else if (_firstPlatoon.NumberSolders == 0)
            {
                Console.WriteLine("\nПобеда взвода 2");
            }
            else if (_secondPlatoon.NumberSolders == 0)
            {
                Console.WriteLine("\nПобеда взвода 1");
            }

            Console.ReadKey();
        }
    }
}

class Platoon
{
    private int _numberSoldierPlatoon;
    private int _minNumberSoldierPlatoon = 15;
    private int _maxNumberSoldierPlatoon = 60;
    private List<Soldier> _solders;

    public Platoon(int numberSoldierPlatoon)
    {
        _numberSoldierPlatoon = (numberSoldierPlatoon < _minNumberSoldierPlatoon) ? _minNumberSoldierPlatoon : (numberSoldierPlatoon > _maxNumberSoldierPlatoon) ? _maxNumberSoldierPlatoon : numberSoldierPlatoon;

        _solders = new List<Soldier>();
    }

    public int NumberSolders => _solders.Count;

    public void Formation()
    {
        Soldier[] soldierTypes = { new SingleSoldier("Иван “Простой”", 400, 15, 50), new SingleTargetHighDamageSoldier("Алексей “Убийца”", 100, 15, 50, 2.5f),
            new MultiTargetSoldierRepeatedStrikes("Наталья “Множитель”", 200, 15, 50,2,4), new MultiTargetSoldierRepeatedStrikes("Дмитрий “Многократный”", 200, 15, 50, 2, 4)};

        for (int i = 0; i < _numberSoldierPlatoon; i++)
        {
            AddSoldier(soldierTypes);
        }
    }

    public void UpdateList()
    {
        List<Soldier> deadSoldiers = new List<Soldier>();

        foreach (Soldier soldier in _solders)
        {
            if (soldier.IsSoldierDead)
                deadSoldiers.Add(soldier);
        }

        foreach (Soldier deadSoldier in deadSoldiers)
        {
            _solders.Remove(deadSoldier);
        }
    }

    public void Attack(Platoon platoonEnemy)
    {
        foreach (Soldier attacker in _solders)
        {
            if (platoonEnemy.NumberSolders == 0)
                break;

            if (attacker is MultiTargetSoldierRepeatedStrikes multiTargetSoldierRepeat)
            {
                multiTargetSoldierRepeat.Attack(null, platoonEnemy);
            }
            else if (attacker is MultiTargetSoldierNotRepeatedStrikes multiTargetSoldierNotRepeat)
            {
                multiTargetSoldierNotRepeat.Attack(null, platoonEnemy);
            }
            else
            {
                int targetIndex = _solders.IndexOf(attacker);

                if (targetIndex >= 0 && targetIndex < platoonEnemy.NumberSolders)
                {
                    attacker.Attack(platoonEnemy.GetSolderByIndex(targetIndex));
                }
            }
        }
    }

    public Soldier GetSolderByIndex(int index) => _solders[index];

    private void AddSoldier(Soldier[] soldierTypes)
    {
        int randomValue = UserUtils.GenerateRandomNumber(0, soldierTypes.Length);
        Soldier newSoldier = (Soldier)soldierTypes[randomValue].Clone();
        _solders.Add(newSoldier);
    }
}

abstract class Soldier : ICloneable
{
    protected float Damage;

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
    public bool IsSoldierDead => Health <= 0;

    public void TakeDamage(float damage)
    {
        if (IsSoldierDead)
            return;

        float damageDealt = damage - Armor;

        Armor -= Damage;

        if (Armor < 0)
            Armor = 0;

        Health -= damageDealt;
    }

    public abstract void Attack(Soldier soldier = null, Platoon platoon = null);

    public object Clone() => MemberwiseClone();
}

class SingleSoldier : Soldier
{
    public SingleSoldier(string name, float health, float damage, float armor) : base(name, health, damage, armor) { }

    public override void Attack(Soldier soldier, Platoon platoon)
    {
        soldier.TakeDamage(Damage);
        Console.WriteLine("Аттаку произвел " + Name + " по " + soldier.Name + " с силой " + Damage + " жизнями " + Health + " и бронёй " + Armor);
    }
}

class SingleTargetHighDamageSoldier : Soldier
{
    private float _damageMultiplier;

    public SingleTargetHighDamageSoldier(string name, float health, float damage, float armor, float damageMultiplier) : base(name, health, damage, armor)
    {
        _damageMultiplier = damageMultiplier;
    }

    public override void Attack(Soldier soldier, Platoon platoon)
    {
        soldier.TakeDamage(Damage * _damageMultiplier);
        Console.WriteLine("Аттаку произвел " + Name + " по " + soldier.Name + " с силой " + Damage + " жизнями " + Health + " и бронёй " + Armor);
    }
}

class MultiTargetSoldierRepeatedStrikes : Soldier
{
    private int _numberSoldiersPerStrike;
    private int _minNumberSoldiersAttacked;
    private int _maxNumberSoldiersAttacked;

    public MultiTargetSoldierRepeatedStrikes(string name, float health, float damage, float armor, int minNumberSoldiersAttacked, int maxNumberSoldiersAttacked) : base(name, health, damage, armor)
    {
        _minNumberSoldiersAttacked = minNumberSoldiersAttacked;
        _maxNumberSoldiersAttacked = maxNumberSoldiersAttacked;

        if (_minNumberSoldiersAttacked > _maxNumberSoldiersAttacked)
            _minNumberSoldiersAttacked = _maxNumberSoldiersAttacked--;

        _numberSoldiersPerStrike = UserUtils.GenerateRandomNumber(_minNumberSoldiersAttacked, _maxNumberSoldiersAttacked);
    }

    public override void Attack(Soldier soldier, Platoon platoonEnemy)
    {
        List<int> numbers = GenerateRandomIndices(platoonEnemy.NumberSolders, _numberSoldiersPerStrike);

        foreach (int index in numbers)
        {
            platoonEnemy.GetSolderByIndex(index).TakeDamage(Damage);
            Console.WriteLine("Аттаку произвел " + Name + " по " + platoonEnemy.GetSolderByIndex(index).Name + " с силой " + Damage + " жизнями " + Health + " и бронёй " + Armor);
        }
    }

    public List<int> GenerateRandomIndices(int maxIndex, int count)
    {
        List<int> indices = new List<int>();

        while (indices.Count < count)
        {
            int index = UserUtils.GenerateRandomNumber(0, maxIndex);

            indices.Add(index);
        }

        return indices;
    }
}

class MultiTargetSoldierNotRepeatedStrikes : Soldier
{
    private int _numberSoldiersPerStrike;
    private int _minNumberSoldiersAttacked;
    private int _maxNumberSoldiersAttacked;

    public MultiTargetSoldierNotRepeatedStrikes(string name, float health, float damage, float armor, int minNumberSoldiersAttacked, int maxNumberSoldiersAttacked) : base(name, health, damage, armor)
    {
        _minNumberSoldiersAttacked = minNumberSoldiersAttacked;
        _maxNumberSoldiersAttacked = maxNumberSoldiersAttacked;

        if (_minNumberSoldiersAttacked > _maxNumberSoldiersAttacked)
            _minNumberSoldiersAttacked = _maxNumberSoldiersAttacked--;

        _numberSoldiersPerStrike = UserUtils.GenerateRandomNumber(_minNumberSoldiersAttacked, _maxNumberSoldiersAttacked);
    }

    public override void Attack(Soldier soldier, Platoon platoonEnemy)
    {
        List<int> numbers = GenerateRandomIndices(platoonEnemy.NumberSolders, _numberSoldiersPerStrike);

        foreach (int index in numbers)
        {
            platoonEnemy.GetSolderByIndex(index).TakeDamage(Damage);
            Console.WriteLine("Аттаку произвел " + Name + " по " + platoonEnemy.GetSolderByIndex(index).Name + " с силой " + Damage + " жизнями " + Health + " и бронёй " + Armor);
        }
    }

    private List<int> GenerateRandomIndices(int maxIndex, int count)
    {
        List<int> indices = new List<int>();

        while (indices.Count < count)
        {
            int index = UserUtils.GenerateRandomNumber(0, maxIndex);

            if (indices.Contains(index) == false)
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
