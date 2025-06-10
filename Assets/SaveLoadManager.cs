using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;
using System.IO;
using UnityEditor;

public class SaveLoadManager: MonoBehaviour
{
    private string _savePath;

    public void SetPath(string path)
    {
        _savePath = path;
    }
    
    public void SaveItem(Item item)
    {
        ItemData itemData = ToItemData(item);
        string json = JsonConvert.SerializeObject(itemData, Formatting.Indented, new JsonSerializerSettings
        {
            TypeNameHandling = TypeNameHandling.All
        });
        File.WriteAllText(_savePath, json);
    }
    
    public ItemData LoadItem()
    {
        if (!File.Exists(_savePath))
        {
            Debug.Log("Save file not found at " + _savePath);
            return null;
        }

        string json = File.ReadAllText(_savePath);
        ItemData itemData = JsonConvert.DeserializeObject<ItemData>(json, new JsonSerializerSettings
        {
            TypeNameHandling = TypeNameHandling.All
        });
        return itemData;
    }
    
    public void SaveItems(List<Item> items)
    {
        List<ItemData> itemDataList = new List<ItemData>();
        foreach (var item in items)
        {
            itemDataList.Add(ToItemData(item));
        }
        
        string json = JsonConvert.SerializeObject(itemDataList, Formatting.Indented, new JsonSerializerSettings
        {
            TypeNameHandling = TypeNameHandling.All
        });
        File.WriteAllText(_savePath, json);
    }
    
    public List<ItemData> LoadItems()
    {
        if (!File.Exists(_savePath))
        {
            Debug.Log("Save file not found at " + _savePath);
            return null;
        }

        string json = File.ReadAllText(_savePath);
        List<ItemData> itemDataList = JsonConvert.DeserializeObject<List<ItemData>>(json, new JsonSerializerSettings
        {
            TypeNameHandling = TypeNameHandling.All
        });
        return itemDataList;
    }
    
    
    public void SaveInt(int cash)
    {
        string json = JsonConvert.SerializeObject(cash, Formatting.Indented);
        File.WriteAllText(_savePath, json);
    }

    public int LoadInt()
    {
        if (!File.Exists(_savePath))
        {
            Debug.Log("Save file not found at " + _savePath);
            return 0; // Default value if file doesn't exist
        }
        string json = File.ReadAllText(_savePath);
        int cash = JsonConvert.DeserializeObject<int>(json);
        return cash;
    }
    
    public GameObject ConstructGameObjectFromItemData(ItemData itemData)
    {
        GameObject itemObject  = Instantiate(Resources.Load<GameObject>("Item"));
        if (itemData is AmmoData ammoData)
        {
            Ammo ammo = itemObject.AddComponent<Ammo>();
            FromAmmoData(ammo, ammoData);
        }
        else if (itemData is BackpackData backpackData)
        {
            Backpack backpack = itemObject.AddComponent<Backpack>();
            FromBackpackData(backpack, backpackData);
        }
        else if (itemData is BodyArmorData armorData)
        {
            BodyArmor armor = itemObject.AddComponent<BodyArmor>();
            FromBodyArmorData(armor, armorData);
        }
        else if (itemData is HelmetData helmetData)
        {
            Helmet helmet = itemObject.AddComponent<Helmet>();
            FromHelmetData(helmet, helmetData);
        }
        else if (itemData is MedicalKitData medicalKitData)
        {
            MedicalKit medicalKit = itemObject.AddComponent<MedicalKit>();
            FromMedicalKitData(medicalKit, medicalKitData);
        }
        else if (itemData is WeaponData weaponData)
        {
            Weapon weapon = itemObject.AddComponent<Weapon>();
            FromWeaponData(weapon, weaponData);
        }
        else if (itemData is CollectionData collectionData)
        {
            Collection collection = itemObject.AddComponent<Collection>();
            FromCollectionData(collection, collectionData);
        }
        else if (itemData is DrinkData drinkData)
        {
            Drink drink = itemObject.AddComponent<Drink>();
            FromDrinkData(drink, drinkData);
        }
        else if (itemData is FoodData foodData)
        {
            Food food = itemObject.AddComponent<Food>();
            FromFoodData(food, foodData);
        }
        else if (itemData is ChestRigData chestRigData)
        {
            ChestRig chestRig = itemObject.AddComponent<ChestRig>();
            FromChestRigData(chestRig, chestRigData);
        }
        else
        {
            Debug.LogError("Unknown item type: " + itemData.type);
            return null;
        }

        return itemObject;
    }
    
