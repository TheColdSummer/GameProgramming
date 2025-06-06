using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    public Container container;
    private bool _isPlayerNearby = false;
    private static GameObject _containerUI;
    private static GameObject _containerContent;
    private static GameObject _playerInventoryUI;

    private void Start()
    {
        if (_containerUI == null)
        {
            _containerUI = GameObject.Find("InventoryWithContainer/Container");
            if (_containerUI == null)
            {
                Debug.LogError("InventoryWithContainer not found in the scene.");
            }
            
            _containerContent = GameObject.Find("InventoryWithContainer/Container/Inside/Content");
            if (_containerContent == null)
            {
                Debug.LogError("Content not found in the specified path.");
            }
            
            _playerInventoryUI = GameObject.Find("InventoryWithContainer/PlayerInventory");
            if (_playerInventoryUI == null)
            {
                Debug.LogError("PlayerInventory not found in the specified path.");
            }
            _containerUI.SetActive(false);
            _playerInventoryUI.SetActive(false);
        }
    }

    void Update()
    {
        if (_isPlayerNearby && !_containerUI.activeSelf && Input.GetKeyDown(KeyCode.F))
        {
            OpenUI();
        }

        if (_containerUI.activeSelf && Input.GetKeyDown(KeyCode.Escape))
        {
            CloseUI();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            _isPlayerNearby = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            _isPlayerNearby = false;
        }
    }

    private void OpenUI()
    {
        DisplayItems();
        _playerInventoryUI.GetComponent<PlayerInventory>().RefreshRemainingSpace();
        _containerUI.SetActive(true);
        _playerInventoryUI.SetActive(true);
    }

    private void CloseUI()
    {
        _containerContent.GetComponent<ContainerContentUI>().ReturnItemsToContainer();
        _containerUI.SetActive(false);
        _playerInventoryUI.SetActive(false);
    }

    private void DisplayItems()
    {
        foreach (Transform child in _containerContent.transform)
        {
            Destroy(child.gameObject);
        }

        container.Open(_containerContent);
    }

}


