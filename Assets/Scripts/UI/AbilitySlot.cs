using UnityEngine;

namespace FCS
{
    public class AbilitySlot : MonoBehaviour
    {
        public void OnSlotClicked()
        {
            InputController.Instance.HandleAbility();
        }
    }
}