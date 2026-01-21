using BoomLib.Dialog_System;
using BoomLib.Tools;
using Map;
using UnityEngine;

namespace Character_Selection.Character.Dialog
{
    public class DialogStarter : Interactable
    {
        [SerializeField] protected DialogData dialogData;

        private float offsetY;
        protected Vector2 dialogPosition => transform.position.ToVector2() + Vector2.up * offsetY;
        
        private void Start()
        {
            offsetY = GetComponent<Collider2D>().bounds.size.y / 2 + 0.5f;
        }

        public override void ExecuteInteract()
        {
            base.ExecuteInteract();
            StartDialog();
        }

        protected virtual void StartDialog()
        {
            DialogManager.instance.StartDialog(dialogData.DialogTexts, dialogPosition);
        }
    }
}
