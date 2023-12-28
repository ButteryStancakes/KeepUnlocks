using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

namespace KeepUnlocks
{
    internal class KeepThis
    {
        static List<int> unlocks = new List<int>();

        public static void KeepThisItem(int id)
        {
            if (unlocks.Contains(id))
                return;

            UnlockableItem item = StartOfRound.Instance.unlockablesList.unlockables[id];

            if (item.unlockableType == 0)
            {
                if (Plugin.configKeepSuits.Value)
                    unlocks.Add(id);
            }
            else if (item.unlockableName == "Teleporter" || item.unlockableName == "Signal translator" || item.unlockableName == "Loud horn" || item.unlockableName == "Inverse Teleporter")
            {
                if (Plugin.configKeepUpgrades.Value)
                    unlocks.Add(id);
            }
            else if (Plugin.configKeepFurniture.Value)
                unlocks.Add(id);
        }

        public static IEnumerator RepopulateShipWithUnlocks()
        {
            foreach (int id in unlocks)
                StartOfRound.Instance.BuyShipUnlockableServerRpc(id, 60);

            if (Plugin.configAutoStore.Value)
            {
                yield return new WaitForSeconds(2f);
                foreach (PlaceableShipObject placeableShipObject in Object.FindObjectsOfType<PlaceableShipObject>())
                {
                    if (unlocks.Contains(placeableShipObject.unlockableID) && StartOfRound.Instance.unlockablesList.unlockables[placeableShipObject.unlockableID].canBeStored)
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

            unlocks.Clear();
        }

        public static void ChangeSave()
        {
            unlocks.Clear();
        }
    }
}
