using System;
using System.Collections.Generic;
using Character;
using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;

namespace FCS
{
    public class CharacterBehaviour : ThirdPersonCharacter
    {
        private CharacterHealth _characterHealth;
        private CharacterHealth Health => _characterHealth ?? (_characterHealth = GetComponent<CharacterHealth>());
      
        [SerializeField]
        private Transform _shieldTransform;


        public Transform Shield { get { return _shieldTransform; } }

        private readonly HashSet<IStat> _stats = new HashSet<IStat>();

        public event Action<IStat> OnStatChanged;
        public event Action<IStat> OnStatDie;

        public void Change(StatType type, int value)
        {
          Health.Damage(type, value);
        }

        private void OnTriggerEnter(Collider col)
        {
            var ability = col.gameObject.GetComponent<Ability>();
            if (ability != null)
            {
                ability.OnCollideWithCharacter(this);
            }
        }

        public void SetPosition(Vector3 position)
        {
            GetComponent<CharacterMovement>().SetPosition(position);
        }
    }
}