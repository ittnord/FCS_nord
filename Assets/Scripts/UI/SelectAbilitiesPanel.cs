using System.Collections;
using System.Collections.Generic;
using FCS;
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

        private void ChangeState(Button button, Abilities abilities)
        {
            var selectedAbilities = AbilitiesStorage.Instance.SelectedAbilities;
            AbilitiesStorage.Instance.ChangeState(abilities);
            if (selectedAbilities.Contains(abilities))
            {
                SetSelected(button, false);
            }
            else
            {
                if (selectedAbilities.Count >= 3)
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
            ChangeState(_explosionButton, Abilities.Explosion);
        }

        public void OnAddFireball()
        {
            ChangeState(_fireballButton, Abilities.Fireball);
        }

        public void OnAddForcePush()
        {
            ChangeState(_forcePushButton, Abilities.ForcePush);
        }

        public void OnAddHook()
        {
            ChangeState(_hookButton, Abilities.Hook);
        }

        public void OnAddInvisibility()
        {
            ChangeState(_invisibilityButton, Abilities.Invisibility);
        }

        public void OnAddLifeDrain()
        {
            ChangeState(_lifeDrainButton, Abilities.LifeDrain);
        }

        public void OnAddShield()
        {
            ChangeState(_shieldButton, Abilities.Shield);
        }

        public void OnAddSwap()
        {
            ChangeState(_swapButton, Abilities.Swap);
        }

        public void OnAddTeleport()
        {
            ChangeState(_teleportButton, Abilities.Teleport);
        }
    }
}
