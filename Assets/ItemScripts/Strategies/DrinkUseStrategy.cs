using System.Collections;
using UnityEngine;

public class DrinkUseStrategy : IUseStrategy
{
    public bool Use(Player player, Consumable consumable)
    {
        if (consumable is Drink drink)
        {
            if (player.currentHydration < player.maxHydration)
            {
                drink.UseAnimation();
                player.StartCoroutine(UseDrinkCoroutine(player, drink));
                return true;
            }
            else
            {
                MessagePopup.Show("Player hydration is already full.");
                return false;
            }
        }
        else
        {
            Debug.LogError("Consumable is not a Drink.");
            return false;
        }
    }
    
    private IEnumerator UseDrinkCoroutine(Player player, Drink drink)
    {
        yield return new WaitForSeconds(drink.useTime);
        player.ChangeHydrationDelta(drink.water);
        GameObject.Destroy(drink.gameObject);
    }
}