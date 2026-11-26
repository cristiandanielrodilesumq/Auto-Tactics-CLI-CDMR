// Coordina el bucle de juego: preparación, combate y resolución.
public class Game
{
    private const int GoldPerRound = 3;
    private const int DamagePerSurvivor = 2;

    private Player player = new Player();
    private Shop shop = new Shop();
    private List<Army> waves = new WaveData().CreateWaves();
    private ConsoleView view = new ConsoleView();
    private int currentWave = 0;
    private GamePhase phase = GamePhase.Preparation;

    // Resultado del último combate, para la fase de resolución.
    private bool lastBattleWon;
    private int lastSurvivorCount;

    public void Run()
    {
        while (phase != GamePhase.Victory && phase != GamePhase.Defeat)
        {
            switch (phase)
            {
                case GamePhase.Preparation:
                    RunPreparation();
                    break;
                case GamePhase.Combat:
                    RunCombat();
                    break;
                case GamePhase.Resolution:
                    RunResolution();
                    break;
            }
        }

        if (phase == GamePhase.Victory)
        {
            view.ShowMessage($"¡Victoria! Venciste las {waves.Count} oleadas.");
        }
        else
        {
            view.ShowMessage("Derrota. Tu vida llegó a 0.");
        }
    }
    
    // PROVISIONAL: arma una fila fija para poder probar el bucle. Se reemplaza en «Tú haces».
    private void RunPreparation()
    {
        player.Earn(GoldPerRound);
        shop.Refresh();

        if (player.Army.Count == 0)
        {
            player.Army.TryAdd(new Warrior(1));
            player.Army.TryAdd(new Archer(1));
        }
        phase = GamePhase.Combat;
    }

    private void RunCombat()
    {
        Battle battle = new Battle(player.Army, waves[currentWave]);
        while (!battle.IsOver)
        {
            BattleEvent battleEvent = battle.NextTurn();
            view.ShowTurn(battle, battleEvent);
        }
        view.ShowResult(battle);

        lastBattleWon = battle.PlayerWon;
        lastSurvivorCount = battle.SurvivorCount;
        phase = GamePhase.Resolution;
    }

// PROVISIONAL: solo muestra lo que guardó el combate y termina la partida. Se reemplaza en «Tú haces».
    private void RunResolution()
    {
        view.ShowMessage($"Resolución provisional. ¿Ganó el jugador? {lastBattleWon}. Enemigos vivos: {lastSurvivorCount}");
        phase = GamePhase.Victory;
    }
}