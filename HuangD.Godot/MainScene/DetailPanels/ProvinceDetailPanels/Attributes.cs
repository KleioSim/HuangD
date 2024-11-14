using Chrona.Engine.Godot;
using Godot;
using HuangD.Godot.Utilties;
using HuangD.Sessions;
using System.Linq;

public partial class Attributes : Control, IView
{
    public Label Terrain => GetNode<Label>("GridContainer/Terrain/Value");
    public Label PopCount => GetNode<Label>("GridContainer/PopCount/Value");
    public Label Tax => GetNode<Label>("GridContainer/Tax/Value");
    public Label LocalArmyCount => GetNode<Label>("GridContainer/LocalArmyCount/Value");
    public Label CentralArmyCount => GetNode<Label>("GridContainer/CentralArmyCount/Value");

    public override void _Process(double delta)
    {
        var view = this as IView;
        if (!view.IsDirty()) { return; }

        var province = this.GetSelectEntity().Current as Province;
        if (province == null)
        {
            return;
        }

        Terrain.Text = province.Terrain.ToString();
        PopCount.Text = province.PopCount.ToString();
        Tax.Text = province.PopTax.Current.ToString();
        CentralArmyCount.Text = province.centralArmies.Sum(x => x.Count).ToString();
        LocalArmyCount.Text = province.LocalArmy.Count.ToString();
    }
}
