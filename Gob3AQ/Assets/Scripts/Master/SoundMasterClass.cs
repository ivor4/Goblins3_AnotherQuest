using Gob3AQ.FixedConfig;
using Gob3AQ.ResourceAtlas;
using Gob3AQ.VARMAP.SoundMaster;
using Gob3AQ.VARMAP.Types;
using Gob3AQ.VARMAP.Types.Delegates;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Gob3AQ.SoundMaster
{
    public class SoundMasterClass : MonoBehaviour
    {
        private static SoundMasterClass _singleton;


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
            VARMAP_SoundMaster.REG_GAMESTATUS(_GameStatusChanged);
            VARMAP_SoundMaster.MODULE_LOADING_COMPLETED(GameModules.MODULE_SoundMaster);
        }




        private void OnDestroy()
        {
            if(_singleton == this)
            {
                _singleton = null;

                VARMAP_SoundMaster.UNREG_GAMESTATUS(_GameStatusChanged);
            }
        }

        private void _GameStatusChanged(ChangedEventType evtype, in Game_Status oldval, in Game_Status newval)
        {
            _ = evtype;

            if (oldval != newval)
            {
                switch (newval)
                {
                    case Game_Status.GAME_STATUS_LOADING:
                        VARMAP_SoundMaster.MODULE_LOADING_COMPLETED(GameModules.MODULE_SoundMaster);
                        break;
                    default:
                        break;
                }
            }
        }
    }
}
