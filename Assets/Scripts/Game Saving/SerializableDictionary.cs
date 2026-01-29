using System.Collections.Generic;
using UnityEngine;

namespace FG
{
    [System.Serializable]
    public class SerializableDictionary<TKey, TValue> : Dictionary<TKey, TValue>, ISerializationCallbackReceiver
    {
        [SerializeField] private List<TKey> keys = new List<TKey>();
        [SerializeField] private List<TValue> values = new List<TValue>();

        public void OnBeforeSerialize()     // C# -> JSON
        {
            // CLEAR
            keys.Clear();
            values.Clear();

            // FILL
            foreach (KeyValuePair<TKey, TValue> pair in this)
            {
                keys.Add(pair.Key);
                values.Add(pair.Value);
            }
        }

        public void OnAfterDeserialize()    // JSON -> C#
        {
            // CLEAR
            Clear();

            // CHECK
            if (keys.Count != values.Count)
            {
                Debug.LogError("keys.Count != values.Count");
                return;
            }

            // FILL
            for (int i = 0; i < keys.Count; ++i)
            {
                Add(keys[i], values[i]);
            }
        }

        public void DebugLog(string dictName = "")
        {
            Debug.Log($"---{dictName}---");
            foreach (KeyValuePair<TKey, TValue> pair in this)
                Debug.Log($"key: {pair.Key}, value: {pair.Value}");
        }
    }
}
