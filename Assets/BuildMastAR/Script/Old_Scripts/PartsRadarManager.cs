using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events; // We need this for UnityEvent

public class PartsRadarManager : MonoBehaviour
{
    [Header("UI References")]
    public Button radarButton;
    public Image radarIcon;

    [Header("Radar Events (Link your background script here)")]
    public UnityEvent onRadarActivated;
    public UnityEvent onRadarDeactivated;

    [Header("Other UI to Hide")]
    public GameObject[] objectsToHide;

    [Header("State Visuals")]
    public Color activeColor = new Color(0.1f, 0.6f, 0.1f);
    public Color inactiveColor = Color.white;

    private bool isRadarActive = false;

    void Start()
    {
        if (radarButton != null)
        {
            radarButton.onClick.AddListener(ToggleRadar);
            if (radarIcon != null) radarIcon.color = inactiveColor;
        }
    }

    public void ToggleRadar()
    {
        isRadarActive = !isRadarActive;

        if (isRadarActive)
        {
            radarIcon.color = activeColor;

            // Trigger whatever is linked in the Inspector to hide the background
            onRadarActivated.Invoke();

            // Hide the other assembly buttons
            foreach (GameObject obj in objectsToHide)
                if (obj != null) obj.SetActive(false);

            Debug.Log("Radar Flow Active: Events invoked, UI hidden.");
        }
        else
        {
            radarIcon.color = inactiveColor;

            // Trigger whatever is linked in the Inspector to show the background
            onRadarDeactivated.Invoke();

            // Bring the other assembly buttons back
            foreach (GameObject obj in objectsToHide)
                if (obj != null) obj.SetActive(true);

            Debug.Log("Radar Flow Deactivated: UI restored.");
        }
    }
}