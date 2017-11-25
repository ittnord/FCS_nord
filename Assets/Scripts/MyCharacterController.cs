using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;

namespace FCS
{
    [RequireComponent(typeof(Character))]
    public class MyCharacterController : ThirdPersonUserControl
    {
        private Character _character;

        protected override void Start()
        {
            base.Start();
            _character = GetComponent<Character>();
        }

        protected override void Update()
        {
            base.Update();
        }

        protected override void FixedUpdate()
        {
            base.FixedUpdate();

#if !MOBILE_INPUT
            if (Input.GetMouseButtonUp(0))
            {
                _character.UseAbility();
            }
#endif
        }
    }
}