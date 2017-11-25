using FCS;
using UnityEngine;
using UnityEngine.Networking;

namespace Character
{
    public class CharacterMovement : NetworkBehaviour
    {
        public int PlayerNumber = 1; // Used to identify which tank belongs to which player.  This is set by this tank's manager.

        public int LocalID = 1;
        public float Speed = 12f; // How fast the tank moves forward and back.
        public float TurnSpeed = 180f; // How fast the tank turns in degrees per second.
        public float PitchRange = 0.2f; // The amount by which the pitch of the engine noises can vary.

        public Rigidbody Rigidbody; // Reference used to move the tank.
        
        private float MovementInput; // The current value of the movement input.
        private float TurnInput; // The current value of the turn input.

        private CharacterBehaviour _character;

        private void Awake()
        {
            Rigidbody = GetComponent<Rigidbody>();
            _character = GetComponent<CharacterBehaviour>();
        }

        private void FixedUpdate()
        {
            if (!isLocalPlayer)
                return;

            _character.Move(InputController.Instance.InputDirection, false, false);
        }


        // This function is called at the start of each round to make sure each tank is set up correctly.
        public void SetDefaults()
        {
            Rigidbody.velocity = Vector3.zero;
            Rigidbody.angularVelocity = Vector3.zero;

            MovementInput = 0f;
            TurnInput = 0f;
        }


        //We freeze the rigibody when the control is disabled to avoid the tank drifting!
        protected RigidbodyConstraints OriginalConstrains;

        void OnDisable()
        {
            OriginalConstrains = Rigidbody.constraints;
            Rigidbody.constraints = RigidbodyConstraints.FreezeAll;
        }

        void OnEnable()
        {
            Rigidbody.constraints = OriginalConstrains;
        }
    }
}