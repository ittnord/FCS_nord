using System;
using Character;
using FCS.Character;
using UnityEngine;

namespace FCS.Managers
{
    [Serializable]
    public class CharacterManager
    {
        // This class is to manage various settings on a tank.
        // It works with the GameManager class to control how the tanks behave
        // and whether or not players have control of their tank in the 
        // different phases of the game.

        [SerializeField]
        public Color PlayerColor;               // This is the color this tank will be tinted.
        [SerializeField]
        public Transform SpawnPoint;            // The position and direction the tank will have when it spawns.
       
        [HideInInspector]
        public int PlayerNumber;                // This specifies which player this the manager for.
        
        [HideInInspector]
        public int PlayerTeeam;                // This specifies which player this the manager for.
        [HideInInspector]
        public GameObject Instance;             // A reference to the instance of the tank when it is created.
        [HideInInspector]
        public GameObject CharacterRenderers;        // The transform that is a parent of all the tank's renderers.  This is deactivated when the tank is dead.
        [HideInInspector]
        public int Wins;                        // The number of wins this player has so far.
        [HideInInspector]
        public string PlayerName;                    // The player name set in the lobby
        [HideInInspector]
        public int LocalPlayerID;                    // The player localID (if there is more than 1 player on the same machine)

        public CharacterMovement Movement;        // References to various objects for control during the different game phases.
        public CharacterShooting Shooting;
        public CharacterHealth Health;
        public CharacterSetup CharacterSetup;
        public CharacterShield CharacterShield;
        public CharacterBehaviour CharacterBehaviour;

        public void Setup()
        {
            // Get references to the components.
            Movement = Instance.GetComponent<CharacterMovement>();
            Shooting = Instance.GetComponent<CharacterShooting>();
            Health = Instance.GetComponent<CharacterHealth>();
            CharacterBehaviour = Instance.GetComponent<CharacterBehaviour>();
            CharacterShield = Instance.GetComponent<CharacterShield>();
            this.CharacterSetup = Instance.GetComponent<CharacterSetup>();

            // Get references to the child objects.
            CharacterRenderers = Health.CharacterRenderers;

            //Set a reference to that amanger in the health script, to disable control when dying
            Health.Manager = this;

            // Set the player numbers to be consistent across the scripts.
            Movement.PlayerNumber = PlayerNumber;
            Movement.LocalID = LocalPlayerID;

            Shooting.PlayerNumber = PlayerNumber;
            Shooting.localID = LocalPlayerID;

            //setup is use for diverse Network Related sync
            CharacterSetup.Color = PlayerColor;
            CharacterSetup.PlayerName = PlayerName;
            CharacterSetup.PlayerNumber = PlayerNumber;
            CharacterSetup.LocalId = LocalPlayerID;
         
            CharacterShield.PlayerNumber = PlayerNumber;
            CharacterShield._character = CharacterBehaviour;
        }

        // Used during the phases of the game where the player shouldn't be able to control their tank.
        public void DisableControl()
        {
            Movement.enabled = false;
            Shooting.enabled = false;
        }


        // Used during the phases of the game where the player should be able to control their tank.
        public void EnableControl()
        {
            Movement.enabled = true;
            Shooting.enabled = true;
        }

        public string GetName()
        {
            return CharacterSetup.PlayerName;
        }

        public void SetLeader(bool leader)
        { 
            CharacterSetup.SetLeader(leader);
        }

        public bool IsReady()
        {
            return CharacterSetup.IsReady;
        }

        // Used at the start of each round to put the tank into it's default state.
        public void Reset()
        {
            Movement.SetDefaults();
            Shooting.SetDefaults();
            Health.SetDefaults();

            if (Movement.hasAuthority)
            {
                Movement.Rigidbody.position = SpawnPoint.position;
                Movement.Rigidbody.rotation = SpawnPoint.rotation;
            }
        }
    }
}