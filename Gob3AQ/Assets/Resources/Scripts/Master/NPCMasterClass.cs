using UnityEditor;
using UnityEngine;
using Gob3AQ.VARMAP.NPCMaster;
using Gob3AQ.VARMAP.Types;
using Gob3AQ.FixedConfig;
using System.Collections.Generic;

namespace Gob3AQ.GameElement.NPC
{
    public class NPCMasterClass : MonoBehaviour
    {

        private Rigidbody2D _rigidbody;
        private Collider2D _collider;

        public Collider2D Collider
        {
            get
            {
                return _collider;
            }
        }


        void Awake()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
            _collider = GetComponent<Collider2D>();
        }


        void Start()
        {

            VARMAP_NPCMaster.REG_GAMESTATUS(OnGameStatusChanged);

            VARMAP_NPCMaster.NPC_REGISTER(true, this);
        }


        void Update()
        {
            Game_Status gstatus = VARMAP_NPCMaster.GET_GAMESTATUS();

            switch(gstatus)
            {
                case Game_Status.GAME_STATUS_PLAY:
                    
                    break;
            }
        }




        private void OnDestroy()
        {
            VARMAP_NPCMaster.UNREG_GAMESTATUS(OnGameStatusChanged);
        }




        private void OnGameStatusChanged(ChangedEventType evtype, ref Game_Status oldstatus, ref Game_Status newstatus)
        {
            
        }
    }
}