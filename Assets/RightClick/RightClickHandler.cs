using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

public class RightClickHandler : MonoBehaviour, IPointerClickHandler
{
    public static GameObject RightClickMenuPanel;

    // Start is called before the first frame update
    void Start()
    {
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
            RightClickMenuPanel.SetActive(true);
            RightClickMenu menu = RightClickMenuPanel.GetComponent<RightClickMenu>();
            if (menu != null)
            {
                menu.Configure(gameObject);
            }
            else
            {
                Debug.LogError("RightClickMenu component not found on RightClickMenu GameObject.");
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