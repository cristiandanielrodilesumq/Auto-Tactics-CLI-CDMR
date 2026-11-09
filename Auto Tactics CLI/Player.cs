public class Player
{
  public int Gold { get; private set; } = 0;

  public void Earn(int amount)
  {
    Gold = Math.Max(Gold + amount, 0);
  }
  public bool TrySpend(int amount)
  {
    if (amount > Gold)
    {
      return false;
    }
    else
    {
      Gold = Gold - amount;
      return true;
    }
  }
}