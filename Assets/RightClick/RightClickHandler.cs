using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

public class RightClickHandler : MonoBehaviour, IPointerClickHandler
{
    public static GameObject RightClickMenuPanel;

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            Debug.Log("Right-clicked on: " + gameObject.name);
            GameObject player = GameObject.Find("Player");
            if (player != null)
            {
                RightClickMenu menu = RightClickMenuPanel.GetComponent<RightClickMenu>();
                if (menu != null)
                {
                    RightClickMenuPanel.SetActive(menu.Configure(gameObject));
                }
                else
                {
                    Debug.LogError("RightClickMenu component not found on RightClickMenu GameObject.");
                }
            }
            else
            {
                RightClickMenuInWarehouse menu = RightClickMenuPanel.GetComponent<RightClickMenuInWarehouse>();
                if (menu != null)
                {
                    RightClickMenuPanel.SetActive(menu.Configure(gameObject));
                }
                else
                {
                    Debug.LogError("RightClickMenu component not found on RightClickMenu GameObject.");
                }
            }

            if (RightClickMenuPanel.transform.childCount == 0)
            {
                RightClickMenuPanel.SetActive(false);
                return;
            }

            RightClickMenuPanel.transform.position = eventData.position;
        }
    }
}