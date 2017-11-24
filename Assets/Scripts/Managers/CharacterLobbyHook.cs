using UnityEngine;
using UnityEngine.Networking;

namespace FCS.Managers
{
    public class CharacterLobbyHook : Prototype.NetworkLobby.LobbyHook 
    {
        public override void OnLobbyServerSceneLoadedForPlayer(NetworkManager manager, GameObject lobbyPlayer, GameObject gamePlayer)
        {
            if (lobbyPlayer == null)
                return;

            var lp = lobbyPlayer.GetComponent<Prototype.NetworkLobby.LobbyPlayer>();

            if(lp != null)
                GameManager.AddCharacter(gamePlayer, lp.slot, lp.playerColor, lp.nameInput.text, lp.playerControllerId);
        }
    }
}
