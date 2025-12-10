using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class Interactable : MonoBehaviour
{
    protected bool isWithinRange;

    [HideInInspector] public UnityEvent OnCharacterInteract = new UnityEvent();

    protected virtual void Update()
    {
        if (isWithinRange && PlayerInput.GetInteractInput())
            ExecuteInteract();
    }
    
    protected virtual void ExecuteInteract()
    {
        OnCharacterInteract.Invoke();
        Debug.Log("Object Has Been Interacted");
    }

    protected void OnTriggerStay2D(Collider2D otherCollider)
    {
        if (otherCollider.transform.CompareTag("Character"))
            isWithinRange = true;
    }

    protected void OnTriggerExit2D(Collider2D otherCollider)
    {
        if (otherCollider.transform.CompareTag("Character"))
            isWithinRange = false;
    }
}
