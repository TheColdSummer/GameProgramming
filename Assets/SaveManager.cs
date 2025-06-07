using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveManager : MonoBehaviour
{
    public GameObject backpack;
    public GameObject chestRig;
    public GameObject backpackItems;
    public GameObject helmet;
    public GameObject armor;
    public GameObject weapon;
    public GameObject warehouse;

    private string _backpackFilePath;
    private string _chestRigFilePath;
    private string _backpackItemsFilePath;
    private string _helmetFilePath;
    private string _armorFilePath;
    private string _weaponFilePath;
    private string _warehouseFilePath;
    private string _cashFilePath;
    private SaveLoadManager _saveLoadManager;

    void OnEnable()
    {
        _backpackFilePath = Application.persistentDataPath + "/backpack.json";
        _chestRigFilePath = Application.persistentDataPath + "/chestRig.json";
        _backpackItemsFilePath = Application.persistentDataPath + "/backpackItems.json";
        _helmetFilePath = Application.persistentDataPath + "/helmet.json";
        _armorFilePath = Application.persistentDataPath + "/armor.json";
        _weaponFilePath = Application.persistentDataPath + "/weapon.json";
        _warehouseFilePath = Application.persistentDataPath + "/warehouse.json";
        _cashFilePath = Application.persistentDataPath + "/cash.json";
        _saveLoadManager = new SaveLoadManager();
    }

    public void Save()
    {
        _saveLoadManager.SetPath(_backpackFilePath);
        _saveLoadManager.ClearSaveData();
        Item equippedBackpack = backpack.GetComponent<EquipmentInInventory>().equipment;
        if (equippedBackpack != null)
        {
            _saveLoadManager.SaveItem(equippedBackpack);
        }

        _saveLoadManager.SetPath(_chestRigFilePath);
        _saveLoadManager.ClearSaveData();
        Item equippedChestRig = chestRig.GetComponent<EquipmentInInventory>().equipment;
        if (equippedChestRig != null)
        {
            _saveLoadManager.SaveItem(equippedChestRig);
        }

        _saveLoadManager.SetPath(_backpackItemsFilePath);
        _saveLoadManager.ClearSaveData();
        List<Item> items = new List<Item>();
        foreach (Transform item in backpackItems.transform)
        {
            Item backpackItem = item.GetComponent<Item>();
            if (backpackItem != null)
            {
                items.Add(backpackItem);
            }
        }

        _saveLoadManager.SaveItems(items);

        _saveLoadManager.SetPath(_helmetFilePath);
        _saveLoadManager.ClearSaveData();
        Item equippedHelmet = helmet.GetComponent<EquipmentInInventory>().equipment;
        if (equippedHelmet != null)
        {
            _saveLoadManager.SaveItem(equippedHelmet);
        }

        _saveLoadManager.SetPath(_armorFilePath);
        _saveLoadManager.ClearSaveData();
        Item equippedArmor = armor.GetComponent<EquipmentInInventory>().equipment;
        if (equippedArmor != null)
        {
            _saveLoadManager.SaveItem(equippedArmor);
        }

        _saveLoadManager.SetPath(_weaponFilePath);
        _saveLoadManager.ClearSaveData();
        Item equippedWeapon = weapon.GetComponent<EquipmentInInventory>().equipment;
        if (equippedWeapon != null)
        {
            _saveLoadManager.SaveItem(equippedWeapon);
        }

        SaveWarehouse();
    }

    public void SaveWarehouse()
    {
        if (warehouse != null)
        {
            _saveLoadManager.SetPath(_warehouseFilePath);
            _saveLoadManager.ClearSaveData();
            List<Item> warehouseItems = new List<Item>();
            foreach (Transform item in warehouse.transform)
            {
                Item warehouseItem = item.GetComponent<Item>();
                if (warehouseItem != null)
                {
                    warehouseItems.Add(warehouseItem);
                }
            }

            _saveLoadManager.SaveItems(warehouseItems);
            _saveLoadManager.SetPath(_cashFilePath);
            _saveLoadManager.ClearSaveData();
            int cash = warehouse.GetComponent<Warehouse>().Cash;
            _saveLoadManager.SaveInt(cash);
        }
    }

    public void Load()
    {
        _saveLoadManager.SetPath(_backpackFilePath);
        ItemData loadedBackpack = _saveLoadManager.LoadItem();
        if (loadedBackpack != null)
        {
            backpack.GetComponent<EquipmentInInventory>()
                .ChangeEquipment(_saveLoadManager.ConstructGameObjectFromItemData(loadedBackpack));
        }

        _saveLoadManager.SetPath(_chestRigFilePath);
        ItemData loadedChestRig = _saveLoadManager.LoadItem();
        if (loadedChestRig != null)
        {
            chestRig.GetComponent<EquipmentInInventory>()
                .ChangeEquipment(_saveLoadManager.ConstructGameObjectFromItemData(loadedChestRig));
        }

        _saveLoadManager.SetPath(_backpackItemsFilePath);
        List<ItemData> loadedBackpackItems = _saveLoadManager.LoadItems();
        if (loadedBackpackItems != null)
        {
            foreach (ItemData itemData in loadedBackpackItems)
            {
                GameObject itemObject = _saveLoadManager.ConstructGameObjectFromItemData(itemData);
                itemObject.transform.SetParent(backpackItems.transform, false);
                itemObject.SetActive(true);
            }
        }

        GameObject playerInventory = GameObject.Find("Canvas/InventoryWithContainer/PlayerInventory");
        if (playerInventory != null)
        {
            playerInventory.GetComponent<PlayerInventory>().RefreshRemainingSpace();
        }

        _saveLoadManager.SetPath(_helmetFilePath);
        ItemData loadedHelmet = _saveLoadManager.LoadItem();
        if (loadedHelmet != null)
        {
            helmet.GetComponent<EquipmentInInventory>()
                .ChangeEquipment(_saveLoadManager.ConstructGameObjectFromItemData(loadedHelmet));
        }

        _saveLoadManager.SetPath(_armorFilePath);
        ItemData loadedArmor = _saveLoadManager.LoadItem();
        if (loadedArmor != null)
        {
            armor.GetComponent<EquipmentInInventory>()
                .ChangeEquipment(_saveLoadManager.ConstructGameObjectFromItemData(loadedArmor));
        }

        _saveLoadManager.SetPath(_weaponFilePath);
        ItemData loadedWeapon = _saveLoadManager.LoadItem();
        if (loadedWeapon != null)
        {
            GameObject w = _saveLoadManager.ConstructGameObjectFromItemData(loadedWeapon);
            weapon.GetComponent<EquipmentInInventory>()
                .ChangeEquipment(w);
            GameObject player = GameObject.Find("Player");
            if (player != null)
            {
                player.GetComponent<Player>().ChangeWeapon(w.GetComponent<Weapon>());
            }
        }

        LoadWarehouse();
    }

    public void LoadWarehouse()
    {
        if (warehouse != null)
        {
            _saveLoadManager.SetPath(_warehouseFilePath);
            List<ItemData> loadedWarehouseItems = _saveLoadManager.LoadItems();
            if (loadedWarehouseItems != null)
            {
                foreach (ItemData itemData in loadedWarehouseItems)
                {
                    GameObject itemObject = _saveLoadManager.ConstructGameObjectFromItemData(itemData);
                    itemObject.transform.SetParent(warehouse.transform, false);
                    itemObject.SetActive(true);
                }
            }

            _saveLoadManager.SetPath(_cashFilePath);
            int loadedCash = _saveLoadManager.LoadInt();
            if (loadedCash >= 0)
            {
                warehouse.GetComponent<Warehouse>().Cash = loadedCash;
            }
            else
            {
                Debug.LogWarning("Loaded cash value is negative, resetting to zero.");
                warehouse.GetComponent<Warehouse>().Cash = 0;
            }
        }
    }

    public void GameFail()
    {
        _saveLoadManager.SetPath(_backpackFilePath);
        _saveLoadManager.ClearSaveData();
        _saveLoadManager.SetPath(_chestRigFilePath);
        _saveLoadManager.ClearSaveData();
        _saveLoadManager.SetPath(_backpackItemsFilePath);
        _saveLoadManager.ClearSaveData();
        _saveLoadManager.SetPath(_helmetFilePath);
        _saveLoadManager.ClearSaveData();
        _saveLoadManager.SetPath(_armorFilePath);
        _saveLoadManager.ClearSaveData();
        _saveLoadManager.SetPath(_weaponFilePath);
        _saveLoadManager.ClearSaveData();
    }
}