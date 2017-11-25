using UnityEngine;
using UnityEngine.Networking;

namespace FCS
{
    public enum Abilities
    {
        Ability = 0,
        DefaultAbility = 1,
        Explosion = 2,
        Fireball = 3,
        ForcePush = 4,
        Hook = 5,
        Invisibility = 6,
        LifeDrain = 7,
        Shield = 8,
        Swap = 9,
        Teleport = 10
    }

    public class Ability : NetworkBehaviour
    {
        protected float Distance { get; private set; }
        public Abilities AbilityType;

        public CharacterBehaviour Caster;
        public int Speed = 10;
        public int MaxDistance = 10;

        public GameObject SpawnEffect;
        public GameObject ImpactEffect;

        private float _colliderActivationTime;

        protected void Start()
        {
            OnInstantiate();
        }

        protected virtual void Update()
        {
            //if (Time.time >=_colliderActivationTime)
            //{
            //    GetComponent<Collider>().enabled = true;
            //}

            Distance += Speed * Time.deltaTime;
            Vector3 v = transform.rotation * Vector3.forward * Speed * Time.deltaTime;
            transform.position += v;

            if (Distance > MaxDistance)
            {
                OnMaxDistance();
            }
        }

        public virtual void OnInstantiate()
        {
            if (SpawnEffect != null)
            {
                Instantiate(SpawnEffect, Caster.transform.position, Quaternion.identity);
            }

            var colliderSize = GetComponent<BoxCollider>().size;
            var testScale = new Vector3(colliderSize.x, colliderSize.y, colliderSize.z);
            transform.localScale = testScale;

            //_colliderActivationTime = Time.time + 0.15f;
        }

        public virtual void OnMaxDistance()
        {
            Destroy(gameObject);
        }

        public virtual void OnCollideWithEnvironment(Environment env)
        {
            Destroy(gameObject);
        }

        public virtual void OnCollideWithCharacter(CharacterBehaviour character)
        {
            if (character == Caster && Distance < 10)
            {
                return;
            }

            Destroy(gameObject);
        }

        [ServerCallback]
        public virtual void OnCollideWithShield(CharacterShield shield)
        {
            if (shield.Owner == Caster && Distance < 10)
            {
                return;
            }
            var effectDirection = transform.rotation * Vector3.forward;
            var shieldDirection = shield.transform.rotation * Vector3.forward;
            var reflect = Vector3.Reflect(effectDirection, shieldDirection);
            transform.rotation = Quaternion.FromToRotation(effectDirection, reflect);
        }
    }
}