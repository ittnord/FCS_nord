using System;
using System.Collections.Generic;
using System.Linq;
using Character;
using UnityEngine;
using UnityEngine.Networking;
using UnityStandardAssets.Characters.ThirdPerson;

namespace FCS
{
    public class CharacterBehaviour : ThirdPersonCharacter
    {
        [SerializeField]
        private Transform _spawnAbilityTransform;

        [SerializeField]
        private Transform _shieldTransform;


        public Transform Shield { get { return _shieldTransform; } }

        private readonly HashSet<IStat> _stats = new HashSet<IStat>();

        public event Action<IStat> OnStatChanged;
        public event Action<IStat> OnStatDie;

        protected override void Start()
        {
            base.Start();
            _stats.Add(new Stat(StatType.Hp, 100));
        }

        public HashSet<IStat> GetStats()
        {
            return _stats;
        }

        public void Change(StatType type, int value)
        {
            var stat = _stats.First(element => element.Type == type);
            stat.Current += value;
            if (stat.Current <= 0)
            {
                if (OnStatDie != null)
                {
                    OnStatDie(stat);
                }
            }
            else
            {
                if (OnStatChanged != null)
                {
                    OnStatChanged(stat);
                }
            }
        }

        private void OnTriggerEnter(Collider col)
        {
            var ability = col.gameObject.GetComponent<Ability>();
            if (ability != null)
            {
                ability.OnCollideWithCharacter(this);
            }
        }
    }
}