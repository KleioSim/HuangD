using Godot;
using HuangD.Sessions;
using System;
using System.Linq;

public partial class ProvinceBattleInfo : Control
{
    public RichTextLabel Desc => GetNode<RichTextLabel>("");

    InstancePlaceholder BattleReportPlaceHoder => GetNode<InstancePlaceholder>("");

    internal void Update(Province province)
    {
        this.Visible = province.Battle != null;
        if (this.Visible)
        {
            Desc.Text = String.Join("\n", province.Battle.BattleReports.Select(x => x.ToString()));

            //var reportPanels = BattleReportPlaceHoder.GetParent().GetChildren().OfType<BattleReportPanel>();

            //var needRemoves = reportPanels.Where(x => !province.Battle.BattleReports.Contains(x.ReportObj)).ToArray();
            //var needAdds = province.Battle.BattleReports.Except(reportPanels.Select(x => x.ReportObj)).ToArray();

            //foreach (var item in needRemoves)
            //{
            //    item.QueueFree();
            //}

            //foreach (var item in needAdds)
            //{
            //    var reportPanel = BattleReportPlaceHoder.CreateInstance() as BattleReportPanel;
            //    reportPanel.ReportObj = item;
            //}
        }
    }
}
