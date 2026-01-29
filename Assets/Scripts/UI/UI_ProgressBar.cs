using UnityEngine;
using UnityEngine.UI;

namespace FG
{
    public class UI_ProgressBar : MonoBehaviour
    {
        [SerializeField] private bool scaleWidthWithMaxValue = true;
        [SerializeField] private float widthScaleMultiplier = 1.0f;

        private Slider slider;
        private RectTransform rect;

        protected virtual void Awake()
        {
            slider = GetComponent<Slider>();
            rect = GetComponent<RectTransform>();
        }

        protected virtual void Start()
        {
            
        }

        protected virtual void Update()
        {
            
        }

        protected virtual void OnDisable()
        {

        }

        protected virtual void OnDestroy()
        {
            
        }

        public virtual void SetNewValue(float value)
        {
            slider.value = value;
        }

        public virtual void SetMaxValue(int maxValue)
        {
            slider.maxValue = maxValue;

            if (scaleWidthWithMaxValue)
            {
                rect.sizeDelta = new Vector2(maxValue * widthScaleMultiplier, rect.sizeDelta.y);
                PlayerUIManager.instance.hudManager.RefreshUI();
            }
        }
    }
}
