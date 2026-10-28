using System;

int health = 40;

for (int turn = 1; turn <= 6; turn++)
{
    health = health - 8;

    if (health < 0)
    {
        health = 0;
    }

    Console.WriteLine($"Turn {turn}: Aya has {health} HP");

    if (health == 0)
    {
        Console.WriteLine("Aya is down.");
         break ; 
    }
}

