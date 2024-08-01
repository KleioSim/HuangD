using HuangD.Sessions.Utilties;

namespace HuangD.Sessions;

public partial class Country
{
    public string Key { get; } = UUID.Generate("CNT");
}