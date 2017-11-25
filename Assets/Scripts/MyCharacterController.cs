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
            //InputController.Instance.OnAbilityUsed += UseAbility;
        }

        //protected void OnDestroy()
        //{
        //    InputController.Instance.OnAbilityUsed -= UseAbility;
        //}

        protected override void Update()
        {
        //    base.Update();
        }

        protected override void FixedUpdate()
        {
            _character.Move(InputController.Instance.InputDirection, false, false);
        }

        private void UseAbility()
        {
            _character.UseAbility();
        }
    }
}