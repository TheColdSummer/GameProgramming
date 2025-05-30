using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Backpack : Item
{
    public int innerSize; // The number of items the backpack can hold
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
               "Inner Size: " + innerSize;
    }
}
