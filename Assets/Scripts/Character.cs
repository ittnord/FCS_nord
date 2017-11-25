using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;

namespace FCS
{
    public class Character : ThirdPersonCharacter
    {
        [SerializeField]
        private Ability _abilityPrefab;
        [SerializeField]
        private Transform _spawnAbilityTransform;

        private readonly HashSet<IStat> _stats = new HashSet<IStat>();

        public event Action<IStat> OnStatChanged;
        public event Action<IStat> OnStatDie;

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

        public void UseAbility()
        {
            var ability = Instantiate(_abilityPrefab);
            ability.transform.position = _spawnAbilityTransform.position;
            ability.transform.rotation = transform.rotation;
            ability.Caster = this;
        }

        void OnTriggerEnter(Collider col)
        {
            var ability = col.gameObject.GetComponent<Ability>();
            if (ability != null)
            {
                ability.OnCollideWithCharacter(this);
            }
        }
    }
}