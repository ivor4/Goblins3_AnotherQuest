using Gob3AQ.FixedConfig;
using Gob3AQ.ResourceSoundsAtlas;
using Gob3AQ.VARMAP.CardMaster;
using Gob3AQ.VARMAP.Types;
using Gob3AQ.VARMAP.Types.Cards;
using Gob3AQ.VARMAP.Types.Delegates;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Gob3AQ.CardMaster
{
    [Serializable]
    public class CardMasterClass : MonoBehaviour
    {
        private static CardMasterClass _singleton;

        private int[] playerScore;
        private Queue<CardInfo> remainingDeckCards;
        private List<CardInfo>[] playerHandCards;
        private List<CardInfo>[] playerScoredCards;
        private CardSuit gameSuit;
        private CardInfo exposedSuitCard;
        private bool exposedSuitCardExchanged;
        private int currentPlayer;
        private bool playerWonLastCards;




        private void Awake()
        {
            if (_singleton != null)
            {
                Destroy(gameObject);
            }
            else
            {
                _singleton = this;
                playerScore = new int[GameFixedConfig.CARD_GAME_MAX_PLAYERS];
                remainingDeckCards = new Queue<CardInfo>((int)CardType.TOTAL_CARDS);
                playerHandCards = new List<CardInfo>[GameFixedConfig.CARD_GAME_MAX_PLAYERS];
                playerScoredCards = new List<CardInfo>[GameFixedConfig.CARD_GAME_MAX_PLAYERS];

                ResetGame();
            }

        }

        // Start is called before the first frame update
        private void Start()
        {
            VARMAP_CardMaster.REG_GAMESTATUS(_GameStatusChanged);
            VARMAP_CardMaster.MODULE_LOADING_COMPLETED(GameModules.MODULE_CardMaster);
        }



        private void OnDestroy()
        {
            if(_singleton == this)
            {
                _singleton = null;
                VARMAP_CardMaster.UNREG_GAMESTATUS(_GameStatusChanged);
            }
        }


        private void Update()
        {

        }

        private void ResetGame()
        {
            gameSuit = CardSuit.SUIT_NONE;
            exposedSuitCard = CardInfo.UNDEFINED_CARD;
            exposedSuitCardExchanged = false;
            currentPlayer = 0;
            playerWonLastCards = false;

            for(int i= 0; i < GameFixedConfig.CARD_GAME_MAX_PLAYERS; i++)
            {
                playerScore[i] = 0;
                playerHandCards[i].Clear();
                playerScoredCards[i].Clear();
            }
        }


        private void _GameStatusChanged(ChangedEventType evtype, in Game_Status oldval, in Game_Status newval)
        {
            _ = evtype;

            if (oldval != newval)
            {
                switch (newval)
                {
                    /* In this moment, actual room is next one. If bgMusic is not actual, stop sound */
                    case Game_Status.GAME_STATUS_CHANGING_ROOM:
                        break;
                    case Game_Status.GAME_STATUS_LOADING:
                        VARMAP_CardMaster.MODULE_LOADING_COMPLETED(GameModules.MODULE_CardMaster);
                        break;
                    /* Not always coming from loading, could come from Inventory or Dialog */
                    case Game_Status.GAME_STATUS_PLAY:
                        break;
                    case Game_Status.GAME_STATUS_STOPPED:
                        break;
                    default:
                        break;
                }
            }
        }
    }
}
