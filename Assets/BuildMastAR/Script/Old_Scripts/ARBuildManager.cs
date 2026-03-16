using UnityEngine;
using UnityEngine.XR.ARFoundation;
using System.Collections.Generic;

public class ARBuildManager : MonoBehaviour
{
    [System.Serializable]
    public struct FurnitureSet
    {
        public string markerName; // Must match the name in Reference Image Library
        public GameObject parentGroup; // The parent (e.g., "Table_Pivot")
        public GameObject[] steps; // Array of steps for this specific item
    }

    public ARTrackedImageManager imageManager;
    public List<FurnitureSet> furnitureLibrary;

    private FurnitureSet currentSet;
    private int currentStepIndex = 0;
    private bool hasActiveSet = false;

    void OnEnable() => imageManager.trackedImagesChanged += OnChanged;
    void OnDisable() => imageManager.trackedImagesChanged -= OnChanged;

    void OnChanged(ARTrackedImagesChangedEventArgs eventArgs)
    {
        foreach (var trackedImage in eventArgs.added)
        {
            SwitchCategory(trackedImage.referenceImage.name);
        }
        foreach (var trackedImage in eventArgs.updated)
        {
            // Optional: Re-trigger if the marker was lost and found again
            if (trackedImage.trackingState == UnityEngine.XR.ARSubsystems.TrackingState.Tracking)
                SwitchCategory(trackedImage.referenceImage.name);
        }
    }

    public void SwitchCategory(string name)
    {
        foreach (var set in furnitureLibrary)
        {
            if (set.markerName == name)
            {
                // Disable all others first
                DisableAllFurniture();

                currentSet = set;
                currentSet.parentGroup.SetActive(true);
                currentStepIndex = 0;
                UpdateStepVisibility();
                hasActiveSet = true;
                break;
            }
        }
    }

    private void DisableAllFurniture()
    {
        foreach (var set in furnitureLibrary)
        {
            set.parentGroup.SetActive(false);
        }
    }

    public void NextStep()
    {
        if (!hasActiveSet) return;
        if (currentStepIndex < currentSet.steps.Length - 1)
        {
            currentStepIndex++;
            UpdateStepVisibility();
        }
    }

    public void PrevStep()
    {
        if (!hasActiveSet) return;
        if (currentStepIndex > 0)
        {
            currentStepIndex--;
            UpdateStepVisibility();
        }
    }

    private void UpdateStepVisibility()
    {
        for (int i = 0; i < currentSet.steps.Length; i++)
        {
            currentSet.steps[i].SetActive(i == currentStepIndex);
        }
    }
}