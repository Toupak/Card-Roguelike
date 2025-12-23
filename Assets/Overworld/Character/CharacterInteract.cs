using UnityEngine;

namespace Overworld.Character
{
    public class CharacterInteract : MonoBehaviour
    {
        [SerializeField] private GameObject interactButtonPrefab;
        [SerializeField] private float offsety;

        private bool isWithinRange;

        private GameObject interactButton;
        private Vector2 interactButtonPosition;

        private Interactable interactable;

        private void Update()
        {
            if (!CharacterSingleton.instance.IsLocked && isWithinRange && PlayerInput.GetInteractInput())
                Interact();
        }

        protected void OnTriggerEnter2D(Collider2D otherCollider)
        {
            EnterInteractRange(otherCollider);
        }

        protected void OnTriggerExit2D(Collider2D otherCollider)
        {
            ExitInteractRange();
        }

        private void Interact()
        {
            if (interactable != null)
                interactable.ExecuteInteract();
        }

        private void EnterInteractRange(Collider2D collider)
        {
            interactable = collider.GetComponent<Interactable>();

            if (interactable != null)
            {
                isWithinRange = true;

                if (interactButton == null)
                    SpawnInteractButton(collider.bounds.size.y / 2 + offsety);
            }
        }

        private void ExitInteractRange()
        {
            isWithinRange = false;

            if (interactButton != null)
                DestroyInteractButton();

            interactable = null;
        }

        private void SpawnInteractButton(float offset)
        {
            if (interactButton == null)
            {
                interactButtonPosition = new Vector2(interactable.transform.position.x, interactable.transform.position.y + offset);
                interactButton = Instantiate(interactButtonPrefab, interactButtonPosition, Quaternion.identity, interactable.transform);
            }
        }

        private void DestroyInteractButton()
        {
            if (interactButton != null)
            {
                Destroy(interactButton.gameObject);
                interactButton = null;
                interactButtonPosition = Vector2.zero;
            }
        }
    }
}
