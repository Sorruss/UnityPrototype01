using System.Linq;
using UnityEngine;

namespace FG
{
    public class DebugManager : MonoBehaviour
    {
        public static DebugManager instance;

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void Start()
        {
            DontDestroyOnLoad(gameObject);
        }

        // DETERMINATED VALUES
        public void DamageReceiveLog(float damageReceived)
        {
            Debug.Log($"[DAMAGE RECEIVED]: {damageReceived}");
        }

        public void DamageBlockedReceiveLog(float damageReceived)
        {
            Debug.Log($"[BLOCKED DAMAGE RECEIVED]: {damageReceived}");
        }

        public void StaminaDeductLog(float staminaDeducted)
        {
            Debug.Log($"[STAMINA DEDUCTED]: {staminaDeducted}");
        }

        // CODE DEBUG
        public void PointTriggeredLog(string pointName)
        {
            Debug.Log($"[POINT TRIGGERED]: {pointName}");
        }

        // NON-DETERMINATED VALUES
        public void ValueLog<T>(string valueName, T value)
        {
            Debug.Log($"[VALUE OUTPUT]: {valueName} = {value}");
        }
    }
}
