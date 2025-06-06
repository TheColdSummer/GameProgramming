using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drink : Consumable
{
    public int water;

    void Start()
    {
        base.Start();

    }

    private void Awake()
    {
        UseStrategy = new DrinkUseStrategy();
    }

    public override string GetSpecificDescription()
    {
        return base.GetSpecificDescription() + "\n" +
               "Water: " + water;
    }
}
