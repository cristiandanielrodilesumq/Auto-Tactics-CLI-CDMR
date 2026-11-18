ConsoleView view = new ConsoleView();
List<Army> waves = new WaveData().CreateWaves();

Player player = new Player();
player.Army.TryAdd(new Unit("Guerrero", 12, 4, 1));
player.Army.TryAdd(new Unit("Arquero", 6, 2, 1));

// waves[0] es la oleada 1: la lista empieza en 0.
Battle battle = new Battle(player.Army, waves[4]);
view.ShowMessage("Combate contra la oleada 5");

while (!battle.IsOver)
{
    BattleEvent battleEvent = battle.NextTurn();
    view.ShowTurn(battle, battleEvent);
}
view.ShowResult(battle);