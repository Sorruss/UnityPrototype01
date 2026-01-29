using System.Collections.Generic;
using UnityEngine;

namespace FG
{
    public class DurkStompDamageCollider : DamageCollider
    {
        DurkCharacterManager durk;

        protected override void Awake()
        {
            base.Awake();

            durk = GetComponentInParent<DurkCharacterManager>();
        }

        public void Trigger()
        {
            // ---------
            // VFX LOGIC
            if (durk.durkCombatManager.stompVFX != null)
                Instantiate(durk.durkCombatManager.stompVFX, transform);

            // ------------
            // DAMAGE LOGIC
            Collider[] colliders = Physics.OverlapSphere(
                transform.position, 
                durk.durkCombatManager.stompRadius, 
                UtilityManager.instance.GetCharacterMasks());
            List<ulong> damagedCharacters = new List<ulong>();

            foreach (var collider in colliders)
            {
                CharacterManager character = collider.GetComponent<CharacterManager>();

                // FILTRING FOR THE CHARACTER TO DAMAGE
                if (character == null)
                    continue;   // IF IT'S NULL

                ulong characterID = character.NetworkObjectId;

                if (characterID == durk.NetworkObjectId)
                    continue;   // IF IT'S THE DAMAGE_CAUSER ITSELF

                if (damagedCharacters.Contains(characterID))
                    continue;   // IF WE ALREADY DAMAGED THIS CHARACTER

                if (!character.IsOwner)
                    continue;   // THIS MAKES SO DAMAGE TO THE CHARACTER APPLIES ONLY IF THEY GOT HIT ON THEIR END
                                // CAUSE IF THEY WOULD GET HIT ON OUR END BUT NOT ON THEIR BUT GOT DAMAGED ANYWAY
                                // THAT WOULD BE FRUSTRATING, WOUDN'T IT

                // DAMAGING THE CHARACTER
                TakeHealthDamageEffect effect = Instantiate(EffectsManager.instance.healthDamageEffect);
                effect.physicalDamage = durk.durkCombatManager.baseDamage * durk.durkCombatManager.attack_stomp_modifier;
                effect.poiseDamage = durk.durkCombatManager.stompPoiseDamage;
                effect.damageCauser = durk;
                character.characterEffectsManager.ApplyInstantEffect(effect);

                // TO NOT DAMAGE THIS CHARACTER AGAIN
                damagedCharacters.Add(characterID);
            }
        }
    }
}
