using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class Interactable : MonoBehaviour
{
    [HideInInspector] public UnityEvent OnCharacterInteract = new UnityEvent();
    
    public virtual void ExecuteInteract()
    {
        OnCharacterInteract.Invoke();
        Debug.Log("Object Has Been Interacted, no interaction implemented");
    }
}
