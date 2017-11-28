using System.Linq;
using FCS.Abilities;
using FCS.Managers;
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
            var abilities = AbilitiesStorage.Instance.SelectedAbilities;
            _abilitySlotMain.AbilityType = abilities[0];
            _abilitySlotMain.Init(GuiFactory.Instance.GetSprite(abilities[0]));

            _abilitySlotAdd1.AbilityType = abilities[1];
            _abilitySlotAdd1.Init(GuiFactory.Instance.GetSprite(abilities[1]));

            _abilitySlotAdd2.AbilityType = abilities[2];
            _abilitySlotAdd2.Init(GuiFactory.Instance.GetSprite(abilities[2]));

            _abilitySlotAdd3.AbilityType = abilities[3];
            _abilitySlotAdd3.Init(GuiFactory.Instance.GetSprite(abilities[3]));

        }
    }
}