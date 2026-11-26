public class Player
{
    private const int ArmyCapacity = 3;

    public int Gold { get; private set; }
    public Army Army { get; }

    public Player()
    {
        Gold = 0;
        Health = StartingHealth;
        MaxHealth = StartingHealth;
        Army = new Army(ArmyCapacity);
    }

    public void Earn(int amount)
    {
        Gold += amount;
    }

    public bool TrySpend(int amount)
    {
        if (amount > Gold)
        {
            return false;
        }

        Gold -= amount;
        return true;
    }

    private const int StartingHealth = 15;
    public int MaxHealth { get; }
    public int Health { get; private set; }
    public bool IsDefeated => Health == 0;

    public void TakeDamage(int amount)
    {
        Health = Math.Max(Health - amount, 0);
    }
}