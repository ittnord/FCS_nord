using UnityEngine;
using UnityEngine.UI;

namespace FCS
{
    public class AbilitySlot : MonoBehaviour
    {
        [SerializeField]
        private Image _image;

        public Abilities AbilityType;
        private bool _canCast = true;

        public void Init(Sprite ability)
        {
            _image.sprite = ability;
            //_image.SetNativeSize();

            var asd = AbilitiesStorage.Instance;
            asd.OnCdBegin += OnCdBegin;
            asd.OnCdEnd += OnCdEnd;
            asd.OnCdChanged += OnCdChanged;
        }

        public void OnDestroy()
        {
            var asd = AbilitiesStorage.Instance;
            asd.OnCdBegin -= OnCdBegin;
            asd.OnCdEnd -= OnCdEnd;
            asd.OnCdChanged -= OnCdChanged;
        }

        public void OnSlotClicked()
        {
            if (!_canCast)
            {
                return;
            }

            InputController.Instance.HandleAbility(AbilityType);
        }

        private void OnCdBegin(Abilities abilityType)
        {
            if (AbilityType != abilityType)
            {
                return;
            }
            _canCast = false;
        }

        private void OnCdEnd(Abilities abilityType)
        {
            if (AbilityType != abilityType)
            {
                return;
            }
            _canCast = true;
        }

        private void OnCdChanged(Abilities abilityType, float value)
        {
            if (AbilityType != abilityType)
            {
                return;
            }

            _image.fillAmount = value;
        }
    }
}