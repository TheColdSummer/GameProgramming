using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContainerContentUI : MonoBehaviour
{
    private Transform _connectedContainer;

    public void ConnectTo(Transform container)
    {
        if (container == null)
        {
            Debug.LogError("Container is null. Cannot connect to it.");
            return;
        }
        
        _connectedContainer = container;
    }

    public void ReturnItemsToContainer()
    {
        if (transform.childCount == 0)
        {
            return;
        }
        
        if (_connectedContainer == null)
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
            item.transform.SetParent(_connectedContainer.transform, false);
            item.SetActive(true);
        }

        _connectedContainer = null;
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
