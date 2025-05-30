using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    public Container container;
    private bool isPlayerNearby = false;
    private static GameObject inventoryUI;
    private static GameObject containerContent;
    private static GameObject playerInventory;

    private void Start()
    {
        if (inventoryUI == null)
        {
            inventoryUI = GameObject.Find("InventoryWithContainer");
            if (inventoryUI == null)
            {
                Debug.LogError("InventoryWithContainer not found in the scene.");
            }
            
            containerContent = GameObject.Find("InventoryWithContainer/Container/Inside/Content");
            if (containerContent == null)
            {
                Debug.LogError("Content not found in the specified path.");
            }
            
            playerInventory = GameObject.Find("InventoryWithContainer/PlayerInventory");
            if (playerInventory == null)
            {
                Debug.LogError("PlayerInventory not found in the specified path.");
            }
            inventoryUI.SetActive(false);
        }
    }

    void Update()
    {
        if (isPlayerNearby && !inventoryUI.activeSelf && Input.GetKeyDown(KeyCode.F))
        {
            OpenUI();
        }

        if (inventoryUI.activeSelf && Input.GetKeyDown(KeyCode.Escape))
        {
            CloseUI();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player is nearby");
            isPlayerNearby = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player left the area");
            isPlayerNearby = false;
        }
    }

    private void OpenUI()
    {
        DisplayItems();
        playerInventory.GetComponent<PlayerInventory>().RefreshRemainingSpace();
        inventoryUI.SetActive(true);
    }

    private void CloseUI()
    {
        containerContent.GetComponent<ContainerContentUI>().ReturnItemsToContainer();
        inventoryUI.SetActive(false);
    }

    private void DisplayItems()
    {
        foreach (Transform child in containerContent.transform)
        {
            Destroy(child.gameObject);
        }

        container.Open(containerContent);
    }

}


