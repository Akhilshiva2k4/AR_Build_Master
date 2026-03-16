using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using System.Collections.Generic;

public class UniversalMarkerHandler : MonoBehaviour
{
    [System.Serializable]
    public struct FurnitureData
    {
        public string markerName;      // INTERNAL name from the Library
        public GameObject pivotParent; // Child of MainCamera
        public GameObject[] steps;     // Step1, Step2, etc.
    }

    public ARTrackedImageManager imageManager;
    public List<FurnitureData> furnitureLibrary;
    public GameObject uiCanvas;

    private FurnitureData activeFurniture;
    private bool isFurnitureActive = false;
    private int currentStepIndex = 0;

    void Start()
    {
        if (uiCanvas != null) uiCanvas.SetActive(false);
        foreach (var item in furnitureLibrary) item.pivotParent.SetActive(false);
    }

    void OnEnable() => imageManager.trackedImagesChanged += OnImageChanged;
    void OnDisable() => imageManager.trackedImagesChanged -= OnImageChanged;

    void OnImageChanged(ARTrackedImagesChangedEventArgs eventArgs)
    {
        foreach (var img in eventArgs.added) HandleDetection(img);
        foreach (var img in eventArgs.updated) HandleDetection(img);
    }

    void HandleDetection(ARTrackedImage img)
    {
        if (img.trackingState != TrackingState.None)
        {
            // This will show you the ACTUAL name in the Console
            Debug.Log("AR Detected Internal Name: " + img.referenceImage.name);
            SetFurnitureActive(img.referenceImage.name);
        }
    }

    public void SetFurnitureActive(string name)
    {
        foreach (var item in furnitureLibrary)
        {
            if (item.markerName == name)
            {
                if (isFurnitureActive && activeFurniture.markerName == name) return;

                foreach (var f in furnitureLibrary) f.pivotParent.SetActive(false);

                activeFurniture = item;
                activeFurniture.pivotParent.SetActive(true);
                if (uiCanvas != null) uiCanvas.SetActive(true);

                currentStepIndex = 0;
                UpdateStepVisibility();
                isFurnitureActive = true;
                break;
            }
        }
    }

    public void NextStep() { if (isFurnitureActive && currentStepIndex < activeFurniture.steps.Length - 1) { currentStepIndex++; UpdateStepVisibility(); } }
    public void PrevStep() { if (isFurnitureActive && currentStepIndex > 0) { currentStepIndex--; UpdateStepVisibility(); } }

    private void UpdateStepVisibility()
    {
        for (int i = 0; i < activeFurniture.steps.Length; i++)
        {
            if (activeFurniture.steps[i] != null)
                activeFurniture.steps[i].SetActive(i <= currentStepIndex); // Keeps previous steps visible
        }
    }
}