    public ItemData ToItemData(Item item)
    {
        if (item is Ammo ammo) return ToAmmoData(ammo);
        if (item is Backpack backpack) return ToBackpackData(backpack);
        if (item is BodyArmor armor) return ToBodyArmorData(armor);
        if (item is Helmet helmet) return ToHelmetData(helmet);
        if (item is MedicalKit medicalKit) return ToMedicalKitData(medicalKit);
        if (item is Weapon weapon) return ToWeaponData(weapon);
        if (item is Collection collection) return ToCollectionData(collection);
        if (item is Drink drink) return ToDrinkData(drink);
        if (item is Food food) return ToFoodData(food);
        if (item is ChestRig chestRig) return ToChestRigData(chestRig);
        return new ItemData
        {
            itemName = item.itemName,
            type = item.type,
            price = item.price,
            size = item.size,
            spriteName = item.sprite != null ? GetSpriteResourcePath(item) : null,
            id = item.id
        };
    }

    private string GetSpriteResourcePath(Item item)
    {
        if (item.sprite == null)
        {
            Debug.LogWarning("Sprite is null for item: " + item.itemName);
            return null;
        }
        if (item is Backpack backpack) return "ItemTexture/Backpack/" + item.sprite.name + "/" + item.sprite.name;
        if (item is BodyArmor armor) return "ItemTexture/Armor/" + item.sprite.name + "/" + item.sprite.name;
        if (item is Helmet helmet) return "ItemTexture/Helmet/" + item.sprite.name + "/" + item.sprite.name;
        if (item is MedicalKit medicalKit) return "ItemTexture/MedicalKit/" + item.sprite.name + "/" + item.sprite.name;
        if (item is Weapon weapon) return "ItemTexture/Weapon/" + item.sprite.name + "/" + item.sprite.name;
        if (item is Collection collection) return "ItemTexture/Collection/" + item.sprite.name + "/" + item.sprite.name;
        if (item is Drink drink) return "ItemTexture/Drink/" + item.sprite.name + "/" + item.sprite.name;
        if (item is Food food) return "ItemTexture/Food/" + item.sprite.name + "/" + item.sprite.name;
        if (item is ChestRig chestRig) return "ItemTexture/ChestRig/" + item.sprite.name + "/" + item.sprite.name;
        if (item is Ammo ammo) return "ItemTexture/Ammo/" + item.sprite.name;
        return null;
    }

    public void FromItemData(Item item, ItemData itemData)
    {
        item.itemName = itemData.itemName;
        item.type = itemData.type;
        item.price = itemData.price;
        item.size = itemData.size;
        item.sprite = Resources.Load<Sprite>(itemData.spriteName);
        item.id = itemData.id;
    }

    public AmmoData ToAmmoData(Ammo ammo)
    {
        return new AmmoData
        {
            itemName = ammo.itemName,
            type = ammo.type,
            price = ammo.price,
            size = ammo.size,
            spriteName = ammo.sprite != null ? GetSpriteResourcePath(ammo) : null,
            id = ammo.id,
            maxStackSize = ammo.maxStackSize,
            currentStackSize = ammo.currentStackSize
        };
    }
    
    public void FromAmmoData(Ammo ammo, AmmoData ammoData)
    {
        FromItemData(ammo, ammoData);
        ammo.maxStackSize = ammoData.maxStackSize;
        ammo.currentStackSize = ammoData.currentStackSize;
    }
    
    public BackpackData ToBackpackData(Backpack backpack)
    {
        return new BackpackData
        {
            itemName = backpack.itemName,
            type = backpack.type,
            price = backpack.price,
            size = backpack.size,
            spriteName = backpack.sprite != null ? GetSpriteResourcePath(backpack) : null,
            id = backpack.id,
            innerSize = backpack.innerSize
        };
    }
    
