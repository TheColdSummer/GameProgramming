using UnityEngine;
using UnityEngine.SceneManagement;

public class ShopLoadSave : MonoBehaviour
{
    public SaveManager saveManager;
    public Warehouse warehouse;
    public GameObject shopContent;
    
    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        saveManager.LoadWarehouse();
        Commodity.SetWarehouse(warehouse);
        Commodity.SetSaveManager(saveManager);
        
        LoadCommodities();
    }

    private void LoadCommodities()
    {
        if (shopContent == null)
        {
            Debug.LogError("Shop content GameObject is not assigned.");
            return;
        }

        GameObject commodityPrefab = Resources.Load<GameObject>("Commodity");
        if (commodityPrefab == null)
        {
            Debug.LogError("CommodityPrefab not found in Resources.");
            return;
        }

        string[] folders = { "Ammo", "Armor", "Backpack", "ChestRig", "Consumable", "Helmet", "Weapon" };

        foreach (string folder in folders)
        {
            Object[] prefabs = Resources.LoadAll(folder, typeof(GameObject));
            foreach (Object prefabObj in prefabs)
            {
                GameObject prefab = prefabObj as GameObject;
                if (prefab == null) continue;

                Item item = prefab.GetComponent<Item>();
                if (item == null)
                {
                    Debug.LogError($"Prefab {prefab.name} does not have an Item component.");
                    continue;
                }

                GameObject commodityObj = Instantiate(commodityPrefab, shopContent.transform, false);
                Commodity commodity = commodityObj.GetComponent<Commodity>();
                if (commodity == null)
                {
                    Debug.LogError("Commodity component not found on the instantiated GameObject.");
                    continue;
                }
                commodity.Init(item);
            }
        }
    }
}