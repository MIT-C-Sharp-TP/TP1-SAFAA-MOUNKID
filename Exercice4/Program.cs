using System ; 

int Heal(int health, int amount, int maxHealth){

    health += amount ; 

     return Math.Min(health,maxHealth) ; 
}

Console.WriteLine($"50 + 30 -> {Heal(50, 30, 100)}");
Console.WriteLine($"90 + 30 -> {Heal(90, 30, 100)}");
Console.WriteLine($"0 + 25 -> {Heal(0, 25, 100)}");

