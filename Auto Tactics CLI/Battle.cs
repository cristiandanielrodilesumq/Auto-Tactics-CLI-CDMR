public class Battle
{
    private List<BattleEvent> log = new List<BattleEvent>();
    // Índice de la última unidad que actuó en el bando del jugador. -1: todavía no actúa nadie.
    private int playerCursor = -1;
    // Lo mismo, pero para el bando enemigo.
    private int enemyCursor = -1;
    // Quién tiene el turno. Empieza el jugador.
    private bool isPlayerTurn = true;

    public Army PlayerArmy { get; }
    public Army EnemyArmy { get; }

    public int TurnCount => log.Count;
    
    // El combate termina cuando cualquiera de los dos ejércitos está derrotado.
    public bool IsOver => PlayerArmy.IsDefeated || EnemyArmy.IsDefeated;
    
    // El jugador gana cuando el ejército enemigo fue derrotado.
    public bool PlayerWon => EnemyArmy.IsDefeated;
    
    // Enemigos que siguen vivos; se usa para calcular el daño al jugador si pierde.
    public int SurvivorCount => EnemyArmy.GetAliveUnits().Count;

    public Battle(Army playerArmy, Army enemyArmy)
    {
        PlayerArmy = playerArmy;
        EnemyArmy = enemyArmy;
    }
    
    // Resuelve un turno. Solo debe llamarse mientras IsOver sea false.
    public BattleEvent NextTurn()
    {
        Army attackers;
        Army defenders;
        int cursor;
 
        if (isPlayerTurn)
        {
            attackers = PlayerArmy;
            defenders = EnemyArmy;
            cursor = playerCursor;
        }
        else
        {
            attackers = EnemyArmy;
            defenders = PlayerArmy;
            cursor = enemyCursor;
        }
 
        cursor = FindNextAlive(attackers, cursor);
        Unit attacker = attackers.GetUnit(cursor);
        int combatPosition = attackers.GetCombatPosition(attacker);
 
        BattleEvent battleEvent;
        if (attacker.CanAttackFrom(combatPosition))
        {
            List<Unit> targets = attacker.Attack(defenders);
            battleEvent = new BattleEvent(attacker, targets, attacker.Damage, false);
        }
        else
        {
            // Pierde su turno: no alcanza a ningún enemigo.
            battleEvent = new BattleEvent(attacker, new List<Unit>(), 0, true);
        }
 
        // Se guarda el cursor del bando que actuó y se cede el turno al otro bando.
        if (isPlayerTurn)
        {
            playerCursor = cursor;
        }
        else
        {
            enemyCursor = cursor;
        }
        isPlayerTurn = !isPlayerTurn;
 
        log.Add(battleEvent);
        return battleEvent;
    }

    // Siguiente unidad viva después de lastIndex. Al llegar al final de la fila, vuelve al inicio.
    private int FindNextAlive(Army army, int lastIndex)
    {
        int index = (lastIndex + 1) % army.Count;
        while (!army.GetUnit(index).IsAlive)
        {
            index = (index + 1) % army.Count;
        }
        return index;
    }
}