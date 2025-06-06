using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public GameObject helmet;
    public GameObject armor;
    public GameObject weapon;
    public GameObject chestRig;
    public GameObject backpack;
    public Transform inventory; // The parent GameObject for the inventory items
    public GameObject remainingSpaceText;
    public GameObject containerContent;
    public Player player;
    
    bool AddItemToInventory(GameObject item)
    {
        if (item == null)
        {
            return false;
        }
        int remainingSize = GetRemainingSizeOfInventory();
        if (remainingSize < item.GetComponent<Item>().size)
        {
            MessagePopup.Show("Not enough space in inventory to add: " + item.GetComponent<Item>().itemName);
            return false;
        }
        if (CheckIfItemInInventory(item))
        {
            Debug.LogWarning("Item already exists in inventory: " + item.name);
            return false;
        }
        item.transform.SetParent(inventory, false);
        item.SetActive(true);
        return true;
    }
    
    public bool CheckIfItemInInventory(GameObject item)
    {
        if (item == null)
        {
            Debug.LogWarning("Cannot check for null item in inventory.");
            return false;
        }
        foreach (Transform child in inventory)
        {
            if (child.gameObject == item)
            {
                return true; // Item found in inventory
            }
        }
        return false; // Item not found in inventory
    }

    public int GetRemainingSizeOfInventory()
    {
        if (backpack == null || chestRig == null)
        {
            Debug.LogError("Backpack or Chest Rig GameObject not found in the inventory.");
            return 0;
        }
        EquipmentInInventory backpackScript = backpack.GetComponent<EquipmentInInventory>();
        EquipmentInInventory chestRigScript = chestRig.GetComponent<EquipmentInInventory>();
        if (backpackScript == null || chestRigScript == null)
        {
            Debug.LogError("EquipmentInInventory script not found on Backpack or Chest Rig GameObject.");
            return 0;
        }
        Backpack backpackItem = backpackScript.equipment as Backpack;
        ChestRig chestRigItem = chestRigScript.equipment as ChestRig;
        
        int backpackSize = 0;
        int chestRigSize = 0;
        if (backpackItem != null)
        {
            backpackSize = backpackItem.innerSize;
        }
        if (chestRigItem != null)
        {
            chestRigSize = chestRigItem.innerSize;
        }
        
        int maxSize = backpackSize + chestRigSize;
        int usedSize = 0;
        foreach (Transform child in inventory)
        {
            Item item = child.GetComponent<Item>();
            if (item != null)
            {
                usedSize += item.size;
            }
        }
        return maxSize - usedSize;
    }

    public void RefreshRemainingSpace()
    {
        if (remainingSpaceText == null)
        {
            Debug.LogError("Remaining space text GameObject not found in the scene.");
            return;
        }
        int remainingSize = GetRemainingSizeOfInventory();
        TextMeshProUGUI textComponent = remainingSpaceText.GetComponent<TextMeshProUGUI>();
        if (textComponent != null)
        {
            textComponent.text = remainingSize.ToString();
        }
        else
        {
            Debug.LogError("Text component not found on the Remaining Space GameObject.");
        }
    }

    public void ChangeEquipment(Item equipment)
    {
        GameObject equipmentObject = equipment.gameObject;
        if (equipment == null)
        {
            Debug.LogWarning("Cannot equip null item.");
            return;
        }
        GameObject ret = null;
        int remainingSize = GetRemainingSizeOfInventory();
        switch (equipment.type)
        {
            case "Helmet":
                if (helmet != null)
                {
                    EquipmentInInventory helmetScript = helmet.GetComponent<EquipmentInInventory>();
                    if (helmetScript != null)
                    {
                        if (helmetScript.equipment != null)
                        {
                            int newSize = helmetScript.equipment.size;
                            if (remainingSize + equipment.size - newSize < 0)
                            {
                                MessagePopup.Show("Not enough space in inventory to equip: " + equipment.itemName);
                                return;
                            }
                        }
                        ret = helmetScript.ChangeEquipment(equipmentObject);
                        if (player != null)
                        {
                            player.ChangeHelmet(equipment);
                        }
                    }
                    else
                    {
                        Debug.LogError("Helmet script not found on the Helmet GameObject.");
                    }
                }
                else
                {
                    Debug.LogError("Helmet GameObject not found in the scene.");
                }
                break;
            case "Armor":
                if (armor != null)
                {
                    EquipmentInInventory armorScript = armor.GetComponent<EquipmentInInventory>();
                    if (armorScript != null)
                    {
                        if (armorScript.equipment != null)
                        {
                            int newSize = armorScript.equipment.size;
                            if (remainingSize + equipment.size - newSize < 0)
                            {
                                MessagePopup.Show("Not enough space in inventory to equip: " + equipment.itemName);
                                return;
                            }
                        }
                        ret = armorScript.ChangeEquipment(equipmentObject);
                        if (player != null)
                        {
                            player.ChangeBodyArmor(equipment);
                        }
                    }
                    else
                    {
                        Debug.LogError("Armor script not found on the Armor GameObject.");
                    }
                }
                else
                {
                    Debug.LogError("Armor GameObject not found in the scene.");
                }
                break;
            case "Weapon":
                if (weapon != null)
                {
                    EquipmentInInventory weaponScript = weapon.GetComponent<EquipmentInInventory>();
                    if (weaponScript != null)
                    {
                        if (weaponScript.equipment != null)
                        {
                            int newSize = weaponScript.equipment.size;
                            if (remainingSize + equipment.size - newSize < 0)
                            {
                                MessagePopup.Show("Not enough space in inventory to equip: " + equipment.itemName);
                                return;
                            }
                        }
                        ret = weaponScript.ChangeEquipment(equipmentObject);
                        if (player != null)
                        {
                            player.ChangeWeapon(equipment);
                        }
                    }
                    else
                    {
                        Debug.LogError("Weapon script not found on the Weapon GameObject.");
                    }
                }
                else
                {
                    Debug.LogError("Weapon GameObject not found in the scene.");
                }
                break;
            case "Chest Rig":
                if (chestRig != null)
                {
                    EquipmentInInventory chestRigScript = chestRig.GetComponent<EquipmentInInventory>();
                    if (chestRigScript != null)
                    {
                        if (chestRigScript.equipment != null)
                        {
                            int newSize = chestRigScript.equipment? chestRigScript.equipment.size : 0;
                            if (remainingSize + equipment.size - newSize - (chestRigScript.equipment as ChestRig).innerSize + (equipment as ChestRig).innerSize  < 0)
                            {
                                MessagePopup.Show("Not enough space in inventory to equip: " + equipment.itemName);
                                return;
                            }
                        }
                        ret = chestRigScript.ChangeEquipment(equipmentObject);
                    }
                    else
                    {
                        Debug.LogError("Chest Rig script not found on the Chest Rig GameObject.");
                    }
                }
                else
                {
                    Debug.LogError("Chest Rig GameObject not found in the scene.");
                }
                break;
            case "Backpack":
                if (backpack != null)
                {
                    EquipmentInInventory backpackScript = backpack.GetComponent<EquipmentInInventory>();
                    if (backpackScript != null)
                    {
                        int newSize = backpackScript.equipment? backpackScript.equipment.size : 0;
                        if (remainingSize + equipment.size - newSize - (backpackScript.equipment as Backpack).innerSize + (equipment as Backpack).innerSize < 0)
                        {
                            MessagePopup.Show("Not enough space in inventory to equip: " + equipment.itemName);
                            return;
                        }
                        ret = backpackScript.ChangeEquipment(equipmentObject);
                    }
                    else
                    {
                        Debug.LogError("Backpack script not found on the Backpack GameObject.");
                    }
                }
                else
                {
                    Debug.LogError("Backpack GameObject not found in the scene.");
                }
                break;
            default:
                Debug.LogWarning("Unknown equipment type: " + equipment.type);
                return;
        }
        AddItemToInventory(ret);
        RefreshRemainingSpace();
    }
    
    public void ChangeEquipmentFromContainer(Item equipment)
    {
        GameObject equipmentObject = equipment.gameObject;
        if (equipment == null)
        {
            Debug.LogWarning("Cannot equip null item.");
            return;
        }
        GameObject ret = null;
        switch (equipment.type)
        {
            case "Helmet":
                if (helmet != null)
                {
                    EquipmentInInventory helmetScript = helmet.GetComponent<EquipmentInInventory>();
                    if (helmetScript != null)
                    {
                        ret = helmetScript.ChangeEquipment(equipmentObject);
                        if (player != null)
                        {
                            player.ChangeHelmet(equipment);
                        }
                    }
                    else
                    {
                        Debug.LogError("Helmet script not found on the Helmet GameObject.");
                    }
                }
                else
                {
                    Debug.LogError("Helmet GameObject not found in the scene.");
                }
                break;
            case "Armor":
                if (armor != null)
                {
                    EquipmentInInventory armorScript = armor.GetComponent<EquipmentInInventory>();
                    if (armorScript != null)
                    {
                        ret = armorScript.ChangeEquipment(equipmentObject);
                        if (player != null)
                        {
                            player.ChangeBodyArmor(equipment);
                        }
                    }
                    else
                    {
                        Debug.LogError("Armor script not found on the Armor GameObject.");
                    }
                }
                else
                {
                    Debug.LogError("Armor GameObject not found in the scene.");
                }
                break;
            case "Weapon":
                if (weapon != null)
                {
                    EquipmentInInventory weaponScript = weapon.GetComponent<EquipmentInInventory>();
                    if (weaponScript != null)
                    {
                        ret = weaponScript.ChangeEquipment(equipmentObject);
                        if (player != null)
                        {
                            player.ChangeWeapon(equipment);
                        }
                    }
                    else
                    {
                        Debug.LogError("Weapon script not found on the Weapon GameObject.");
                    }
                }
                else
                {
                    Debug.LogError("Weapon GameObject not found in the scene.");
                }
                break;
            case "Chest Rig":
                if (chestRig != null)
                {
                    EquipmentInInventory chestRigScript = chestRig.GetComponent<EquipmentInInventory>();
                    if (chestRigScript != null)
                    {
                        int remainingSize = GetRemainingSizeOfInventory();
                        int oldInnerSize = 0;
                        int newInnerSize = (equipment as ChestRig).innerSize;
                        if (chestRigScript.equipment != null)
                        {
                            oldInnerSize = (chestRigScript.equipment as ChestRig).innerSize;
                        }
                        if (remainingSize - oldInnerSize + newInnerSize < 0)
                        {
                            MessagePopup.Show("Not enough space in inventory to equip: " + equipment.itemName);
                            return;
                        }
                        ret = chestRigScript.ChangeEquipment(equipmentObject);
                    }
                    else
                    {
                        Debug.LogError("Chest Rig script not found on the Chest Rig GameObject.");
                    }
                }
                else
                {
                    Debug.LogError("Chest Rig GameObject not found in the scene.");
                }
                break;
            case "Backpack":
                if (backpack != null)
                {
                    EquipmentInInventory backpackScript = backpack.GetComponent<EquipmentInInventory>();
                    if (backpackScript != null)
                    {
                        int remainingSize = GetRemainingSizeOfInventory();
                        int oldInnerSize = 0;
                        int newInnerSize = (equipment as Backpack).innerSize;
                        if (backpackScript.equipment != null)
                        {
                            oldInnerSize = (backpackScript.equipment as Backpack).innerSize;
                        }
                        if (remainingSize - oldInnerSize + newInnerSize < 0)
                        {
                            MessagePopup.Show("Not enough space in inventory to equip: " + equipment.itemName);
                            return;
                        }
                        ret = backpackScript.ChangeEquipment(equipmentObject);
                    }
                    else
                    {
                        Debug.LogError("Backpack script not found on the Backpack GameObject.");
                    }
                }
                else
                {
                    Debug.LogError("Backpack GameObject not found in the scene.");
                }
                break;
            default:
                Debug.LogWarning("Unknown equipment type: " + equipment.type);
                return;
        }
        RefreshRemainingSpace();
        if (ret == null)
        {
            return;
        }
        containerContent.GetComponent<ContainerContentUI>().ReceiveItemFromPlayer(ret);
    }

    public void DropItem(GameObject item)
    {
        if (item == null)
        {
            Debug.LogWarning("Cannot drop null item.");
            return;
        }
        containerContent.GetComponent<ContainerContentUI>().ReceiveItemFromPlayer(item);
        RefreshRemainingSpace();
    }

    public void PickItem(GameObject item)
    {
        if (item == null)
        {
            Debug.LogWarning("Cannot pick null item.");
            return;
        }
        if (AddItemToInventory(item))
        {
            Debug.Log("Picked up item: " + item.name);
        }
        else
        {
            Debug.LogWarning("Failed to pick up item: " + item.name);
        }
        RefreshRemainingSpace();
    }

    public void DropEquippedItem(GameObject clickedGameObject)
    {
        EquipmentInInventory equipmentInInventory = clickedGameObject.GetComponent<EquipmentInInventory>();
        Item equipped = equipmentInInventory.equipment;
        if (equipped == null)
        {
            Debug.LogWarning("No equipment to drop.");
            return;
        }
        
        int remainingSize = GetRemainingSizeOfInventory();
        int innerSize = 0;
        if (equipped.type == "Backpack")
        {
            innerSize = (equipped as Backpack).innerSize;
        }
        else if (equipped.type == "Chest Rig")
        {
            innerSize = (equipped as ChestRig).innerSize;
        }
        
        if (remainingSize - innerSize < 0)
        {
            MessagePopup.Show("Not enough space in inventory to drop: " + equipped.itemName);
            return;
        }

        if (player != null)
        {
            player.UnequipItem(equipped);
        }
        GameObject itemToDrop = equipmentInInventory.ChangeEquipment(null);
        if (itemToDrop == null)
        {
            return;
        }
        containerContent.GetComponent<ContainerContentUI>().ReceiveItemFromPlayer(itemToDrop);
        RefreshRemainingSpace();
    }

    public void UnequipItem(GameObject clickedGameObject)
    {
        EquipmentInInventory equipmentInInventory = clickedGameObject.GetComponent<EquipmentInInventory>();
        Item equipped = equipmentInInventory.equipment;
        if (equipped == null)
        {
            Debug.LogWarning("No equipment to drop.");
            return;
        }

        int remainingSize = GetRemainingSizeOfInventory();
        int innerSize = 0;
        if (equipped.type == "Backpack")
        {
            innerSize = (equipped as Backpack).innerSize;
        }
        else if (equipped.type == "Chest Rig")
        {
            innerSize = (equipped as ChestRig).innerSize;
        }

        if (remainingSize - innerSize - equipped.size < 0)
        {
            MessagePopup.Show("Not enough space in inventory to drop: " + equipped.itemName);
            return;
        }

        if (player != null)
        {
            player.UnequipItem(equipped);
        }
        GameObject itemToUnequip = equipmentInInventory.ChangeEquipment(null);
        if (itemToUnequip == null)
        {
            return;
        }
        AddItemToInventory(itemToUnequip);
        RefreshRemainingSpace();
    }

    public void UseConsumable(Consumable consumable)
    {
        if (consumable == null)
        {
            Debug.LogWarning("Cannot use null consumable.");
            return;
        }
        consumable.Use(player);
    }
}
