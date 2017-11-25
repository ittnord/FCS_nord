using UnityEngine;

namespace FCS
{
    public class BattleHud : MonoBehaviour
    {
        [SerializeField]
        private Joystick _joystick;
        [SerializeField]
        private AbilitySlot _abilitySlotMain;
        [SerializeField]
        private AbilitySlot _abilitySlotAdd1;
        [SerializeField]
        private AbilitySlot _abilitySlotAdd2;
        [SerializeField]
        private AbilitySlot _abilitySlotAdd3;

        protected void Start()
        {
            // TODO: вытащить CharacterSetup
        }
    }
}