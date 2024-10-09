using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Gob3AQ.VARMAP.Types;
using Gob3AQ.VARMAP.GraphicsMaster;
using Gob3AQ.FixedConfig;


namespace Gob3AQ.GraphicsMaster
{
    public class GraphicsMasterClass : MonoBehaviour
    {
        private static GraphicsMasterClass _singleton;
        
        private Camera mainCamera;
        private Transform mainCameraTransform;


        private void Awake()
        {
            if (_singleton != null)
            {
                Destroy(gameObject);
            }
            else
            {
                _singleton = this;
            }

        }

        // Start is called before the first frame update
        private void Start()
        {
            mainCamera = Camera.main;
            mainCameraTransform = mainCamera.transform;


            VARMAP_GraphicsMaster.REG_GAMESTATUS(_GameStatusChanged);

        }

        // Update is called once per frame
        void Update()
        {
            Game_Status gstatus = VARMAP_GraphicsMaster.GET_GAMESTATUS();

            MousePropertiesStruct mouse = VARMAP_GraphicsMaster.GET_MOUSE_PROPERTIES();

            Vector2 screenzone = new Vector2((float)mouse.posPixels.x / Screen.width, (float)mouse.posPixels.y / Screen.height);

            switch (gstatus)
            {
                case Game_Status.GAME_STATUS_PLAY_FREEZE:
                    break;
                case Game_Status.GAME_STATUS_PLAY:
                    FollowMouseWithCamera(ref screenzone);
                    break;
                case Game_Status.GAME_STATUS_PAUSE:
                    break;
            }
        }


        private void OnDestroy()
        {
            if(_singleton == this)
            {
                _singleton = null;

                VARMAP_GraphicsMaster.UNREG_GAMESTATUS(_GameStatusChanged);
            }
        }

        private void FollowMouseWithCamera(ref Vector2 screenzone)
        {
            /* If not in menu zone */
            if(screenzone.y < GameFixedConfig.GAME_ZONE_HEIGHT_PERCENT)
            {
                /* Expand Y of game zone to 100% */
                Vector2 szone = new Vector2(screenzone.x, screenzone.y * GameFixedConfig.GAME_ZONE_HEIGHT_FACTOR);

                
            }
        }



        private void _GameStatusChanged(ChangedEventType evtype, ref Game_Status oldval, ref Game_Status newval)
        {
            if(newval == Game_Status.GAME_STATUS_PAUSE)
            {
                //paused_text.gameObject.SetActive(true);
            }
            else
            {
                //paused_text.gameObject.SetActive(false);
            }
        }
    }
}
