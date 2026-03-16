using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ModelInteraction : MonoBehaviour, IDragHandler
{
    [Header("Rotation & Zoom")]
    public float rotationSpeed = 0.5f;
    public Slider zoomSlider;
    public float minScale = 0.1f;
    public float maxScale = 2.0f;

    [Header("X-Ray Settings")]
    public Material solidMaterial;
    public Material ghostMaterial;
    public Image xrayButtonImage;
    public Sprite solidIcon;
    public Sprite ghostIcon;

    [Header("UI Elements")]
    public GameObject resetContainer;

    private Quaternion originalRotation;
    private bool isModified = false;
    private bool isXrayActive = false;

    void Start()
    {
        originalRotation = transform.rotation;

        // FIX: Force reset button to be hidden at the start
        if (resetContainer != null) resetContainer.SetActive(false);
        isModified = false;

        if (zoomSlider != null)
        {
            zoomSlider.minValue = minScale;
            zoomSlider.maxValue = maxScale;
            zoomSlider.value = transform.localScale.x;
            zoomSlider.onValueChanged.AddListener(OnSliderZoom);
        }

        if (xrayButtonImage != null) xrayButtonImage.sprite = solidIcon;
    }

    public void OnDrag(PointerEventData eventData)
    {
        float xRotation = eventData.delta.x * rotationSpeed;
        float yRotation = eventData.delta.y * rotationSpeed;
        transform.Rotate(Vector3.up, -xRotation, Space.World);
        transform.Rotate(Vector3.right, yRotation, Space.World);

        if (!isModified && resetContainer != null)
        {
            isModified = true;
            resetContainer.SetActive(true);
        }
    }

    public void OnSliderZoom(float value)
    {
        transform.localScale = new Vector3(value, value, value);
    }

    public void ToggleXray()
    {
        isXrayActive = !isXrayActive;
        if (xrayButtonImage != null) xrayButtonImage.sprite = isXrayActive ? ghostIcon : solidIcon;

        Renderer[] renderers = GetComponentsInChildren<Renderer>(true);
        Material selectedMat = isXrayActive ? ghostMaterial : solidMaterial;
        foreach (Renderer rend in renderers) rend.material = selectedMat;
    }

    public void ResetModel()
    {
        transform.rotation = originalRotation;
        isModified = false;
        if (resetContainer != null) resetContainer.SetActive(false);
    }
}
