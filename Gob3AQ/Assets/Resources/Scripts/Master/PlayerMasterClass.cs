using UnityEngine;
using Gob3AQ.VARMAP.PlayerMaster;
using Gob3AQ.VARMAP.Types;
using Gob3AQ.FixedConfig;
using System;

namespace Gob3AQ.PlayerMaster
{
    

    public class PlayerMasterClass : MonoBehaviour
    {
        private static PlayerMasterClass _singleton;



        public static void MovePlayerService(Vector2 position)
        {

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

        private void Start()
        {

        }


        private void Update()
        {
            Execute_Play();
        }



        private void Execute_Play()
        {
            KeyStruct keyInfo = VARMAP_PlayerMaster.GET_PRESSED_KEYS();
            Vector3Struct posstruct = new Vector3Struct();

            posstruct.position = transform.position;
            VARMAP_PlayerMaster.SET_PLAYER_POSITION(posstruct);
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