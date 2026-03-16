using UnityEngine;
using System.Collections.Generic;
using UnityEngine.XR.ARFoundation;

public class AppFlowManager : MonoBehaviour
{
    [Header("UI Panels")]
    public GameObject homePanel;
    public GameObject arUiCanvas;

    [Header("AR References")]
    public ARSession arSession; // NEW: Added to reset tracking memory
    public ARPlaneManager planeManager; // NEW: Added to control planes
    public ARRaycastManager raycastManager; // NEW: Added to control raycasts
    public ARTrackedImageManager imageManager;
    public ARCameraBackground arBackground;

    [Header("Furniture Pivots")]
    public List<GameObject> allPivots = new List<GameObject>();

    [Header("AR UI Sub-Panels")]
    public GameObject scanningOverlay;
    public GameObject selectionOverlay;
    public GameObject assemblyButtons;
    public GameObject placementUI;

    [Header("Active Interaction Bridge")]
    public AssemblyManager currentActiveAssembly;

    public string currentFurniture;

    void Start()
    {
        ReturnToHome();
    }

    public void HideAllPivots()
    {
        foreach (GameObject pivot in allPivots)
        {
            if (pivot != null) pivot.SetActive(false);
        }
    }

    public void ResetSubPanels()
    {
        if (scanningOverlay != null) scanningOverlay.SetActive(true);
        if (selectionOverlay != null) selectionOverlay.SetActive(false);
        if (assemblyButtons != null) assemblyButtons.SetActive(false);
        if (placementUI != null) placementUI.SetActive(false);
    }

    public void EnterScanMode()
    {
        if (homePanel != null) homePanel.SetActive(false);
        if (arUiCanvas != null) arUiCanvas.SetActive(true);

        ResetSubPanels();

        // 1. Turn ON image tracking
        if (imageManager != null) imageManager.enabled = true;

        // 2. Turn OFF spatial tracking to prevent bad reflections
        if (planeManager != null) planeManager.enabled = false;
        if (raycastManager != null) raycastManager.enabled = false;

        if (arBackground != null)
        {
            arBackground.enabled = true;
            Camera.main.clearFlags = CameraClearFlags.SolidColor;
            Camera.main.backgroundColor = new Color(0.95f, 0.95f, 0.95f); // Clean light mode background
        }

        MarkerDetectionHandler handler = GetComponent<MarkerDetectionHandler>();
        if (handler != null) handler.ResetDetection();
    }

    public void ShowSelectionOverlay()
    {
        if (scanningOverlay != null) scanningOverlay.SetActive(false);
        if (selectionOverlay != null) selectionOverlay.SetActive(true);
        if (assemblyButtons != null) assemblyButtons.SetActive(false);
        if (placementUI != null) placementUI.SetActive(false);
    }

    public void StartPlacementModeUI()
    {
        if (selectionOverlay != null) selectionOverlay.SetActive(false);
        if (placementUI != null) placementUI.SetActive(true);

        // 1. Turn OFF image tracking
        if (imageManager != null) imageManager.enabled = false;

        // 2. Wipe the corrupted spatial map from the scan phase
        if (arSession != null) arSession.Reset();

        // 3. Turn ON spatial tracking for floor detection
        if (planeManager != null) planeManager.enabled = true;
        if (raycastManager != null) raycastManager.enabled = true;
    }

    public void StartAssemblyMode()
    {
        if (selectionOverlay != null) selectionOverlay.SetActive(false);
        if (placementUI != null) placementUI.SetActive(false);
        if (assemblyButtons != null) assemblyButtons.SetActive(true);

        RoomPlacementHandler placement = GetComponent<RoomPlacementHandler>();
        if (placement != null) placement.StopPlacementMode();

        if (imageManager != null) imageManager.enabled = false;
        if (arBackground != null) arBackground.enabled = false;

        Camera.main.clearFlags = CameraClearFlags.SolidColor;
        Camera.main.backgroundColor = new Color(0.15f, 0.15f, 0.15f);//e background

        if (currentActiveAssembly != null)
        {
            currentActiveAssembly.gameObject.SetActive(true);

            currentActiveAssembly.transform.position =
                Camera.main.transform.position +
                Camera.main.transform.forward * 1.5f +
                new Vector3(0, -0.35f, 0);

            currentActiveAssembly.transform.rotation =
                Quaternion.LookRotation(Camera.main.transform.forward);

            currentActiveAssembly.UpdateStepVisibility();

            Debug.Log("Assembly mode started for: " + currentActiveAssembly.name);
        }
    }

    public void SetCurrentFurniture(string name)
    {
        currentFurniture = name;
    }

    public void ReturnToHome()
    {
        if (arUiCanvas != null) arUiCanvas.SetActive(false);

        if (imageManager != null) imageManager.enabled = false;
        if (planeManager != null) planeManager.enabled = false;
        if (raycastManager != null) raycastManager.enabled = false;

        if (arBackground != null)
        {
            arBackground.enabled = true;
            Camera.main.clearFlags = CameraClearFlags.SolidColor;
            Camera.main.backgroundColor = new Color(0.95f, 0.95f, 0.95f);
        }

        HideAllPivots();
        ResetSubPanels();

        RoomPlacementHandler placement = GetComponent<RoomPlacementHandler>();
        if (placement != null) placement.StopPlacementMode();

        currentActiveAssembly = null;
        currentFurniture = "";

        if (homePanel != null) homePanel.SetActive(true);
    }

    public void GlobalNextStep()
    {
        if (currentActiveAssembly != null) currentActiveAssembly.NextStep();
    }

    public void GlobalPreviousStep()
    {
        if (currentActiveAssembly != null) currentActiveAssembly.PreviousStep();
    }

    public void GlobalResetModel()
    {
        if (currentActiveAssembly != null) currentActiveAssembly.GetComponent<ModelInteraction>()?.ResetModel();
    }

    public void GlobalToggleXray()
    {
        if (currentActiveAssembly != null) currentActiveAssembly.GetComponent<ModelInteraction>()?.ToggleXray();
    }

    // --- RADAR CAMERA TOGGLES ---
    public void EnableCameraForRadar()
    {
        if (arBackground != null) arBackground.enabled = true;
    }

    public void DisableCameraForRadar()
    {
        if (arBackground != null) arBackground.enabled = false;
    }
}