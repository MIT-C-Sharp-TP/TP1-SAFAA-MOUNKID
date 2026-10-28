
using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;

List<int> wins = new List<int>();
int gameNumber = 0;

while (true)
{
    gameNumber++;

    int turns = PlayGame();

    
string result;

if (turns > 0)
{
    wins.Add(turns);
    result = $"Game{gameNumber}: victory in {turns} turns";
    Console.WriteLine(result);
}
else
{
    result = $"Game{gameNumber}: defeat";
    Console.WriteLine(result);
}

try
{
    File.AppendAllText("results.txt", result + Environment.NewLine);
}
catch (IOException e)
{
    Console.WriteLine($"Could not save the result: {e.Message}");
}
catch (UnauthorizedAccessException e)
{
    Console.WriteLine($"Could not save the result: {e.Message}");
}


    Console.WriteLine();
    Console.Write("Play again? (y/n) > ");
    string? answer = Console.ReadLine();

    if (answer != "y")
    {
        break;
    }
}

Console.WriteLine();
Console.WriteLine($"Victories: {wins.Count} of {gameNumber}");

if (wins.Count > 0)
{
    Console.WriteLine("Top 3 fastest victories:");

    foreach (int turns in wins.OrderBy(t => t).Take(3))
    {
        Console.WriteLine($"- {turns} turns");
    }
}

int PlayGame()
{
    Player player = new Player("Aya", 100, 2);
    player.Died += name => Console.WriteLine($"Game over for {name}.");

    List<Enemy> enemies = new List<Enemy>();
    enemies.Add(new Goblin());
    enemies.Add(new Orc());
    enemies.Add(new Goblin());
    enemies.Add(new Boss());

    Random rng = new Random();
    int turn = 0;

    Console.WriteLine("=== Arena ===");
    Console.WriteLine($"{player.Name} faces {enemies.Count} enemies:");

    foreach (Enemy e in enemies)
    {
        Console.WriteLine($"- {e.Name}, {e.Health} HP");
    }

    while (player.IsAlive && enemies.Count > 0)
    {
        turn++;

        Enemy enemy = enemies[0];

        Console.WriteLine();
        Console.WriteLine($"Turn {turn}: {player.Name} {player.Health} HP, {player.Potions} potions | {enemy.Name} {enemy.Health} HP");
        Console.Write("1 attack, 2 drink a potion > ");

        string? text = Console.ReadLine();

        if (!int.TryParse(text, out int choice))
        {
            Console.WriteLine("Type 1 or 2.");
            continue;
        }

        switch (choice)
        {
            case 1:
                enemy.TakeDamage(rng.Next(12, 25));
                break;

            case 2:
                player.DrinkPotion();
                break;

            default:
                Console.WriteLine("Type 1 or 2.");
                continue;
        }

        if (!enemy.IsAlive)
        {
            Console.WriteLine($"{enemy.Name} is defeated!");
            enemies.RemoveAt(0);
            continue;
        }

        enemy.Attack(player, rng);
    }

    if (player.IsAlive)
    {
        Console.WriteLine($"Victory in {turn} turns!");
        return turn;
    }
    else
    {
        Console.WriteLine("Defeat...");
        return 0;
    }
}


public class Player
{
    private int health;
    public event Action<string>? Died;
    public string Name { get; }
    public int MaxHealth { get; }
    public int Potions { get; private set; }

    public int Health
    {
        get { return health; }
        private set { health = Math.Clamp(value, 0, MaxHealth); }
    }

    public bool IsAlive => health > 0;

    public Player(string name, int maxHealth, int potions)
    {
        Name = name;
        MaxHealth = maxHealth;
        Potions = potions;
        Health = maxHealth;
    }

    public void TakeDamage(int amount)
{
    bool wasAlive = IsAlive;

    Health -= amount;

    Console.WriteLine($"{Name} takes {amount} damage ({Health} HP)");

    if (wasAlive && !IsAlive)
    {
        Died?.Invoke(Name);
    }
}

    public void DrinkPotion()
    {
        if (Potions <= 0)
        {
            Console.WriteLine("No potion left!");
            return;
        }

        Potions--;
        Health += 30;
        Console.WriteLine($"{Name} drinks a potion ({Health} HP)");
    }
}

public abstract class Enemy
{
    public string Name { get; }
    public int Health { get; private set; }
    public bool IsAlive => Health > 0;

    protected Enemy(string name, int health)
    {
        Name = name;
        Health = health;
    }

    public virtual void TakeDamage(int amount)
{
    Health = Math.Max(0, Health - amount);
    Console.WriteLine($"{Name} takes {amount} damage ({Health} HP)");
}

    public abstract void Attack(Player target, Random rng);
}

public class Goblin : Enemy
{
    public Goblin() : base("Goblin", 30)
    {
    }

    public override void Attack(Player target, Random rng)
    {
        Console.WriteLine("Goblin stabs!");
        target.TakeDamage(rng.Next(4, 9));
    }
}

public class Orc : Enemy
{
    public Orc() : base("Orc", 60)
    {
    }

    public override void Attack(Player target, Random rng)
    {
        Console.WriteLine("Orc swings its club!");
        target.TakeDamage(rng.Next(8, 15));
    }
}


public class Boss : Enemy
{
    public int Armour { get; } = 6;

    public Boss() : base("Boss", 60)
    {
    }

    public override void TakeDamage(int amount)
    {
        int reduced = Math.Max(amount - Armour, 1);

        Console.WriteLine($"The armour absorbs {amount - reduced} damage.");

        base.TakeDamage(reduced);
    }

    public override void Attack(Player target, Random rng)
    {
        Console.WriteLine("Boss slams the ground!");
        target.TakeDamage(rng.Next(9, 17));
    }
}
