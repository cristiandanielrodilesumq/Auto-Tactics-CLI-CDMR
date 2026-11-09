Unit warrior = new Unit("Guerrero", 12, 4);
Unit archer = new Unit("Arquero", 6, 2);
Unit attacker = warrior;
Unit defender = archer;
int turn = 1;

while (attacker.IsAlive && defender.IsAlive)
{
    defender.TakeDamage(attacker.Damage);
    Console.WriteLine("Turno " + turn + ": ¡" + attacker.Name + " ataca a " + defender.Name + "!");
    Console.WriteLine("¡" + defender.Name + " sufre " + attacker.Damage + " puntos de daño!");
    
    Unit temp = attacker;
    attacker = defender; 
    defender = temp;
    
    turn++;
}
Console.WriteLine("---");
Console.WriteLine("¡" + attacker.Name + " gana el combate!");
Console.WriteLine("-");
Console.WriteLine(defender.Name + ": " + defender.Health);
Console.WriteLine(attacker.Name + ": " + attacker.Health);

Console.WriteLine(" ");
Console.WriteLine("---------------------------------------");
Console.WriteLine(" ");

Player player =  new Player();
player.Earn(3);
int Item = 2;
Console.WriteLine("Oro del Jugador: " + player.Gold);

for (int i = 0; i < 2; i++)
{
    if (player.TrySpend(2))
    {
        Console.WriteLine("¡Compraste un Item!");
        Console.WriteLine("Oro Restante: " + player.Gold);
    }
    else
    {
        Console.WriteLine("No pudiste comprar el Item.");
        Console.WriteLine("Oro Restante: " + player.Gold);
    }
}