    public void FromBackpackData(Backpack backpack, BackpackData backpackData)
    {
        FromItemData(backpack, backpackData);
        backpack.innerSize = backpackData.innerSize;
    }
    
    public BodyArmorData ToBodyArmorData(BodyArmor armor)
    {
        return new BodyArmorData
        {
            itemName = armor.itemName,
            type = armor.type,
            price = armor.price,
            size = armor.size,
            spriteName = armor.sprite != null ? GetSpriteResourcePath(armor) : null,
            id = armor.id,
            durability = armor.durability,
            maxDurability = armor.maxDurability
        };
    }
    
    public void FromBodyArmorData(BodyArmor armor, BodyArmorData armorData)
    {
        FromItemData(armor, armorData);
        armor.durability = armorData.durability;
        armor.maxDurability = armorData.maxDurability;
    }
    
    public HelmetData ToHelmetData(Helmet helmet)
    {
        return new HelmetData
        {
            itemName = helmet.itemName,
            type = helmet.type,
            price = helmet.price,
            size = helmet.size,
            spriteName = helmet.sprite != null ? GetSpriteResourcePath(helmet) : null,
            id = helmet.id,
            durability = helmet.durability,
            maxDurability = helmet.maxDurability
        };
    }
    
    public void FromHelmetData(Helmet helmet, HelmetData helmetData)
    {
        FromItemData(helmet, helmetData);
        helmet.durability = helmetData.durability;
        helmet.maxDurability = helmetData.maxDurability;
    }
    
    public MedicalKitData ToMedicalKitData(MedicalKit medicalKit)
    {
        return new MedicalKitData
        {
            itemName = medicalKit.itemName,
            type = medicalKit.type,
            price = medicalKit.price,
            size = medicalKit.size,
            spriteName = medicalKit.sprite != null ? GetSpriteResourcePath(medicalKit) : null,
            id = medicalKit.id,
            useTime = medicalKit.useTime,
            hp = medicalKit.hp
        };
    }
    
    public void FromMedicalKitData(MedicalKit medicalKit, MedicalKitData medicalKitData)
    {
        FromConsumableData(medicalKit, medicalKitData);
        medicalKit.hp = medicalKitData.hp;
    }
    
    public WeaponData ToWeaponData(Weapon weapon)
    {
        return new WeaponData
        {
            itemName = weapon.itemName,
            type = weapon.type,
            price = weapon.price,
            size = weapon.size,
            spriteName = weapon.sprite != null ? GetSpriteResourcePath(weapon) : null,
            id = weapon.id,
            bodyDmg = weapon.bodyDmg,
            ArmorDmg = weapon.ArmorDmg,
            range = weapon.range,
            RPM = weapon.RPM,
            capacity = weapon.capacity,
            reloadTime = weapon.reloadTime,
            control = weapon.control,
            mode = weapon.mode,
            ammoType = weapon.ammoType,
            currentAmmo = weapon.currentAmmo
        };
    }
    
    public void FromWeaponData(Weapon weapon, WeaponData weaponData)
    {
        FromItemData(weapon, weaponData);
        weapon.bodyDmg = weaponData.bodyDmg;
        weapon.ArmorDmg = weaponData.ArmorDmg;
        weapon.range = weaponData.range;
        weapon.RPM = weaponData.RPM;
        weapon.capacity = weaponData.capacity;
        weapon.reloadTime = weaponData.reloadTime;
        weapon.control = weaponData.control;
        weapon.mode = weaponData.mode;
        weapon.ammoType = weaponData.ammoType;
        weapon.currentAmmo = weaponData.currentAmmo;
    }
    
    public CollectionData ToCollectionData(Collection collection)
    {
        return new CollectionData
        {
            itemName = collection.itemName,
            type = collection.type,
            price = collection.price,
            size = collection.size,
            spriteName = collection.sprite != null ? GetSpriteResourcePath(collection) : null,
            id = collection.id
        };
    }
    
    public void FromCollectionData(Collection collection, CollectionData collectionData)
    {
        FromItemData(collection, collectionData);
        // Collection has no additional properties, so no need to set anything else
    }

