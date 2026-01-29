using UnityEngine;

namespace FG
{
    public class DurkCharacterManager : BossAICharacterManager
    {
        [HideInInspector] public DurkCombatManager durkCombatManager;
        [HideInInspector] public DurkSFXManager durkSFXManager;

        protected override void Awake()
        {
            base.Awake();

            durkCombatManager = GetComponent<DurkCombatManager>();
            durkSFXManager = GetComponent<DurkSFXManager>();
        }
    }
}
