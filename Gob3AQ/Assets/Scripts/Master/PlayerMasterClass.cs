using Gob3AQ.FixedConfig;
using Gob3AQ.GameElement.Door;
using Gob3AQ.GameElement.PlayableChar;
using Gob3AQ.Libs.Arith;
using Gob3AQ.VARMAP.PlayerMaster;
using Gob3AQ.VARMAP.Types;
using Gob3AQ.Waypoint;
using Gob3AQ.Waypoint.Network;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Gob3AQ.PlayerMaster
{

    public class PlayerMasterClass : MonoBehaviour
    {
        private static PlayerMasterClass _singleton;
        private static byte playersLoaded;
        private static byte playersToLoad;



        public static void InteractPlayerService(CharacterType character, int destWp_index, out bool accepted)
        {
            PlayableCharScript instance = GetPlayerInstance(character);
            accepted = instance.ActionRequest(destWp_index);
        }

        public static void LockPlayerService(CharacterType character, bool lockPlayer)
        {
            PlayableCharScript instance = GetPlayerInstance(character);
            instance.LockRequest(lockPlayer);
        }

        #region "Internal Services"


        public static void SetPlayerLoadPresent(CharacterType character)
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