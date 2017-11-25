using FCS;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

namespace Character
{
    public class CharacterShooting : NetworkBehaviour
    {
        public int PlayerNumber = 1; // Used to identify the different players.
        public Rigidbody Shell; // Prefab of the shell.
        public Transform FireTransform; // A child of the tank where the shells are spawned.

        public float MinLaunchForce = 15f; // The force given to the shell if the fire button is not held.

        public float MaxLaunchForce = 30f; // The force given to the shell if the fire button is held for the max charge time.

        public float MaxChargeTime = 0.75f; // How long the shell can charge for before it is fired at max force.

        [SyncVar] public int localID;

        private string FireButton; // The input axis that is used for launching shells.
        private Rigidbody Rigidbody; // Reference to the rigidbody component.

        [SyncVar] private float CurrentLaunchForce; // The force that will be given to the shell when the fire button is released.

        [SyncVar] private float ChargeSpeed; // How fast the launch force increases, based on the max charge time.
        private bool Fired; // Whether or not the shell has been launched with this button press.

        private CharacterBehaviour _character;

        private void Awake()
        {
            // Set up the references.
            Rigidbody = GetComponent<Rigidbody>();

            _character = GetComponent<CharacterBehaviour>();
            if (isServer || isLocalPlayer)
            {
                InputController.Instance.OnAbilityUsed += UseAbility;
            }
        }

        private void Start()
        {
            // The fire axis is based on the player number.
            FireButton = "Fire" + (localID + 1);

            // The rate that the launch force charges up is the range of possible forces by the max charge time.
            ChargeSpeed = (MaxLaunchForce - MinLaunchForce) / MaxChargeTime;
        }

        private void OnDestroy()
        {
            InputController.Instance.OnAbilityUsed -= UseAbility;
        }

        [ClientCallback]
        private void UseAbility()
        {
            _character.UseAbility();
        }

        [ClientCallback]
        private void Update()
        {
            if (!isLocalPlayer)
                return;

            // If the max force has been exceeded and the shell hasn't yet been launched...
            if (CurrentLaunchForce >= MaxLaunchForce && !Fired)
            {
                // ... use the max force and launch the shell.
                CurrentLaunchForce = MaxLaunchForce;
                Fire();
            }
            // Otherwise, if the fire button has just started being pressed...
            else if (Input.GetButtonDown(FireButton))
            {
                // ... reset the fired flag and reset the launch force.
                Fired = false;
                CurrentLaunchForce = MinLaunchForce;
            }
            // Otherwise, if the fire button is being held and the shell hasn't been launched yet...
            else if (Input.GetButton(FireButton) && !Fired)
            {
                // Increment the launch force and update the slider.
                CurrentLaunchForce += ChargeSpeed * Time.deltaTime;
            }
            // Otherwise, if the fire button is released and the shell hasn't been launched yet...
            else if (Input.GetButtonUp(FireButton) && !Fired)
            {
                // ... launch the shell.
                // Fire();
            }
        }

        private void Fire()
        {
            // Set the fired flag so only Fire is only called once.
            Fired = true;

            CmdFire(Rigidbody.velocity, CurrentLaunchForce, FireTransform.forward, FireTransform.position,
                FireTransform.rotation);

            // Reset the launch force.  This is a precaution in case of missing button events.
            CurrentLaunchForce = MinLaunchForce;
        }

        [Command]
        private void CmdFire(Vector3 rigidbodyVelocity, float launchForce, Vector3 forward, Vector3 position,
            Quaternion rotation)
        {
            // Create an instance of the shell and store a reference to it's rigidbody.
            Rigidbody shellInstance =
                Instantiate(Shell, position, rotation) as Rigidbody;

            // Create a velocity that is the tank's velocity and the launch force in the fire position's forward direction.
            Vector3 velocity = rigidbodyVelocity + launchForce * forward;

            // Set the shell's velocity to this velocity.
            shellInstance.velocity = velocity;

            NetworkServer.Spawn(shellInstance.gameObject);
        }

        // This is used by the game manager to reset the tank.
        public void SetDefaults()
        {
            CurrentLaunchForce = MinLaunchForce;
        }
    }
}