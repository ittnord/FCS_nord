using UnityEngine;
using UnityEngine.Networking;

namespace FCS
{
    public class CharacterShield : NetworkBehaviour
    {
        private CharacterBehaviour _character;
        public CharacterBehaviour Owner { get { return _character; } }

        [SerializeField]
        private CharacterShield _shieldPrefab;

        private CharacterShield _shield;

        public void Awake()
        {
            _character = gameObject.GetComponentInParent<CharacterBehaviour>();
        }

        [ClientCallback]
        void Update()
        {
            if (!isLocalPlayer)
            {
                return;
            }
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

        [Command]
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

        [Command]
        private void CmdShieldDisable()
        {
            NetworkServer.Destroy(_shield.gameObject);
            //Destroy(_shield.gameObject);
            _shield = null;
        }
    }
}