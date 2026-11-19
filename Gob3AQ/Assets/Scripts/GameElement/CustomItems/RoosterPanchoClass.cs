using Gob3AQ.FixedConfig;
using Gob3AQ.GameElement.Item;
using Gob3AQ.GameElement.PlayableChar;
using Gob3AQ.ItemMaster;
using Gob3AQ.Libs.Arith;
using Gob3AQ.VARMAP.ItemMaster;
using Gob3AQ.VARMAP.Types;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEditor.Animations;
using UnityEngine;

namespace Gob3AQ.GameElement.Extension.RoosterPancho
{

    public class RoosterPanchoClass : MonoBehaviour, IGameElementExtension
    {
        private enum RoosterGameStep
        {
            GAME_STEP_STOPPED,
            GAME_STEP_WAITING_SCOUT,
            GAME_STEP_SCOUTING_TURNING_RIGHT,
            GAME_STEP_SCOUTING_SCOUTING,
            GAME_STEP_TURNING_LEFT_FROM_SCOUTING,
            GAME_STEP_FLEEING_1,
            GAME_STEP_FLEEING_2,
            GAME_STEP_FLEEING_3
        }

        private ItemClass itemRooster;
        private IReadOnlyDictionary<GameItem, GameElementClass> scenarioItems;
        private RoosterGameStep gameStep;
        private bool activated;
        private ulong startTimestamp;
        private bool receivedAnimationEndFlag;
        private bool receivedAnimationCackleEndFlag;

        void IGameElementExtension.OnDespawn()
        {
            activated = false;
        }

        void IGameElementExtension.OnExtensionDestroy()
        {
            activated = false;
            ItemMasterClass.RemoveItemExtension(ItemExtensionFunction.ITEM_EXTENSION_ROOSTER_PANCHO_GAME_START);
        }

        void IGameElementExtension.OnReceiveExtFn(ItemExtensionFunction extFn, in NumberPack numberPack)
        {
            switch(extFn)
            {
                case ItemExtensionFunction.ITEM_EXTENSION_ROOSTER_PANCHO_GAME_START:
                    gameStep = RoosterGameStep.GAME_STEP_WAITING_SCOUT;
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
            itemRooster = GetComponent<ItemClass>();

            activated = false;
            gameStep = RoosterGameStep.GAME_STEP_STOPPED;
        }

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        private void Start()
        {
            ItemMasterClass.AddItemExtension(ItemExtensionFunction.ITEM_EXTENSION_ROOSTER_PANCHO_GAME_START, this);
            
        }

        // Update is called once per frame
        private void Update()
        {
            if(activated && (gameStep != RoosterGameStep.GAME_STEP_STOPPED))
            {
                /* Transitions based on animation end */
                if(receivedAnimationEndFlag)
                {
                    switch (gameStep)
                    {
                        case RoosterGameStep.GAME_STEP_SCOUTING_TURNING_RIGHT:
                            /* Scout */
                            itemRooster.PerformAnimation(AnimationTrigger.ANIMATION_TRIGGER_FOUR, null, ScoutAnimationEnded, false, null, false);
                            gameStep = RoosterGameStep.GAME_STEP_SCOUTING_SCOUTING;
                            break;

                        case RoosterGameStep.GAME_STEP_SCOUTING_SCOUTING:
                            /* Store left steady */
                            itemRooster.PerformAnimation(AnimationTrigger.ANIMATION_TRIGGER_STEADY_ONE, null, null, true, null, false);

                            /* Turn left */
                            itemRooster.PerformAnimation(AnimationTrigger.ANIMATION_TRIGGER_THREE, null, ScoutAnimationEnded, false, null, false);

                            gameStep = RoosterGameStep.GAME_STEP_TURNING_LEFT_FROM_SCOUTING;
                            break;

                        case RoosterGameStep.GAME_STEP_TURNING_LEFT_FROM_SCOUTING:
                            /* Scout end */
                            startTimestamp = VARMAP_ItemMaster.GET_ELAPSED_TIME_MS();
                            gameStep = RoosterGameStep.GAME_STEP_WAITING_SCOUT;
                            break;

                        case RoosterGameStep.GAME_STEP_FLEEING_1:
                            /* Fly */
                            if (receivedAnimationCackleEndFlag)
                            {
                                itemRooster.PerformAnimation(AnimationTrigger.ANIMATION_TRIGGER_CYCLE_ONE, null, ScoutAnimationEnded, false, null, false);
                                gameStep = RoosterGameStep.GAME_STEP_FLEEING_2;
                            }
                            break;

                        case RoosterGameStep.GAME_STEP_FLEEING_2:
                            /* Permission to flee */
                            gameStep = RoosterGameStep.GAME_STEP_FLEEING_3;
                            break;

                        default:
                            break;
                    }

                    receivedAnimationEndFlag = false;
                    receivedAnimationCackleEndFlag = false;
                }



                switch(gameStep)
                {
                    case RoosterGameStep.GAME_STEP_WAITING_SCOUT:
                        {
                            ulong elapsedSinceStart = VARMAP_ItemMaster.GET_ELAPSED_TIME_MS() - startTimestamp;

                            if (elapsedSinceStart > GameFixedConfig.ROOSTER_PANCHO_GAME_SCOUT_PERIOD_MS)
                            {
                                gameStep = RoosterGameStep.GAME_STEP_SCOUTING_TURNING_RIGHT;

                                /* Store right steady */
                                itemRooster.PerformAnimation(AnimationTrigger.ANIMATION_TRIGGER_STEADY_TWO, null, null, true, null, false);

                                /* Turn right */
                                itemRooster.PerformAnimation(AnimationTrigger.ANIMATION_TRIGGER_TWO, null, ScoutAnimationEnded, false, null, true);
                            }

                        }
                        break;

                    case RoosterGameStep.GAME_STEP_SCOUTING_TURNING_RIGHT:
                    case RoosterGameStep.GAME_STEP_SCOUTING_SCOUTING:
                        {
                            GameElementClass mainChar = scenarioItems[GameItem.ITEM_PLAYER_MAIN];

                            /* Spotted! */
                            if (mainChar.CurrentPhysicalState == GameElementPhysicalState.PHYSICAL_STATE_WALKING)
                            {
                                /* Stop scouting and flee */
                                gameStep = RoosterGameStep.GAME_STEP_FLEEING_1;

                                /* Cackle right */
                                itemRooster.PerformAnimation(AnimationTrigger.ANIMATION_TRIGGER_FIVE, null, CackleEnded, false, null, true);

                                GameAction cackle_sound = GameAction.ACTION_PLAY_SOUND_ROOSTER_SCARED;

                                ReadOnlySpan<GameAction> oneAction = MemoryMarshal.CreateReadOnlySpan(ref cackle_sound, 1);
                                VARMAP_ItemMaster.PERFORM_ACTION(oneAction, null);
                            }
                        }
                        break;

                    case RoosterGameStep.GAME_STEP_FLEEING_3:
                        itemRooster.transform.position += new Vector3(1, 0) * Time.deltaTime;
                        break;
                }
            }
        }

        private void ScoutAnimationEnded()
        {
            receivedAnimationEndFlag = true;
        }

        private void CackleEnded()
        {
            receivedAnimationCackleEndFlag = true;
            receivedAnimationEndFlag = true;
        }
    }
}