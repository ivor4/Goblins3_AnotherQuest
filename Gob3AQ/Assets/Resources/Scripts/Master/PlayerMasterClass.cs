using UnityEngine;
using Gob3AQ.VARMAP.PlayerMaster;
using Gob3AQ.VARMAP.Types;
using Gob3AQ.FixedConfig;
using Gob3AQ.Libs.Arith;
using Gob3AQ.GameElement.PlayableChar;
using Gob3AQ.Waypoint;
using System;

namespace Gob3AQ.PlayerMaster
{
    

    public class PlayerMasterClass : MonoBehaviour
    {
        private static PlayerMasterClass _singleton;
        private static PlayableCharScript _selectedPlayer;


        public static void MovePlayerService(WaypointClass wp)
        {
            if (_selectedPlayer != null)
            {
                _selectedPlayer.MoveRequest(wp);
            }
        }

        public static void SelectPlayerService(CharacterType character)
        {
            VARMAP_PlayerMaster.SET_PLAYER_SELECTED(character);

            _selectedPlayer = GetActivePlayer();
        }

        public static void InteractPlayerItemService(GameItem item, WaypointClass itemwp)
        {
            if (_selectedPlayer != null)
            {
                _selectedPlayer.ItemInteractRequest(item, itemwp);
            }
        }

        private static PlayableCharScript GetActivePlayer()
        {
            CharacterType selectedChar;
            PlayableCharScript selectedPlayer = null;

            selectedChar = VARMAP_PlayerMaster.GET_PLAYER_SELECTED();
            VARMAP_PlayerMaster.GET_PLAYER_LIST(out ReadOnlyList<PlayableCharScript> playerlist);
            int totalPlayers = playerlist.Count;

            for(int i=0; i< totalPlayers; i++)
            {
                if(selectedChar == playerlist[i].charType)
                {
                    selectedPlayer = playerlist[i];
                    break;
                }
            }

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
            _selectedPlayer = null;
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