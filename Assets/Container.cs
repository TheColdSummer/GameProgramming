using System.Collections.Generic;
using UnityEngine;

public class Container : MonoBehaviour
{
    public GameObject Content;
    void Start()
    {
        InitItems();
        
    }

    private void InitItems()
    {
        ItemFactory.GenerateItemsForContainer(this);
    }

    public void Open(GameObject containerContent)
    {
        if (containerContent == null)
        {
            Debug.LogError("Container content UI is null. Cannot open the container.");
            return;
        }

        Transform contentTransform = gameObject.transform.Find("Content");
        if (contentTransform == null)
        {
            Debug.LogError("Content GameObject not found in the container content UI.");
            return;
        }
        containerContent.GetComponent<ContainerContentUI>().ConnectTo(contentTransform);
        List<Transform> children = new List<Transform>();
        foreach (Transform child in contentTransform)
        {
            children.Add(child);
        }
        foreach (Transform child in children)
        {
            GameObject item = child.gameObject;
            item.transform.SetParent(containerContent.transform, false);
            item.SetActive(true);
        }
    }

    public void AddItems(List<Item> items)
    {
        if (items == null)
        {
            Debug.LogError("Item is null. Cannot add to container.");
            return;
        }
        foreach (Item item in items)
        {
            if (item == null)
            {
                Debug.LogError("Item is null. Cannot add to container.");
                continue;
            }

            SaveLoadManager manager = gameObject.AddComponent<SaveLoadManager>();
            GameObject itemObject = manager.ConstructGameObjectFromItemData(manager.ToItemData(item));
            itemObject.transform.SetParent(Content.transform, false);
            itemObject.SetActive(true);
            Destroy(manager);
        }
        
    }
}
