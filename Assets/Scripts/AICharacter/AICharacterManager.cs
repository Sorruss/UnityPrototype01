using UnityEngine;
using UnityEngine.AI;

namespace FG
{
    public class AICharacterManager : CharacterManager
    {
        // MANAGERS
        [HideInInspector] public AICharacterCombatManager aiCombatManager;
        [HideInInspector] public AICharacterLocomotionManager aiCharacterLocomotionManager;
        [HideInInspector] public AICharacterAnimatorManager aiCharacterAnimatorManager;

        [Header("Config")]
        public string aiCharacterName;

        [Header("State Machine (Auto)")]
        public AIState currentState;

        [Header("State Machine (Hand)")]
        public AIStateIdle idleState;
        public AIStatePursue pursueTargetState;
        public AIStateCombatStance combatStanceState;
        public AIStateAttack attackState;

        // COMPONENTS
        [HideInInspector] public NavMeshAgent navMeshAgent;

        // ------------
        // UNITY EVENTS
        protected override void Awake()
        {
            base.Awake();

            navMeshAgent = GetComponentInChildren<NavMeshAgent>();

            // COMPONENTS INITIALIZATION
            aiCombatManager = GetComponent<AICharacterCombatManager>();
            aiCharacterLocomotionManager = GetComponent<AICharacterLocomotionManager>();
            aiCharacterAnimatorManager = GetComponent<AICharacterAnimatorManager>();
        }

        protected override void Update()
        {
            base.Update();

            if (characterNetwork.networkIsDead.Value)
                return;

            aiCombatManager.HandleReducingRecoveryTime(this);
        }

        protected override void FixedUpdate()
        {
            base.FixedUpdate();

            if (IsOwner)
                HandleAIState();
        }

        // ------------
        // NETWORK EVENTS
        public override void OnNetworkSpawn()
        {
            base.OnNetworkSpawn();

            // HEALTH BAR
            characterNetwork.networkCurrentHealth.OnValueChanged += characterUIManager.UpdateHealthBar;

            if (!IsServer)
                return;

            // HEALTH
            CheckHealth(0.0f, characterNetwork.networkCurrentHealth.Value);
            characterNetwork.networkCurrentHealth.OnValueChanged += CheckHealth;

            // STATE MACHINE STARTING CONFIGURATION
            idleState = Instantiate(idleState);
            pursueTargetState = Instantiate(pursueTargetState);
            combatStanceState = Instantiate(combatStanceState);
            attackState = Instantiate(attackState);

            currentState = idleState;
        }

        public override void OnNetworkDespawn()
        {
            base.OnNetworkDespawn();

            characterNetwork.networkCurrentHealth.OnValueChanged -= characterUIManager.UpdateHealthBar;

            if (!IsServer)
                return;

            characterNetwork.networkCurrentHealth.OnValueChanged -= CheckHealth;
        }

        // -----
        // OTHER
        protected virtual void HandleAIState()
        {
            if (characterNetwork.networkIsDead.Value)
                return;

            // UPDATE CURRENT STATE
            AIState nextState = currentState?.Tick(this);
            if (nextState != null)
                currentState = nextState;

            // RESET THE TRANSFORM VALUES OF AN AGENT SO IT KEEPS ON TOP OF THE AI_CHARACTER (POSITION)
            // AND SO IT ROTATES STRICTLY TOWARDS THE TARGET (ROTATION)
            // IT'S IMPORTANT THAT THIS CODE IS PLACED HERE AFTER THE ROTATION WAS ASSIGNED IN 'TICK'
            navMeshAgent.transform.localPosition = Vector3.zero;
            navMeshAgent.transform.localRotation = Quaternion.identity;

            // CALCULATE NEEEDED VARIABLES IF WE HAVE A TARGET
            if (aiCombatManager.currentTarget != null)
            {
                aiCombatManager.targetDirection = aiCombatManager.currentTarget.transform.position - transform.position;
                aiCombatManager.angleToTarget = Vector3.SignedAngle(transform.forward, aiCombatManager.targetDirection, Vector3.up);
                aiCombatManager.distanceToTarget = Vector3.Distance(aiCombatManager.currentTarget.transform.position, transform.position);
            }

            // MAKE AI_CHARACTER MOVE WITH ROOT MOTION IF HE'S STILL NOT NEAR THE TARGET
            // MAKE AI_CHARACTER STOP IF HE'S NEAR THE TARGETS
            if (currentState == pursueTargetState || currentState == combatStanceState)
                characterNetwork.networkIsMoving.Value = aiCombatManager.distanceToTarget > navMeshAgent.stoppingDistance;
            else
                characterNetwork.networkIsMoving.Value = false;
        }
    }
}
