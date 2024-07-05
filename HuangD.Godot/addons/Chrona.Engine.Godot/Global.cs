﻿using Chrona.Engine.Core;
using Chrona.Engine.Core.Events;
using Chrona.Engine.Core.Interfaces;
using Chrona.Engine.Core.Modders;
using Chrona.Engine.Godot.Utilties;
using Godot;
using System;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Chrona.Engine.Godot;

public partial class Global : Node
{
    public Chroncle Chroncle { get; private set; }

    public override void _Ready()
    {
        var modPath = Path.Combine(ProjectSettings.GlobalizePath("user://"), "mods");

        CreateNativeMode(modPath);
        Chroncle = new Chroncle(new Modder(modPath));
    }

    private void CreateNativeMode(string modPath)
    {
        var sourcePath = OS.HasFeature("editor") ?
            Path.Combine(ProjectSettings.GlobalizePath("res://"), ".godot/mono/temp/bin/Debug")
          : Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

        LOG.INFO(Assembly.GetExecutingAssembly().Location);
        ;

        var files = Directory.EnumerateFiles(sourcePath, "*Native*");
        if (files.Any())
        {
            var targetPath = Path.Combine(modPath, "native/dll");
            Directory.CreateDirectory(targetPath);

            foreach (var file in files)
            {
                File.Copy(file, Path.Combine(targetPath, Path.GetFileName(file)), true);
            }
        }
    }
}
