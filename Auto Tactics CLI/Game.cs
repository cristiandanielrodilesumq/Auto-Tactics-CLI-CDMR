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

    private void RunPreparation()
    {
        player.Earn(GoldPerRound);
        shop.Refresh();
        Army wave = waves[currentWave];

        // Se repite hasta que el jugador inicia la batalla (StartBattle cambia la fase).
        while (phase == GamePhase.Preparation)
        {
            view.ShowPreparation(player, shop, wave, currentWave + 1, waves.Count);
            char action = view.AskAction();

            switch (action)
            {
                case 'C':
                    Buy();
                    break;
                case 'V':
                    Sell();
                    break;
                case 'M':
                    Move();
                    break;
                case 'R':
                    Reroll();
                    break;
                case 'B':
                    StartBattle();
                    break;
            }
        }
    }

    private void Buy()
    {
        int slot = view.AskPosition("Espacio de la tienda", shop.OfferCount);
        if (shop.TryBuy(slot, player))
        {
            return;
        }

        // La compra se rechazó; se busca la causa para explicarla.
        if (shop.GetOffer(slot) == null)
        {
            view.ShowMessage("Ese espacio de la tienda está vacío.");
        }
        else if (player.Army.IsFull)
        {
            view.ShowMessage("Tu fila está llena. Vende una unidad primero.");
        }
        else
        {
            view.ShowMessage("No tienes oro suficiente.");
        }
    }

    private void Sell()
    {
        if (player.Army.Count == 0)
        {
            view.ShowMessage("No tienes unidades para vender.");
            return;
        }

        int position = view.AskPosition("Posición de la unidad a vender", player.Army.Count);
        shop.Sell(position, player);
    }

    private void Move()
    {
        if (player.Army.Count < 2)
        {
            view.ShowMessage("Necesitas al menos dos unidades para moverlas.");
            return;
        }

        int first = view.AskPosition("Primera posición", player.Army.Count);
        int second = view.AskPosition("Segunda posición", player.Army.Count);
        player.Army.Swap(first, second);
    }

    private void Reroll()
    {
        if (!shop.TryReroll(player))
        {
            view.ShowMessage("No tienes oro suficiente para renovar la tienda.");
        }
    }

    private void StartBattle()
    {
        if (player.Army.Count == 0)
        {
            view.ShowMessage("No puedes iniciar la batalla con la fila vacía.");
            return;
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

    private void RunResolution()
    {
        if (!lastBattleWon)
        {
            int damage = lastSurvivorCount * DamagePerSurvivor;
            player.TakeDamage(damage);
            view.ShowMessage($"Recibes {damage} de daño. Vida: {player.Health}/{player.MaxHealth}");
        }

        // Siempre: ambos ejércitos se curan por completo y el jugador conserva su orden.
        player.Army.RestoreAll();
        waves[currentWave].RestoreAll();

        if (player.IsDefeated)
        {
            phase = GamePhase.Defeat;
            return;
        }

        if (lastBattleWon)
        {
            currentWave++;
            if (currentWave == waves.Count)
            {
                phase = GamePhase.Victory;
                return;
            }
        }

        // Si perdió, currentWave no cambia: la oleada se repite.
        phase = GamePhase.Preparation;
    }
}