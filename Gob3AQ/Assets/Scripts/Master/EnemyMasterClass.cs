using UnityEditor;
using UnityEngine;
using MVerse.VARMAP.EnemyMaster;
using MVerse.VARMAP.Types;
using MVerse.FixedConfig;
using System.Collections.Generic;

namespace MVerse.EnemyMaster
{
    public class EnemyMasterClass : MonoBehaviour
    {

        [SerializeField]
        private bool belongsToOtherWorld;

        public bool BelongsToOtherWorld => belongsToOtherWorld;

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
            myrigidbody = GetComponent<Rigidbody>();
            mycollider = GetComponent<Collider>();
            myrenderer = GetComponent<MeshRenderer>();


            on_world = VARMAP_EnemyMaster.GET_OTHER_WORLD() == belongsToOtherWorld;
            on_transition = true;

            kinematic_on_other_reason = myrigidbody.isKinematic;
            collider_enable_other_reason = mycollider.enabled;


            ComposeKinematicAndCollider();

            VARMAP_EnemyMaster.REG_OTHER_WORLD(OnOtherWorldChanged);
            VARMAP_EnemyMaster.REG_GAMESTATUS(OnGameStatusChanged);

            VARMAP_EnemyMaster.ENEMY_REGISTER(true, this);
        }


        private void Update()
        {
            Game_Status gstatus = VARMAP_EnemyMaster.GET_GAMESTATUS();

            switch(gstatus)
            {
                case Game_Status.GAME_STATUS_PLAY_FREEZE:
                    if (VARMAP_EnemyMaster.GET_OTHER_WORLD_TRANSITION_ACTIVE())
                    {
                        OtherWorldTransition();
                    }
                    break;

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
            VARMAP_EnemyMaster.UNREG_OTHER_WORLD(OnOtherWorldChanged);
            VARMAP_EnemyMaster.UNREG_GAMESTATUS(OnGameStatusChanged);
        }

        private void OtherWorldTransition()
        {
            float progress = VARMAP_EnemyMaster.GET_OTHER_WORLD_TRANSITION_PROGRESS();
            Color materialColor = myrenderer.material.color;

            if (VARMAP_EnemyMaster.GET_OTHER_WORLD() == belongsToOtherWorld)
            {
                myrenderer.material.color = new Color(materialColor.r, materialColor.g, materialColor.b, progress);
            }
            else
            {
                myrenderer.material.color = new Color(materialColor.r, materialColor.g, materialColor.b, 1f - progress);
            }
        }

        private void ComposeKinematicAndCollider()
        {
            bool collidercomposition = on_world && on_transition && collider_enable_other_reason;
            bool kinematiccomposition = (!(on_world && on_transition)) || kinematic_on_other_reason;

            bool prevColliderEnabled = mycollider.enabled;

            if (prevColliderEnabled && (!collidercomposition))
            {
                stored_velocity = myrigidbody.linearVelocity;
                stored_ang_velocity = myrigidbody.angularVelocity;
            }

            mycollider.enabled = collidercomposition;
            myrigidbody.isKinematic = kinematiccomposition;

            if ((!prevColliderEnabled) && collidercomposition)
            {
                myrigidbody.linearVelocity = stored_velocity;
                myrigidbody.angularVelocity = stored_ang_velocity;

                stored_velocity = Vector3.zero;
                stored_ang_velocity = Vector3.zero;
            }
        }


        private void OnOtherWorldChanged(ChangedEventType evtype, ref bool oldstatus, ref bool newstatus)
        {
            on_world = newstatus == belongsToOtherWorld;

            ComposeKinematicAndCollider();
        }

        private void OnGameStatusChanged(ChangedEventType evtype, ref Game_Status oldstatus, ref Game_Status newstatus)
        {
            if(newstatus == Game_Status.GAME_STATUS_PLAY)
            {
                on_transition = true;
            }
            else if(oldstatus == Game_Status.GAME_STATUS_PLAY)
            {
                on_transition = false;
            }

            ComposeKinematicAndCollider();
        }
    }
}