using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Food : Consumable
{
    public int repletion;

    void Start()
    {
        base.Start();

    }
    
    private void Awake()
    {
        UseStrategy = new FoodUseStrategy();
    }

    public override string GetSpecificDescription()
    {
        return base.GetSpecificDescription() + "\n" +
               "Repletion: " + repletion;
    }
}
