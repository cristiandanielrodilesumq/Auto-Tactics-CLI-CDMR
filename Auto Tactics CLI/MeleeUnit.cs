// Unidad cuerpo a cuerpo: solo puede atacar desde el frente.
public abstract class MeleeUnit : Unit
{
    protected MeleeUnit(string name, int baseHealth, int baseDamage, int cost, int stars)
        : base(name, baseHealth, baseDamage, cost, stars)
    {
    }

    public override bool CanAttackFrom(int position)
    {
        // La posición 0 es el frente.
        return position == 0;
    }
}