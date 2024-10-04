using UnityEngine;
using UnityEditor;
using Gob3AQ.VARMAP.PlayerMaster;
using Gob3AQ.VARMAP.Types;
using Gob3AQ.FixedConfig;
using System;
using Gob3AQ.Waypoint;
using Gob3AQ.Waypoint.Types;
using Gob3AQ.Libs.Arith;
using System.Collections.Generic;

namespace Gob3AQ.GameElement.PlayableChar
{
    public enum PhysicalState
    {
        PHYSICAL_STATE_STANDING = 0x0,
        PHYSICAL_STATE_TALKING = 0x1,
        PHYSICAL_STATE_ACTING = 0x2,
        PHYSICAL_STATE_WALKING = 0x4
    }


    [System.Serializable]
    public class PlayableCharScript : MonoBehaviour
    {
        [SerializeField]
        public WaypointClass initialWaypoint;


        /* Fields */
        public Collider2D Collider
        {
            get
            {
                return _collider;
            }
        }

        /* GameObject components */
        private SpriteRenderer _sprRenderer;
        private Collider2D _collider;

        /* Status */
        private PhysicalState physicalstate;

        private byte playerID;
        private bool selected;
        private WaypointClass actualWaypoint;
        private List<WaypointClass> wpsolution;


        public void MoveRequest(WaypointClass wp)
        {
            /* Move only if not talking or doing an action */
            if((physicalstate & (PhysicalState.PHYSICAL_STATE_TALKING | PhysicalState.PHYSICAL_STATE_ACTING)) == 0)
            {
                physicalstate = PhysicalState.PHYSICAL_STATE_WALKING;

                WaypointSolution solution = wp.Network.GetWaypointSolution(actualWaypoint, wp, WaypointSkillType.WAYPOINT_SKILL_NORMAL);

                if(solution.totalDistance == float.PositiveInfinity)
                {
                    Debug.LogError("Point is not reachable from actual waypoint");
                }
                else
                {

                }
            }
        }


        private void Awake()
        {
            _sprRenderer = GetComponent<SpriteRenderer>();
            _collider = GetComponent<Collider2D>();
        }

        private void Start()
        {
            physicalstate = PhysicalState.PHYSICAL_STATE_STANDING;
            actualWaypoint = initialWaypoint;
            transform.position = actualWaypoint.transform.position;
            selected = false;

            VARMAP_PlayerMaster.MONO_REGISTER(this, true, out playerID);
            VARMAP_PlayerMaster.REG_PLAYER_ID_SELECTED(ChangedSelectedPlayerEvent);
        }


        private void Update()
        {
            Execute_Play();
        }

        private void OnDestroy()
        {
            VARMAP_PlayerMaster.MONO_REGISTER(this, false, out _);
            VARMAP_PlayerMaster.UNREG_PLAYER_ID_SELECTED(ChangedSelectedPlayerEvent);
        }


        private void Execute_Play()
        {

        }


        private void ChangedSelectedPlayerEvent(ChangedEventType eventType, ref byte oldval, ref byte newval)
        {
            if(newval == playerID)
            {
                _sprRenderer.color = Color.red;
                selected = true;
            }
            else
            {
                _sprRenderer.color = Color.white;
                selected = false;
            }
        }


    }

}
