using System.Collections.Generic;
using UnityEngine;

public static class ItemFactory
{
    private static Dictionary<string, float> _itemCategoryWeights = new Dictionary<string, float>
    {
        { "Weapon/", 0.2f },
        { "Helmet/", 0.2f },
        { "Armor/", 0.2f },
        { "backpack/", 0.3f },
        { "Collection/", 3f },
        { "Consumable/", 0.6f },
        { "ChestRig/", 0.3f },
        { "Ammo/", 0.5f },
    };

    public static List<GameObject> CreateItemsFromPrefabs()
    {
        int count = Random.Range(1, 5);
        List<GameObject> items = new List<GameObject>();
        for (int i = 0; i < count; i++)
        {
            string category = GetRandomCategoryByWeight();
            string[] prefabs = GetPrefabsInCategory(category);
            if (prefabs.Length == 0)
            {
                Debug.LogWarning($"No prefabs found in category: {category}");
                continue;
            }

            int selectedIndex = 0;
            if (category == "Weapon/" || category == "Ammo/")
            {
                selectedIndex = Random.Range(0, prefabs.Length);
            }
            else
            {
                System.Array.Sort(prefabs, (a, b) =>
                {
                    int numA = ExtractNumberAtEnd(a);
                    int numB = ExtractNumberAtEnd(b);
                    return numA.CompareTo(numB);
                });

                if (prefabs.Length == 0) continue;

                float totalWeight = 0f;
                float[] weights = new float[prefabs.Length];
                for (int j = 0; j < prefabs.Length; j++)
                {
                    weights[j] = prefabs.Length - j;
                    totalWeight += weights[j];
                }

                float rand = Random.Range(0, totalWeight);
                float sum = 0f;
                for (int j = 0; j < weights.Length; j++)
                {
                    sum += weights[j];
                    if (rand <= sum)
                    {
                        selectedIndex = j;
                        break;
                    }
                }
            }

            string prefabPath = category + prefabs[selectedIndex];
            GameObject prefab = Resources.Load<GameObject>(prefabPath);
            if (prefab != null)
            {
                items.Add(GameObject.Instantiate(prefab));
            }
        }

        return items;
    }

    private static int ExtractNumberAtEnd(string str)
    {
        int num = 0;
        for (int i = str.Length - 1; i >= 0; i--)
        {
            if (!char.IsDigit(str[i]))
            {
                if (i < str.Length - 1)
                    int.TryParse(str.Substring(i + 1), out num);
                break;
            }

            if (i == 0)
                int.TryParse(str, out num);
        }

        return num;
    }

    private static string GetRandomCategoryByWeight()
    {
        float totalWeight = 0f;
        foreach (var kv in _itemCategoryWeights)
        {
            totalWeight += kv.Value;
        }

        float rand = Random.Range(0, totalWeight);
        float sum = 0f;
        foreach (var kv in _itemCategoryWeights)
        {
            sum += kv.Value;
            if (rand <= sum)
                return kv.Key;
        }

        return "Collection/";
    }

    private static string[] GetPrefabsInCategory(string category)
    {
        GameObject[] prefabs = Resources.LoadAll<GameObject>(category);
        List<string> prefabNames = new List<string>();
        foreach (var prefab in prefabs)
        {
            prefabNames.Add(prefab.name);
        }

        return prefabNames.ToArray();
    }

    public static void GenerateItemsForContainer(Container container)
    {
        if (container == null)
        {
            Debug.LogError("Container is null. Cannot generate items.");
            return;
        }

        List<GameObject> itemObjects = CreateItemsFromPrefabs();

        foreach (GameObject itemObject in itemObjects)
        {
            itemObject.transform.SetParent(container.Content.transform, false);
            itemObject.SetActive(true);
        }
    }
}