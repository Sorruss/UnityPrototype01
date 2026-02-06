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

        // CHARACTER INFORMATION
        public string characterName = "Character";
        public bool isMale = true;
        
        // STATS
        public int enduranceLevel;
        public int vitalityLevel;
        public int strengthLevel;

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

        // WEAPON QUICK SLOTS
        public int leftHandWeaponID_01;
        public int leftHandWeaponID_02;
        public int leftHandWeaponID_03;
        public int rightHandWeaponID_01;
        public int rightHandWeaponID_02;
        public int rightHandWeaponID_03;

        // EQUIPPED WEAPONS INDEX IN QUICK SLOT
        public int leftHandWeaponQuickSlotIndex;
        public int rightHandWeaponQuickSlotIndex;

        // ARMOR
        public int headArmorID;
        public int chestArmorID;
        public int handArmorID;
        public int legArmorID;

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
