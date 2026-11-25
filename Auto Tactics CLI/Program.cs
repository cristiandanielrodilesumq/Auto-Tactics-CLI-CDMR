ConsoleView view = new ConsoleView();
List<Army> waves= new WaveData().CreateWaves();

Player player = new Player();
player.Army.TryAdd(new Archer(1));
player.Army.TryAdd(new Warrior(1));

// Waves[1] es la oleada 2: la lista empieza en 0.
Battle battle = new Battle(player.Army, waves[0]);
view.ShowMessage("Combate Contra la Oleada 1");

while (!battle.IsOver)
{
    BattleEvent battleEvent = battle.NextTurn();
    view.ShowTurn(battle, battleEvent);
}
view.ShowResult(battle);