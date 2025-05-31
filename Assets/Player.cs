using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public int maxHp;
    public int currentHp;
    public int maxRepletion;
    public int currentRepletion;
    public int maxHydration;
    public int currentHydration;
    public GameObject inventoryContent;
    public GameObject backpack;
    public GameObject weapon;
    public GameObject chestRig;
    public GameObject helmet;
    public GameObject bodyArmor;
    public Bar hpBar;
    public Bar repletionBar;
    public Bar hydrationBar;
    public Bar armorBar;
    public Bar helmetBar;
    public WeaponControl weaponControl;
    public GameObject reloadUI;
    private bool _isReloading = false;

    // Start is called before the first frame update
    void Start()
    {
        armorBar.SetValue(GetCurBodyArmorDurability(), GetMaxBodyArmorDurability());
        helmetBar.SetValue(GetCurHelmetDurability(), GetMaxHelmetDurability());
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R) && !_isReloading)
        {
            _isReloading = true;
            Reload();
            _isReloading = false;
        }
    }

    private void Reload()
    {
        if (weapon == null)
        {
            Debug.LogWarning("No weapon to reload.");
            return;
        }
        EquipmentInInventory equipmentInInventory = weapon.GetComponent<EquipmentInInventory>();
        if (equipmentInInventory == null || equipmentInInventory.equipment == null)
        {
            Debug.LogWarning("No valid weapon found in the inventory.");
            return;
        }
        
        Weapon w = equipmentInInventory.equipment.GetComponent<Weapon>();
        int requiredAmmo = w.capacity - w.currentAmmo;
        int getAmmo = requiredAmmo;
        if (requiredAmmo <= 0)
        {
            return;
        }

        string ammoType = w.GetComponent<Weapon>().ammoType;
        List<Ammo> ammoPacks = new List<Ammo>();
        foreach (Transform item in inventoryContent.transform)
        {
            Item inventoryItem = item.GetComponent<Item>();
            if (inventoryItem is Ammo ammo && ammo.itemName == ammoType)
            {
                ammoPacks.Add(ammo);
            }
        }

        ammoPacks.Sort((a, b) => a.currentStackSize.CompareTo(b.currentStackSize));
        foreach (Ammo ammoPack in ammoPacks)
        {
            if (ammoPack.currentStackSize <= 0)
            {
                continue;
            }

            int getAmount = ammoPack.GetReloadAmount(requiredAmmo);
            requiredAmmo -= getAmount;
            if (requiredAmmo <= 0)
            {
                break;
            }
        }
        getAmmo -= requiredAmmo;
        if (getAmmo <= 0)
        {
            Debug.LogWarning("No ammo available for reloading.");
            return;
        }
        StartCoroutine(Reload(w, getAmmo));
    }

    private IEnumerator Reload(Weapon w, int getAmmo)
    {
        if (reloadUI != null)
        {
            reloadUI.SetActive(true);
        }
        yield return new WaitForSeconds(w.reloadTime);
        weaponControl.ReLoad(getAmmo);
        if (reloadUI != null)
        {
            reloadUI.SetActive(false);
        }
    }

    public void ChangeHealthDelta(int delta)
    {
        currentHp += delta;
        if (currentHp > maxHp)
        {
            currentHp = maxHp;
        }
        else if (currentHp < 0)
        {
            currentHp = 0;
        }

        if (hpBar != null)
        {
            hpBar.SetValue(currentHp, maxHp);
        }
    }

    public void ChangeRepletionDelta(int delta)
    {
        currentRepletion += delta;
        if (currentRepletion > maxRepletion)
        {
            currentRepletion = maxRepletion;
        }
        else if (currentRepletion < 0)
        {
            currentRepletion = 0;
        }

        if (repletionBar != null)
        {
            repletionBar.SetValue(currentRepletion, maxRepletion);
        }
    }

    public void ChangeHydrationDelta(int delta)
    {
        currentHydration += delta;
        if (currentHydration > maxHydration)
        {
            currentHydration = maxHydration;
        }
        else if (currentHydration < 0)
        {
            currentHydration = 0;
        }

        if (hydrationBar != null)
        {
            hydrationBar.SetValue(currentHydration, maxHydration);
        }
    }

    public int GetCurBodyArmorDurability()
    {
        EquipmentInInventory armor = bodyArmor.GetComponent<EquipmentInInventory>();
        if (armor != null && armor.equipment != null)
        {
            BodyArmor bodyArmorItem = armor.equipment.GetComponent<BodyArmor>();
            if (bodyArmorItem != null)
            {
                return bodyArmorItem.durability;
            }
        }

        return 0;
    }

    public int GetMaxBodyArmorDurability()
    {
        EquipmentInInventory armor = bodyArmor.GetComponent<EquipmentInInventory>();
        if (armor != null && armor.equipment != null)
        {
            BodyArmor bodyArmorItem = armor.equipment.GetComponent<BodyArmor>();
            if (bodyArmorItem != null)
            {
                return bodyArmorItem.Maxdurability;
            }
        }

        return 0;
    }

    public int GetCurHelmetDurability()
    {
        EquipmentInInventory helmetEquip = helmet.GetComponent<EquipmentInInventory>();
        if (helmetEquip != null && helmetEquip.equipment != null)
        {
            Helmet helmetItem = helmetEquip.equipment.GetComponent<Helmet>();
            if (helmetItem != null)
            {
                return helmetItem.durability;
            }
        }

        return 0;
    }

    public int GetMaxHelmetDurability()
    {
        EquipmentInInventory helmetEquip = helmet.GetComponent<EquipmentInInventory>();
        if (helmetEquip != null && helmetEquip.equipment != null)
        {
            Helmet helmetItem = helmetEquip.equipment.GetComponent<Helmet>();
            if (helmetItem != null)
            {
                return helmetItem.Maxdurability;
            }
        }

        return 0;
    }

    public void CauseHeadDamage(int bulletArmorDamage, int bulletBodyDamage)
    {
        EquipmentInInventory helmetEquip = helmet.GetComponent<EquipmentInInventory>();
        if (helmetEquip.equipment != null)
        {
            Helmet helmetItem = helmetEquip.equipment.GetComponent<Helmet>();
            if (helmetItem != null)
            {
                if (helmetItem.durability > 0)
                {
                    helmetItem.ChangeDurabilityDelta(-bulletArmorDamage);
                    helmetBar.UpdateValueDelta(-bulletArmorDamage);
                    ChangeHealthDelta((int)(-bulletBodyDamage * 1.5 / 10));
                }
                else
                {
                    ChangeHealthDelta((int)(-bulletBodyDamage * 1.5));
                }
            }
        }
        else
        {
            ChangeHealthDelta((int)(-bulletBodyDamage * 1.5));
        }
    }

    public void CauseBodyDamage(int bulletArmorDamage, int bulletBodyDamage)
    {
        EquipmentInInventory armor = bodyArmor.GetComponent<EquipmentInInventory>();
        if (armor.equipment != null)
        {
            BodyArmor bodyArmorItem = armor.equipment.GetComponent<BodyArmor>();
            if (bodyArmorItem != null)
            {
                if (bodyArmorItem.durability > 0)
                {
                    bodyArmorItem.ChangeDurabilityDelta(-bulletArmorDamage);
                    armorBar.UpdateValueDelta(-bulletArmorDamage);
                    ChangeHealthDelta(-bulletBodyDamage / 10);
                }
                else
                {
                    ChangeHealthDelta(-bulletBodyDamage);
                }
            }
        }
        else
        {
            ChangeHealthDelta(-bulletBodyDamage);
        }
    }

    public void ChangeHelmet(Item equipment)
    {
        if (equipment is Helmet helmetItem)
        {
            helmetBar.SetValue(helmetItem.durability, helmetItem.Maxdurability);
        }
        else
        {
            Debug.LogError("Provided item is not a Helmet.");
        }
    }

    public void ChangeBodyArmor(Item equipment)
    {
        if (equipment is BodyArmor bodyArmorItem)
        {
            armorBar.SetValue(bodyArmorItem.durability, bodyArmorItem.Maxdurability);
        }
        else
        {
            Debug.LogError("Provided item is not a BodyArmor.");
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
}