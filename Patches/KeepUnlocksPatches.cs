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
            if (StartOfRound.Instance != null)
            {
                Debug.Log("Resetting unlockables list!");
                List<UnlockableItem> unlockables = StartOfRound.Instance.unlockablesList.unlockables;
                for (int i = 0; i < unlockables.Count; i++)
                {
                    if (__instance.isHostingGame && unlockables[i].hasBeenUnlockedByPlayer)
                        KeepThis.KeepThisItem(i);

                    unlockables[i].hasBeenUnlockedByPlayer = false;
                    if (unlockables[i].unlockableType == 1)
                    {
                        unlockables[i].placedPosition = Vector3.zero;
                        unlockables[i].placedRotation = Vector3.zero;
                        unlockables[i].hasBeenMoved = false;
                        unlockables[i].inStorage = false;
                    }
                }
            }

            return false;
        }

        [HarmonyPatch(typeof(StartOfRound), "ResetShip")]
        [HarmonyPostfix]
        public static void ResetShip(StartOfRound __instance)
        {
            if (!__instance.IsServer)
                return;

            __instance.StartCoroutine(KeepThis.RepopulateShipWithUnlocks());
        }
    }
}