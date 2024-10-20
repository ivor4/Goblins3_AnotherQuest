using UnityEngine;
using Gob3AQ.VARMAP.PlayerMaster;
using Gob3AQ.VARMAP.Types;
using Gob3AQ.FixedConfig;
using Gob3AQ.Libs.Arith;
using Gob3AQ.GameElement.PlayableChar;
using Gob3AQ.Waypoint;
using Gob3AQ.Brain.ItemsInteraction;
using System;

namespace Gob3AQ.PlayerMaster
{

    public class PlayerMasterClass : MonoBehaviour
    {
        private static PlayerMasterClass _singleton;


        public static void MovePlayerService(CharacterType character, WaypointClass wp)
        {
            PlayableCharScript instance = GetPlayerInstance(character);

            instance.MoveRequest(wp);
        }

        public static void SelectPlayerService(CharacterType character)
        {
            VARMAP_PlayerMaster.SET_PLAYER_SELECTED(character);
        }

        public static void InteractPlayerItemService(in ItemUsage usage, WaypointClass itemwp)
        {
            PlayableCharScript instance = GetPlayerInstance(usage.playerSource);
            instance.ItemInteractRequest(in usage, itemwp);
        }

        public static void SetPlayerAnimation(CharacterType character, CharacterAnimation animation)
        {
            PlayableCharScript instance = GetPlayerInstance(character);
            instance.ActAnimationRequest(animation);
        }

        private static PlayableCharScript GetPlayerInstance(CharacterType character)
        {
            PlayableCharScript selectedPlayer = null;

            VARMAP_PlayerMaster.GET_PLAYER_LIST(out ReadOnlySpan<PlayableCharScript> playerlist);

            selectedPlayer = playerlist[(int)character - 1];

            return selectedPlayer;
        }


        private void Awake()
        {
            if(_singleton)
            {
                Destroy(this);
                return;
            }
            else
            {
                _singleton = this;
            }
        }

        void Start()
        {
            VARMAP_PlayerMaster.SELECT_PLAYER(CharacterType.CHARACTER_NONE);
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