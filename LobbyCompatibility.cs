﻿using LobbyCompatibility.Enums;
using LobbyCompatibility.Features;

namespace KeepUnlocks
{
    internal static class LobbyCompatibility
    {
        internal static void Init()
        {
            PluginHelper.RegisterPlugin(Plugin.PLUGIN_GUID, System.Version.Parse(Plugin.PLUGIN_VERSION), CompatibilityLevel.ServerOnly, VersionStrictness.None);
        }
    }
}