using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ammo : Item
{
    public int maxStackSize;
    public int currentStackSize;
    
    void Start()
    {
        base.Start();
    }

    public int GetReloadAmount(int amount)
    {
        if (amount <= 0)
        {
            Debug.LogError("Reload amount must be greater than 0.");
            return 0;
        }
        int reloadAmount = Mathf.Min(amount, currentStackSize);
        currentStackSize -= reloadAmount;
        if (currentStackSize <= 0)
        {
            Destroy(gameObject);
        }
        return reloadAmount;
    }

    public override string GetSpecificDescription()
    {
        return base.GetSpecificDescription() + "\n" +
               "Stack Size: " + currentStackSize + "/" + maxStackSize;
    }

    public override int GetSellPrice()
    {
        return price * currentStackSize / maxStackSize;
    }
}
