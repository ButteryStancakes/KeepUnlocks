using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

namespace KeepUnlocks
{
    internal class KeepThis
    {
        static List<int> unlocks = new();

        internal static void FindItemsToKeep()
        {
            unlocks.Clear();

            for (int i = 0; i < StartOfRound.Instance.unlockablesList.unlockables.Count; i++)
            {
                if (StartOfRound.Instance.unlockablesList.unlockables[i].hasBeenUnlockedByPlayer)
                    KeepThisItem(i);
            }
        }

        static void KeepThisItem(int id)
        {
            if (unlocks.Contains(id))
                return;

            UnlockableItem item = StartOfRound.Instance.unlockablesList.unlockables[id];

            if (item.unlockableType == 0)
            {
                if (Plugin.configKeepSuits.Value)
                    unlocks.Add(id);
            }
            else if (IsShipUpgrade(item))
            {
                if (Plugin.configKeepUpgrades.Value)
                    unlocks.Add(id);
            }
            else if (Plugin.configKeepFurniture.Value)
                unlocks.Add(id);
        }

        internal static IEnumerator RepopulateShipWithUnlocks()
        {
            int credits = TimeOfDay.Instance.quotaVariables.startingCredits;

            Terminal terminal = Object.FindObjectOfType<Terminal>();
            if (terminal != null)
                credits = terminal.groupCredits;

            foreach (int id in unlocks)
                StartOfRound.Instance.BuyShipUnlockableServerRpc(id, credits);

            if (Plugin.configAutoStore.Value)
            {
                yield return new WaitForSeconds(2f);
                foreach (PlaceableShipObject placeableShipObject in Object.FindObjectsOfType<PlaceableShipObject>())
                {
                    if (unlocks.Contains(placeableShipObject.unlockableID) && StartOfRound.Instance.unlockablesList.unlockables[placeableShipObject.unlockableID].canBeStored && !IsShipUpgrade(StartOfRound.Instance.unlockablesList.unlockables[placeableShipObject.unlockableID]))
                    {
                        if (!StartOfRound.Instance.unlockablesList.unlockables[placeableShipObject.unlockableID].spawnPrefab)
                        {
                            placeableShipObject.parentObject.disableObject = true;
                            Debug.Log("DISABLE OBJECT F");
                        }
                        ShipBuildModeManager.Instance.StoreObjectServerRpc(placeableShipObject.parentObject.GetComponent<NetworkObject>(), (int)GameNetworkManager.Instance.localPlayerController.playerClientId);
                    }
                }
            }
        }

        static bool IsShipUpgrade(UnlockableItem item)
        {
            return item.unlockableName == "Teleporter" || item.unlockableName == "Signal translator" || item.unlockableName == "Loud horn" || item.unlockableName == "Inverse Teleporter";
        }
    }
}
