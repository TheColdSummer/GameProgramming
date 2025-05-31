using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MedicalKit : Consumable
{
    public int hp;
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
        UseStrategy = new MedicalKitUseStrategy();
    }
    
    public override string GetSpecificDescription()
    {
        return base.GetSpecificDescription() + "\n" +
               "Health Points: " + hp;
    }
}
