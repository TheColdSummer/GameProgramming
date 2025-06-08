using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class Helmet : Item
{
    public int durability;
    public int maxDurability;

    void Start()
    {
        base.Start();
    }
    
    public override string GetSpecificDescription()
    {
        return base.GetSpecificDescription() + "\n" +
               "Durability: " + durability + "/" + maxDurability;
    }

    public void ChangeDurabilityDelta(int delta)
    {
        durability += delta;
        if (durability < 0)
        {
            durability = 0;
        }
        if (durability > maxDurability)
        {
            durability = maxDurability;
        }
    }

    public override int GetSellPrice()
    {
        return price * durability / maxDurability;
    }
}
