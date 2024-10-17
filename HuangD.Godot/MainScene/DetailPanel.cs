using Chrona.Engine.Godot;
using Godot;
using Godot.Collections;
using HuangD.Godot.Utilties;
using HuangD.Sessions;
using System;
using System.Collections.Generic;
using System.Linq;

public abstract partial class DetailPanel : Control
{
    //public string EntityId { get; set; }

    public Label Title => GetNode<Label>("../PanelContainer/Title/Label");
    public Button Close => GetNode<Button>("close");
}