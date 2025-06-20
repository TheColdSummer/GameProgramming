using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : Item
{
    public int bodyDmg;
    public int ArmorDmg;
    public int range;
    public int RPM; // Rounds per minute
    public int capacity;
    public float reloadTime; // in seconds
    public int control;
    public int mode; // 0 = single, 1 = auto
    public string ammoType; // Type of ammo this weapon uses
    public int currentAmmo;

    void Start()
    {
        base.Start();

    }
    
    public override string GetSpecificDescription()
    {
        return base.GetSpecificDescription() + "\n" +
               "Body Damage: " + bodyDmg + "\n" +
               "Armor Damage: " + ArmorDmg + "\n" +
               "Range: " + range + "\n" +
               "RPM: " + RPM + "\n" +
               "Capacity: " + currentAmmo + "/" + capacity + "\n" +
               "Reload Time: " + reloadTime + " seconds\n" +
               "Control: " + control + "\n" +
               "Mode: " + (mode == 0 ? "Single" : "Auto") + "\n" +
               "Ammo Type: " + ammoType + "\n";
    }

    public override int GetSellPrice()
    {
        if (currentAmmo <= 0)
            return price;
        int ammoUnitPrice = 0;
        GameObject[] ammoPrefab = Resources.LoadAll<GameObject>("Ammo");
        foreach (GameObject ammo in ammoPrefab)
        {
            Ammo ammoScript = ammo.GetComponent<Ammo>();
            if (ammoScript != null && ammoScript.itemName == ammoType)
            {
                ammoUnitPrice = ammoScript.price / ammoScript.maxStackSize;
                break;
            }
        }
        return price + ammoUnitPrice * currentAmmo;
    }
}
