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



        private void OnDestroy()
        {
            if(_singleton == this)
            {
                _singleton = null;
            }
        }
    }
}