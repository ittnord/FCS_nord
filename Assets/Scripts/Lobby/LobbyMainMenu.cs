using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using FCS;
using FCS.Managers;
using UnityEngine.Networking;
using UnityStandardAssets.Utility;

namespace Prototype.NetworkLobby
{
    //Main menu, mainly only a bunch of callback called by the UI (setup throught the Inspector)
    public class LobbyMainMenu : MonoBehaviour 
    {
        [SerializeField]
        private LobbyManager _lobbyManager;

        [SerializeField] private RectTransform _gameTypeSelect;
        [SerializeField] private RectTransform _abilityesSelect;
        
        [SerializeField] private RectTransform _lobbyServerList;
        [SerializeField] private RectTransform _lobbyPanel;

        [SerializeField] private InputField _ipInput;
        [SerializeField] private Button _goToBattleButton;

        private Action _callback;

        public void OnEnable()
        {
            _lobbyManager.topPanel.ToggleVisibility(true);

            _ipInput.onEndEdit.RemoveAllListeners();
            _ipInput.onEndEdit.AddListener(OnEndEditIp);

            NetClientSingleton.Instance.StopClient();
            NetServerSingleton.Instance.StopServer();
        }

        public void OnClickHost()
        {
            _callback = () =>
            {
                NetServerSingleton.Instance.StartServer();
                _lobbyManager.StartHost();
            };
            _gameTypeSelect.gameObject.SetActive(true);

            
        }

        private void BackFromJoint()
        {
            _lobbyManager.StopClientClbk();
            NetClientSingleton.Instance.StopClient();
        }

        public void OnClickJoin()
        {
            _callback = () =>
            {
                NetClientSingleton.Instance.StartClient(OnReceivedBroadcast);

                _lobbyManager.ChangeTo(_lobbyPanel);
                _lobbyPanel.GetComponent<LobbyPlayerList>().Init(_lobbyManager.GameType);

                _lobbyManager.backDelegate = BackFromJoint;
                _lobbyManager.DisplayIsConnecting();
            };
            
            _gameTypeSelect.gameObject.SetActive(true);
        }

        public void OnReceivedBroadcast(string fromAddress)
        {
            NetClientSingleton.Instance.StopClient();

            _lobbyManager.networkAddress = fromAddress;
            _lobbyManager.StartClient();

            _lobbyManager.SetServerInfo("Connecting...", _lobbyManager.networkAddress);
        }

        public void OnClickSolo()
        {
            LobbyManager.Instance.GameType = GameType.Solo;
            _gameTypeSelect.gameObject.SetActive(false);
            _callback();
        }

        public void OnClickCoop()
        {
            LobbyManager.Instance.GameType = GameType.Coop;
            _gameTypeSelect.gameObject.SetActive(false);
            _callback();
        }

        public void OnClilckToBattle()
        {
            _abilityesSelect.gameObject.SetActive(false);
            _callback();
        }

        public void OnClickDedicated()
        {
            _lobbyManager.ChangeTo(null);
            _lobbyManager.StartServer();

            _lobbyManager.backDelegate = _lobbyManager.StopServerClbk;

            _lobbyManager.SetServerInfo("Dedicated Server", _lobbyManager.networkAddress);
        }

        public void OnClickOpenServerList()
        {
            _lobbyManager.StartMatchMaker();
            _lobbyManager.backDelegate = _lobbyManager.SimpleBackClbk;
            _lobbyManager.ChangeTo(_lobbyServerList);
        }

        private void OnEndEditIp(string text)
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                OnClickJoin();
            }
        }
        
    }
}
