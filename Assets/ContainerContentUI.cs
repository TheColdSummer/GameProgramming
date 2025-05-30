using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContainerContentUI : MonoBehaviour
{
    private Transform connectedContainer;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ConnectTo(Transform container)
    {
        if (container == null)
        {
            Debug.LogError("Container is null. Cannot connect to it.");
            return;
        }
        
        connectedContainer = container;
    }

    public void ReturnItemsToContainer()
    {
        if (transform.childCount == 0)
        {
            return;
        }
        
        if (connectedContainer == null)
        {
            Debug.LogError("No connected container to return items to, destroy them.");
            foreach (Transform child in transform)
            {
                GameObject item = child.gameObject;
                Destroy(item);
            }
            return;
        }
        List<Transform> children = new List<Transform>();
        foreach (Transform child in transform)
        {
            children.Add(child);
        }
        foreach (Transform child in children)
        {
            GameObject item = child.gameObject;
            item.transform.SetParent(connectedContainer.transform, false);
            item.SetActive(true);
        }

        Debug.Log("Items returned to container: " + connectedContainer.name);
        connectedContainer = null;
    }

    public void ReceiveItemFromPlayer(GameObject item)
    {
        if (item == null)
        {
            Debug.LogError("Item is null. Cannot get item from drop.");
            return;
        }

        item.transform.SetParent(transform, false);
        item.SetActive(true);
    }
}
