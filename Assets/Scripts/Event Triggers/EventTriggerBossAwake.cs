using Unity.Netcode;
using UnityEngine;

namespace FG
{
    public class EventTriggerBossAwake : MonoBehaviour
    {
        [Header("Config")]
        [SerializeField] private BossID bossToAwake;

        private void OnTriggerEnter(Collider other)
        {
            if (!NetworkManager.Singleton.IsServer)
                return;

            BossAICharacterManager boss = AIManager.instance.GetSpawnedBossByID(bossToAwake);
            boss.AwakeBoss();
            gameObject.SetActive(false);
        }
    }
}
