using System.Linq;
using FCS.Abilities;
using FCS.Character;
using FCS.Managers;
using UnityEngine;
using UnityEngine.Networking;

namespace FCS
{
    public class CharacterShield : NetworkBehaviour
    {
        [SyncVar] public int PlayerNumber;

        public CharacterBehaviour _character;
        public CharacterBehaviour Owner { get { return _character; } }

        [SerializeField]
        private CharacterShield _shieldPrefab;

        private CharacterShield _shield;
        
        
        void Update()
        {
            if (_shield == null)
            {
                return;
            }


            _shield.transform.position = _character.Shield.position;
            _shield.transform.rotation = _character.Shield.rotation;
        }

        [ClientCallback]
        void OnTriggerEnter(Collider col)
        {
            if (!isLocalPlayer)
            {
                return;
            }
            var ability = col.gameObject.GetComponent<Ability>();
            if (ability != null)
            {
                ability.OnCollideWithShield(this);
            }
        }

        public void SetShieldAvailable(bool available)
        {
            if (available)
            {
                CmdShieldEnable();
            }
            else
            {
                CmdShieldDisable();
            }
        }

        [ServerCallback]
        private void CmdShieldEnable()
        {
            if (_shield != null)
            {
                return;
            }

            _shield = Instantiate(_shieldPrefab);
            _shield._character = _character;

            
            NetworkServer.Spawn(_shield.gameObject);
        }

        [ServerCallback]
        private void CmdShieldDisable()
        {
            //Destroy(_shield.gameObject);
            NetworkServer.Destroy(_shield.gameObject);
            _shield = null;
        }
    }
}