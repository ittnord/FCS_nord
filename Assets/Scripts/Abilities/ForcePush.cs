using UnityEngine;
using UnityEngine.Networking;

namespace FCS
{
    public class ForcePush : Ability
    {
        private const int _moveDistance = 7;
        private const int _moveSpeed = 20;

        private const int _damage = 10;

        private const int _pushDistance = 10;
        private const int _pushSpeed = 10;

        private Vector3 _projectilePosition;

        protected override void Update()
        {
            base.Update();
            _projectilePosition = new Vector3(transform.position.x, Caster.transform.position.y, transform.position.z);
            Caster.transform.position = _projectilePosition;
        }

        [ServerCallback]
        public override void OnCollideWithCharacter(CharacterBehaviour character)
        {
            if (character == Caster)
            {
                return;
            }

            var direction = (_projectilePosition - character.transform.position).normalized;
            var moveEffect = character.gameObject.AddComponent<MoveEffect>();
            moveEffect.Init(-direction, _pushSpeed, _pushSpeed);

            character.Change(StatType.Hp, -10);
            Destroy(gameObject);
        }

        [ServerCallback]
        public override void OnCollideWithEnvironment(Environment env)
        {
            Destroy(gameObject);
        }
    }
}
