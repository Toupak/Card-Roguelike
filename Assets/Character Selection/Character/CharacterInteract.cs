using BoomLib.Tools;
using Overworld.UI.Selection_Cursor;
using UnityEngine;

namespace Overworld.Character
{
    public class CharacterInteract : MonoBehaviour
    {
        [SerializeField] private GameObject interactButtonPrefab;
        [SerializeField] private GameObject selectionCursorPrefab;
        [SerializeField] private float offsety;

        private bool isWithinRange;

        private GameObject interactButton;

        private SelectionCursor selectionCursor;

        private Interactable interactable;

        private void Update()
        {
            if (!CharacterSingleton.instance.IsLocked && isWithinRange && PlayerInput.GetInteractInput())
                Interact();
        }

        protected void OnTriggerEnter2D(Collider2D otherCollider)
        {
            EnterInteractRange(otherCollider.GetComponent<Interactable>(), otherCollider.bounds);
        }

        protected void OnTriggerExit2D(Collider2D otherCollider)
        {
            ExitInteractRange(otherCollider.GetComponent<Interactable>());
        }

        private void Interact()
        {
            if (interactable != null)
                interactable.ExecuteInteract();
        }

        private void EnterInteractRange(Interactable colliderInteractable, Bounds bounds)
        {
            if (colliderInteractable != null)
            {
                interactable = colliderInteractable;
                isWithinRange = true;

                SpawnInteractButton(bounds);
            }
        }

        private void ExitInteractRange(Interactable colliderInteractable)
        {
            if (colliderInteractable != interactable)
                return;
            
            isWithinRange = false;

            DestroyInteractButton();

            interactable = null;
        }

        private void SpawnInteractButton(Bounds bounds)
        {
            float offset = bounds.size.y / 2 + offsety;
            
            DestroyInteractButton();

            Vector2 interactablePosition = interactable.transform.position.ToVector2();
            Vector2 interactButtonPosition = interactablePosition + Vector2.up * offset;
            interactButton = Instantiate(interactButtonPrefab, interactButtonPosition, Quaternion.identity, interactable.transform);

            selectionCursor = Instantiate(selectionCursorPrefab, interactablePosition, Quaternion.identity, interactable.transform).GetComponent<SelectionCursor>();
            selectionCursor.Setup(bounds.size.ToVector2() * 0.5f, Vector2.zero);
        }

        private void DestroyInteractButton()
        {
            if (interactButton != null)
            {
                Destroy(interactButton.gameObject);
                interactButton = null;
            }

            if (selectionCursor != null)
            {
                Destroy(selectionCursor.gameObject);
                selectionCursor = null;
            }
        }
    }
}
