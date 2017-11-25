using FCS;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

namespace Character
{
    public class CharacterShooting : NetworkBehaviour
    {
        public int PlayerNumber = 1; // Used to identify the different players.

        public Transform FireTransform; // A child of the tank where the shells are spawned.

        [SyncVar] public int localID;

        private CharacterBehaviour _character;

        private Ability _useAbility;

        private void Awake()
        {
            _character = GetComponent<CharacterBehaviour>();
            InputController.Instance.OnAbilityUsed += UseAbility;
        }

        private void OnDestroy()
        {
            if (InputController.Instance != null)
                InputController.Instance.OnAbilityUsed -= UseAbility;
        }

        private void UseAbility(Ability ability)
        {
            if (!isLocalPlayer)
                return;

            _useAbility = ability;
            CmdUseAbility();
        }

        [Command]
        private void CmdUseAbility()
        {
            var ability = Instantiate(_useAbility);
            ability.transform.position = FireTransform.position;
            ability.transform.rotation = transform.rotation;
            ability.Caster = _character;
            NetworkServer.Spawn(ability.gameObject);
        }

        [ClientCallback]
        private void Update()
        {
            if (!isLocalPlayer)
                return;
#if !MOBILE_INPUT
            if (Input.GetKeyUp(KeyCode.Space))
            {
                InputController.Instance.HandleAbility(GuiFactory.Instance.Instantiate(Abilities.DefaultAbility));
            }
#endif
        }

        // This is used by the game manager to reset the tank.
        public void SetDefaults()
        {}
    }
}