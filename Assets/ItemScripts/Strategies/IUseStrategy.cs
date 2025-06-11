/*
 * This is an interface for use strategies in the game.
 */
public interface IUseStrategy
{
    bool Use(Player player, Consumable consumable);
}