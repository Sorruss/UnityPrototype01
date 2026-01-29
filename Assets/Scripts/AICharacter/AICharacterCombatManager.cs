using UnityEngine;

namespace FG
{
    public class AICharacterCombatManager : CharacterCombatManager
    {
        [Header("Target Information")]
        public Vector3 targetDirection;
        public float angleToTarget;
        public float distanceToTarget;

        [Header("Detection")]
        [SerializeField] float viewDetectionRadius = 5.0f;
        [SerializeField] private float viewMinAngle = -35.0f;
        [SerializeField] private float viewMaxAngle = 35.0f;

        [Header("Combat Radius")]
        public float combatStateRadius = 5.0f;

        [Header("Action Recovery Time")]
        public float actionRecoveryTime = 0.0f;

        [Header("Pivoting")]
        [SerializeField] public bool doPivot = false;

        public void TryToGetATarget(AICharacterManager aiCharacter)
        {
            if (currentTarget != null)
                return;

            Collider[] colliders = Physics.OverlapSphere(
                aiCharacter.transform.position, viewDetectionRadius, UtilityManager.instance.GetCharacterMasks());

            foreach (var collider in colliders)
            {
                CharacterManager target = collider.GetComponent<CharacterManager>();

                if (target == null)     // VALIDITY CHECK
                    continue;

                if (target.characterNetwork.networkIsDead.Value)    // IS DEAD CHECK
                    continue;

                if (target == aiCharacter)  // SELF-TARGET PREVENTION
                    continue;

                if (!UtilityManager.instance.CanCharacterAttackThisTargetTeam(aiCharacter.characterTeam, target.characterTeam)) // SAME GROUP DAMAGE PREVENTION
                    continue;

                Vector3 targetDirection = target.transform.position - aiCharacter.transform.position;
                float viewAngle = Vector3.Angle(aiCharacter.transform.forward, targetDirection);
                if (viewAngle < viewMinAngle || viewAngle > viewMaxAngle)   // BESIDES VIEWABLE ANGLE PREVENTION
                    continue;

                if (Physics.Linecast(   // IS TARGET BEHIND AN ENVIRONMENT CHECK
                    aiCharacter.aiCombatManager.lockOnTransform.position,
                    target.characterCombatManager.lockOnTransform.position,
                    UtilityManager.instance.GetEnvironmentMasks()))
                    continue;

                SetCurrentTarget(target);
            }
        }

        public void HandleReducingRecoveryTime(AICharacterManager aiCharacter)
        {
            if (aiCharacter.isPerfomingAction)
                return;

            if (actionRecoveryTime > 0.0f)
                actionRecoveryTime -= Time.deltaTime;
        }

        public bool IsAngleOutOfFOV(float angle)
        {
            return angle > viewMaxAngle || angle < viewMinAngle;
        }
    }
}
