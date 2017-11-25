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

        private Abilities _useAbility;

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

        private void UseAbility(Abilities ability)
        {
            if (!isLocalPlayer)
                return;

            _useAbility = ability;
            CmdUseAbility(ability);
        }

        [Command]
        private void CmdUseAbility(Abilities abilityType)
        {
            var ability = GuiFactory.Instance.Instantiate(abilityType);
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
                InputController.Instance.HandleAbility(Abilities.DefaultAbility);
            }
#endif
        }

        // This is used by the game manager to reset the tank.
        public void SetDefaults()
        {}
    }
}