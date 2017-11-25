using UnityEngine;
using UnityEngine.Networking;

namespace FCS
{
    public class Ability : NetworkBehaviour
    {
        private CharacterBehaviour _owner;
        private IAbilityPerformer _performer;

        protected float Distance { get; private set; }

        public CharacterBehaviour Caster;
        public int Speed = 10;
        public int MaxDistance = 10;

        public GameObject SpawnEffect;
        public GameObject ImpactEffect;

        protected void Start()
        {
            OnInstantiate();
        }

        protected virtual void Update()
        {
            Distance += Speed * Time.deltaTime;
            Vector3 v = transform.rotation * Vector3.forward * Speed * Time.deltaTime;
            transform.position += v;

            if (Distance > MaxDistance)
            {
                OnMaxDistance();
            }
        }

        [ServerCallback]
        public virtual void OnInstantiate()
        {
            if (SpawnEffect != null)
            {
                Instantiate(SpawnEffect, Caster.transform.position, Quaternion.identity);
            }

            var colliderSize = GetComponent<SphereCollider>().radius;
            var testScale = new Vector3(colliderSize, colliderSize, colliderSize);
            transform.localScale = testScale;
        }

        public virtual void OnMaxDistance()
        {
            Destroy(gameObject);
        }

        [ServerCallback]
        public virtual void OnCollideWithEnvironment(Environment env)
        {
            Debug.Log("FUCK EAH!");
            Destroy(gameObject);
        }

        [ServerCallback]
        public virtual void OnCollideWithCharacter(CharacterBehaviour character)
        {
            Debug.Log("FUCK EAH!");
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