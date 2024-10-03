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

        
        private GameObject guicanvas;
        private Image[] guicanvas_hearts;
        private Text paused_text;
        private Camera mainCamera;
        private Transform mainCameraTransform;
        private Light sunLight;
        private byte cached_life;
        private byte cached_totallife;

        private Vector3 lastCharacterPos;


        public static Vector3 ScreenToWorldService(Vector3 screenPos)
        {
            return _singleton.mainCamera.ScreenToWorldPoint(screenPos);
        }

        public static Vector3 WorldToScreenService(Vector3 worldpos)
        {
            return _singleton.mainCamera.WorldToScreenPoint(worldpos);
        }

        public static Ray RayFromScreenService(Vector3 screenpos)
        {
            return _singleton.mainCamera.ScreenPointToRay(screenpos);
        }

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
            sunLight = GameObject.Find("SpotLight1").GetComponent<Light>();
            guicanvas = GameObject.Find("GUICanvas");

            guicanvas_hearts = new Image[GameFixedConfig.MAX_POSSIBLE_LIFE];

            for(int i=0; i < guicanvas_hearts.Length; i++)
            {
                guicanvas_hearts[i] = guicanvas.transform.Find("heart" + i.ToString()).GetComponent<Image>();
            }

            paused_text = guicanvas.transform.Find("paused").GetComponent<Text>();


            VARMAP_GraphicsMaster.REG_GAMESTATUS(_GameStatusChanged);

        }

        // Update is called once per frame
        void Update()
        {
            Game_Status gstatus = VARMAP_GraphicsMaster.GET_GAMESTATUS();

            switch(gstatus)
            {
                case Game_Status.GAME_STATUS_PLAY_FREEZE:
                    break;
                case Game_Status.GAME_STATUS_PLAY:
                    FollowPlayerWithCamera();
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

        private void FollowPlayerWithCamera()
        {
            Vector3 playerpos = VARMAP_GraphicsMaster.GET_PLAYER_POSITION().position;

            bool firstpersoncamera = false;

            if (!firstpersoncamera)
            {
                mainCameraTransform.position = new Vector3(playerpos.x, playerpos.y, mainCameraTransform.position.z);
            }
            else
            {
                /* First person camera LOL xD */
                if(playerpos.x < lastCharacterPos.x)
                {
                    mainCameraTransform.position = new Vector3(playerpos.x + 3, playerpos.y, playerpos.z);
                    mainCameraTransform.LookAt(playerpos, Vector3.up);
                }
                else if(playerpos.x > lastCharacterPos.x)
                {
                    mainCameraTransform.position = new Vector3(playerpos.x - 3, playerpos.y, playerpos.z);
                    mainCameraTransform.LookAt(playerpos, Vector3.up);
                }
                else
                {
                    /**/
                }
            }

            lastCharacterPos = playerpos;
        }



        private void _GameStatusChanged(ChangedEventType evtype, ref Game_Status oldval, ref Game_Status newval)
        {
            if(newval == Game_Status.GAME_STATUS_PAUSE)
            {
                paused_text.gameObject.SetActive(true);
            }
            else
            {
                paused_text.gameObject.SetActive(false);
            }
        }
    }
}
