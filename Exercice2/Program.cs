
using System;

void CheckHealth(int health){
if (health > 70)
{
    Console.WriteLine("healthy");
}
else if (health >= 31)
{
    Console.WriteLine("injured");
}
else if (health <= 30 && health > 0 )
{
    Console.WriteLine("critical");
}
else
{
    Console.WriteLine("Aya is down");
}
}


int health = 45;
Console.WriteLine($"Aya has {health} HP : ");
CheckHealth(health);

 health = 100;
Console.WriteLine($"Aya has {health} HP : ");
CheckHealth(health);

 health = 12;
Console.WriteLine($"Aya has {health} HP : ");
CheckHealth(health);

 health = 0 ;
Console.WriteLine($"Aya has {health} HP : ");
CheckHealth(health);