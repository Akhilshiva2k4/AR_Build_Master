using UnityEngine;

public class ButtonSwitcher : MonoBehaviour
{
    public GameObject sofaPivot;
    public GameObject teapoyPivot;

    // --- Assembly Navigation (Calls AssemblyManager on Pivots) ---
    public void GlobalNext()
    {
        if (sofaPivot.activeSelf) sofaPivot.GetComponent<AssemblyManager>().NextStep();
        if (teapoyPivot.activeSelf) teapoyPivot.GetComponent<AssemblyManager>().NextStep();
    }

    public void GlobalPrev()
    {
        if (sofaPivot.activeSelf) sofaPivot.GetComponent<AssemblyManager>().PreviousStep();
        if (teapoyPivot.activeSelf) teapoyPivot.GetComponent<AssemblyManager>().PreviousStep();
    }

    // --- Interaction (Calls ModelInteraction on Pivots) ---
    public void GlobalXRay()
    {
        if (sofaPivot.activeSelf) sofaPivot.GetComponent<ModelInteraction>().ToggleXray();
        if (teapoyPivot.activeSelf) teapoyPivot.GetComponent<ModelInteraction>().ToggleXray();
    }

    public void GlobalReset()
    {
        if (sofaPivot.activeSelf) sofaPivot.GetComponent<ModelInteraction>().ResetModel();
        if (teapoyPivot.activeSelf) teapoyPivot.GetComponent<ModelInteraction>().ResetModel();
    }

    public void GlobalZoom(float value)
    {
        // Changed from AdjustZoom to OnSliderZoom to match your script
        if (sofaPivot.activeSelf) sofaPivot.GetComponent<ModelInteraction>().OnSliderZoom(value);
        if (teapoyPivot.activeSelf) teapoyPivot.GetComponent<ModelInteraction>().OnSliderZoom(value);
    }
}