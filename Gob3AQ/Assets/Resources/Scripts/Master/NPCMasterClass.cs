using UnityEditor;
using UnityEngine;
using Gob3AQ.GameElement.NPC;
using Gob3AQ.VARMAP.NPCMaster;
using Gob3AQ.Libs.Arith;
using Gob3AQ.VARMAP.Types;
using Gob3AQ.FixedConfig;
using System.Collections.Generic;
using System;

namespace Gob3AQ.NPCMaster
{
    public class NPCMasterClass : MonoBehaviour
    {

        private static NPCMasterClass _singleton;


        public static void InteractPlayerNPCService(CharacterType character, int npcindex)
        {
            NPCClass npc = GetNPCInstance(npcindex);

            npc.InteractWithPlayer(character);
        }


        private static NPCClass GetNPCInstance(int index)
        {
            NPCClass selectedNPC;

            VARMAP_NPCMaster.GET_NPC_LIST(out ReadOnlyList<NPCClass> npclist);

            selectedNPC = npclist[index];

            return selectedNPC;
        }


        private void Awake()
        {
            if (_singleton)
            {
                Destroy(this);
                return;
            }
            else
            {
                _singleton = this;
            }
        }

        void Start()
        {
            
        }



        private void OnDestroy()
        {
            if (_singleton == this)
            {
                _singleton = null;
            }
        }
    }
}