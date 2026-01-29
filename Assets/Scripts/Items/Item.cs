using UnityEngine;

namespace FG
{
    public class Item : ScriptableObject
    {
        [Header("Basic")]
        public string Name;
        [TextArea] public string Description;
        public int ID;
        public Sprite Icon;
    }
}
