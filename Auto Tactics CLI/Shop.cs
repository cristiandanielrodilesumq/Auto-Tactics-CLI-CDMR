public class Shop
{
    private const int OfferSlots = 3;
    private const int UnitTypeCount = 3;

    private List<Unit?> offers = new List<Unit?>();
    private Random random = new Random();
 
    public int RerollCost { get; } = 1;
    public int OfferCount => offers.Count;
    
    public void Refresh() 
    {
        offers.Clear();
        for (int i = 0; i < OfferSlots; i++)
        {
            offers.Add(CreateUnit(random.Next(UnitTypeCount)));
        }
    }
    
    public Unit? GetOffer(int slot)
    {
        return offers[slot];
    }
 
    private Unit CreateUnit(int typeIndex)
    {
        switch (typeIndex)
        {
            case 0:
                return new Warrior(1);
            case 1:
                return new Archer(1);
            default:
                return new Mage(1);
        }
    }
    
    public bool TryBuy(int slot, Player player)
    {
        Unit? offer = offers[slot];
        
        if (offer == null || player.Army.IsFull)
        {
            return false;
        }
        if (!player.TrySpend(offer.Cost))
        {
            return false;
        }
 
        player.Army.TryAdd(offer);
        offers[slot] = null;
        return true;
    }
    
    public void Sell(int position, Player player)
    {
        if (position < 0 || position >= player.Army.Count)
        {
            return;
        }
 
        Unit sold = player.Army.RemoveAt(position);
        player.Earn(sold.Cost);
    }
 
    public bool TryReroll(Player player)
    {
        if (!player.TrySpend(RerollCost))
        {
            return false;
        }
        Refresh();
        return true;
    }
}