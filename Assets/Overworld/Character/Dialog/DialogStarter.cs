using BoomLib.Dialog_System;
using UnityEngine;

namespace Overworld.Character.Dialog
{
    public class DialogStarter : Interactable
    {
        [SerializeField] private DialogData dialogData;

        public override void ExecuteInteract()
        {
            base.ExecuteInteract();

            Collider2D collider = GetComponent<Collider2D>();

            float offsety = 0;
            if (collider != null)
                offsety = collider.bounds.size.y / 2 + 0.5f;

            Vector2 position = new Vector2(transform.position.x, transform.position.y + offsety);

            Debug.Log($"Execute Interaction : {position}");
            
            DialogManager.instance.StartDialog(dialogData.DialogTexts, position);
        }
    }
}
