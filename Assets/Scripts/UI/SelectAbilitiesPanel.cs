using System.Collections;
using System.Collections.Generic;
using FCS;
using FCS.Abilities;
using UnityEngine;
using UnityEngine.UI;

namespace FCS
{
    public class SelectAbilitiesPanel : MonoBehaviour
    {
        [SerializeField] private Button _explosionButton;
        [SerializeField] private Button _fireballButton;
        [SerializeField] private Button _forcePushButton;
        [SerializeField] private Button _hookButton;
        [SerializeField] private Button _invisibilityButton;
        [SerializeField] private Button _lifeDrainButton;
        [SerializeField] private Button _shieldButton;
        [SerializeField] private Button _swapButton;
        [SerializeField] private Button _teleportButton;
        

        private Color usedColor = Color.cyan;
        private Color unusedColor = Color.gray;

        protected void Awake()
        {
            SetSelected(_explosionButton, false);
            SetSelected(_fireballButton, false);
            SetSelected(_forcePushButton, false);
            SetSelected(_hookButton, false);
            SetSelected(_invisibilityButton, false);
            SetSelected(_lifeDrainButton, false);
            SetSelected(_shieldButton, false);
            SetSelected(_swapButton, false);
            SetSelected(_teleportButton, false);
        }

        private void ChangeState(Button button, AbilityType abilityType)
        {
            var selectedAbilities = AbilitiesStorage.Instance.SelectedAbilities;
            AbilitiesStorage.Instance.ChangeState(abilityType);
            if (selectedAbilities.Contains(abilityType))
            {
                SetSelected(button, false);
            }
            else
            {
                if (selectedAbilities.Count >= 4)
                {
                    return;
                }
                
                SetSelected(button, true);
            }
        }

        public void SetSelected(Button button, bool state)
        {
            button.image.color = state ? usedColor : unusedColor;
        }

        public void OnAddExplosion()
        {
            ChangeState(_explosionButton, AbilityType.Explosion);
        }

        public void OnAddFireball()
        {
            ChangeState(_fireballButton, AbilityType.Fireball);
        }

        public void OnAddForcePush()
        {
            ChangeState(_forcePushButton, AbilityType.ForcePush);
        }

        public void OnAddHook()
        {
            ChangeState(_hookButton, AbilityType.Hook);
        }

        public void OnAddInvisibility()
        {
            ChangeState(_invisibilityButton, AbilityType.Invisibility);
        }

        public void OnAddLifeDrain()
        {
            ChangeState(_lifeDrainButton, AbilityType.LifeDrain);
        }

        public void OnAddShield()
        {
            ChangeState(_shieldButton, AbilityType.Shield);
        }

        public void OnAddSwap()
        {
            ChangeState(_swapButton, AbilityType.Swap);
        }

        public void OnAddTeleport()
        {
            ChangeState(_teleportButton, AbilityType.Teleport);
        }
    }
}
