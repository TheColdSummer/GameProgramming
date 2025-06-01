using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Consumable : Item
{
    public int useTime;
    protected IUseStrategy UseStrategy;

    // use strategy mode to define how the item is used

    // Start is called before the first frame update
    void Start()
    {
        base.Start();

    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void Use(Player player)
    {
        bool used = UseStrategy != null && UseStrategy.Use(player, this);
        if (used)
        {
            Debug.Log("Item used: " + itemName);
        }
        else
        {
            Debug.Log("Failed to use item: " + itemName);
        }
    }
    
    public override string GetSpecificDescription()
    {
        return base.GetSpecificDescription() + "\n" +
               "Use Time: " + useTime + " seconds";
    }
}
