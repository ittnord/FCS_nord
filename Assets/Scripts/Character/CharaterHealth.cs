using FCS.Managers;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

namespace FCS.Character
{
    public class CharaterHealth : NetworkBehaviour
    {
        [SerializeField] 
        private float _startingHealth = 100f; // The amount of health each tank starts with.
        [SerializeField] 
        private  Slider _slider; // The slider to represent how much health the tank currently has.
        [SerializeField] 
        private  Image _fillImage; // The image component of the slider.

        [SerializeField]
        private  Color _fullHealthColor = Color.green; // The color the health bar will be when on full health.

        [SerializeField]
        private  Color _zeroHealthColor = Color.red; // The color the health bar will be when on no health.

        [SerializeField] private  AudioClip _tankExplosion; // The clip to play when the tank explodes.

        [SerializeField]
        private  ParticleSystem _explosionParticles; // The particle system the will play when the tank is destroyed.

        // References to all the gameobjects that need to be disabled when the tank is dead.
        [SerializeField] private  GameObject _charactersRenderers;

        [SerializeField] private  GameObject _healthCanvas;
        [SerializeField] private  GameObject _aimCanvas;
        [SerializeField] private  GameObject _leftDustTrail;
        [SerializeField] private  GameObject _rightDustTrail;
        [SerializeField] private  CharacterSetup _setup;
        [SerializeField] private  CharacterManager _manager; //Associated manager, to disable control when dying.

        [SyncVar(hook = "OnCurrentHealthChanged")]
        private float _currentHealth; // How much health the tank currently has.*

        [SyncVar] private bool _zeroHealthHappened; // Has the tank been reduced beyond zero health yet?
        private BoxCollider _collider; // Used so that the tank doesn't collide with anything when it's dead.


        private void Awake()
        {
            _collider = GetComponent<BoxCollider>();
        }


        // This is called whenever the tank takes damage.
        public void Damage(float amount)
        {
            // Reduce current health by the amount of damage done.
            _currentHealth -= amount;

            // If the current health is at or below zero and it has not yet been registered, call OnZeroHealth.
            if (_currentHealth <= 0f && !_zeroHealthHappened)
            {
                OnZeroHealth();
            }
        }


        private void SetHealthUI()
        {
            // Set the slider's value appropriately.
            _slider.value = _currentHealth;

            // Interpolate the color of the bar between the choosen colours based on the current percentage of the starting health.
            _fillImage.color = Color.Lerp(_zeroHealthColor, _fullHealthColor, _currentHealth / _startingHealth);
        }


        private void OnCurrentHealthChanged(float value)
        {
            _currentHealth = value;
            // Change the UI elements appropriately.
            SetHealthUI();

        }

        private void OnZeroHealth()
        {
            // Set the flag so that this function is only called once.
            _zeroHealthHappened = true;

            RpcOnZeroHealth();
        }

        private void InternalOnZeroHealth()
        {
            // Disable the collider and all the appropriate child gameobjects so the tank doesn't interact or show up when it's dead.
            SetCharacterkActive(false);
        }

        [ClientRpc]
        private void RpcOnZeroHealth()
        {
            // Play the particle system of the tank exploding.
            _explosionParticles.Play();

            // Create a gameobject that will play the tank explosion sound effect and then destroy itself.
            AudioSource.PlayClipAtPoint(_tankExplosion, transform.position);

            InternalOnZeroHealth();
        }

        private void SetCharacterkActive(bool active)
        {
            _collider.enabled = active;

            _charactersRenderers.SetActive(active);
            _healthCanvas.SetActive(active);
            _aimCanvas.SetActive(active);
            _leftDustTrail.SetActive(active);
            _rightDustTrail.SetActive(active);

            if (active) _manager.EnableControl();
            else _manager.DisableControl();

            _setup.ActivateCrown(active);
        }

        // This function is called at the start of each round to make sure each tank is set up correctly.
        public void SetDefaults()
        {
            _currentHealth = _startingHealth;
            _zeroHealthHappened = false;
            SetCharacterkActive(true);
        }
    }
}