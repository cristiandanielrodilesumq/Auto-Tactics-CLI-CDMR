public abstract class RangedUnit : Unit
{
    protected RangedUnit(string name, int baseHealth, int baseDamage, int cost, int stars)
        : base(name, baseHealth, baseDamage, cost, stars)
    {
    }
    
    public override bool CanAttackFrom(int position)
    {
        return true;
    }
}