using Godot;
using HuangD.Sessions;

public partial class BattleReportPanel : Control
{
    public Label Desc => GetNode<Label>("desc");

    internal BattleReport ReportObj
    {
        get => reportObj;
        set
        {
            reportObj = value;
            Desc.Text = reportObj.ToString();
        }
    }

    private BattleReport reportObj;
}