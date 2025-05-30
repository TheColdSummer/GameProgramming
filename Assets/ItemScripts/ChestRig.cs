using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestRig : Item
{
    public int innerSize; // Size of the inner chest, used for item placement

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
