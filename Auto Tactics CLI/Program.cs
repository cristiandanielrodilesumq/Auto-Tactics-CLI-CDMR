Unit archer = new Unit("Arquero", 6, 2);   // vida 6
archer.TakeDamage(4);
archer.TakeDamage(4);
Console.WriteLine(archer.Health);
Console.WriteLine(archer.IsAlive);