    public ConsumableData ToConsumableData(Consumable consumable)
    {
        return new ConsumableData
        {
            itemName = consumable.itemName,
            type = consumable.type,
            price = consumable.price,
            size = consumable.size,
            spriteName = consumable.sprite != null ? GetSpriteResourcePath(consumable) : null,
            id = consumable.id,
            useTime = consumable.useTime
        };
    }
    
    public void FromConsumableData(Consumable consumable, ConsumableData consumableData)
    {
        FromItemData(consumable, consumableData);
        consumable.useTime = consumableData.useTime;
    }
    
    public DrinkData ToDrinkData(Drink drink)
    {
        return new DrinkData
        {
            itemName = drink.itemName,
            type = drink.type,
            price = drink.price,
            size = drink.size,
            spriteName = drink.sprite != null ? GetSpriteResourcePath(drink) : null,
            id = drink.id,
            useTime = drink.useTime,
            water = drink.water
        };
    }
    
    public void FromDrinkData(Drink drink, DrinkData drinkData)
    {
        FromConsumableData(drink, drinkData);
        drink.water = drinkData.water;
    }
    
    public FoodData ToFoodData(Food food)
    {
        return new FoodData
        {
            itemName = food.itemName,
            type = food.type,
            price = food.price,
            size = food.size,
            spriteName = food.sprite != null ? GetSpriteResourcePath(food) : null,
            id = food.id,
            useTime = food.useTime,
            repletion = food.repletion
        };
    }
    
    public void FromFoodData(Food food, FoodData foodData)
    {
        FromConsumableData(food, foodData);
        food.repletion = foodData.repletion;
    }
    
    public ChestRigData ToChestRigData(ChestRig chestRig)
    {
        return new ChestRigData
        {
            itemName = chestRig.itemName,
            type = chestRig.type,
            price = chestRig.price,
            size = chestRig.size,
            spriteName = chestRig.sprite != null ? GetSpriteResourcePath(chestRig) : null,
            id = chestRig.id,
            innerSize = chestRig.innerSize
        };
    }
    
    public void FromChestRigData(ChestRig chestRig, ChestRigData chestRigData)
    {
        FromItemData(chestRig, chestRigData);
        chestRig.innerSize = chestRigData.innerSize;
    }

    public void ClearSaveData()
    {
        if (File.Exists(_savePath))
        {
            File.Delete(_savePath);
        }
        else
        {
            Debug.Log("No save data found to clear at " + _savePath);
        }
    }

    public float LoadFloat()
    {
        if (!File.Exists(_savePath))
        {
            Debug.Log("Save file not found at " + _savePath);
            return 1f;
        }
        string json = File.ReadAllText(_savePath);
        float value = JsonConvert.DeserializeObject<float>(json);
        return value;
    }

    public void SaveFloat(float value)
    {
        string json = JsonConvert.SerializeObject(value, Formatting.Indented);
        File.WriteAllText(_savePath, json);
    }
}

[System.Serializable]
public class ItemData
{
    public string itemName;
    public string type;
    public int price;
    public int size;
    public string spriteName;
    public int id;
}

[System.Serializable]
public class AmmoData : ItemData
{
    public int maxStackSize;
    public int currentStackSize;
}

[System.Serializable]
public class BackpackData : ItemData
{
    public int innerSize;
}

[System.Serializable]
public class BodyArmorData : ItemData
{
    public int durability;
    public int maxDurability;
}

[System.Serializable]
public class ChestRigData : ItemData
{
    public int innerSize;
}

[System.Serializable]
public class CollectionData : ItemData
{
}

[System.Serializable]
public class ConsumableData : ItemData
{
    public int useTime;
}

[System.Serializable]
public class DrinkData : ConsumableData
{
    public int water;
}

[System.Serializable]
public class FoodData : ConsumableData
{
    public int repletion;
}

[System.Serializable]
public class HelmetData : ItemData
{
    public int durability;
    public int maxDurability;
}

[System.Serializable]
public class MedicalKitData : ConsumableData
{
    public int hp;
}

[System.Serializable]
public class WeaponData : ItemData
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
}


