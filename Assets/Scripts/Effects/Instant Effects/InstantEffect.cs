using UnityEngine;

namespace FG
{
    public class InstantEffect : ScriptableObject
    {
        [HideInInspector] public int instantEffectID;

        public virtual void ApplyEffect(ref CharacterManager character)
        {

        }
    }
}
