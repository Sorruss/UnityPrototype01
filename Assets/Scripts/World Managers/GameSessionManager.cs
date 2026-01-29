using UnityEngine;
using System.Collections.Generic;

namespace FG
{
    public class GameSessionManager : MonoBehaviour
    {
        [HideInInspector] public static GameSessionManager instance;

        [SerializeField] public List<PlayerManager> players = new List<PlayerManager>();

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

            DontDestroyOnLoad(gameObject);
        }

        public void AddPlayerToList(PlayerManager player)
        {
            if (!players.Contains(player))
            {
                players.Add(player);
            }

            CleanupPlayersList();
        }

        public void RemovePlayerFromList(PlayerManager player)
        {
            if (players.Contains(player))
            {
                players.Remove(player);
            }

            CleanupPlayersList();
        }

        private void CleanupPlayersList()
        {
            for (int i = players.Count - 1; i >= 0; --i)
            {
                if (players[i] == null)
                {
                    players.RemoveAt(i);
                }
            }
        }
    }
}
