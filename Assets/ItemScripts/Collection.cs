using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collection : Item
{
    void Start()
    {
        base.Start();

    }
    
    public override string GetSpecificDescription()
    {
        return base.GetSpecificDescription() + "\n" +
               "This is a collection item.";
    }
}
