using UnityEngine;

namespace FG
{
    public class InstantEffect : ScriptableObject
    {
        [HideInInspector] public int instantEffectID;

        public virtual void ApplyInstantEffect(ref CharacterManager character)
        {

        }
    }
}
