using System.Collections;
using UnityEngine;

public class FoodUseStrategy : IUseStrategy
{
    public bool Use(Player player, Consumable consumable)
    {
        if (consumable is Food food)
        {
            if (player.currentRepletion < player.maxRepletion)
            {
                food.UseAnimation();
                player.StartCoroutine(UseFoodCoroutine(player, food));
                return true;
            }
            else
            {
                MessagePopup.Show("Player repletion is already full.");
                return false;
            }
        }
        else
        {
            Debug.LogError("Consumable is not a Food.");
            return false;
        }
    }
    
    private IEnumerator UseFoodCoroutine(Player player, Food food)
    {
        yield return new WaitForSeconds(food.useTime);
        player.ChangeRepletionDelta(food.repletion);
        GameObject.Destroy(food.gameObject);
    }
}