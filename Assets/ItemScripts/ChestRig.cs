using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestRig : Item
{
    public int innerSize; // Size of the inner chest, used for item placement

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
