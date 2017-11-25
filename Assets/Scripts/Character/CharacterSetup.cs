using System.Collections.Generic;
using System.Linq;
using FCS.Managers;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

namespace FCS.Character
{
    public class CharacterSetup : NetworkBehaviour
    {
        [Header("UI")] 
        [SerializeField]
        private Text _nameText;

        [Header("Network")] [Space] [SyncVar] 
        private Color _color;

        [SyncVar] 
        private string _playerName;

        //this is the player number in all of the players
        [SyncVar] 
        private int _playerNumber;

        //This is the local ID when more than 1 player per client
        [SyncVar] 
        private int _localId;

        [SyncVar] 
        private bool _isReady = false;

        private SyncList<Abilities> _abilityTypes;

        //This allow to know if the crown must be displayed or not
        protected bool isLeader = false;

        public int LocalId
        {
            get { return _localId; } 
            set { _localId = value; }
        }

        public Color Color { get { return _color; } set { _color = value; } }
        public string PlayerName { get { return _playerName; } set { _playerName = value; } }
        public int PlayerNumber { get { return _playerNumber; } set { _playerNumber = value; } }
        public bool IsReady { get { return _isReady; } set { _isReady = true; } }

        private bool _guiInited;

        public override void OnStartClient()
        {
            base.OnStartClient();

            _abilityTypes = new SyncListStruct<Abilities>
            {
                Abilities.DefaultAbility,
                Abilities.Teleport,
                Abilities.Hook,
                Abilities.ForcePush
            };

            if (!isServer) //if not hosting, we had the tank to the gamemanger for easy access!
                GameManager.AddCharacter(gameObject, _playerNumber, _color, _playerName, _localId);

            // Get all of the renderers of the tank.
            Renderer[] renderers = gameObject.GetComponentsInChildren<Renderer>();

            // Go through all the renderers...
            for (int i = 0; i < renderers.Length; i++)
            {
                // ... set their material color to the color specific to this tank.
                renderers[i].material.color = _color;
            }

            gameObject.SetActive(true);
            _nameText.text = "<color=#" + ColorUtility.ToHtmlStringRGB(_color) + ">" + _playerName + "</color>";
        }

        public List<Abilities> GetAbilities()
        {
            return _abilityTypes.ToList();
        }

        [ClientCallback]
        public void Update()
        {
            if (!isLocalPlayer)
            {
                return;
            }

            if (!_guiInited)
            {
                InitGui();
                _guiInited = true;
            }

            if (GameManager.Instance.GameIsFinished && !_isReady)
            {
                if (Input.GetButtonDown("Fire"))
                {
                    CmdSetReady();
                }
            }
        }

        public void SetLeader(bool leader)
        {
            RpcSetLeader(leader);
        }

        [ClientRpc]
        public void RpcSetLeader(bool leader)
        {
            isLeader = leader;
        }

        [Command]
        public void CmdSetReady()
        {
            _isReady = true;
        }

        public void ActivateCrown(bool active)
        {
            _nameText.gameObject.SetActive(active);
        }

        public override void OnNetworkDestroy()
        {
            GameManager.RemoveCharacter(gameObject);
        }

        private void InitGui()
        {
            var hud = Instantiate(GuiFactory.Instance.BattleHud);
            GuiFactory.Instance.Add(hud.GetComponent<RectTransform>());
        }
    }
}