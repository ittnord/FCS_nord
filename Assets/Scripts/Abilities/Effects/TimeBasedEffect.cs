using UnityEngine;

namespace FCS.Abilities.Effects
{
    public class TimeBasedEffect : MonoBehaviour
    {
        private float _expireTime;
        private float _nextTickTime;
        private int _tickCount;

        public virtual void Init(float duration)
        {
            _expireTime = Time.time + duration;
        }

        public virtual void Init(float duration, float tickTime)
        {
            _nextTickTime = Time.time + _nextTickTime;
            Init(duration);
        }

        public void Update()
        {
            if (Time.time > _expireTime)
            {
                OnExpire();
            }

            if (Time.time > _nextTickTime)
            {
                _tickCount++;
                OnTick(_tickCount);
                _nextTickTime = Time.time + _nextTickTime;
            }
        }

        public virtual void OnExpire()
        {
            Destroy(this);
        }

        public virtual void OnTick(int tickCount)
        {
        }
    }
}
