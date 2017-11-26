using UnityEngine;
using UnityEngine.Networking;

namespace FCS
{
    public class ForcePush : Ability
    {
        public int MoveDistance = 7;
        public int MoveSpeed = 20;

        public int Damage = 10;

        public int PushDistance = 10;
        public int PushSpeed = 10;

        private Vector3 _projectilePosition;

        protected override void Update()
        {
            base.Update();
            
            if(Caster == null)
                return;
            
            _projectilePosition = new Vector3(transform.position.x, Caster.transform.position.y, transform.position.z);
            Caster.SetPosition(_projectilePosition);
        }

        public override void OnCollideWithCharacter(CharacterBehaviour character)
        {
            if (character == Caster)
            {
                return;
            }

            var direction = (_projectilePosition - character.transform.position).normalized;
            var moveEffect = character.gameObject.AddComponent<MoveEffect>();
            moveEffect.Init(-direction, PushSpeed, PushDistance);

            character.Change(StatType.Hp, Damage);
            Destroy(gameObject);
        }

        public override void OnCollideWithEnvironment(Environment env)
        {
            Destroy(gameObject);
        }
    }
}
