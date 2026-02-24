using Character_Selection.Character;
using UnityEngine;
using UnityEngine.Events;

namespace Map
{
    public class Interactable : MonoBehaviour
    {
        [HideInInspector] public UnityEvent OnCharacterInteract = new UnityEvent();

        protected CharacterInteract currentCharacterInteract;
        
        public virtual bool CanInteract()
        {
            return true;
        }
        
        public virtual void ExecuteInteract(CharacterInteract characterInteract)
        {
            currentCharacterInteract = characterInteract;
            OnCharacterInteract.Invoke();
            Debug.Log("Object Has Been Interacted");
        }
    }
}
