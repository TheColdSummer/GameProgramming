using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

public class RightClickHandler : MonoBehaviour, IPointerClickHandler
{
    private static GameObject rightClickMenuPanel;

    // Start is called before the first frame update
    void Start()
    {
        if (rightClickMenuPanel == null)
        {
            rightClickMenuPanel = GameObject.Find("RightClickMenu");
            if (rightClickMenuPanel == null)
            {
                Debug.LogError("Right-click menu not found in the scene.");
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            Debug.Log("Right-clicked on: " + gameObject.name);
            rightClickMenuPanel.SetActive(true);
            RightClickMenu menu = rightClickMenuPanel.GetComponent<RightClickMenu>();
            if (menu != null)
            {
                menu.Configure(gameObject);
            }
            else
            {
                Debug.LogError("RightClickMenu component not found on RightClickMenu GameObject.");
            }
            if (rightClickMenuPanel.transform.childCount == 0)
            {
                rightClickMenuPanel.SetActive(false);
                return;
            }
            rightClickMenuPanel.transform.position = eventData.position;
        }
    }
}