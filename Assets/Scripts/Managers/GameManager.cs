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
    public class GameManager : NetworkBehaviour
    {
        public static GameManager Instance;

        //this is static so tank characters be added even withotu the scene loaded (i.e. from lobby)
        public static readonly List<CharacterManager> Characters = new List<CharacterManager>();             // A collection of managers for enabling and disabling different aspects of the tanks.

        [SerializeField]
        private int _numRoundsToWin = 5;          // The number of rounds a single player has to win to win the game.
        [SerializeField]
        private float _startDelay = 3f;           // The delay between the start of RoundStarting and RoundPlaying phases.
        [SerializeField]
        private  float _endDelay = 3f;             // The delay between the end of RoundPlaying and RoundEnding phases.
        [SerializeField]
        private CameraControl _cameraControl;     // Reference to the CameraControl script for control during different phases.
        [SerializeField]
        private Text _messageText;                // Reference to the overlay Text to display winning text, etc.
        [SerializeField]
        private GameObject CharacterPrefab;           // Reference to the prefab the players will control.

        [SerializeField]
        private Transform[] _spawnPoint;

        [HideInInspector]
        [SyncVar]
        [SerializeField]
        public bool GameIsFinished = false;

        //Various UI references to hide the screen between rounds.
        [Space]
        [Header("UI")]
        [SerializeField]
        private CanvasGroup _fadingScreen;  
        [SerializeField]
        private CanvasGroup _endRoundScreen;

        [SerializeField]
        private int _roundNumber;                  // Which round the game is currently on.

        private WaitForSeconds _startWait;         // Used to have a delay whilst the round starts.
        private WaitForSeconds _endWait;           // Used to have a delay whilst the round or game ends.
        private CharacterManager _roundWinner;          // Reference to the winner of the current round.  Used to make an announcement of who won.
        private CharacterManager _gameWinner;           // Reference to the winner of the game.  Used to make an announcement of who won.

        private void Awake()
        {
            Instance = this;
        }

        [ServerCallback]
        private void Start()
        {
            // Create the delays so they only have to be made once.
            _startWait = new WaitForSeconds(_startDelay);
            _endWait = new WaitForSeconds(_endDelay);

            // Once the tanks have been created and the camera is using them as targets, start the game.
            StartCoroutine(GameLoop());
        }

        /// <summary>
        /// Add a tank from the lobby hook
        /// </summary>
        /// <param name="character">The actual GameObject instantiated by the lobby, which is a NetworkBehaviour</param>
        /// <param name="playerNum">The number of the player (based on their slot position in the lobby)</param>
        /// <param name="c">The color of the player, choosen in the lobby</param>
        /// <param name="name">The name of the Player, choosen in the lobby</param>
        /// <param name="localID">The localID. e.g. if 2 player are on the same machine this will be 1 & 2</param>
        public static void AddCharacter(GameObject character, int playerNum, Color c, string name, int localID)
        {
            var tmp = new CharacterManager
            {
                Instance = character,
                PlayerNumber = playerNum,
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

            //wait to be sure that all are ready to start
            yield return new WaitForSeconds(2.0f);

            // Start off by running the 'RoundStarting' coroutine but don't return until it's finished.
            yield return StartCoroutine(RoundStarting());

            // Once the 'RoundStarting' coroutine is finished, run the 'RoundPlaying' coroutine but don't return until it's finished.
            yield return StartCoroutine(RoundPlaying());

            // Once execution has returned here, run the 'RoundEnding' coroutine.
            yield return StartCoroutine(RoundEnding());

            // This code is not run until 'RoundEnding' has finished.  At which point, check if there is a winner of the game.
            if (_gameWinner != null)
            {// If there is a game winner, wait for certain amount or all player confirmed to start a game again
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

                LobbyManager.s_Singleton.ServerReturnToLobby();
            }
            else
            {
                // If there isn't a winner yet, restart this coroutine so the loop continues.
                // Note that this coroutine doesn't yield.  This means that the current version of the GameLoop will end.
                StartCoroutine(GameLoop());
            }
        }


        private IEnumerator RoundStarting()
        {
            //we notify all clients that the round is starting
            RpcRoundStarting();

            // Wait for the specified length of time until yielding control back to the game loop.
            yield return _startWait;
        }

        [ClientRpc]
        void RpcRoundStarting()
        {
            // As soon as the round starts reset the tanks and make sure they can't move.
            ResetAllCharacters();
            DisableCharacterControl();

            // Snap the camera's zoom and position to something appropriate for the reset tanks.
            _cameraControl.SetAppropriatePositionAndSize();

            // Increment the round number and display text showing the players what round it is.
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

                //sometime, synchronization lag behind because of packet drop, so we make sure our tank are reseted
                if (elapsedTime / wait < 0.5f)
                    ResetAllCharacters();

                yield return null;
            }
        }

        private IEnumerator RoundPlaying()
        {
            //notify clients that the round is now started, they should allow player to move.
            RpcRoundPlaying();

            // While there is not one tank left...
            while (!OneTankLeft())
            {
                // ... return on the next frame.
                yield return null;
            }
        }

        [ClientRpc]
        void RpcRoundPlaying()
        {
            // As soon as the round begins playing let the players control the tanks.
            EnableCharacterControl();

            // Clear the text from the screen.
            _messageText.text = string.Empty;
        }

        private IEnumerator RoundEnding()
        {
            // Clear the winner from the previous round.
            _roundWinner = null;

            // See if there is a winner now the round is over.
            _roundWinner = GetRoundWinner();

            // If there is a winner, increment their score.
            if (_roundWinner != null)
                _roundWinner.Wins++;

            // Now the winner's score has been incremented, see if someone has one the game.
            _gameWinner = GetGameWinner();

            RpcUpdateMessage(EndMessage(0));

            //notify client they should disable tank control
            RpcRoundEnding();

            // Wait for the specified length of time until yielding control back to the game loop.
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

        // This is used to check if there is one or fewer tanks remaining and thus the round should end.
        private bool OneTankLeft()
        {
            // Start the count of tanks left at zero.
            int numTanksLeft = 0;

            // Go through all the tanks...
            for (int i = 0; i < Characters.Count; i++)
            {
                // ... and if they are active, increment the counter.
                if (Characters[i].CharacterRenderers.activeSelf)
                    numTanksLeft++;
            }

            // If there are one or fewer tanks remaining return true, otherwise return false.
            return numTanksLeft <= 1;
        }


        // This function is to find out if there is a winner of the round.
        // This function is called with the assumption that 1 or fewer tanks are currently active.
        private CharacterManager GetRoundWinner()
        {
            // Go through all the tanks...
            for (int i = 0; i < Characters.Count; i++)
            {
                // ... and if one of them is active, it is the winner so return it.
                if (Characters[i].CharacterRenderers.activeSelf)
                    return Characters[i];
            }

            // If none of the tanks are active it is a draw so return null.
            return null;
        }


        // This function is to find out if there is a winner of the game.
        private CharacterManager GetGameWinner()
        {
            int maxScore = 0;

            // Go through all the tanks...
            for (int i = 0; i < Characters.Count; i++)
            {
                if(Characters[i].Wins > maxScore)
                {
                    maxScore = Characters[i].Wins;
                }

                // ... and if one of them has enough rounds to win the game, return it.
                if (Characters[i].Wins == _numRoundsToWin)
                    return Characters[i];
            }

            //go throught a second time to enable/disable the crown on tanks
            //(note : we don't enter it if the maxScore is 0, as no one is current leader yet!)
            for (int i = 0; i < Characters.Count && maxScore > 0; i++)
            {
                Characters[i].SetLeader(maxScore == Characters[i].Wins);
            }

            // If no tanks have enough rounds to win, return null.
            return null;
        }


        // Returns a string of each player's score in their tank's color.
        private string EndMessage(int waitTime)
        {
            // By default, there is no winner of the round so it's a draw.
            string message = "DRAW!";


            // If there is a game winner set the message to say which player has won the game.
            if (_gameWinner != null)
                message = "<color=#" + ColorUtility.ToHtmlStringRGB(_gameWinner.PlayerColor) + ">"+ _gameWinner.PlayerName + "</color> WINS THE GAME!";
            // If there is a winner, change the message to display 'PLAYER #' in their color and a winning message.
            else if (_roundWinner != null)
                message = "<color=#" + ColorUtility.ToHtmlStringRGB(_roundWinner.PlayerColor) + ">" + _roundWinner.PlayerName + "</color> WINS THE ROUND!";

            // After either the message of a draw or a winner, add some space before the leader board.
            message += "\n\n";

            // Go through all the tanks and display their scores with their 'PLAYER #' in their color.
            for (int i = 0; i < Characters.Count; i++)
            {
                message += "<color=#" + ColorUtility.ToHtmlStringRGB(Characters[i].PlayerColor) + ">" + Characters[i].PlayerName + "</color>: " + Characters[i].Wins + " WINS " 
                           + (Characters[i].IsReady()? "<size=15>READY</size>" : "") + " \n";
            }

            if (_gameWinner != null)
                message += "\n\n<size=20 > Return to lobby in " + waitTime + "\nPress Fire to get ready</size>";

            return message;
        }


        // This function is used to turn all the tanks back on and reset their positions and properties.
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
