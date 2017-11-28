using FCS;
using FCS.Abilities;
using UnityEngine;
using UnityEngine.Networking;

namespace Character
{
    public class CharacterShooting : NetworkBehaviour
    {
        public int PlayerNumber = 1; // Used to identify the different players.

        public Transform FireTransform; // A child of the tank where the shells are spawned.

        [SyncVar] public int localID;

        private CharacterBehaviour _character;

        private AbilityType _useAbilityType;

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

        private void UseAbility(AbilityType abilityType)
        {
            if (!isLocalPlayer)
                return;
            
            AbilitiesStorage.Instance.CallOnAbilityCdBegin(abilityType);
            AbilitiesStorage.Instance.CallOnCdChanged(abilityType, 0);

            _useAbilityType = abilityType;
            CmdUseAbility(abilityType);
        }

        [Command]
        private void CmdUseAbility(AbilityType abilityTypeType)
        {
            var ability = GuiFactory.Instance.Instantiate(abilityTypeType);
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

            var s = AbilitiesStorage.Instance;
            if (s.CanStart)
            {
                var ss = s.SelectedAbilities;
                if (ss.Count == 4)
                {
                    s.CallOnCdChanged(ss[0], -Time.deltaTime);
                    s.CallOnCdChanged(ss[1], -Time.deltaTime);
                    s.CallOnCdChanged(ss[2], -Time.deltaTime);
                    s.CallOnCdChanged(ss[3], -Time.deltaTime);
                }
            }

#if !MOBILE_INPUT
            var abilities = s.SelectedAbilities;
            if (Input.GetKeyUp(KeyCode.Space) && !s.IsUnderCd(abilities[0]))
            {
                InputController.Instance.HandleAbility(abilities[0]);
            }
            if (Input.GetKeyUp(KeyCode.A) && !s.IsUnderCd(abilities[1]))
            {
                InputController.Instance.HandleAbility(abilities[1]);
            }
            if (Input.GetKeyUp(KeyCode.S) && !s.IsUnderCd(abilities[2]))
            {
                InputController.Instance.HandleAbility(abilities[2]);
            }
            if (Input.GetKeyUp(KeyCode.D) && !s.IsUnderCd(abilities[3]))
            {
                InputController.Instance.HandleAbility(abilities[3]);
            }
#endif
        }

        // This is used by the game manager to reset the tank.
        public void SetDefaults()
        {}
    }
}