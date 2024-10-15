using Godot;
using HuangD.Godot.Utilties;
using HuangD.Sessions;
using HuangD.Sessions.Messages;
using System;
using System.Linq;

public partial class PoliticalContainer : Control
{
    InstancePlaceholder PoliticalInfoPlaceHolder => GetNode<InstancePlaceholder>("Item");

    public override void _Ready()
    {

    }

    public void BuildPoliticalInfos(Func<string, Vector2> FuncGetProvinceCenter)
    {
        var session = this.GetSession();

        foreach (var province in session.Entities.Values.OfType<Province>())
        {
            var politicalInfo = PoliticalInfoPlaceHolder.CreateInstance() as PoliticalItem;
            politicalInfo.Name = province.Id;

            politicalInfo.Position = FuncGetProvinceCenter(province.Id);
            politicalInfo.province = province;
            politicalInfo.ArmyInfo.Connect(ArmyInfo.SignalName.ClickArmy, new Callable(this, MethodName.OnClickEntity));
            politicalInfo.EnemyInfo.Connect(ArmyInfo.SignalName.ClickArmy, new Callable(this, MethodName.OnClickEntity));
            politicalInfo.MoveTarget.Connect(Button.SignalName.ButtonDown, Callable.From(() =>
            {
                GD.Print($"mouse {GetGlobalMousePosition()}");
                GD.Print($"politicalInfo.MoveTarget  GlobalPositionPosition:{politicalInfo.MoveTarget.GlobalPosition}");
                GD.Print($"politicalInfo.MoveTarget  GlobalPositionPositionWithOffset:{politicalInfo.MoveTarget.GetGlobalPositionWithPivotOffset()}");
                var selectEntity = this.GetSession().SelectedEntity;
                if (selectEntity is Army army)
                {
                    this.GetSession().OnMessage(new Command_ArmyMove(army.Id, province.Id));
                }

            }));
            politicalInfo.MoveTarget.Visible = false;
        }
    }

    internal void OnCameraZoom(Vector2 zoom)
    {
        foreach (var item in PoliticalInfoPlaceHolder.GetParent().GetChildren().OfType<PoliticalItem>())
        {
            item.OnZoomed(zoom);
        }
    }

    private void OnClickEntity(string id)
    {
        this.GetSession().OnMessage(new Command_SelectEntity(id));
    }

    internal PoliticalItem GetItem(string id)
    {
        return PoliticalInfoPlaceHolder.GetParent().GetChildren().OfType<PoliticalItem>().SingleOrDefault(x => x.province.Id == id);
    }
}
