using Dalamud.Game.Command;
using Dalamud.IoC;
using Dalamud.Plugin;
using System.IO;
using Dalamud.Interface.Windowing;
using Dalamud.Plugin.Services;
using NoBongos.Windows;
using Dalamud.Hooking;
using Dalamud.Utility.Signatures;
using System.Linq;

namespace NoBongos
{
    public sealed class Plugin : IDalamudPlugin
    {
        private const string ConfigCommandName = "/nobongos";

        private readonly ConfigWindow configWindow;
        private readonly WindowSystem windowSystem;

        private readonly uint[] annoyingSounds = { 37, 38, 39, 40, 41, 42, 43, 44, 45, 46, 47, 48, 49, 50, 51, 52 };
        private readonly uint[] annoyingCCSounds = { 1000096, 1000097, 1000098, 1000099, 1000100, 1000101, 1000102, 1000103, 1000104 };

        private delegate void SoundDelegate(uint soundId, long u1, long u2);

        [Signature("E8 ?? ?? ?? ?? 48 63 45 80", DetourName = nameof(SoundDetour))]
        private Hook<SoundDelegate>? PlaySoundEffectHook { get; init; }

        private void SoundDetour(uint soundId, long u1, long u2)
        {
            if((Services.ClientState.IsPvP && Services.Config.BlockInPvP) || (Services.DutyState.IsDutyStarted && Services.Config.BlockInPvE) || (Services.Config.BlockAlways))
            {
                if (Services.Config.BlockSoundEffects == true && annoyingSounds.Contains(soundId))
                {
                    soundId = 0;
                }
                if (Services.Config.BlockCCSoundEffects == true && annoyingCCSounds.Contains(soundId))
                {
                    soundId = 0;
                }
            }
            this.PlaySoundEffectHook!.Original(soundId, u1, u2);
        }

        public Plugin(IDalamudPluginInterface pluginInterface)
        {
            pluginInterface.Create<Services>();

            Services.Config = Services.PluginInterface.GetPluginConfig() as Configuration ?? new Configuration();

            Services.GameInteropProvider.InitializeFromAttributes(this);
            this.PlaySoundEffectHook?.Enable();

            configWindow = new ConfigWindow();
            windowSystem = new WindowSystem("NoBongos");
            windowSystem.AddWindow(configWindow);

            Services.CommandManager.AddHandler(ConfigCommandName, new CommandInfo(OnConfigCommand)
            {
                HelpMessage = "Opens the NoBongos config window."
            });

            Services.PluginInterface.UiBuilder.Draw += DrawUi;
            Services.PluginInterface.UiBuilder.OpenConfigUi += DrawConfigUI;
        }

        public void Dispose()
        {
            windowSystem.RemoveAllWindows();
            configWindow.Dispose();
            Services.CommandManager.RemoveHandler(ConfigCommandName);
            this.PlaySoundEffectHook?.Disable();
            this.PlaySoundEffectHook?.Dispose();

            Services.PluginInterface.UiBuilder.Draw -= DrawUi;
            Services.PluginInterface.UiBuilder.OpenConfigUi -= DrawConfigUI;
        }

        private void OnConfigCommand(string command, string args)
        {
            // in response to the slash command, just display our main ui
            DrawConfigUI();
        }

        private void DrawUi()
        {
            windowSystem.Draw();
        }

        public void DrawConfigUI()
        {
            configWindow.IsOpen = !configWindow.IsOpen;
        }
    }
}
