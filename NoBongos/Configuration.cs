using Dalamud.Configuration;
using Dalamud.Plugin;
using System;

namespace NoBongos
{
    [Serializable]
    public class Configuration : IPluginConfiguration
    {
        public int Version { get; set; } = 0;

        public bool BlockAlways { get; set; } = true;

        public bool BlockInPvP { get; set; } = true;

        public bool BlockInPvE { get; set; } = true;

        public bool BlockSoundEffects { get; set; } = true;

        public bool BlockCCSoundEffects { get; set; } = true;

        public void Save()
        {
            Services.PluginInterface.SavePluginConfig(this);
        }
    }
}
