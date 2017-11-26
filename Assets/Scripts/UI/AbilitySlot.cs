using UnityEngine;
using UnityEngine.UI;

namespace FCS
{
    public class AbilitySlot : MonoBehaviour
    {
        [SerializeField]
        private Image _image;

        public Abilities AbilityType;

        public void Init(Sprite ability)
        {
            _image.sprite = ability;
            //_image.SetNativeSize();
        }

        public void OnSlotClicked()
        {
            InputController.Instance.HandleAbility(AbilityType);
        }
    }
}