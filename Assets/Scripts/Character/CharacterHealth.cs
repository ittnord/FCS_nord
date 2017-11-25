using System.Collections.Generic;
using System.Linq;
using FCS;
using FCS.Character;
using FCS.Managers;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

namespace Character
{
    public class CharacterHealth : NetworkBehaviour
    {
        [SerializeField]
        private float _startingHealth = 100f; // The amount of health each tank starts with.
        [SerializeField]
        private Slider _slider; // The slider to represent how much health the tank currently has.
        [SerializeField]
        private Image _fillImage; // The image component of the slider.
        [SerializeField]
        private Color _fullHealthColor = Color.green; // The color the health bar will be when on full health.
        [SerializeField]
        private Color _zeroHealthColor = Color.red; // The color the health bar will be when on no health.

        [SerializeField]
        private GameObject _characterRenderers; // References to all the gameobjects that need to be disabled when the tank is dead.

        [SerializeField]
        private GameObject _healthCanvas;
        [SerializeField]
        private CharacterSetup _setup;

        [SyncVar(hook = "OnCurrentHealthChanged")]
        private float _currentHealth; // How much health the tank currently has.*

        [SyncVar] private bool _zeroHealthHappened; // Has the tank been reduced beyond zero health yet?
        
        private readonly HashSet<IStat> _stats = new HashSet<IStat>();


        public CharacterManager Manager { get; set; }
        public GameObject CharacterRenderers { get { return _characterRenderers; } set { _characterRenderers = value; } }


        public void Damage(StatType type, float value)
        {
            var stat = _stats.First(element => element.Type == type);
            stat.Current += value;
            
            _currentHealth += value;
            if (_currentHealth <= 0f && !_zeroHealthHappened)
            {
                OnZeroHealth();
            }
        }

        private void SetHealthUI()
        {
            _slider.value = _currentHealth;
            _fillImage.color = Color.Lerp(_zeroHealthColor, _fullHealthColor, _currentHealth / _startingHealth);
        }


        private void OnCurrentHealthChanged(float value)
        {
            _currentHealth = value;
            SetHealthUI();
        }

        private void OnZeroHealth()
        {
            _zeroHealthHappened = true;
            RpcOnZeroHealth();
        }

        private void InternalOnZeroHealth()
        {
            SetCharacterActive(false);
        }

        [ClientRpc]
        private void RpcOnZeroHealth()
        {
            InternalOnZeroHealth();
        }

        private void SetCharacterActive(bool active)
        {
            _characterRenderers.SetActive(active);
            _healthCanvas.SetActive(active);

            if (active)
            {
                Manager.EnableControl();
            }
            else
            {
                Manager.DisableControl();
            }

            _setup.ActivateCrown(active);
        }

        // This function is called at the start of each round to make sure each tank is set up correctly.
        public void SetDefaults()
        {
            _stats.Add(new Stat(StatType.Hp, 100));
            _currentHealth = _startingHealth;
            _zeroHealthHappened = false;
            SetCharacterActive(true);
        }
    }
}