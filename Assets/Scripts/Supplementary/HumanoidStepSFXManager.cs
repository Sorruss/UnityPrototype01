using UnityEngine;

namespace FG
{
    public class HumanoidStepSFXManager : MonoBehaviour
    {
        CharacterManager character;

        [Header("Debug Values")]
        [SerializeField] private bool hasPlayedStepSFX = false;
        [SerializeField] private GameObject steppedOnObject = null;

        private void Awake()
        {
            character = GetComponentInParent<CharacterManager>();
        }

        private void FixedUpdate()
        {
            if (character == null)      // IF NO CHARACTER THEN WILL BE ERROR
                return;

            if (!character.characterNetwork.networkIsMoving.Value)  // IF NOT MOVING THEN WHY BOTHERING
                return;

            RaycastHit hit;     // LOOK IF LEG IS TOUCHING ANYTHING BELOW IT
            bool didStepOnSomething = Physics.Raycast(
                transform.position, 
                character.transform.TransformDirection(Vector3.down), 
                out hit,
                character.characterSFXManager.stepHeight, 
                UtilityManager.instance.GetEnvironmentMasks());

            if (didStepOnSomething && hasPlayedStepSFX) // WE ALREADY PLAYED STEP SFX ON THIS GROUND TOUCH
                return;

            if (!didStepOnSomething)                    // LEG IN THE AIR SO WE RESET VALUES
            {
                hasPlayedStepSFX = false;
                steppedOnObject = null;
                return;
            }

            if (didStepOnSomething && !hasPlayedStepSFX)// STEPPED ON SOMETHING AND DIDN'T PLAY SFX YET SO WE PLAY IT HERE
            {
                hasPlayedStepSFX = true;
                steppedOnObject = hit.transform.gameObject;
                PlayStepSoundFX();
            }
        }

        private void PlayStepSoundFX()
        {
            character.characterSFXManager.PlayStepSoundFX();
        }
    }
}
