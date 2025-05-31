using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Food : Consumable
{
    public int repletion;
    // Start is called before the first frame update
    void Start()
    {
        base.Start();

    }

    // Update is called once per frame
    void Update()
    {
        
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
