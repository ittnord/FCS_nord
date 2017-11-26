using System.Collections.Generic;
using Character;
using UnityEngine;

namespace FCS
{
    public class Lava : MonoBehaviour
    {
        [SerializeField]
        private float _damage;
        [SerializeField]
        private float _damagePeriod;
        private float _cooldown;
        
        private List<CharacterHealth> _characters = new List<CharacterHealth>();

        public void AddCharacter(CharacterHealth characterHealth)
        {
            _characters.Add(characterHealth);    
        }

        public void RemoveCharacter(CharacterHealth characterHealth)
        {
            _characters.Remove(characterHealth);    
        }

        private void Start()
        {
            _cooldown = _damagePeriod;
        }

        private void Update()
        {
            _cooldown -= Time.deltaTime;

            if (_cooldown <= 0)
            {
                _cooldown = _damagePeriod;
                for (var i = 0; i < _characters.Count; i++)
                {
                    var character = _characters[i];
                    character.Damage(StatType.Hp, _damage);
                    if (character.GeStat(StatType.Hp) < 0)
                    {
                        _characters.Remove(character);
                        i--;
                    }
                }
            }
        }
    }
}