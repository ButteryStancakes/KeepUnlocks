using HarmonyLib;

namespace KeepUnlocks.Patches
{
    [HarmonyPatch]
    class KeepUnlocksPatches
    {
        [HarmonyPatch(typeof(GameNetworkManager), nameof(GameNetworkManager.ResetSavedGameValues))]
        [HarmonyPrefix]
        static void PreResetSavedGameValues(GameNetworkManager __instance)
        {
            if (__instance.isHostingGame && !StartOfRound.Instance.isChallengeFile)
                KeepThis.FindItemsToKeep();
        }

        [HarmonyPatch(typeof(StartOfRound), nameof(StartOfRound.ResetShip))]
        [HarmonyPostfix]
        static void PostResetShip(StartOfRound __instance)
        {
            if (__instance.IsServer && !__instance.isChallengeFile)
                __instance.StartCoroutine(KeepThis.RepopulateShipWithUnlocks());
        }
    }
}