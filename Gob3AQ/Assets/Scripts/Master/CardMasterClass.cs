using Gob3AQ.FixedConfig;
using Gob3AQ.ResourceSoundsAtlas;
using Gob3AQ.VARMAP.CardMaster;
using Gob3AQ.VARMAP.Types;
using Gob3AQ.VARMAP.Types.Cards;
using Gob3AQ.VARMAP.Types.Delegates;
using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace Gob3AQ.CardMaster
{
    [Serializable]
    public class CardMasterClass : MonoBehaviour
    {
        private static CardMasterClass _singleton;

        /* INSTANCES */
        [SerializeField]
        private List<CardClass> card_resource_pool;

        private Queue<CardClass> card_instance_available;
        private Queue<CardClass> card_instance_used;

        private CardClass card_instance_exposedSuit;
        private CardClass[] card_instances_placedBoard;
        private CardClass[,] card_instances_playerHand;
        private CardClass card_instance_deck;

        /* IN-GAME VARIABLES */
        private int[] playerScore;
        private HashSet<CardType> initialTrickedCards;
        private List<CardInfo> remainingDeckCards;
        private List<CardBoardInfo> placedBoardCards;
        private List<CardInfo>[] playerHandCards;
        private List<CardInfo>[] playerScoredCards;
        private CardSuit gameSuit;
        private CardInfo exposedSuitCard;
        private CardGameMoment gameMoment;
        private bool exposedSuitCardExchanged;
        private bool exposedSuitCardDrawn;
        private int currentPlayer;
        private byte momentSubStep;
        private int turnNum;


        /* GAME STUP */
        private CardType imposedGameSuit;
        private CardType[] imposedOtherPlayerCards;
        private int imposedOtherPlayerCardNum;
        private int imposedStartingPlayer;
        private int gameDifficulty;
        private int numPlayers;


        public static void StartGameService(int difficulty, int startingPlayer, CardType othercard1, CardType othercard2, CardType othercard3, CardType gamesuit)
        {
            if(_singleton != null)
            {
                _singleton.ResetGame();

                _singleton.gameDifficulty = difficulty;
                _singleton.imposedStartingPlayer = startingPlayer;
                _singleton.imposedOtherPlayerCards[0] = othercard1;
                _singleton.imposedOtherPlayerCards[1] = othercard2;
                _singleton.imposedOtherPlayerCards[2] = othercard3;
                _singleton.imposedGameSuit = gamesuit;
                _singleton.numPlayers = 2;


                if(gamesuit != CardType.CARD_NONE)
                {
                    _singleton.initialTrickedCards.Add(gamesuit);
                }

                for(int i= 0; i < 3; i++)
                {
                    if(_singleton.imposedOtherPlayerCards[i] != CardType.CARD_NONE)
                    {
                        _singleton.initialTrickedCards.Add(_singleton.imposedOtherPlayerCards[i]);
                        ++_singleton.imposedOtherPlayerCardNum;
                    }
                }

                _singleton.ShuffleDeck(_singleton.initialTrickedCards);
            }
        }


        private void Awake()
        {
            if (_singleton != null)
            {
                Destroy(gameObject);
            }
            else
            {
                CardClass dequeuedInstance;
                _singleton = this;

                card_instance_available = new Queue<CardClass>(card_resource_pool);
                card_instance_used = new Queue<CardClass>(card_resource_pool.Count);

                for (int i = 0; i < card_resource_pool.Count; i++)
                {
                    card_resource_pool[i].SetOnClickCallback(Card_Clicked);
                }

                playerScore = new int[GameFixedConfig.CARD_GAME_MAX_PLAYERS];
                remainingDeckCards = new List<CardInfo>((int)CardType.TOTAL_CARDS);
                placedBoardCards = new List<CardBoardInfo>(GameFixedConfig.CARD_GAME_MAX_PLAYERS);
                playerHandCards = new List<CardInfo>[GameFixedConfig.CARD_GAME_MAX_PLAYERS];
                playerScoredCards = new List<CardInfo>[GameFixedConfig.CARD_GAME_MAX_PLAYERS];
                card_instances_playerHand = new CardClass[GameFixedConfig.CARD_GAME_MAX_PLAYERS, 3];
                card_instances_placedBoard = new CardClass[GameFixedConfig.CARD_GAME_MAX_PLAYERS];
                initialTrickedCards = new HashSet<CardType>(4);
                imposedOtherPlayerCards = new CardType[3];

                for (int i= 0; i < GameFixedConfig.CARD_GAME_MAX_PLAYERS; i++)
                {
                    playerHandCards[i] = new List<CardInfo>(3);
                    playerScoredCards[i] = new List<CardInfo>((int)CardType.TOTAL_CARDS);

                    dequeuedInstance = card_instance_available.Dequeue();
                    card_instances_placedBoard[i] = dequeuedInstance;
                    card_instance_used.Enqueue(dequeuedInstance);

                    for (int e = 0; e < 3; e++)
                    {
                        dequeuedInstance = card_instance_available.Dequeue();
                        card_instances_playerHand[i,e] = dequeuedInstance;
                        card_instance_used.Enqueue(dequeuedInstance);
                    }
                }


                dequeuedInstance = card_instance_available.Dequeue();
                card_instance_deck = dequeuedInstance;
                card_instance_used.Enqueue(dequeuedInstance);

                dequeuedInstance = card_instance_available.Dequeue();
                card_instance_exposedSuit = dequeuedInstance;
                card_instance_used.Enqueue(dequeuedInstance);

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
            Game_Status gstatus = VARMAP_CardMaster.GET_GAMESTATUS();
            ulong timestamp = VARMAP_CardMaster.GET_ELAPSED_TIME_MS();

            switch(gstatus)
            {
                case Game_Status.GAME_STATUS_PLAY_CARDS:
                    Game_Cycle(timestamp);
                    break;
                default:
                    break;
            }
        }

        private void Game_Cycle(ulong timestamp)
        {
            switch(gameMoment)
            {
                case CardGameMoment.GAME_MOMENT_DRAW_FIRST_CARDS:
                    Game_Moment_DrawFirstCards(timestamp);
                    break;
                case CardGameMoment.GAME_MOMENT_EXPOSE_SUIT_CARD:
                    Game_Moment_ExposeSuitCard(timestamp);
                    break;
                case CardGameMoment.GAME_MOMENT_RANDOM_FIRST_TURN:
                    Game_Moment_RandomFirstTurn(timestamp);
                    break;
                case CardGameMoment.GAME_MOMENT_PLAY:
                    Game_Moment_Play(timestamp);
                    break;
                case CardGameMoment.GAME_MOMENT_COMPUTE_ROUND:
                    Game_Moment_ComputeRound(timestamp);
                    break;
                case CardGameMoment.GAME_MOMENT_DRAW:
                    Game_Moment_DrawNextRound(timestamp);
                    break;

                default:
                    break;
            }
        }

        private void Game_Moment_DrawFirstCards(ulong timestamp)
        {
            /* Suit card */
            if(momentSubStep < (numPlayers * 3))
            {
                CardInfo drawnCard;
                int playerToDraw = momentSubStep % numPlayers;
                int cardNumToDraw = momentSubStep / numPlayers;

                /* Tricked Cards */
                if ((playerToDraw == 1) && (cardNumToDraw < imposedOtherPlayerCardNum))
                {
                    drawnCard = CardInfo.GAME_CARDS[(int)imposedOtherPlayerCards[cardNumToDraw]];
                }
                /* Non-tricked Cards */
                else
                {
                    drawnCard = remainingDeckCards[remainingDeckCards.Count - 1];
                    remainingDeckCards.RemoveAt(remainingDeckCards.Count - 1);
                }

                playerHandCards[playerToDraw].Add(drawnCard);
                CardClass handInstance = card_instances_playerHand[playerToDraw, cardNumToDraw];
                handInstance.SetCardType(drawnCard.cardType, null, null);
                handInstance.SetVisible(true);

                /* Animate to player deck */

                /**/

                ++momentSubStep;
            }
            else
            {
                gameMoment = CardGameMoment.GAME_MOMENT_EXPOSE_SUIT_CARD;
                momentSubStep = 0;
            }
        }

        private void Game_Moment_ExposeSuitCard(ulong timestamp)
        {
            if (imposedGameSuit != CardType.CARD_NONE)
            {
                exposedSuitCard = CardInfo.GAME_CARDS[(int)imposedGameSuit];
                gameSuit = exposedSuitCard.cardSuit;
            }
            else
            {
                exposedSuitCard = remainingDeckCards[remainingDeckCards.Count - 1];
                remainingDeckCards.RemoveAt(remainingDeckCards.Count - 1);
                gameSuit = exposedSuitCard.cardSuit;
            }

            card_instance_exposedSuit.SetCardType(exposedSuitCard.cardType, null, null);
            card_instance_exposedSuit.SetVisible(true);

            /* Animate to center */

            /**/

            gameMoment = CardGameMoment.GAME_MOMENT_RANDOM_FIRST_TURN;
            momentSubStep = 0;
        }

        private void Game_Moment_RandomFirstTurn(ulong timestamp)
        {
            if(imposedStartingPlayer != -1)
            {
                currentPlayer = imposedStartingPlayer;
            }
            else
            {
                currentPlayer = (int)(UnityEngine.Random.value * numPlayers) % numPlayers;
            }
    
            /* Animate to show first player */
    
            /**/
    
            gameMoment = CardGameMoment.GAME_MOMENT_PLAY;
            momentSubStep = 0;
        }

        private void Game_Moment_Play(ulong timestamp)
        {
            /* Animations and AI */
            switch(momentSubStep)
            {
                case 0:
                    if (turnNum == numPlayers)
                    {
                        turnNum = 0;
                        momentSubStep = 0;
                        gameMoment = CardGameMoment.GAME_MOMENT_COMPUTE_ROUND;
                    }
                    else if (playerHandCards[currentPlayer].Count == 0)
                    {
                        currentPlayer = (currentPlayer + 1) % numPlayers;
                        ++turnNum;
                    }
                    else if (currentPlayer != 0)
                    {
                        Game_Action_AI_Turn(currentPlayer);
                    }
                    else
                    {
                        /**/
                    }
                    break;
                default:
                    break;
            }
        }

        private void Game_Moment_ComputeRound(ulong timestamp)
        {
            CardSuit winningSuit = CardSuit.SUIT_NONE;
            int winningPlayer = -1;
            int winningScore = -1;
            int winningValue = -1;
            int sumScore = 0;

            for(int i=0; i < placedBoardCards.Count; i++)
            {
                CardBoardInfo cardBoardInfo = placedBoardCards[i];
                sumScore += cardBoardInfo.cardInfo.cardScore;
                bool winCycle;

                /* First card */
                if(winningSuit == CardSuit.SUIT_NONE)
                {
                    winCycle = true;
                }
                /* Change from a loser suit to game suit */
                else if ((cardBoardInfo.cardInfo.cardSuit == gameSuit) && (winningSuit != gameSuit))
                {
                    winCycle = true;
                }
                /* A card of actual winning suit */
                else if (cardBoardInfo.cardInfo.cardSuit == winningSuit)
                {
                    /* Card has more value than highest in board stack */
                    if (cardBoardInfo.cardInfo.cardScore > winningScore)
                    {
                        winCycle = true;
                    }
                    /* Card has score 0, but its number is higher */
                    else if ((cardBoardInfo.cardInfo.cardScore == winningScore) && (cardBoardInfo.cardInfo.cardValue > winningValue))
                    {
                        winCycle = true;
                    }
                    /* A card with less score or numeric value */
                    else
                    {
                        winCycle = false;
                    }
                }
                /* A card from a losing suit against a higher placed */
                else
                {
                    winCycle = false;
                }

                if (winCycle)
                {
                    winningPlayer = cardBoardInfo.playerID;
                    winningScore = cardBoardInfo.cardInfo.cardScore;
                    winningValue = cardBoardInfo.cardInfo.cardValue;
                }
            }

            playerScore[winningPlayer] += sumScore;

            for (int i = 0; i < placedBoardCards.Count; i++)
            {
                playerScoredCards[winningPlayer].Add(placedBoardCards[i].cardInfo);
                card_instances_placedBoard[i].SetCardType(CardType.CARD_NONE, null, null);
                card_instances_placedBoard[i].SetVisible(false);
            }

            placedBoardCards.Clear();

            if((remainingDeckCards.Count > 0)||(!exposedSuitCardDrawn))
            {
                if ((!exposedSuitCardExchanged) && (remainingDeckCards.Count > 0))
                {
                    gameMoment = CardGameMoment.GAME_MOMENT_DECIDE_EXCHANGE;
                    momentSubStep = 0;
                }
                else
                {
                    gameMoment = CardGameMoment.GAME_MOMENT_DRAW;
                    momentSubStep = 0;
                }
                currentPlayer = winningPlayer;
            }
            else
            {
                int handCards = 0;
                for(int i=0; i < numPlayers; i++)
                {
                    handCards += playerHandCards[i].Count;
                }

                if (handCards > 0)
                {
                    gameMoment = CardGameMoment.GAME_MOMENT_PLAY;
                    momentSubStep = 0;
                    currentPlayer = winningPlayer;
                }
                else
                {
                    gameMoment = CardGameMoment.GAME_MOMENT_FINAL_RESULT;
                    momentSubStep = 0;
                }
            }
        }

        private void Game_Moment_DrawNextRound(ulong timestamp)
        {
            /* Suit card */
            if (((remainingDeckCards.Count > 0) || (!exposedSuitCardDrawn)) && (momentSubStep < numPlayers))
            {
                CardInfo drawnCard;
                int playerToDraw = momentSubStep % numPlayers;

                if (remainingDeckCards.Count > 0)
                {
                    drawnCard = remainingDeckCards[remainingDeckCards.Count - 1];
                    remainingDeckCards.RemoveAt(remainingDeckCards.Count - 1);

                    if(remainingDeckCards.Count == 0)
                    {
                        card_instance_deck.SetVisible(false);
                    }
                }
                else
                {
                    drawnCard = exposedSuitCard;
                    exposedSuitCardDrawn = true;

                    card_instance_exposedSuit.SetCardType(CardType.CARD_NONE, null, null);
                    card_instance_exposedSuit.SetVisible(false);
                }

                playerHandCards[playerToDraw].Add(drawnCard);
                CardClass handInstance = card_instances_playerHand[playerToDraw, playerHandCards[playerToDraw].Count-1];
                handInstance.SetCardType(drawnCard.cardType, null, null);
                handInstance.SetVisible(true);

                /* Animate to player deck */

                /**/

                ++momentSubStep;
            }
            else
            {
                gameMoment = CardGameMoment.GAME_MOMENT_PLAY;
                momentSubStep = 0;
            }
        }

        private void Game_Action_AI_Turn(int player)
        {

        }

        private void Card_Clicked(CardClass instance)
        {
            if ((momentSubStep == 0) && instance.IsClickable)
            {
                if (currentPlayer == 0)
                {
                    /* Use hand card */
                    if (gameMoment == CardGameMoment.GAME_MOMENT_PLAY)
                    {
                        if (isCardInPlayerHand(instance, currentPlayer, out int handIndex))
                        {
                            Game_Action_PlaceCard(currentPlayer, handIndex);
                        }
                    }
                    else if (gameMoment == CardGameMoment.GAME_MOMENT_DECIDE_EXCHANGE)
                    {
                        if(((card_instance_deck == instance)&&(remainingDeckCards.Count > 0))||
                            ((card_instance_exposedSuit == instance) && (remainingDeckCards.Count == 0)))
                        {
                            Game_Action_StartNextDrawRound(currentPlayer);
                        }
                        else if(card_instance_exposedSuit == instance)
                        {
                            Game_Action_ExchangeExposedSuitCard(currentPlayer);
                        }
                        else
                        {
                            /**/
                        }
                    }
                    else
                    {
                        /**/
                    }
                }
            }
        }

        private void Game_Action_PlaceCard(int player, int handIndex)
        {
            if ((playerHandCards[player].Count > handIndex) && (currentPlayer == player) && (gameMoment == CardGameMoment.GAME_MOMENT_PLAY) && (momentSubStep == 0))
            {
                int actualBoardCards = placedBoardCards.Count;
                CardInfo usedCard = playerHandCards[player][handIndex];

                card_instances_playerHand[player, handIndex].SetCardType(CardType.CARD_NONE, null, null);
                card_instances_playerHand[player, handIndex].SetVisible(false);

                card_instances_placedBoard[actualBoardCards].SetCardType(usedCard.cardType, null, null);
                card_instances_placedBoard[actualBoardCards].SetVisible(true);

                CardBoardInfo cardBoardInfo = new CardBoardInfo(usedCard, player);
                placedBoardCards.Add(cardBoardInfo);
                playerHandCards[player].RemoveAt(handIndex);

                currentPlayer = (currentPlayer + 1) % numPlayers;

                ++turnNum;
            }
        }

        private void Game_Action_StartNextDrawRound(int player)
        {
            if ((gameMoment == CardGameMoment.GAME_MOMENT_DECIDE_EXCHANGE) && (currentPlayer == player) && (momentSubStep == 0))
            {
                gameMoment = CardGameMoment.GAME_MOMENT_DRAW;
                momentSubStep = 0;
            }
        }

        private void Game_Action_ExchangeExposedSuitCard(int player)
        {
            if ((currentPlayer == player) && (gameMoment == CardGameMoment.GAME_MOMENT_DECIDE_EXCHANGE) && (!exposedSuitCardExchanged) && (momentSubStep == 0))
            {
                bool success = false;
                int searchValue;

                /* If it is a valuable card */
                if(exposedSuitCard.cardScore > 0)
                {
                    searchValue = 7;
                }
                else
                {
                    searchValue = 2;
                }

                for(int i=0; i < playerHandCards[player].Count; i++)
                {
                    CardInfo handCardInfo = playerHandCards[player][i];
                    if ((handCardInfo.cardValue == searchValue)&&(handCardInfo.cardSuit == gameSuit))
                    {
                        /* Exchange cards */
                        playerHandCards[player][i] = exposedSuitCard;
                        exposedSuitCard = handCardInfo;
                        card_instances_playerHand[player, i].SetCardType(playerHandCards[player][i].cardType, null, null);

                        /* Animate exchange */
                        /**/
                        success = true;
                        break;
                    }
                }

                if (success)
                {
                    exposedSuitCardExchanged = true;
                    gameMoment = CardGameMoment.GAME_MOMENT_DRAW;
                    momentSubStep = 0;
                }
            }
        }

        private void ResetGame()
        {
            gameSuit = CardSuit.SUIT_NONE;
            exposedSuitCard = CardInfo.UNDEFINED_CARD;
            exposedSuitCardExchanged = false;
            exposedSuitCardDrawn = false;
            currentPlayer = 0;
            numPlayers = 2;
            turnNum = 0;
            gameMoment = CardGameMoment.GAME_MOMENT_STOP;
            momentSubStep = 0;
            remainingDeckCards.Clear();
            initialTrickedCards.Clear();
            placedBoardCards.Clear();

            for (int i= 0; i < GameFixedConfig.CARD_GAME_MAX_PLAYERS; i++)
            {
                playerScore[i] = 0;
                playerHandCards[i].Clear();
                playerScoredCards[i].Clear();
            }

            imposedGameSuit = CardType.CARD_NONE;
            imposedOtherPlayerCards[0] = CardType.CARD_NONE;
            imposedOtherPlayerCards[1] = CardType.CARD_NONE;
            imposedOtherPlayerCards[2] = CardType.CARD_NONE;
            imposedOtherPlayerCardNum = 0;

            imposedStartingPlayer = -1;
            gameDifficulty = 0;
        }

        private void ShuffleDeck(HashSet<CardType> alreadyTakenCards)
        {
            UnityEngine.Random.InitState((int)DateTime.Now.Ticks);

            for (int i = 0; i < (int)CardType.TOTAL_CARDS; i++)
            {
                if (!alreadyTakenCards.Contains((CardType)i))
                {
                    int position = (int)(UnityEngine.Random.value * remainingDeckCards.Count);
                    if((position > 0) && (position >= remainingDeckCards.Count))
                    {
                        position = remainingDeckCards.Count - 1;
                    }

                    remainingDeckCards.Insert(position, CardInfo.GAME_CARDS[i]);
                }
            }
        }

        private bool isCardInPlayerHand(CardClass instance, int player, out int handIndex)
        {
            for(int i = 0; i < 3; i++)
            {
                if(card_instances_playerHand[player, i] == instance)
                {
                    handIndex = i;
                    return true;
                }
            }
            handIndex = -1;
            return false;
        }


        private void _GameStatusChanged(ChangedEventType evtype, in Game_Status oldval, in Game_Status newval)
        {
            _ = evtype;

            if (oldval != newval)
            {
                switch (newval)
                {
                    case Game_Status.GAME_STATUS_LOADING:
                        VARMAP_CardMaster.MODULE_LOADING_COMPLETED(GameModules.MODULE_CardMaster);
                        break;
                    default:
                        break;
                }

                switch (oldval)
                {
                    case Game_Status.GAME_STATUS_PLAY_CARDS:
                        ResetGame();
                        break;
                    default:
                        break;
                }
            }
        }
    }
}
