using UnityEngine;
using UnityEngine.Networking;

namespace FCS
{
    public class ShieldEffect : MonoBehaviour
    {
        private CharacterShield _shield;
        private float _elapsedTime = 0.0f;
        private const float lifeTime = 5.0f;

        protected void Start()
        {
            _shield = GetComponent<CharacterShield>();
            _shield.SetShieldAvailable(true);
        }

        public void Update()
        {
            _elapsedTime += Time.deltaTime;
            if (_elapsedTime > lifeTime)
            {
                _shield.SetShieldAvailable(false);
                Destroy(this);
            }
        }
    }
}