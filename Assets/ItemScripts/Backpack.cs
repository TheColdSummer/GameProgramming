using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Backpack : Item
{
    public int innerSize; // The number of items the backpack can hold

    void Start()
    {
        base.Start();

    }
    
    public override string GetSpecificDescription()
    {
        return base.GetSpecificDescription() + "\n" +
               "Inner Size: " + innerSize;
    }
}
