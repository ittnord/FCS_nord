using System.Collections;
using System.Collections.Generic;
using System.Linq;
using FCS.Character;
using Prototype.NetworkLobby;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

namespace FCS.Managers
{
    public enum GameType
    {
        Solo, Coop
    }
    
    [RequireComponent(typeof(NetworkIdentity))]
    public class GameManager : NetworkBehaviour
    {
        public static readonly List<CharacterManager> Characters = new List<CharacterManager>();             
        public static GameManager Instance;
        
        [SerializeField] private int _numRoundsToWin = 5;          
        [SerializeField] private float _startDelay = 3f;           
        [SerializeField] private float _endDelay = 3f;
        [SerializeField] private CameraControl _cameraControl;     
        [SerializeField] private Text _messageText;
        [SerializeField] private Transform[] _spawnPoint;

        [HideInInspector]
        [SyncVar]
        [SerializeField] public bool GameIsFinished = false;

        //Various UI references to hide the screen between rounds.
        [Space]
        [Header("UI")]
        [SerializeField] private CanvasGroup _fadingScreen;  
        [SerializeField] private CanvasGroup _endRoundScreen;

        [SerializeField] private int _roundNumber;                  
        
        private WaitForSeconds _startWait;         
        private WaitForSeconds _endWait;       
        private CharacterManager _roundWinner;         
        private CharacterManager _gameWinner;

        private void Awake()
        {
            Instance = this;
        }
        
        [ServerCallback]
        private void Start()
        {
            _startWait = new WaitForSeconds(_startDelay);
            _endWait = new WaitForSeconds(_endDelay);

            StartCoroutine(GameLoop());
        }

        public static void AddCharacter(GameObject character, int playerNum, Color c, string name, int localID)
        {
            var tmp = new CharacterManager
            {
                Instance = character,
                PlayerNumber = playerNum,
                PlayerTeeam = -1,
                PlayerColor = c,
                PlayerName = name,
                LocalPlayerID = localID
            };
            tmp.Setup();

            Characters.Add(tmp);
        }

        public static void RemoveCharacter(GameObject character)
        {
            var toRemove = Characters.FirstOrDefault(tmp => tmp.Instance == character);
            if (toRemove != null)
                Characters.Remove(toRemove);
        }

        // This is called from start and will run each phase of the game one after another. ONLY ON SERVER (as Start is only called on server)
        private IEnumerator GameLoop()
        {
            while (Characters.Count < 2)
                yield return null;

            yield return new WaitForSeconds(2.0f);
            yield return StartCoroutine(RoundStarting());
            yield return StartCoroutine(RoundPlaying());
            yield return StartCoroutine(RoundEnding());

            if (_gameWinner != null)
            {
                GameIsFinished = true;
                float leftWaitTime = 15.0f;
                bool allAreReady = false;
                int flooredWaitTime = 15;

                while (leftWaitTime > 0.0f && !allAreReady)
                {
                    yield return null;

                    allAreReady = true;
                    foreach (var tmp in Characters)
                    {
                        allAreReady &= tmp.IsReady();
                    }

                    leftWaitTime -= Time.deltaTime;

                    int newFlooredWaitTime = Mathf.FloorToInt(leftWaitTime);

                    if (newFlooredWaitTime != flooredWaitTime)
                    {
                        flooredWaitTime = newFlooredWaitTime;
                        string message = EndMessage(flooredWaitTime);
                        RpcUpdateMessage(message);
                    }
                }

                LobbyManager.Instance.ServerReturnToLobby();
            }
            else
            {
                StartCoroutine(GameLoop());
            }
        }

        public Vector3 GetLocalPlayerPosition()
        {
            var charcter = Characters.FirstOrDefault(c => c.IsLocalPlayer());
            return charcter?.Movement.transform.position ?? Vector3.zero;
        }

        public GameObject GetLocalPlayer()
        {
            var charcter = Characters.FirstOrDefault(c => c.IsLocalPlayer());
            return charcter?.Movement.gameObject;
        }


        private IEnumerator RoundStarting()
        {
            RpcRoundStarting();
            yield return _startWait;
        }

        [ClientRpc]
        void RpcRoundStarting()
        {
            ResetAllCharacters();
            DisableCharacterControl();

            _cameraControl.SetAppropriatePositionAndSize();
            _roundNumber++;
            _messageText.text = "ROUND " + _roundNumber;


            StartCoroutine(ClientRoundStartingFade());
        }

