// Registro de un turno de combate. Solo guarda datos; no imprime nada.
public class BattleEvent
{
    public Unit Attacker { get; }
    public List<Unit> Targets { get; }
    public int Damage { get; }
    // true cuando la unidad no pudo atacar desde su posición ("no alcanza").
    public bool Blocked { get; }

    public BattleEvent(Unit attacker, List<Unit> targets, int damage, bool blocked)
    {
        Attacker = attacker;
        Targets = targets;
        Damage = damage;
        Blocked = blocked;
    }
}