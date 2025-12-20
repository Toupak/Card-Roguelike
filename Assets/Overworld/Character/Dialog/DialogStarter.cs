using BoomLib.Dialog_System;
using NUnit.Framework;
using Overworld;
using System.Collections.Generic;
using UnityEngine;

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

        DialogManager.instance.StartDialog(dialogData.DialogTexts, position);
    }
}
