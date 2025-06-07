using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class RightClickMenu : MonoBehaviour
{
    public GameObject playerInventory;
    public GameObject containerUI;
    public GameObject choicePrefab;
    public GameObject descriptionPanel;
    public GameObject descriptionText;
    private RectTransform menuRectTransform;
    private PlayerInventory playerInventoryScript;

    void Start()
    {
        menuRectTransform = GetComponent<RectTransform>();
        if (menuRectTransform == null)
        {
            Debug.LogError("RightClickMenu requires a RectTransform component.");
            return;
        }

        playerInventoryScript = playerInventory.GetComponent<PlayerInventory>();
        if (playerInventoryScript == null)
        {
            Debug.LogError("PlayerInventory script not found on the playerInventory GameObject.");
        }
        RightClickHandler.RightClickMenuPanel = gameObject;
        gameObject.SetActive(false);
        descriptionPanel.transform.Find("Button").GetComponent<Button>().onClick.AddListener(() =>
        {
            descriptionPanel.SetActive(false);
            descriptionText.GetComponent<TMPro.TextMeshProUGUI>().text = string.Empty;
        });
        descriptionPanel.SetActive(false);

    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (!IsPointerOverUIObject())
            {
                gameObject.SetActive(false);
            }
        }
    }

    public bool Configure(GameObject clickedGameObject)
    {
        if (clickedGameObject == null)
        {
            Debug.LogError("GameObject is null. Cannot configure the right-click menu.");
            return false;
        }

        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
        
        if (clickedGameObject.CompareTag("Item"))
        {
            Item item = clickedGameObject.GetComponent<Item>();
            if (item == null)
            {
                Debug.LogError("Item script not found on the GameObject.");
                return false;
            }
            AddMenuItem("Description", () => ShowDescription(item));
            ConfigureItem(clickedGameObject);
        }
        else if (clickedGameObject.CompareTag("EquippedItem"))
        {
            Item item = clickedGameObject.GetComponent<EquipmentInInventory>().equipment;
            if (item == null)
            {
                return false;
            }
            AddMenuItem("Description", () => ShowDescription(item));
            ConfigureEquippedItem(clickedGameObject);
        }
        else
        {
            Debug.LogWarning("Right-click menu configured for an unsupported GameObject type.");
        }
        return true;
    }

    private void ShowDescription(Item item)
    {
        string description = item.GetSpecificDescription();
        if (descriptionPanel == null || descriptionText == null)
        {
            Debug.LogError("Description panel or text not set in the RightClickMenu.");
            return;
        }
        descriptionPanel.SetActive(true);
        descriptionText.GetComponent<TMPro.TextMeshProUGUI>().text = description;
    }

    private void ConfigureItem(GameObject clickedGameObject)
    {
        Item item = clickedGameObject.GetComponent<Item>();
        if (item == null)
        {
            Debug.LogError("Item script not found on the GameObject.");
            return;
        }

        if (playerInventoryScript.CheckIfItemInInventory(clickedGameObject))
        {
            if (containerUI.activeSelf)
            {
                AddMenuItem("Drop", () => playerInventoryScript.DropItem(clickedGameObject));
            }
            switch (item.type)
            {
                case "Helmet":
                case "Armor":
                case "Weapon":
                case "Backpack":
                case "Chest Rig":
                    AddMenuItem("Equip", () => playerInventoryScript.ChangeEquipment(item));
                    break;
                case "Food":
                case "Drink":
                case "MedicalKit":
                    AddMenuItem("Use", () => playerInventoryScript.UseConsumable(item as Consumable));
                    break;
            }
        }
        else
        {
            AddMenuItem("Pick", () => playerInventoryScript.PickItem(clickedGameObject));
            switch (item.type)
            {
                case "Helmet":
                case "Armor":
                case "Weapon":
                case "Backpack":
                case "Chest Rig":
                    AddMenuItem("Equip", () => playerInventoryScript.ChangeEquipmentFromContainer(item));
                    break;
                case "Food":
                case "Drink":
                case "MedicalKit":
                    AddMenuItem("Use", () => playerInventoryScript.UseConsumable(item as Consumable));
                    break;
            }
        }
    }

    private void ConfigureEquippedItem(GameObject clickedGameObject)
    {
        EquipmentInInventory equipmentInInventory = clickedGameObject.GetComponent<EquipmentInInventory>();
        if (equipmentInInventory == null)
        {
            Debug.LogError("EquipmentInInventory script not found on the GameObject.");
            return;
        }
        if (equipmentInInventory.equipment == null)
        {
            return;
        }

        if (containerUI.activeSelf)
        {
            AddMenuItem("Drop", () => playerInventoryScript.DropEquippedItem(clickedGameObject));
        }
        AddMenuItem("Unequip", () => playerInventoryScript.UnequipItem(clickedGameObject));
    }

    private void AddMenuItem(string text, Action onClickAction)
    {
        GameObject rightClickChoice = Instantiate(choicePrefab, gameObject.transform);

        Transform textTransform = rightClickChoice.transform.Find("Text");
        if (textTransform != null)
        {
            textTransform.GetComponent<TMPro.TextMeshProUGUI>().text = text;
        }
        else
        {
            Debug.LogError("Text object not found in the RightClickChoice prefab.");
        }

        Button button = rightClickChoice.GetComponent<Button>();
        if (button != null)
        {
            button.onClick.AddListener(() =>
            {
                onClickAction?.Invoke();
                gameObject.SetActive(false);
            });
        }
        else
        {
            Debug.LogError("Button component not found on the RightClickChoice prefab.");
        }
    }

    private bool IsPointerOverUIObject()
    {
        Vector2 localMousePosition = menuRectTransform.InverseTransformPoint(Input.mousePosition);
        return menuRectTransform.rect.Contains(localMousePosition);
    }
}