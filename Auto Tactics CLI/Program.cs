ConsoleView view = new ConsoleView();
List<Army> waves = new WaveData().CreateWaves();
Player player = new Player();
Shop shop = new Shop();
Army wave = waves[0];
 
player.Earn(3);
shop.Refresh();
 
bool battleStarted = false;
while (!battleStarted)
{
    view.ShowPreparation(player, shop, wave, 1, waves.Count);
    char action = view.AskAction();
 
    switch (action)
    {
        case 'C':
            int slot = view.AskPosition("Espacio de la tienda", shop.OfferCount);
            if (!shop.TryBuy(slot, player))
            {
                view.ShowMessage("No se pudo comprar: espacio vacío, fila llena u oro insuficiente.");
            }
            break;
        case 'V':
            if (player.Army.Count == 0)
            {
                view.ShowMessage("No tienes unidades para vender.");
            }
            else
            {
                int position = view.AskPosition("Posición de la unidad a vender", player.Army.Count);
                shop.Sell(position, player);
            }
            break;
        case 'M':
            if (player.Army.Count < 2)
            {
                view.ShowMessage("Necesitas al menos dos unidades para moverlas.");
            }
            else
            {
                int first = view.AskPosition("Primera posición", player.Army.Count);
                int second = view.AskPosition("Segunda posición", player.Army.Count);
                player.Army.Swap(first, second);
            }
            break;
        case 'R':
            if (!shop.TryReroll(player))
            {
                view.ShowMessage("No tienes oro suficiente para renovar la tienda.");
            }
            break;
        case 'B':
            if (player.Army.Count == 0)
            {
                view.ShowMessage("No puedes iniciar la batalla con la fila vacía.");
            }
            else
            {
                battleStarted = true;
            }
            break;
    }
}
 
// Después del ciclo; el combate de siempre.
Battle battle = new Battle(player.Army, wave);
view.ShowMessage("¡Comienza el combate!");
 
while (!battle.IsOver)
{
    BattleEvent battleEvent = battle.NextTurn();
    view.ShowTurn(battle, battleEvent);
}
view.ShowResult(battle);