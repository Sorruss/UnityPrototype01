using UnityEngine;

namespace FG
{
    public class StaticEffect : ScriptableObject
    {
        [Header("Config (auto)")]
        public int staticEffectID;

        public virtual void ApplyStaticEffect(CharacterManager character)
        {

        }

        public virtual void RemoveStaticEffect(CharacterManager character)
        {

        }
    }
}
