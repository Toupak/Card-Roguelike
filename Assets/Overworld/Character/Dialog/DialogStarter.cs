using BoomLib.Dialog_System;
using NUnit.Framework;
using Overworld;
using System.Collections.Generic;
using UnityEngine;

public class DialogStarter : Interactable
{
    [SerializeField] private DialogData dialogData ;

    public override void ExecuteInteract()
    {
        base.ExecuteInteract();

        DialogManager.instance.StartDialog(dialogData.DialogTexts);
    }

}
