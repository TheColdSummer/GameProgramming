using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MedicalKit : Consumable
{
    public int hp;

    void Start()
    {
        base.Start();

    }
    
    private void Awake()
    {
        UseStrategy = new MedicalKitUseStrategy();
    }
    
    public override string GetSpecificDescription()
    {
        return base.GetSpecificDescription() + "\n" +
               "Health Points: " + hp;
    }
}
