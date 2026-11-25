// Vista de consola: muestra el estado del juego y pide acciones.
// Es la única clase del juego que usa Console (además de ConsoleKit).
public class ConsoleView
{
    private const string DoubleLine = "==================================================================";
    private const string SingleLine = "------------------------------------------------------------------";
    private const int UnitTextWidth = 13;
    private const int PlayerColumnWidth = 25;
    private const int MessagePause = 1500;

    public ConsoleView()
    {
        // El primer uso de ConsoleKit configura la consola en UTF-8 (acentos, ★ y →).
        ConsoleKit.Clear();
    }

    public void ShowPreparation(Player player, Shop shop, Army wave, int waveNumber, int waveCount)
    {
        ConsoleKit.Clear();
        Console.WriteLine(DoubleLine);
        Console.WriteLine($" RONDA {waveNumber}/{waveCount}  |  Oro: {player.Gold}  |  Fila: {player.Army.Count}/{player.Army.Capacity}");
        Console.WriteLine(SingleLine);
        ShowArmies(player.Army, wave, $"OLEADA {waveNumber}");
        Console.WriteLine(SingleLine);
        Console.WriteLine($" TIENDA: {FormatOffers(shop)}");
        Console.WriteLine(DoubleLine);
        Console.WriteLine($" [C]omprar  [V]ender  [M]over  [R]eroll ({shop.RerollCost})  [B]atalla");
    }

    public char AskAction()
    {
        Console.Write(" Acción: ");
        return ConsoleKit.ReadOption("CVMRB");
    }

    // Pide un número entre 1 y max y lo devuelve como índice del código (desde 0).
    public int AskPosition(string question, int max)
    {
        Console.Write($" {question} (1-{max}): ");
        return ConsoleKit.ReadNumber(1, max) - 1;
    }

    // Bitácora de nivel 1: una línea por turno, impresa de corrido.
    public void ShowTurn(Battle battle, BattleEvent battleEvent)
    {
        Console.WriteLine($" {DescribeTurn(battle, battleEvent)}");
    }

    public void ShowResult(Battle battle)
    {
        Console.WriteLine(SingleLine);
        ShowArmies(battle.PlayerArmy, battle.EnemyArmy, "ENEMIGO");
        Console.WriteLine(DoubleLine);
        if (battle.PlayerWon)
        {
            Console.WriteLine(" ¡Ganaste el combate!");
        }
        else
        {
            Console.WriteLine($" Perdiste el combate. Enemigos supervivientes: {battle.SurvivorCount}");
        }
        Console.Write(" Presiona Enter para continuar...");
        Console.ReadLine();
    }

    public void ShowMessage(string text)
    {
        Console.WriteLine($" {text}");
        ConsoleKit.Pause(MessagePause);
    }

    // Dibuja las dos filas lado a lado; la posición 1 es el frente.
    private void ShowArmies(Army playerArmy, Army enemyArmy, string enemyTitle)
    {
        Console.WriteLine($" POS   {ConsoleKit.PadRight("JUGADOR", PlayerColumnWidth + 6)}{enemyTitle}");

        int rows = Math.Max(playerArmy.Capacity, enemyArmy.Count);
        for (int i = 0; i < rows; i++)
        {
            string playerText = "";
            if (i < playerArmy.Count)
            {
                playerText = FormatUnit(playerArmy.GetUnit(i));
            }
            else if (i < playerArmy.Capacity)
            {
                playerText = "---";
            }

            string enemyText = "";
            if (i < enemyArmy.Count)
            {
                enemyText = FormatUnit(enemyArmy.GetUnit(i));
            }

            string separator = "  ";
            if (i == 0)
            {
                separator = "vs";
            }

            Console.WriteLine($"  {i + 1}    {ConsoleKit.PadRight(playerText, PlayerColumnWidth)}{separator}    {enemyText}");
        }
    }

    private string FormatUnit(Unit unit)
    {
        return ConsoleKit.PadRight(unit.ToString(), UnitTextWidth) + $"{unit.Health}/{unit.MaxHealth}";
    }

    private string FormatOffers(Shop shop)
    {
        string text = "";
        for (int i = 0; i < shop.OfferCount; i++)
        {
            Unit? offer = shop.GetOffer(i);
            string offerText = "---";
            if (offer != null)
            {
                offerText = $"{offer.Name} ({offer.Cost})";
            }

            if (i > 0)
            {
                text += "  |  ";
            }
            text += $"[{i + 1}] {offerText}";
        }
        return text;
    }

    private string DescribeTurn(Battle battle, BattleEvent battleEvent)
    {
        string text = $"Turno {battle.TurnCount} | {Label(battle, battleEvent.Attacker)}";
        if (battleEvent.Blocked)
        {
            return text + " no alcanza";
        }

        text += " ataca a ";
        for (int i = 0; i < battleEvent.Targets.Count; i++)
        {
            Unit target = battleEvent.Targets[i];
            if (i > 0)
            {
                text += " y a ";
            }
            text += $"{Label(battle, target)}: -{battleEvent.Damage} → {target.Health}/{target.MaxHealth}";
        }
        return text;
    }

    // Etiqueta de bitácora: J = jugador, E = enemigo, seguido del lugar en la fila (desde 1).
    private string Label(Battle battle, Unit unit)
    {
        int position = battle.PlayerArmy.GetPosition(unit);
        if (position >= 0)
        {
            return $"{unit.Name} (J{position + 1})";
        }
        return $"{unit.Name} (E{battle.EnemyArmy.GetPosition(unit) + 1})";
    }
}