using Godot;
using System;

public partial class SelectCountryPanel : PanelContainer
{
    public Label CountryName => GetNode<Label>("MarginContainer/VBoxContainer/CountryName");

    public Label ProvinceCount => GetNode<Label>("MarginContainer/VBoxContainer/ProvinceCount");

    public Label PopCount => GetNode<Label>("MarginContainer/VBoxContainer/PopCount");
}
