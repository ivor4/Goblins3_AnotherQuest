using UnityEngine;
using UnityEditor;
using Gob3AQ.VARMAP.PlayerMaster;
using Gob3AQ.VARMAP.Types;
using Gob3AQ.FixedConfig;
using System;
using Gob3AQ.Waypoint;

namespace Gob3AQ.GameElement.PlayableChar
{
    public enum PhysicalState
    {
        PHYSICAL_STATE_STANDING,
        PHYSICAL_STATE_TALKING,
        PHYSICAL_STATE_ACTING,
        PHYSICAL_STATE_ANIMATION
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
        private WaypointClass actualWaypoint;

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
            Vector3Struct posstruct = new Vector3Struct();

            posstruct.position = transform.position;
            VARMAP_PlayerMaster.SET_PLAYER_POSITION(posstruct);
        }


        private void ChangedSelectedPlayerEvent(ChangedEventType eventType, ref byte oldval, ref byte newval)
        {
            if(newval == playerID)
            {
                Debug.Log("I have been selected");
            }
        }


    }

}
