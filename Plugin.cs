using BepInEx;
using BepInEx.Configuration;
using HarmonyLib;

namespace KeepUnlocks
{
    [BepInPlugin(PLUGIN_GUID, PLUGIN_NAME, PLUGIN_VERSION)]
    public class Plugin : BaseUnityPlugin
    {
        const string PLUGIN_GUID = "butterystancakes.lethalcompany.keepunlocks", PLUGIN_NAME = "Keep Unlocks", PLUGIN_VERSION = "1.0.0";
        public static ConfigEntry<bool> configKeepSuits, configKeepFurniture, configKeepUpgrades, configAutoStore;

        void Awake()
        {
            configKeepSuits = Config.Bind("Cosmetic", "KeepSuits", true, "Retain player suits? (Green suit, Hazard suit, Pajama suit)");
            configKeepFurniture = Config.Bind("Cosmetic", "KeepFurniture", true, "Retain misc. furniture? (Television, Shower, etc.)");
            configKeepUpgrades = Config.Bind("Gameplay", "KeepUpgrades", false, "Retain ship upgrades? (Loud horn, Signal translator, Teleporter, Inverse Teleporter)");
            configAutoStore = Config.Bind("Experimental", "AutoStore", true, "Automatically place furniture and/or upgrades into storage. Might be buggy");

            Harmony harmony = new Harmony(PLUGIN_GUID);
            harmony.PatchAll();

            Logger.LogInfo($"{PLUGIN_NAME} v{PLUGIN_VERSION} loaded");
        }
    }
}