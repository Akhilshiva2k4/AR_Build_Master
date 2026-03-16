using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

[System.Serializable]
public class HardwareUIRow
{
    public GameObject rowObject;
    public Image iconImage;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI sizeText;
    public TextMeshProUGUI quantityText;
}

public class HardwarePopupManager : MonoBehaviour
{
    [Header("App Core")]
    public AppFlowManager appFlowManager;

    [Header("UI Panels")]
    public GameObject popupBlockerAndWindow;

    [Header("Row References")]
    public HardwareUIRow[] hardwareRows = new HardwareUIRow[3];

    public void OpenHardwarePopup()
    {
        if (appFlowManager == null || appFlowManager.currentActiveAssembly == null)
        {
            Debug.LogWarning("No active assembly found!");
            return;
        }

        string furnitureName = appFlowManager.currentFurniture;
        int stepIndex = appFlowManager.currentActiveAssembly.GetCurrentStepNumber() - 1;

        List<HardwareItem> requiredHardware = HardwareDatabase.GetHardwareForStep(furnitureName, stepIndex);

        for (int i = 0; i < hardwareRows.Length; i++)
        {
            if (i < requiredHardware.Count)
            {
                hardwareRows[i].rowObject.SetActive(true);

                hardwareRows[i].nameText.text = requiredHardware[i].partName;
                hardwareRows[i].sizeText.text = "Size: " + requiredHardware[i].partSize;
                hardwareRows[i].quantityText.text = "Quantity: " + requiredHardware[i].quantity;

                // This looks for Assets/BuildMastAR/Resources/HardwareIcons/[imageName]
                Sprite loadedIcon = Resources.Load<Sprite>("HardwareIcons/" + requiredHardware[i].imageName);

                if (loadedIcon != null)
                {
                    hardwareRows[i].iconImage.sprite = loadedIcon;
                }
                else
                {
                    Debug.LogWarning("Icon not found in Resources/HardwareIcons: " + requiredHardware[i].imageName);
                }
            }
            else
            {
                hardwareRows[i].rowObject.SetActive(false);
            }
        }

        popupBlockerAndWindow.SetActive(true);
    }

    public void CloseHardwarePopup()
    {
        popupBlockerAndWindow.SetActive(false);
    }
}