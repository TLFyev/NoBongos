using System;
using System.Numerics;
using Dalamud.Interface.Windowing;
using ImGuiNET;

namespace NoBongos.Windows;

public class ConfigWindow : Window, IDisposable
{

    internal ConfigWindow() : base(
        "NoBongos Settings", ImGuiWindowFlags.NoScrollbar | ImGuiWindowFlags.NoScrollWithMouse | ImGuiWindowFlags.NoResize | ImGuiWindowFlags.NoCollapse)
    {
        Size = new Vector2(250, 200);
    }

    public void Dispose() 
    {
        GC.SuppressFinalize(this);
    }

    public override void Draw()
    {
        ImGui.Text("Tick box to disable relevant sound effects");
        var blockAlways = Services.Config.BlockAlways;
        if (ImGui.Checkbox("Block Always", ref blockAlways))
        {
            Services.Config.BlockAlways = blockAlways;
            Services.Config.Save();
        }
        ImGui.Separator();
        var blockInPvP = Services.Config.BlockInPvP;
        if (ImGui.Checkbox("Block During PvP", ref blockInPvP))
        {
            Services.Config.BlockInPvP = blockInPvP;
            Services.Config.Save();
        }
        var blockInPvE = Services.Config.BlockInPvE;
        if (ImGui.Checkbox("Block During PvE", ref blockInPvE))
        {
            Services.Config.BlockInPvE = blockInPvE;
            Services.Config.Save();
        }
        ImGui.Separator();
        var blockSounds = Services.Config.BlockSoundEffects;
        if (ImGui.Checkbox("<se.#> sounds", ref blockSounds))
        {
            Services.Config.BlockSoundEffects = blockSounds;
            Services.Config.Save();
        }
        var blockCCSounds = Services.Config.BlockCCSoundEffects;
        if (ImGui.Checkbox("Crystalline Conflict quick chat sounds", ref blockCCSounds))
        {
            Services.Config.BlockSoundEffects = blockCCSounds;
            Services.Config.Save();
        }
    }
}
