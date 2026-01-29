using UnityEngine;

namespace FG
{
    public class EventTriggerBossAwake : MonoBehaviour
    {
        [Header("Config")]
        [SerializeField] private BossID bossToAwake;

        private void OnTriggerEnter(Collider other)
        {
            BossAICharacterManager boss = AIManager.instance.GetSpawnedBossByID(bossToAwake);
            boss.AwakeBoss();
            gameObject.SetActive(false);
        }
    }
}
