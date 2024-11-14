using Chrona.Engine.Godot;
using Godot;
using HuangD.Godot.Utilties;
using HuangD.Sessions;
using System.Linq;

public partial class Armies : PanelContainer, IView
{
    public Label Current => GetNode<Label>("VBoxContainer/Total/HBoxContainer/VBoxContainer/Datas/Current");
    public Label Expect => GetNode<Label>("VBoxContainer/Total/HBoxContainer/VBoxContainer/Datas/Expect");
    public Label Increase => GetNode<Label>("VBoxContainer/Total/HBoxContainer/VBoxContainer/Datas/Increase");
    public Label Spend => GetNode<Label>("VBoxContainer/Total/HBoxContainer/VBoxContainer/Datas/Spend");

    public InstancePlaceholder ArmyItem => GetNode<InstancePlaceholder>("VBoxContainer/ScrollContainer/Content/ArmyItem");

    public override void _Process(double delta)
    {
        var view = this as IView;
        if (!view.IsDirty()) { return; }

        var province = this.GetSelectEntity().Current as Province;
        if (province == null)
        {
            return;
        }

        var allArmies = province.centralArmies.OfType<Army>().Prepend(province.LocalArmy);
        Current.Text = allArmies.Sum(x => x.Count).ToString();
        Expect.Text = allArmies.Sum(x => x.ExpectCount).ToString();
        Increase.Text = allArmies.Sum(x => x.IncCount).ToString();
        Spend.Text = allArmies.Sum(x => x.Cost).ToString();

        var items = ArmyItem.GetParent().GetChildren().OfType<ArmyItem>().ToArray();

        foreach (var item in items.Where(x => !allArmies.Contains(x.ArmyObj)).ToArray())
        {
            item.QueueFree();
        }

        foreach (var army in allArmies.Except(items.Select(x => x.ArmyObj)).ToArray())
        {
            var newItem = ArmyItem.CreateInstance() as ArmyItem;
            newItem.ArmyObj = army;
        }
    }
}
