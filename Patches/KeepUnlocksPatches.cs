using System.Collections.Generic;
using HarmonyLib;
using UnityEngine;

namespace KeepUnlocks.Patches
{
    [HarmonyPatch]
    class KeepUnlocksPatches
    {
        [HarmonyPatch(typeof(GameNetworkManager), "ResetUnlockablesListValues")]
        [HarmonyPrefix]
        public static bool ResetUnlockablesListValues(GameNetworkManager __instance)
        {
            if (__instance.isHostingGame)
            {
                for (int i = 0; i < StartOfRound.Instance.unlockablesList.unlockables.Count; i++)
                {
                    if (StartOfRound.Instance.unlockablesList.unlockables[i].hasBeenUnlockedByPlayer)
                        KeepThis.KeepThisItem(i);
                }
            }

            return true;
        }

        [HarmonyPatch(typeof(StartOfRound), "ResetShip")]
        [HarmonyPostfix]
        public static void ResetShip(StartOfRound __instance)
        {
            if (__instance.IsServer)
                __instance.StartCoroutine(KeepThis.RepopulateShipWithUnlocks());
        }

        [HarmonyPatch(typeof(GameNetworkManager), "Disconnect")]
        [HarmonyPostfix]
        public static void Disconnect()
        {
            KeepThis.ChangeSave();
        }
    }
}