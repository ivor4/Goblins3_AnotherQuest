using UnityEngine;
using UnityEditor;
using Gob3AQ.VARMAP.PlayerMaster;
using Gob3AQ.VARMAP.Types;
using Gob3AQ.FixedConfig;
using System;

namespace Gob3AQ.GameElement.PlayableChar
{
    public enum PhysicalState
    {
        PHYSICAL_STATE_STANDING,
        PHYSICAL_STATE_TALKING,
        PHYSICAL_STATE_ACTING,
        PHYSICAL_STATE_ANIMATION
    }


    public class PlayableCharScript : MonoBehaviour
    {
        /* GameObject components */
        private SpriteRenderer myspriteRenderer;

        /* Status */
        private PhysicalState physicalstate;

        private void Awake()
        {
            myspriteRenderer = GetComponent<SpriteRenderer>();
        }

        private void Start()
        {
            physicalstate = PhysicalState.PHYSICAL_STATE_STANDING;

            VARMAP_PlayerMaster.MONO_REGISTER(this, true);
        }


        private void Update()
        {
            Execute_Play();
        }



        private void Execute_Play()
        {
            Vector3Struct posstruct = new Vector3Struct();

            posstruct.position = transform.position;
            VARMAP_PlayerMaster.SET_PLAYER_POSITION(posstruct);
        }


        private void OnDestroy()
        {
            VARMAP_PlayerMaster.MONO_REGISTER(this, false);
        }
    }

}
