using Chrona.Engine.Godot;
using Godot;
using Godot.Collections;
using HuangD.Godot.Utilties;
using HuangD.Sessions;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class DetailPanel : Panel
{
    public Label label => GetNode<Label>("Label");

    private IEnumerable<ViewControl> viewContrls;

    public string EntityId
    {
        get => entityId;
        set
        {
            label.Text = value;
            entityId = value;

            switch (this.GetSession().Entities[EntityId])
            {
                case Province province:
                    viewContrls.OfType<ProvinceDetailPanel>().Single().Visible = true;
                    break;
            }
        }
    }

    private string entityId;

    public override void _Ready()
    {
        viewContrls = new ViewControl[]
        {
            GetNode<ProvinceDetailPanel>("")
        };
    }
}