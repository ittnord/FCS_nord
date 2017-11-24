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

        private string MovementAxis; // The name of the input axis for moving forward and back.
        private string TurnAxis; // The name of the input axis for turning.
        private float MovementInput; // The current value of the movement input.
        private float TurnInput; // The current value of the turn input.

        private void Awake()
        {
            Rigidbody = GetComponent<Rigidbody>();
        }


        private void Start()
        {
            // The axes are based on player number.
            MovementAxis = "Vertical" + (LocalID + 1);
            TurnAxis = "Horizontal" + (LocalID + 1);
        }

        private void Update()
        {
            if (!isLocalPlayer)
                return;

            // Store the value of both input axes.
            MovementInput = Input.GetAxis(MovementAxis);
            TurnInput = Input.GetAxis(TurnAxis);
        }

        private void FixedUpdate()
        {
            if (!isLocalPlayer)
                return;

            // Adjust the rigidbodies position and orientation in FixedUpdate.
            Move();
            Turn();
        }


        private void Move()
        {
            // Create a movement vector based on the input, speed and the time between frames, in the direction the tank is facing.
            Vector3 movement = transform.forward * MovementInput * Speed * Time.deltaTime;

            // Apply this movement to the rigidbody's position.
            Rigidbody.MovePosition(Rigidbody.position + movement);
        }


        private void Turn()
        {
            // Determine the number of degrees to be turned based on the input, speed and time between frames.
            float turn = TurnInput * TurnSpeed * Time.deltaTime;

            // Make this into a rotation in the y axis.
            Quaternion inputRotation = Quaternion.Euler(0f, turn, 0f);

            // Apply this rotation to the rigidbody's rotation.
            Rigidbody.MoveRotation(Rigidbody.rotation * inputRotation);
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