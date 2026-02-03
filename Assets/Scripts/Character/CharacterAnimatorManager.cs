using UnityEngine;
using Unity.Netcode;
using System.Collections.Generic;

namespace FG
{
    public class CharacterAnimatorManager : MonoBehaviour
    {
        protected CharacterManager character;

        [Header("Motion Inputs")]
        private int Horizontal;
        private int Vertical;

        [Header("Flags")]
        [HideInInspector] public bool applyRootMotion = false;

        [Header("Smooth Time")]
        [SerializeField] private float locomotionSmoothTime = 0.1f;
        [SerializeField] public float actionSmoothTime = 0.2f;

        [HideInInspector] public string lastUsedDamageAnimation;

        [Header("Damage Animations - Medium")]
        [SerializeField] private string hit_front_medium_01 = "hit_front_medium_01";
        [SerializeField] private string hit_front_medium_02 = "hit_front_medium_02";
        [SerializeField] private string hit_back_medium_01 = "hit_back_medium_01";
        [SerializeField] private string hit_back_medium_02 = "hit_back_medium_02";
        [SerializeField] private string hit_left_medium_01 = "hit_left_medium_01";
        [SerializeField] private string hit_left_medium_02 = "hit_left_medium_02";
        [SerializeField] private string hit_right_medium_01 = "hit_right_medium_01";
        [SerializeField] private string hit_right_medium_02 = "hit_right_medium_02";

        [HideInInspector] public List<string> HitFrontMedium = new List<string>();
        [HideInInspector] public List<string> HitBackMedium = new List<string>();
        [HideInInspector] public List<string> HitLeftMedium = new List<string>();
        [HideInInspector] public List<string> HitRightMedium = new List<string>();

        [Header("Damage Animations - Ping")]
        [SerializeField] private string hit_front_ping_01 = "hit_front_ping_01";
        [SerializeField] private string hit_front_ping_02 = "hit_front_ping_02";
        [SerializeField] private string hit_back_ping_01 = "hit_back_ping_01";
        [SerializeField] private string hit_back_ping_02 = "hit_back_ping_02";
        [SerializeField] private string hit_left_ping_01 = "hit_left_ping_01";
        [SerializeField] private string hit_left_ping_02 = "hit_left_ping_02";
        [SerializeField] private string hit_right_ping_01 = "hit_right_ping_01";
        [SerializeField] private string hit_right_ping_02 = "hit_right_ping_02";

        [HideInInspector] public List<string> HitFrontPing = new List<string>();
        [HideInInspector] public List<string> HitBackPing = new List<string>();
        [HideInInspector] public List<string> HitLeftPing = new List<string>();
        [HideInInspector] public List<string> HitRightPing = new List<string>();

        protected virtual void Awake()
        {
            character = GetComponent<CharacterManager>();

            Horizontal = Animator.StringToHash("Horizontal");
            Vertical = Animator.StringToHash("Vertical");
        }

        protected virtual void Start()
        {
            // Damage Animations - Medium
            HitFrontMedium.Add(hit_front_medium_01);
            HitFrontMedium.Add(hit_front_medium_02);
            HitBackMedium.Add(hit_back_medium_01);
            HitBackMedium.Add(hit_back_medium_02);
            HitLeftMedium.Add(hit_left_medium_01);
            HitLeftMedium.Add(hit_left_medium_02);
            HitRightMedium.Add(hit_right_medium_01);
            HitRightMedium.Add(hit_right_medium_02);

            // Damage Animations - Ping
            HitFrontPing.Add(hit_front_ping_01);
            HitFrontPing.Add(hit_front_ping_02);
            HitBackPing.Add(hit_back_ping_01);
            HitBackPing.Add(hit_back_ping_02);
            HitLeftPing.Add(hit_left_ping_01);
            HitLeftPing.Add(hit_left_ping_02);
            HitRightPing.Add(hit_right_ping_01);
            HitRightPing.Add(hit_right_ping_02);
        }

