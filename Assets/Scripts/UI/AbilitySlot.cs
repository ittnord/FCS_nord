using UnityEngine;

namespace FCS
{
    public class AbilitySlot : MonoBehaviour
    {
        public Abilities AbilityType;
        

        public void OnSlotClicked()
        {
            InputController.Instance.HandleAbility(GuiFactory.Instance.Instantiate(AbilityType));
        }
    }
}