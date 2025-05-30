using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int maxHp;
    public int currentHp;
    public Backpack backpack;
    public Weapon weapon;
    public ChestRig chestRig;
    public Helmet helmet;
    public BodyArmor bodyArmor;
    public EnemyWeaponControl weaponControl;
    
    // Start is called before the first frame update
    void Start()
    {
        InitEnemy();
    }

    private void InitEnemy()
    {
        Random.InitState((int)System.DateTime.Now.Ticks);
        int p = Random.Range(0, 100);
        int level;
        if (p < 50)
        {
            level = 1;
        }
        else if (p < 80)
        {
            level = 2;
        }
        else if (p < 90)
        {
            level = 3;
        }
        else if (p < 95)
        {
            level = 4;
        }
        else
        {
            level = 5;
        }
        InitBackpack(backpack, level);
        InitChestRig(chestRig, level);
        InitHelmet(helmet, level);
        InitBodyArmor(bodyArmor, level);
        
        
        int w = Random.Range(1, 8);
        switch (w)
        {
            case 1:
                InitWeapon(weapon, "AKM");
                break;
            case 2:
                InitWeapon(weapon, "AWM");
                break;
            case 3:
                InitWeapon(weapon, "M4A1");
                break;
            case 4:
                InitWeapon(weapon, "M249");
                break;
            case 5:
                InitWeapon(weapon, "QBZ95");
                break;
            case 6:
                InitWeapon(weapon, "SCAR");
                break;
            case 7:
                InitWeapon(weapon, "SVD");
                break;
        }
        ChangeWeapon(weapon);
    }
    
    private void InitWeapon(Weapon w, string wName)
    {
        Weapon wm = Resources.Load<Weapon>("Weapon/" + wName);
        if (wm != null)
        {
            w.itemName = wm.itemName;
            w.type = wm.type;
            w.price = wm.price;
            w.size = wm.size;
            w.sprite = wm.sprite;
            w.id = wm.id;
            w.bodyDmg = wm.bodyDmg;
            w.ArmorDmg = wm.ArmorDmg;
            w.range = wm.range;
            w.RPM = wm.RPM;
            w.capacity = wm.capacity;
            w.reloadTime = wm.reloadTime;
            w.control = wm.control;
            w.mode = wm.mode;
            w.ammoType = wm.ammoType;
            w.currentAmmo = wm.currentAmmo;
            Destroy(wm);
        }
        else
        {
            Debug.LogError("Weapon not found: " + wName);
        }
    }

    private void InitBodyArmor(BodyArmor b, int level)
    {
        BodyArmor bm = Resources.Load<BodyArmor>("Armor/Armor" + level);
        if (bm != null)
        {
            b.itemName = bm.itemName;
            b.type = bm.type;
            b.price = bm.price;
            b.size = bm.size;
            b.sprite = bm.sprite;
            b.id = bm.id;
            b.durability = bm.durability;
            b.Maxdurability = bm.Maxdurability;
            Destroy(bm);
        }
        else
        {
            Debug.LogError("BodyArmor not found for level " + level);
        }
    }

    private void InitHelmet(Helmet h, int level)
    {
        Helmet hm = Resources.Load<Helmet>("Helmet/Helmet" + level);
        if (hm != null)
        {
            h.itemName = hm.itemName;
            h.type = hm.type;
            h.price = hm.price;
            h.size = hm.size;
            h.sprite = hm.sprite;
            h.id = hm.id;
            h.durability = hm.durability;
            h.Maxdurability = hm.Maxdurability;
            Destroy(hm);
        }
        else
        {
            Debug.LogError("Helmet not found for level " + level);
        }
    }

    private void InitChestRig(ChestRig c, int level)
    {
        ChestRig cm = Resources.Load<ChestRig>("ChestRig/ChestRig" + level);
        if (cm != null)
        {
            c.itemName = cm.itemName;
            c.type = cm.type;
            c.price = cm.price;
            c.size = cm.size;
            c.sprite = cm.sprite;
            c.id = cm.id;
            c.innerSize = cm.innerSize;
            Destroy(cm);
        }
        else
        {
            Debug.LogError("ChestRig not found for level " + level);
        }
    }

    private void InitBackpack(Backpack b, int level)
    {
        Backpack bm = Resources.Load<Backpack>("Backpack/Backpack" + level);
        
        if (bm != null)
        {
            b.itemName = bm.itemName;
            b.type = bm.type;
            b.price = bm.price;
            b.size = bm.size;
            b.sprite = bm.sprite;
            b.id = bm.id;
            b.innerSize = bm.innerSize;
            Destroy(bm);
        }
        else
        {
            Debug.LogError("Backpack not found for level " + level);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CauseHeadDamage(int bulletArmorDamage, int bulletBodyDamage)
    {
        if (helmet != null)
        {
            if (helmet.durability > 0)
            {
                helmet.ChangeDurabilityDelta(-bulletArmorDamage);
                ChangeHealthDelta((int)(-bulletBodyDamage * 1.5 / 10));
            }
            else
            {
                ChangeHealthDelta((int)(-bulletBodyDamage * 1.5));
            }
        }
        else
        {
            ChangeHealthDelta((int)(-bulletBodyDamage * 1.5));
        }
    }

    public void CauseBodyDamage(int bulletArmorDamage, int bulletBodyDamage)
    {
        if (bodyArmor != null)
        {
            if (bodyArmor.durability > 0)
            {
                bodyArmor.ChangeDurabilityDelta(-bulletArmorDamage);
                ChangeHealthDelta((int)(-bulletBodyDamage * 1.5 / 10));
            }
            else
            {
                ChangeHealthDelta(-bulletBodyDamage);
            }
        }
        else
        {
            ChangeHealthDelta(-bulletBodyDamage);
        }
    }
    
    public void ChangeHealthDelta(int delta)
    {
        currentHp += delta;
        if (currentHp <= 0)
        {
            currentHp = 0;
            Die();
        }
        if (currentHp > maxHp)
        {
            currentHp = maxHp;
        }
    }
    
    public void ChangeWeapon(Item equipment)
    {
        if (equipment is Weapon weaponItem)
        {
            weaponControl.ChangeWeapon(weaponItem);
        }
        else
        {
            Debug.LogError("Provided item is not a Weapon.");
        }
    }

    public void Die()
    {
        // remove some components
        weaponControl.enabled = false;
        Destroy(gameObject.GetComponent<HitDetector>());
        
        
        
        // rotate 90°
        transform.Rotate(0, 0, 90);
        
        
        // change the body to container
        
        
    }
}
