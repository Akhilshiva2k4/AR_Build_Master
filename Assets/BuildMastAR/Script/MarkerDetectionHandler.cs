using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class MarkerDetectionHandler : MonoBehaviour
{
    [Header("References")]
    public ARTrackedImageManager trackedImageManager;
    public AppFlowManager appFlowManager;

    private bool markerDetected = false;

    void OnEnable()
    {
        if (trackedImageManager != null)
            trackedImageManager.trackedImagesChanged += OnTrackedImagesChanged;
    }

    void OnDisable()
    {
        if (trackedImageManager != null)
            trackedImageManager.trackedImagesChanged -= OnTrackedImagesChanged;
    }

    void OnTrackedImagesChanged(ARTrackedImagesChangedEventArgs args)
    {
        if (markerDetected) return;

        // Check for completely new markers
        foreach (ARTrackedImage trackedImage in args.added)
        {
            ProcessMarker(trackedImage);
        }

        // Check for markers that were lost and found again
        foreach (ARTrackedImage trackedImage in args.updated)
        {
            if (trackedImage.trackingState == TrackingState.Tracking)
            {
                ProcessMarker(trackedImage);
            }
        }
    }

    void ProcessMarker(ARTrackedImage trackedImage)
    {
        if (markerDetected) return;
        markerDetected = true;

        string furnitureName = trackedImage.referenceImage.name;
        Debug.Log("Marker detected: " + furnitureName);

        appFlowManager.SetCurrentFurniture(furnitureName);
        appFlowManager.HideAllPivots();

        GameObject pivot = null;
        foreach (GameObject p in appFlowManager.allPivots)
        {
            if (p != null && p.name == furnitureName + "_Pivot")
            {
                pivot = p;
                break;
            }
        }

        if (pivot != null)
        {
            pivot.SetActive(true);

            AssemblyManager asm = pivot.GetComponent<AssemblyManager>();

            if (asm != null)
            {
                appFlowManager.currentActiveAssembly = asm;
            }
        }

        appFlowManager.ShowSelectionOverlay();
    }

    public void ResetDetection()
    {
        markerDetected = false;
    }
}