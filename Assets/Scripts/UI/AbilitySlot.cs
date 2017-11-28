using FCS.Abilities;
using UnityEngine;
using UnityEngine.UI;

namespace FCS
{
    public class AbilitySlot : MonoBehaviour
    {
        [SerializeField]
        private Image _image;

        public AbilityType AbilityType;
        private bool _canCast = true;

        public void Init(Sprite ability)
        {
            _image.sprite = ability;
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

        private void OnCdBegin(AbilityType abilityTypeType)
        {
            if (AbilityType != abilityTypeType)
            {
                return;
            }
            _canCast = false;
        }

        private void OnCdEnd(AbilityType abilityTypeType)
        {
            if (AbilityType != abilityTypeType)
            {
                return;
            }
            _canCast = true;
        }

        private void OnCdChanged(AbilityType abilityTypeType, float value)
        {
            if (AbilityType != abilityTypeType)
            {
                return;
            }

            _image.fillAmount = value;
        }
    }
}