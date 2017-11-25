using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using FCS.Managers;

namespace Prototype.NetworkLobby
{
    //List of players in the lobby
    public class LobbyPlayerList : MonoBehaviour
    {
        public static LobbyPlayerList _instance = null;

        public RectTransform playerListContentTransform;
        
        public RectTransform playerFirstTeeamListContentTransform;
        public RectTransform playerSecondTeeamListContentTransform;

        public int GetPlayersCount()
        {
            if (_type == GameType.Solo)
            {
                return playerListContentTransform.childCount;
            }
            
            return playerFirstTeeamListContentTransform.childCount +
                   playerFirstTeeamListContentTransform.childCount;
        }

        [SerializeField]
        private GameObject _soloView;
        [SerializeField]
        private GameObject _coopView;
        
        public GameObject warningDirectPlayServer;

        protected readonly List<VerticalLayoutGroup> Layout = new List<VerticalLayoutGroup>();
        protected readonly List<LobbyPlayer> _players = new List<LobbyPlayer>();

        private GameType _type;
        
        public void OnEnable()
        {
            _instance = this;
            Layout.Clear();

            Layout.Add(playerListContentTransform.GetComponent<VerticalLayoutGroup>());
            Layout.Add(playerFirstTeeamListContentTransform.GetComponent<VerticalLayoutGroup>());
            Layout.Add(playerSecondTeeamListContentTransform.GetComponent<VerticalLayoutGroup>());
        }

        public void Init(GameType type)
        {
            _type = type;
            _soloView.SetActive(type == GameType.Solo);
            _coopView.SetActive(type == GameType.Coop);
        }

        public void DisplayDirectServerWarning(bool enabled)
        {
            if(warningDirectPlayServer != null)
                warningDirectPlayServer.SetActive(enabled);
        }

        private void Update()
        {
            //this dirty the layout to force it to recompute evryframe (a sync problem between client/server
            //sometime to child being assigned before layout was enabled/init, leading to broken layouting)

            foreach (var layoutGroup in Layout)
            {
                layoutGroup.childAlignment = Time.frameCount % 2 == 0 ? TextAnchor.UpperCenter : TextAnchor.UpperLeft;
            }
        }

        public void AddPlayer(LobbyPlayer player)
        {
            if (_players.Contains(player))
                return;

            _players.Add(player);
            if (_type == GameType.Solo)
            {
                player.transform.SetParent(playerListContentTransform, false);
            }
            else
            {
                player.transform.SetParent(
                    _players.Count % 2 == 0
                        ? playerFirstTeeamListContentTransform
                        : playerSecondTeeamListContentTransform, false);
            }

            PlayerListModified();
        }

        public void RemovePlayer(LobbyPlayer player)
        {
            _players.Remove(player);
            PlayerListModified();
        }

        public void PlayerListModified()
        {
            int i = 0;
            foreach (LobbyPlayer p in _players)
            {
                p.OnPlayerListChanged(i);
                ++i;
            }
        }
    }
}
