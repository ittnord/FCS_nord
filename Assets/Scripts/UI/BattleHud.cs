using UnityEngine;

namespace FCS
{
    public class BattleHud : MonoBehaviour
    {
        [SerializeField]
        private Joystick _joystick;
        [SerializeField]
        private RectTransform _abilitiesLayout;
        [SerializeField]
        private AbilitySlot _abilitySlotPrefab;

        protected void Start()
        {
            Instantiate(_abilitySlotPrefab, _abilitiesLayout, false);
        }
    }
}