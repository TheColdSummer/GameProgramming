using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class RightClickHandler : MonoBehaviour, IPointerClickHandler
{
    public static GameObject RightClickMenuPanel;
    
    void Update()
    {
        if (RightClickMenuPanel != null && RightClickMenuPanel.activeSelf)
        {
            if (Input.anyKeyDown)
            {
                if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1))
                {
                    return;
                }
                RightClickMenuPanel.SetActive(false);
            }
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            string sceneName = SceneManager.GetActiveScene().name;
            if (sceneName == "Warehouse")
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
            else
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

            if (RightClickMenuPanel.transform.childCount == 0)
            {
                RightClickMenuPanel.SetActive(false);
                return;
            }

            RightClickMenuPanel.transform.position = eventData.position;
            if (RightClickMenuPanel.transform.childCount == 0)
            {
                RightClickMenuPanel.SetActive(false);
                return;
            }

            RectTransform menuRect = RightClickMenuPanel.GetComponent<RectTransform>();
            Vector2 clampedPos = UIPositionHelper.GetClampedPosition(menuRect, eventData.position);
            menuRect.position = clampedPos;
        }
    }
}

public static class UIPositionHelper
{
    public static Vector2 GetClampedPosition(RectTransform menuRect, Vector2 desiredPosition)
    {
        Vector2 size = menuRect.sizeDelta * menuRect.lossyScale;
        float x = desiredPosition.x;
        float y = desiredPosition.y;

        if (x + size.x > Screen.width)
            x = Screen.width - size.x;
        if (y - size.y < 0)
            y = size.y;

        x = Mathf.Clamp(x, 0, Screen.width - size.x);
        y = Mathf.Clamp(y, size.y, Screen.height);

        return new Vector2(x, y);
    }
}