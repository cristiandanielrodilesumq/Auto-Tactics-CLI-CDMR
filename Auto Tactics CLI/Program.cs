ConsoleView view = new ConsoleView();
Player player = new Player();

player.Army.TryAdd(new Unit("Guerrero", 12, 4));
player.Army.TryAdd(new Unit("Arquero", 6, 2));
player.Army.TryAdd(new Unit("Mago", 4, 2));

// La fila ya tiene 3 unidades: la cuarta se rechaza.
if (!player.Army.TryAdd(new Unit("Guerrero", 12, 4)))
{
    view.ShowMessage("La fila está llena: el cuarto Guerrero no entra.");
}
view.ShowArmy("Fila inicial", player.Army);

player.Army.Swap(0, 2);
view.ShowArmy("Fila Después del Intercambio de Pocisiones 1 y 3",  player.Army);

var mage = player.Army.GetUnit(0);
mage.TakeDamage(10);

view.ShowArmy("Fila Después de Recibir Daño", player.Army);
var aliveUnits = player.Army.GetAliveUnits();
foreach (var unit in aliveUnits)
{
    Console.WriteLine(unit.Name);
}