        public void UpdateMovementValues(float horizontal, float vertical, bool isSprinting = false, bool isWalking = false)
        {
            #region SNAPPING_VALUES
            // SNAPPING VALUES
            if (horizontal < 0.0f && horizontal >= -0.5f)
            {   // LEFT WALKING
                horizontal = -0.5f;
            }
            else if (horizontal < -0.5f && horizontal >= -1.0f)
            {   // LEFT RUNNING
                horizontal = -1.0f;
            }
            else if (horizontal > 0.0f && horizontal <= 0.5f)
            {   // RIGHT WALKING
                horizontal = 0.5f;
            }
            else if (horizontal > 0.5f && horizontal <= 1.0f)
            {   // RIGHT RUNNING
                horizontal = 1.0f;
            }

            if (vertical > 0.0f && vertical <= 0.5f)
            {   // FORWARD WALKING
                vertical = 0.5f;
            }
            else if (vertical > 0.5f && vertical <= 1.0f)
            {   // FORWARD RUNNING
                vertical = 1.0f;
            }
            else if (vertical < 0.0f && vertical >= -0.5f)
            {   // BACKWARDS WALKING
                vertical = -0.5f;
            }
            else if (vertical < -0.5f && vertical >= -1.0f)
            {   // BACKWARDS RUNNING
                vertical = -1.0f;
            }
            #endregion SNAPPING_VALUES

            if (isSprinting)
                vertical = 2.0f;

            if (isWalking)
                vertical = 0.5f;

            character.animator.SetFloat(Horizontal, horizontal, locomotionSmoothTime, Time.deltaTime);
            character.animator.SetFloat(Vertical, vertical, locomotionSmoothTime, Time.deltaTime);
        }

        // -------------
        // SUPPLEMENTARY
        public string GetNextRandomDamageAnimationFromList(ref List<string> list)
        {
            // COPY LIST
            List<string> listCopy = new List<string>();
            foreach (string item in list)
                listCopy.Add(item);

            // REMOVE MOST RECENT USED ANIMATION
            listCopy.Remove(lastUsedDamageAnimation);

            // DO LIST CLEANUP
            for (int i = listCopy.Count - 1; i >= 0; --i)
                if (listCopy[i] == null)
                    listCopy.RemoveAt(i);

            // RETURN RANDOM ANIMATION
            int randomNumber = Random.Range(0, listCopy.Count);
            return listCopy[randomNumber];
        }

        public void UpdateAnimatorOverrider(AnimatorOverrideController animatorOverrider)
        {
            character.animator.runtimeAnimatorController = animatorOverrider;
        }

        // ----------------
        // NO RPC ANIMATION
        public void PerformTargetAnimationAction(
            string actionName,
            bool isPerformingAction,
            bool applyRootMotion = true,
            bool canRotate = false,
            bool canMove = false)
        {
            character.isPerfomingAction = isPerformingAction;
            character.characterLocomotionManager.canRotate = canRotate;
            character.characterLocomotionManager.canMove = canMove;

            character.characterAnimatorManager.applyRootMotion = applyRootMotion;
            character.animator.CrossFade(actionName, actionSmoothTime);
        }

        // ----------------------------------
        // RPC CONNECTED ANIMATION PERFORMERS
        public void PerformAnimationAction(
            string actionName,
            bool isPerformingAction,
            bool applyRootMotion = true,
            bool canRotate = false,
            bool canMove = false)
        {
            character.isPerfomingAction = isPerformingAction;
            character.characterLocomotionManager.canRotate = canRotate;
            character.characterLocomotionManager.canMove = canMove;

            character.characterAnimatorManager.applyRootMotion = applyRootMotion;
            character.animator.CrossFade(actionName, actionSmoothTime);

            character.characterNetwork.NotifyServerOfAnimatorActionServerRpc(
                NetworkManager.Singleton.LocalClientId, 
                actionName, 
                applyRootMotion);
        }

        public void PerformAttackAnimationAction(
            string actionName,
            bool isPerformingAction,
            WeaponMeleeAttackType attackType,
            bool applyRootMotion = true,
            bool canRotate = false,
            bool canMove = false)
        {
            // 1. Decrease stamina
            // 2. Keep track of attack type (light, heavy, etc) to determine if it's parrible
            // 3. Keep track of current attack for combo logic
            character.characterCombatManager.currentAttackTypeBeingUsed = attackType;
            character.characterCombatManager.lastAttackAnimationPerfomed = actionName;
            
            //character.characterCombatManager.UpdateStaminaNeededForCurrentMove();
            //if (character.characterCombatManager.staminaNeededForCurrentAction > character.characterNetwork.networkCurrentStamina.Value)
            //    return;

            character.isPerfomingAction = isPerformingAction;
            character.characterLocomotionManager.canRotate = canRotate;
            character.characterLocomotionManager.canMove = canMove;

            character.characterAnimatorManager.applyRootMotion = applyRootMotion;
            character.animator.CrossFade(actionName, actionSmoothTime);

            character.characterNetwork.NotifyServerOfAnimatorAttackActionServerRpc(
                NetworkManager.Singleton.LocalClientId,
                actionName,
                applyRootMotion);
        }
    }
}
