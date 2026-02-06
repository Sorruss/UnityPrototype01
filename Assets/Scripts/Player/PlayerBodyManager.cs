using UnityEngine;
using System.Collections.Generic;

namespace FG
{
    public class PlayerBodyManager : MonoBehaviour
    {
        [HideInInspector] private PlayerManager player;

        [Header("Body Features - Hair")]
        [SerializeField] private GameObject BodyFeatureHair;

        [Header("Body Features - Male")]
        [SerializeField] private GameObject AllMaleParts;
        [SerializeField] private GameObject BodyFeatureMaleHead;
        [SerializeField] private GameObject[] MaleBody;
        [SerializeField] private GameObject[] MaleArms;
        [SerializeField] private GameObject[] MaleLegs;
        [SerializeField] private GameObject BodyFeatureMaleEyebrows;
        [SerializeField] private GameObject BodyFeatureFacialHair;

        [Header("Body Features - Female")]
        [SerializeField] private GameObject AllFemaleParts;
        [SerializeField] private GameObject BodyFeatureFemaleHead;
        [SerializeField] private GameObject[] FemaleBody;
        [SerializeField] private GameObject[] FemaleArms;
        [SerializeField] private GameObject[] FemaleLegs;
        [SerializeField] private GameObject BodyFeatureFemaleEyebrows;

        private void Awake()
        {
            player = GetComponent<PlayerManager>();
        }

        // -------------
        // COMMON METHODS
        public void SwapGender(bool isMale)
        {
            AllMaleParts.SetActive(isMale);
            AllFemaleParts.SetActive(!isMale);
            player.playerEquipmentManager.EquipFullArmor();
        }

        // --------------
        // ENABLE METHODS
        public void EnableHead()
        {
            // MALE HEAD
            BodyFeatureMaleHead.SetActive(true);

            // MALE EYEBROWS
            BodyFeatureMaleEyebrows.SetActive(true);

            // FEMALE HEAD
            BodyFeatureFemaleHead.SetActive(true);

            // FEMALE EYEBROWS
            BodyFeatureFemaleEyebrows.SetActive(true);

            EnableFacialHair();
        }

        public void EnableHair()
        {
            BodyFeatureHair.SetActive(true);
        }

        public void EnableFacialHair()
        {
            BodyFeatureFacialHair.SetActive(true);
        }

        public void EnableBody()
        {
            foreach (GameObject bodyPart in MaleBody)
                bodyPart.SetActive(true);

            foreach (GameObject bodyPart in FemaleBody)
                bodyPart.SetActive(true);
        }

        public void EnableHands()
        {
            foreach (GameObject bodyPart in MaleArms)
                bodyPart.SetActive(true);

            foreach (GameObject bodyPart in FemaleArms)
                bodyPart.SetActive(true);
        }

        public void EnableLegs()
        {
            foreach (GameObject bodyPart in MaleLegs)
                bodyPart.SetActive(true);

            foreach (GameObject bodyPart in FemaleLegs)
                bodyPart.SetActive(true);
        }

        // --------------
        // DISABLE METHODS
        public void DisableHead()
        {
            // MALE HEAD
            BodyFeatureMaleHead.SetActive(false);

            // MALE EYEBROWS
            BodyFeatureMaleEyebrows.SetActive(false);

            // FEMALE HEAD
            BodyFeatureFemaleHead.SetActive(false);

            // FEMALE EYEBROWS
            BodyFeatureFemaleEyebrows.SetActive(false);

            DisableFacialHair();
        }

        public void DisableHair()
        {
            BodyFeatureHair.SetActive(false);
        }

        public void DisableFacialHair()
        {
            BodyFeatureFacialHair.SetActive(false);
        }

        public void DisableBody()
        {
            foreach (GameObject bodyPart in MaleBody)
                bodyPart.SetActive(false);

            foreach (GameObject bodyPart in FemaleBody)
                bodyPart.SetActive(false);
        }

        public void DisableHands()
        {
            foreach (GameObject bodyPart in MaleArms)
                bodyPart.SetActive(false);

            foreach (GameObject bodyPart in FemaleArms)
                bodyPart.SetActive(false);
        }

        public void DisableLegs()
        {
            foreach (GameObject bodyPart in MaleLegs)
                bodyPart.SetActive(false);

            foreach (GameObject bodyPart in FemaleLegs)
                bodyPart.SetActive(false);
        }
    }
}
