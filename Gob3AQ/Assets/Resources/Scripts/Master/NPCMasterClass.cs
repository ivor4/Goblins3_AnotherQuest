using UnityEditor;
using UnityEngine;
using Gob3AQ.VARMAP.NPCMaster;
using Gob3AQ.VARMAP.Types;
using Gob3AQ.FixedConfig;
using System.Collections.Generic;

namespace Gob3AQ.NPCMaster
{
    public class NPCMasterClass : MonoBehaviour
    {

        [SerializeField]

        private Rigidbody myrigidbody;
        private Collider mycollider;
        private MeshRenderer myrenderer;

        private Vector3 stored_velocity;
        private Vector3 stored_ang_velocity;

        private bool on_world;
        private bool on_transition;

        private bool kinematic_on_other_reason;
        private bool collider_enable_other_reason;

        private void Awake()
        {

        }

        private void Start()
        {

            VARMAP_NPCMaster.REG_GAMESTATUS(OnGameStatusChanged);

            VARMAP_NPCMaster.NPC_REGISTER(true, this);
        }


        private void Update()
        {
            Game_Status gstatus = VARMAP_NPCMaster.GET_GAMESTATUS();

            switch(gstatus)
            {
                case Game_Status.GAME_STATUS_PLAY:
                    
                    break;
            }
        }

        private void OnCollisionStay(Collision collision)
        {
            if (on_world)
            {
                if (myrigidbody.linearVelocity.y < 0.1f)
                {
                    myrigidbody.AddForce(Vector3.up * 500f);
                }
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