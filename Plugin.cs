using BepInEx;
using BepInEx.Bootstrap;
using BepInEx.Configuration;
using HarmonyLib;

namespace KeepUnlocks
{
    [BepInPlugin(PLUGIN_GUID, PLUGIN_NAME, PLUGIN_VERSION)]
    [BepInDependency(GUID_LOBBY_COMPATIBILITY, BepInDependency.DependencyFlags.SoftDependency)]
    public class Plugin : BaseUnityPlugin
    {
        internal const string PLUGIN_GUID = "butterystancakes.lethalcompany.keepunlocks", PLUGIN_NAME = "Keep Unlocks", PLUGIN_VERSION = "1.0.5";
        public static ConfigEntry<bool> configKeepSuits, configKeepFurniture, configKeepUpgrades, configAutoStore;

        const string GUID_LOBBY_COMPATIBILITY = "BMX.LobbyCompatibility";

        void Awake()
        {
            if (Chainloader.PluginInfos.ContainsKey(GUID_LOBBY_COMPATIBILITY))
            {
                Logger.LogInfo("CROSS-COMPATIBILITY - Lobby Compatibility detected");
                LobbyCompatibility.Init();
            }

            configKeepSuits = Config.Bind("Cosmetic", "KeepSuits", true, "Retain player suits? (Green suit, Hazard suit, Pajama suit, Purple Suit, Bee Suit, Bunny Suit)");
            configKeepFurniture = Config.Bind("Cosmetic", "KeepFurniture", false, "Retain misc. furniture? (Television, Shower, etc.)");
            configKeepUpgrades = Config.Bind("Gameplay", "KeepUpgrades", false, "Retain ship upgrades? (Loud horn, Signal translator, Teleporter, Inverse Teleporter)");
            configAutoStore = Config.Bind("Miscellaneous", "AutoStore", true, "Automatically place furniture into storage.");

            // older versions of the mod had this setting in a different section, causing a duplicate config entry
            Config.Bind("Experimental", "AutoStore", false, "Legacy setting, moved to \"Miscellaneous\" section");
            Config.Remove(Config["Experimental", "AutoStore"].Definition);
            Config.Save();

            new Harmony(PLUGIN_GUID).PatchAll();

            Logger.LogInfo($"{PLUGIN_NAME} v{PLUGIN_VERSION} loaded");
        }
    }
}