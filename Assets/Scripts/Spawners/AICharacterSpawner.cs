using UnityEngine;

namespace FG
{
    public class AICharacterSpawner : MonoBehaviour
    {
        [SerializeField] public GameObject prefubToSpawn;

        private void Start()
        {
            AIManager.instance.TryToSpawnAICharacter(this);
            gameObject.SetActive(false);
        }
    }
}
