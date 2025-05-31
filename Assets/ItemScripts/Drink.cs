using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drink : Consumable
{
    public int water;
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
        UseStrategy = new DrinkUseStrategy();
    }

    public override string GetSpecificDescription()
    {
        return base.GetSpecificDescription() + "\n" +
               "Water: " + water;
    }
}
