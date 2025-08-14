using UnityEngine;
using Gob3AQ.VARMAP.PlayerMaster;
using Gob3AQ.VARMAP.Types;
using Gob3AQ.FixedConfig;
using Gob3AQ.Libs.Arith;
using Gob3AQ.GameElement.PlayableChar;
using Gob3AQ.GameElement.Door;
using Gob3AQ.Waypoint;
using System;

namespace Gob3AQ.PlayerMaster
{

    public class PlayerMasterClass : MonoBehaviour
    {
        private static PlayerMasterClass _singleton;
        private static byte playersLoaded;
        private static byte playersToLoad;



        public static void SelectPlayerService(CharacterType character)
        {
            VARMAP_PlayerMaster.SET_PLAYER_SELECTED(character);
        }

        public static void InteractPlayerService(in InteractionUsage usage)
        {
            IncrementPlayerTransactionId(usage.playerSource);
            PlayableCharScript instance = GetPlayerInstance(usage.playerSource);
            instance.ActionRequest(in usage);
        }

        public static void LockPlayerService(CharacterType character, bool lockPlayer)
        {
            IncrementPlayerTransactionId(character);
            PlayableCharScript instance = GetPlayerInstance(character);
            instance.LockRequest(lockPlayer);
        }

        #region "Internal Services"
        /// <summary>
        /// Determines whether the specified player is in the same state based on their transaction ID and waypoint.
        /// </summary>
        /// <param name="character">The character type representing the player to check.</param>
        /// <param name="transactionId">The transaction ID to compare against the player's current transaction ID.</param>
        /// <param name="wp">The waypoint to compare against the player's current waypoint.</param>
        /// <returns><see langword="true"/> if the player's transaction ID matches the specified transaction ID  and the player's
        /// waypoint matches the specified waypoint; otherwise, <see langword="false"/>.</returns>
        public static bool IsPlayerInSameState(CharacterType character, ulong transactionId, WaypointClass wp)
        {
            ulong playerTransactionId = VARMAP_PlayerMaster.GET_ELEM_PLAYER_TRANSACTION((int)character);
            PlayableCharScript instance = GetPlayerInstance(character);
            bool inSameState = (playerTransactionId == transactionId) && (instance.Waypoint == wp);

            return inSameState;
        }

        public static void SetPlayerAvailable(CharacterType character)
        {
            playersToLoad |= (byte)(1 << (int)character);
        }

        public static void SetPlayerLoaded(CharacterType character)
        {
            playersLoaded |= (byte)(1 << (int)character);

            if (playersLoaded == playersToLoad)
            {
                VARMAP_PlayerMaster.MODULE_LOADING_COMPLETED(GameModules.MODULE_PlayerMaster);
            }
        }

        #endregion

        private static PlayableCharScript GetPlayerInstance(CharacterType character)
        {
            PlayableCharScript selectedPlayer;

            VARMAP_PlayerMaster.GET_PLAYER_LIST(out ReadOnlySpan<PlayableCharScript> playerlist);

            selectedPlayer = playerlist[(int)character];

            return selectedPlayer;
        }

        /// <summary>
        /// Increments the transaction ID associated with the specified player character.
        /// </summary>
        /// <remarks>This method retrieves the current transaction ID for the specified character,
        /// increments it by one,  and updates the stored value. The transaction ID is used to track player-related
        /// operations.</remarks>
        /// <param name="character">The character whose transaction ID is to be incremented.</param>
        private static void IncrementPlayerTransactionId(CharacterType character)
        {
            ulong playerTransactionId = VARMAP_PlayerMaster.GET_ELEM_PLAYER_TRANSACTION((int)character);
            ++playerTransactionId;
            VARMAP_PlayerMaster.SET_ELEM_PLAYER_TRANSACTION((int)character, playerTransactionId);
        }


        private void Awake()
        {
            if(_singleton)
            {
                Destroy(gameObject);
            }
            else
            {
                _singleton = this;
            }
        }

        void Start()
        {
            VARMAP_PlayerMaster.SELECT_PLAYER(CharacterType.CHARACTER_NONE);
            playersLoaded = 0;
            playersToLoad = 0;
        }



        private void OnDestroy()
        {
            if(_singleton == this)
            {
                _singleton = null;
            }
        }
    }
}