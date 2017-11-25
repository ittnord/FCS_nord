using UnityEngine;
using UnityEngine.Networking;

namespace FCS
{
    public class Ability : NetworkBehaviour
    {
        private CharacterBehaviour _owner;
        private IAbilityPerformer _performer;

        private float _distance;

        public CharacterBehaviour Caster;
        public int Speed = 10;
        public int MaxDistance = 10;

        public GameObject ImpactEffect;

        protected void Start()
        {
            OnInstantiate();
        }

        protected void Update()
        {
            _distance += Speed * Time.deltaTime;
            Vector3 v = transform.rotation * Vector3.forward * Speed * Time.deltaTime;
            transform.position += v;

            if (_distance > MaxDistance)
            {
                OnMaxDistance();
            }
        }

        [ServerCallback]
        public virtual void OnInstantiate()
        { }

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
    }
}