using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;

namespace FCS
{
    [RequireComponent(typeof(CharacterBehaviour))]
    public class MyCharacterController : ThirdPersonUserControl
    {
        private CharacterBehaviour _character;

        protected override void Start()
        {
            base.Start();
            _character = GetComponent<CharacterBehaviour>();
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