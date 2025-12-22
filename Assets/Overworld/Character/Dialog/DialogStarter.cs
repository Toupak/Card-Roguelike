using BoomLib.Dialog_System;
using BoomLib.Tools;
using UnityEngine;

namespace Overworld.Character.Dialog
{
    public class DialogStarter : Interactable
    {
        [SerializeField] private DialogData dialogData;

        private float offsetY;
        private Vector2 dialogPosition => transform.position.ToVector2() + Vector2.up * offsetY;
        
        private void Start()
        {
            offsetY = GetComponent<Collider2D>().bounds.size.y / 2 + 0.5f;
        }

        public override void ExecuteInteract()
        {
            base.ExecuteInteract();
            DialogManager.instance.StartDialog(dialogData.DialogTexts, dialogPosition);
        }
    }
}