        private IEnumerator ClientRoundStartingFade()
        {
            float elapsedTime = 0.0f;
            float wait = _startDelay - 0.5f;

            yield return null;

            while (elapsedTime < wait)
            {
                if(_roundNumber == 1)
                    _fadingScreen.alpha = 1.0f - (elapsedTime / wait);
                else
                    _endRoundScreen.alpha = 1.0f - (elapsedTime / wait);

                elapsedTime += Time.deltaTime;

                if (elapsedTime / wait < 0.5f)
                    ResetAllCharacters();

                yield return null;
            }
        }

        private IEnumerator RoundPlaying()
        {
            RpcRoundPlaying();

            while (!OneCharacterLeft())
            {
                yield return null;
            }
        }

        [ClientRpc]
        private void RpcRoundPlaying()
        {
            EnableCharacterControl();

            _messageText.text = string.Empty;
        }

        private IEnumerator RoundEnding()
        {
            _roundWinner = null;
            _roundWinner = GetRoundWinner();

            if (_roundWinner != null)
                _roundWinner.Wins++;

            _gameWinner = GetGameWinner();
            RpcUpdateMessage(EndMessage(0));
            RpcRoundEnding();

            yield return _endWait;
        }

        [ClientRpc]
        private void RpcRoundEnding()
        {
            DisableCharacterControl();
            StartCoroutine(ClientRoundEndingFade());
        }

        [ClientRpc]
        private void RpcUpdateMessage(string msg)
        {
            _messageText.text = msg;
        }

        private IEnumerator ClientRoundEndingFade()
        {
            float elapsedTime = 0.0f;
            float wait = _endDelay;
            while (elapsedTime < wait)
            {
                _endRoundScreen.alpha = (elapsedTime / wait);

                elapsedTime += Time.deltaTime;
                yield return null;
            }
        }

        private bool OneCharacterLeft()
        {
            int numCharactersLeft = 0;

            for (int i = 0; i < Characters.Count; i++)
            {
                if (Characters[i].CharacterRenderers.activeSelf)
                    numCharactersLeft++;
            }

            return numCharactersLeft <= 1;
        }


        // This function is to find out if there is a winner of the round.
        // This function is called with the assumption that 1 or fewer tanks are currently active.
        private CharacterManager GetRoundWinner()
        {
            for (int i = 0; i < Characters.Count; i++)
            {
                if (Characters[i].CharacterRenderers.activeSelf)
                    return Characters[i];
            }

            return null;
        }


        // This function is to find out if there is a winner of the game.
        private CharacterManager GetGameWinner()
        {
            int maxScore = 0;

            for (int i = 0; i < Characters.Count; i++)
            {
                if(Characters[i].Wins > maxScore)
                {
                    maxScore = Characters[i].Wins;
                }

                if (Characters[i].Wins == _numRoundsToWin)
                    return Characters[i];
            }

            for (int i = 0; i < Characters.Count && maxScore > 0; i++)
            {
                Characters[i].SetLeader(maxScore == Characters[i].Wins);
            }
            return null;
        }


        private string EndMessage(int waitTime)
        {
            string message = "DRAW!";

            if (_gameWinner != null)
            {
                message = "<color=#" + ColorUtility.ToHtmlStringRGB(_gameWinner.PlayerColor) + ">"+ _gameWinner.PlayerName + "</color> WINS THE GAME!";
            }
            else if (_roundWinner != null)
            {
                message = "<color=#" + ColorUtility.ToHtmlStringRGB(_roundWinner.PlayerColor) + ">" + _roundWinner.PlayerName + "</color> WINS THE ROUND!";
            }

            message += "\n\n";

            for (int i = 0; i < Characters.Count; i++)
            {
                message += "<color=#" + ColorUtility.ToHtmlStringRGB(Characters[i].PlayerColor) + ">" + Characters[i].PlayerName + "</color>: " + Characters[i].Wins + " WINS " 
                           + (Characters[i].IsReady()? "<size=15>READY</size>" : "") + " \n";
            }

            if (_gameWinner != null)
                message += "\n\n<size=20 > Return to lobby in " + waitTime + "\nPress Fire to get ready</size>";

            return message;
        }


        private void ResetAllCharacters()
        {
            for (int i = 0; i < Characters.Count; i++)
            {
                Characters[i].SpawnPoint = _spawnPoint[Characters[i].CharacterSetup.PlayerNumber];
                Characters[i].Reset();
            }
        }


        private void EnableCharacterControl()
        {
            for (int i = 0; i < Characters.Count; i++)
            {
                Characters[i].EnableControl();
            }
        }


        private void DisableCharacterControl()
        {
            for (int i = 0; i < Characters.Count; i++)
            {
                Characters[i].DisableControl();
            }
        }
    }
}
