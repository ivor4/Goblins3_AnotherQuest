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



        public static void MovePlayerService(WaypointClass wp)
        {
            PlayableCharScript selectedPlayer = GetActivePlayer();
            selectedPlayer.MoveRequest(wp);
        }

        public static void InteractItemPlayerService(GameItem item, WaypointClass itemwp)
        {
            PlayableCharScript selectedPlayer = GetActivePlayer();
            selectedPlayer.ItemInteractRequest(item, itemwp);
        }

        private static PlayableCharScript GetActivePlayer()
        {
            ReadOnlyList<PlayableCharScript> playerlist;
            byte pid;
            PlayableCharScript selectedPlayer;

            pid = VARMAP_PlayerMaster.GET_PLAYER_ID_SELECTED();
            playerlist = new(null);
            VARMAP_PlayerMaster.GET_PLAYER_LIST(ref playerlist);
            selectedPlayer = playerlist[pid];

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



        private void OnDestroy()
        {
            if(_singleton == this)
            {
                _singleton = null;
            }
        }
    }
}