using UnityEngine;
using System.Collections.Generic;
using Unity.Netcode;

namespace FG
{
    public class AIManager : MonoBehaviour
    {
        [HideInInspector] static public AIManager instance;

        [Header("AI Characters List")]
        [SerializeField] private List<AICharacterSpawner> characterSpawners = new List<AICharacterSpawner>();
        [SerializeField] private List<AICharacterManager> spawnedAICharacters = new List<AICharacterManager>();
        [SerializeField] private List<BossAICharacterManager> spawnedAIBosses = new List<BossAICharacterManager>();

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

        private void Start()
        {
            DontDestroyOnLoad(gameObject);
        }

        public GameObject TryToSpawnAICharacter(AICharacterSpawner aiSpawner)
        {
            if (!NetworkManager.Singleton.IsServer)
                return null;
            
            GameObject spawnedCharacter = Instantiate(aiSpawner.prefubToSpawn);
            spawnedCharacter.GetComponent<NetworkObject>().Spawn();
            spawnedCharacter.transform.position = aiSpawner.transform.position;
            spawnedCharacter.transform.rotation = aiSpawner.transform.rotation;

            AddCharacterSpawnerToList(aiSpawner);
            AddCharacterSpawnedToList(spawnedCharacter.GetComponent<AICharacterManager>());

            return spawnedCharacter;
        }

        public void AddCharacterSpawnerToList(AICharacterSpawner aiSpawner)
        {
            if (characterSpawners.Contains(aiSpawner))
                return;

            characterSpawners.Add(aiSpawner);
        }

        public void AddCharacterSpawnedToList(AICharacterManager aiCharacter)
        {
            if (spawnedAICharacters.Contains(aiCharacter))
                return;
            
            spawnedAICharacters.Add(aiCharacter);

            // SEPERATING BOSSES
            BossAICharacterManager boss = aiCharacter as BossAICharacterManager;
            if (boss == null)
                return;

            if (spawnedAIBosses.Contains(boss))
                return;

            spawnedAIBosses.Add(boss);
        }

        public BossAICharacterManager GetSpawnedBossByID(BossID ID)
        {
            return spawnedAIBosses.Find(boss => boss.bossID == ID);
        }

        private void DespawnAICharacters()
        {
            foreach (var character in spawnedAICharacters)
            {
                character.GetComponent<NetworkObject>().Despawn();
            }
        
            spawnedAICharacters.Clear();
            spawnedAIBosses.Clear();
        }

        public void RespawnAICharacters()
        {
            DespawnAICharacters();
            
            foreach (var spawner in characterSpawners)
            {
                TryToSpawnAICharacter(spawner);
            }
        }
    }
}
