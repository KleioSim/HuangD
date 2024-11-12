using Godot;

public partial class SelectCountryPanel : Panel
{
    [Signal]
    public delegate void BackEventHandler();

    [Signal]
    public delegate void NextEventHandler(string countryId);

    public Button BackButton => GetNode<Button>("VBoxContainer/Back");
    public CountryContent CountryContent => GetNode<CountryContent>("VBoxContainer/Content");

    public override void _Ready()
    {
        BackButton.Connect(Button.SignalName.Pressed, Callable.From(() => EmitSignal(SignalName.Back)));
        CountryContent.Confirm.Connect(Button.SignalName.Pressed, Callable.From(() => EmitSignal(SignalName.Next, CountryContent.SelectedCountry.Id)));
    }

}
