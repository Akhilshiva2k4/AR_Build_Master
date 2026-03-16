using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.InputSystem.EnhancedTouch;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;
using TouchPhase = UnityEngine.InputSystem.TouchPhase;

public class RoomPlacementHandler : MonoBehaviour
{
    [Header("AR Components")]
    public ARRaycastManager raycastManager;
    public ARPlaneManager planeManager;

    [Header("Furniture Pivots")]
    public GameObject sofaPivot;
    public GameObject teapoyPivot;
    public GameObject largeSofaPivot;
    public GameObject cribPivot;
    public GameObject chairPivot;

    [Header("App Flow")]
    public AppFlowManager appFlowManager;

    GameObject currentObject;
    bool placementMode = false;

    // Tracks if this is the first time the model is placed
    bool isPlaced = false;

    // Tracks the rotation angle for the 2-finger twist
    float previousRotationAngle = 0f;

    private List<ARRaycastHit> hits = new List<ARRaycastHit>();

    void OnEnable() { EnhancedTouchSupport.Enable(); }
    void OnDisable() { EnhancedTouchSupport.Disable(); }

    void Update()
    {
        if (!placementMode) return;

        if (Touch.activeTouches.Count == 0) return;

        // ==========================================
        // 2-FINGER GESTURE: ROTATE THE MODEL
        // ==========================================
        if (Touch.activeTouches.Count == 2)
        {
            Touch touch0 = Touch.activeTouches[0];
            Touch touch1 = Touch.activeTouches[1];

            // Calculate the angle between the two fingers
            Vector2 direction = touch1.screenPosition - touch0.screenPosition;
            float currentAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

            // If fingers just touched down, record the starting angle
            if (touch0.phase == TouchPhase.Began || touch1.phase == TouchPhase.Began)
            {
                previousRotationAngle = currentAngle;
            }
            // If fingers are twisting, rotate the object
            else if (touch0.phase == TouchPhase.Moved || touch1.phase == TouchPhase.Moved)
            {
                float angleDelta = currentAngle - previousRotationAngle;
                if (currentObject != null)
                {
                    currentObject.transform.Rotate(Vector3.up, angleDelta, Space.World);
                }
                previousRotationAngle = currentAngle;
            }
            return; // Skip the dragging math while we are rotating
        }

        // ==========================================
        // 1-FINGER GESTURE: PLACE AND DRAG
        // ==========================================
        if (Touch.activeTouches.Count == 1)
        {
            Touch touch = Touch.activeTouches[0];

            // Listen for BOTH tapping and dragging
            if (touch.phase == TouchPhase.Began || touch.phase == TouchPhase.Moved)
            {
                if (raycastManager.Raycast(touch.screenPosition, hits, TrackableType.Planes))
                {
                    Pose hitPose = hits[0].pose;

                    if (currentObject != null)
                    {
                        currentObject.SetActive(true);
                        currentObject.transform.position = hitPose.position;

                        // Only force it to face the camera on the very first drop.
                        // This prevents it from snapping back if you drag it after rotating it!
                        if (!isPlaced)
                        {
                            currentObject.transform.rotation = Quaternion.Euler(0, Camera.main.transform.eulerAngles.y, 0);
                            isPlaced = true;
                        }
                    }
                }
            }
        }
    }

    public void StartPlacementMode()
    {
        string furniture = appFlowManager.currentFurniture;
        DisableAllFurniture();
        isPlaced = false; // Reset the placement lock for the new object

        if (furniture.Contains("Large") && furniture.Contains("Sofa")) currentObject = largeSofaPivot;
        else if (furniture.Contains("Sofa")) currentObject = sofaPivot;
        else if (furniture.Contains("Teapoy")) currentObject = teapoyPivot;
        else if (furniture.Contains("Crib")) currentObject = cribPivot;
        else if (furniture.Contains("Chair")) currentObject = chairPivot;
        else
        {
            Debug.LogWarning("Unknown marker -> " + furniture);
            return;
        }

        if (currentObject != null)
        {
            placementMode = true;
            currentObject.SetActive(false);
            EnablePlanes(true);

            AssemblyManager asm = currentObject.GetComponent<AssemblyManager>();
            if (asm != null) asm.ShowFullModel();

            appFlowManager.StartPlacementModeUI();
        }
    }

    public void StopPlacementMode()
    {
        placementMode = false;
        isPlaced = false;
        EnablePlanes(false);
        DisableAllFurniture();
    }

    void DisableAllFurniture()
    {
        if (sofaPivot) sofaPivot.SetActive(false);
        if (teapoyPivot) teapoyPivot.SetActive(false);
        if (largeSofaPivot) largeSofaPivot.SetActive(false);
        if (cribPivot) cribPivot.SetActive(false);
        if (chairPivot) chairPivot.SetActive(false);
    }

    void EnablePlanes(bool value)
    {
        if (planeManager != null)
        {
            foreach (ARPlane plane in planeManager.trackables)
            {
                plane.gameObject.SetActive(value);
            }
        }
    }
}