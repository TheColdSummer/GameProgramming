using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Helmet : Item
{
    public int durability;
    public int Maxdurability;
    // Start is called before the first frame update
    void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public override string GetSpecificDescription()
    {
        return base.GetSpecificDescription() + "\n" +
               "Durability: " + durability + "/" + Maxdurability;
    }

    public void ChangeDurabilityDelta(int delta)
    {
        durability += delta;
        if (durability < 0)
        {
            durability = 0;
        }
        if (durability > Maxdurability)
        {
            durability = Maxdurability;
        }
    }
}
