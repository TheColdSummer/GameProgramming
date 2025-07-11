using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryUIManager : MonoBehaviour
{
    public GameObject containerUI;
    public GameObject playerInventoryUI;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (playerInventoryUI.activeSelf && !containerUI.activeSelf)
            {
                playerInventoryUI.SetActive(false);
                containerUI.SetActive(false);
            }
            else
            {
                playerInventoryUI.GetComponent<PlayerInventory>().RefreshRemainingSpace();
                playerInventoryUI.SetActive(true);
            }
        }
    }
}
