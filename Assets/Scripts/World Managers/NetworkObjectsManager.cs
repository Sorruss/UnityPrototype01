using UnityEngine;
using System.Collections.Generic;
using Unity.Netcode;

namespace FG
{
    public class NetworkObjectsManager : MonoBehaviour
    {
        [HideInInspector] public static NetworkObjectsManager instance;

        [Header("Fog Walls")]
        public List<InteractableFogWall> FogWalls = new List<InteractableFogWall>();

        private void Awake()
        {
            if (instance != null)
            {
                Destroy(gameObject);
            }
            else
            {
                instance = this;
            }
        }

        public GameObject TryToSpawnObject(NetworkObjectSpawner networkObjectSpawner)
        {
            if (!NetworkManager.Singleton.IsServer)
                return null;

            GameObject spawnedObject = Instantiate(networkObjectSpawner.prefubToSpawn);
            spawnedObject.transform.position = networkObjectSpawner.transform.position;
            spawnedObject.transform.rotation = networkObjectSpawner.transform.rotation;
            spawnedObject.GetComponent<NetworkObject>().Spawn();

            return spawnedObject;
        }

        // ---------
        // FOG WALLS
        public void AddFogWall(InteractableFogWall fogWall)
        {
            if (FogWalls.Contains(fogWall))
                return;

            FogWalls.Add(fogWall);
        }

        public void RemoveFogWall(InteractableFogWall fogWall)
        {
            if (!FogWalls.Contains(fogWall))
                return;

            FogWalls.Remove(fogWall);
        }
    }
}
