using UnityEngine;
using UnityEngine.Events;

namespace Overworld
{
    public class Interactable : MonoBehaviour
    {
        [HideInInspector] public UnityEvent OnCharacterInteract = new UnityEvent();
    
        public virtual void ExecuteInteract()
        {
            OnCharacterInteract.Invoke();
            Debug.Log("Object Has Been Interacted");
        }
    }
}
