using UnityEngine;

namespace FG
{
    [System.Serializable]
    public class CharacterSaveData
    {
        public CharacterSaveData()
        {
            bossListAwakened = new SerializableDictionary<int, bool>();
            bossListDefeated = new SerializableDictionary<int, bool>();
        }

        // CHARACTER VALUES
        public string characterName = "Character";
        
        // STATS
        public int enduranceLevel;
        public int vitalityLevel;

        // RESOURCES
        public float currentStamina;
        public float currentHealth;

        // STATISTICS
        public float secondsPlayed;

        // POSITION
        public float positionX;
        public float positionY;
        public float positionZ;

        // BOSS LIST
        public SerializableDictionary<int, bool> bossListAwakened;  // INT - ID, BOOL - IF WAS AWAKENED
        public SerializableDictionary<int, bool> bossListDefeated;  // INT - ID, BOOL - IF WAS DEFEATED

        // BONFIRE LIST
        public SerializableDictionary<int, bool> bonfireList;       // INT - ID, BOOL - IF WAS ACTIVATED

        // EQUIPMENT
        public int weaponLeftHandID;
        public int weaponRightHandID;

        public void SavePosition(Vector3 position)
        {
            positionX = position.x;
            positionY = position.y;
            positionZ = position.z;
        }

        public Vector3 GetPosition()
        {
            return new Vector3(positionX, positionY, positionZ);
        }
    }
}
