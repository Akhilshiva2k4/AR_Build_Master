using UnityEngine;

public class AssemblyManager : MonoBehaviour
{
    [Header("Assembly Settings")]
    public GameObject[] assemblySteps;

    private int currentStepIndex = 0;

    void Start()
    {
        currentStepIndex = 0;
        UpdateStepVisibility();
    }

    public void NextStep()
    {
        if (currentStepIndex < assemblySteps.Length - 1)
        {
            currentStepIndex++;
            UpdateStepVisibility();
        }
        else
        {
            Debug.Log("Assembly Complete!");
        }
    }

    public void PreviousStep()
    {
        if (currentStepIndex > 0)
        {
            currentStepIndex--;
            UpdateStepVisibility();
        }
    }

    public void ResetToStepOne()
    {
        currentStepIndex = 0;
        UpdateStepVisibility();
    }

    public void UpdateStepVisibility()
    {
        if (assemblySteps == null || assemblySteps.Length == 0)
        {
            Debug.LogError("Assembly steps not assigned on " + gameObject.name);
            return;
        }

        for (int i = 0; i < assemblySteps.Length; i++)
        {
            if (assemblySteps[i] != null)
            {
                bool isVisible = (i <= currentStepIndex);
                assemblySteps[i].SetActive(isVisible);

                // =====================================
                // Animation Freeze Logic
                // =====================================
                Animator anim = assemblySteps[i].GetComponent<Animator>();

                // Only touch the animator if the object is actually turned on
                if (anim != null && assemblySteps[i].activeInHierarchy)
                {
                    if (i == currentStepIndex)
                    {
                        // It's the current step: let it loop at normal speed
                        anim.speed = 1f;
                        anim.Play(anim.GetCurrentAnimatorStateInfo(0).fullPathHash, 0, 0f);
                    }
                    else if (i < currentStepIndex)
                    {
                        // It's a completed past step: snap to the 100% finished mark and freeze it
                        anim.Play(anim.GetCurrentAnimatorStateInfo(0).fullPathHash, 0, 1f);
                        anim.speed = 0f;
                    }
                }
            }
        }

        Debug.Log("Showing step " + (currentStepIndex + 1) + " for " + gameObject.name);
    }

    public void ShowFullModel()
    {
        currentStepIndex = assemblySteps.Length - 1;

        // Re-use our new logic to ensure all animations are frozen at 100% 
        // when the user is trying to place the furniture on the floor!
        UpdateStepVisibility();
    }

    public int GetCurrentStepNumber()
    {
        return currentStepIndex + 1;
    }
}