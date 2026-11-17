ConsoleView view = new ConsoleView();
List<Army> waves = new WaveData().CreateWaves();

Player player = new Player();
player.Army.TryAdd(new Unit("Guerrero", 12, 4, 1));
player.Army.TryAdd(new Unit("Arquero", 6, 2, 1));

// waves[0] es la oleada 1: la lista empieza en 0.
Battle battle = new Battle(player.Army, waves[0]);
view.ShowMessage("Combate contra la oleada 1 (solo ataca el jugador)");

for (int i = 0; i < 4; i++)
{
    BattleEvent battleEvent = battle.NextTurn();
    view.ShowTurn(battle, battleEvent);
}