ConsoleView view = new ConsoleView();
Player player = new Player();
Shop shop = new Shop();
player.Earn(3);
shop.Refresh();

// Prueba 1: comprar y vender.
view.ShowMessage($"Oro: {player.Gold} | Fila: {player.Army.Count}/{player.Army.Capacity}");
Unit? offer = shop.GetOffer(0);
bool bought = shop.TryBuy(0, player);
view.ShowMessage($"Compré {offer}: {bought} | Oro: {player.Gold} | Fila: {player.Army.Count}/{player.Army.Capacity}");

bool boughtAgain = shop.TryBuy(0, player);
view.ShowMessage($"Mismo espacio otra vez: {boughtAgain} | Oro: {player.Gold}");

shop.Sell(0, player);
view.ShowMessage($"Vendí la unidad | Oro: {player.Gold} | Fila: {player.Army.Count}/{player.Army.Capacity}");

// Prueba 2: renovar cuatro veces.
for (int i = 0; i < 4; i++)
{
    int goldBefore = player.Gold;
    bool rerolled = shop.TryReroll(player);
    view.ShowMessage($"Renovar: {rerolled} | Oro: {goldBefore} → {player.Gold}");
}