using Gob3AQ.VARMAP.Types;
using Gob3AQ.VARMAP.ItemMaster;
using Gob3AQ.ItemMaster;
using UnityEngine;
using Gob3AQ.GameElement.Item;
using System.Collections.Generic;
using Gob3AQ.FixedConfig;

namespace Gob3AQ.GameElement.Extension.RoosterPancho
{

    public class RoosterPanchoClass : MonoBehaviour, IGameElementExtension
    {
        private GameObject topParent;
        private ItemClass item_rooster;
        private Animator myAnimator;
        private IReadOnlyDictionary<GameItem, GameElementClass> scenarioItems;
        private bool activated;
        private bool gameStarted;
        private ulong startTimestamp;
        private bool scouting;

        void IGameElementExtension.OnDespawn()
        {
            /**/
        }

        void IGameElementExtension.OnExtensionDestroy()
        {
            ItemMasterClass.RemoveItemExtension(ItemExtensionFunction.ITEM_EXTENSION_ROOSTER_PANCHO_GAME_START);
        }

        void IGameElementExtension.OnReceiveExtFn(ItemExtensionFunction extFn, in NumberPack numberPack)
        {
            switch(extFn)
            {
                case ItemExtensionFunction.ITEM_EXTENSION_ROOSTER_PANCHO_GAME_START:
                    gameStarted = true;
                    startTimestamp = VARMAP_ItemMaster.GET_ELAPSED_TIME_MS();
                    VARMAP_ItemMaster.OBTAIN_SCENARIO_ITEMS(out scenarioItems);
                    break;
            }
        }

        void IGameElementExtension.OnSpawn()
        {
            activated = true;
        }

        private void Awake()
        {
            topParent = transform.parent.gameObject;
            item_rooster = GetComponent<ItemClass>();
            myAnimator = topParent.GetComponent<Animator>();

            activated = false;
            gameStarted = false;
        }

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        private void Start()
        {
            ItemMasterClass.AddItemExtension(ItemExtensionFunction.ITEM_EXTENSION_ROOSTER_PANCHO_GAME_START, this);
            
        }

        // Update is called once per frame
        private void Update()
        {
            if(activated && gameStarted)
            {
                ulong elapsedSinceStart = VARMAP_ItemMaster.GET_ELAPSED_TIME_MS() - startTimestamp;

                if((elapsedSinceStart > GameFixedConfig.ROOSTER_PANCHO_GAME_SCOUT_PERIOD_MS) && !scouting)
                {
                    scouting = true;
                    item_rooster.PerformAnimation(AnimationTrigger.ANIMATION_TRIGGER_FOUR, null, ScoutingEnded, false, null, true);
                }
            }
        }

        private void ScoutingEnded()
        {
            scouting = false;
            startTimestamp = VARMAP_ItemMaster.GET_ELAPSED_TIME_MS();
        }
    }
}