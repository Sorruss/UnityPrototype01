using UnityEngine;

namespace FG
{
    public class CharacterLocomotionManager : MonoBehaviour
    {
        private CharacterManager character;

        [Header("Vertical Velocity Stuff")]
        [SerializeField] protected float gravityForce = -5.0f;          // Additional force used when falling.
        [SerializeField] private float groundedForce = -10.0f;          // Force used when character is grounded so he sticks to the ground.
        [SerializeField] private float startingFallingForce = -5.0f;    // Force at which fall stars getting bigger with gravityForce.
        [SerializeField] private float fallingForceMultiplier = 5.0f;

        [Header("Flags")]
        public bool isGrounded = false;
        public bool canRotate = true;
        public bool canMove = true;

        // CALCULATION VARIABLES
        protected Vector3 yVelocity;
        private bool fallingVelocityHasBeenSet = false;
        private float inAirTime = 0.0f;
        
        // ANIMATOR STRINGS
        private int InAirTimeString;
        private int IsGroundedString;

        protected virtual void Awake()
        {
            
        }

        protected virtual void Start()
        {
            character = GetComponent<CharacterManager>();
            InAirTimeString = Animator.StringToHash("InAirTime");
            IsGroundedString = Animator.StringToHash("IsGrounded");
        }

        protected virtual void Update()
        {
            UpdateAnimatorParameters();
            UpdateVerticalVelocity();
            character.characterController.Move(yVelocity * Time.deltaTime);
        }

        private void UpdateVerticalVelocity()
        {
            if (character.characterLocomotionManager.isGrounded)    // Character is grounded
            {
                if (yVelocity.y < 0)    // and not trying to jump.
                {
                    inAirTime = 0.0f;
                    fallingVelocityHasBeenSet = false;
                    yVelocity.y = groundedForce;
                }
            }
            else    // Character is not grounded
            {
                if (!character.characterNetwork.networkIsJumping.Value && !fallingVelocityHasBeenSet)
                {
                    fallingVelocityHasBeenSet = true;
                    yVelocity.y = startingFallingForce;
                }

                inAirTime += Time.deltaTime;
                yVelocity.y += gravityForce * fallingForceMultiplier * Time.deltaTime;
            }
        }

        private void UpdateAnimatorParameters()
        {
            character.animator.SetBool(IsGroundedString, character.characterLocomotionManager.isGrounded);
            character.animator.SetFloat(InAirTimeString, inAirTime);
        }
    }
}
