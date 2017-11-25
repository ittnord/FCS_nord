using UnityEngine;

namespace FCS
{
    public class Ability : MonoBehaviour
    {
        private Character _owner;
        private IAbilityPerformer _performer;

        private float _distance;

        public Character Caster;
        public int Speed = 10;
        public int MaxDistance = 10;

        protected void Update()
        {
            _distance += Speed * Time.deltaTime;
            Vector3 v = transform.rotation * Vector3.forward * Speed * Time.deltaTime;
            transform.position += v;

            if (_distance > MaxDistance)
            {
                Destroy(gameObject);
            }
        }

        public void OnCollideWithEnvironment(Environment env)
        {
            Debug.Log("FUCK EAH!");
            Destroy(gameObject);
        }

        public virtual void OnCollideWithCharacter(Character character)
        {
            Debug.Log("FUCK EAH!");
            Destroy(gameObject);
        }
    }
}