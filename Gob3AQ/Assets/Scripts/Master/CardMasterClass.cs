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
        private const int MAX_HAND_CARDS = 3;

        private static CardMasterClass _singleton;

        /* INSTANCES */
        [SerializeField]
        private List<CardClass> card_resource_pool;

        [SerializeField]
        private List<Sprite> card_resource_sprites;

        [SerializeField]
        private Sprite card_resource_reverse_sprite;

        [SerializeField]
        private RectTransform card_placement_board;

        [SerializeField]
        private RectTransform card_placement_deck;

        [SerializeField]
        private RectTransform card_placement_exposed;

        [SerializeField]
        private RectTransform card_placement_p1_hand;

        [SerializeField]
        private RectTransform card_placement_p2_hand;

        private Queue<CardClass> card_instance_available;

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
        private ulong lastActionTimestamp;


        /* GAME STUP */
        private CardType imposedGameSuit;
        private CardType[] imposedOtherPlayerCards;
        private int imposedOtherPlayerCardNum;
        private int imposedStartingPlayer;
        private int gameDifficulty;
        private int numPlayers;


        public static void StartCardGameService(int difficulty, int startingPlayer, CardType othercard1, CardType othercard2, CardType othercard3, CardType gamesuit)
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

                for(int i= 0; i < MAX_HAND_CARDS; i++)
                {
                    if(_singleton.imposedOtherPlayerCards[i] != CardType.CARD_NONE)
                    {
                        _singleton.initialTrickedCards.Add(_singleton.imposedOtherPlayerCards[i]);
                        ++_singleton.imposedOtherPlayerCardNum;
                    }
                }

                _singleton.ShuffleDeck(_singleton.initialTrickedCards);

                _singleton.card_instance_deck.SetVisible(true);

                _singleton.gameMoment = CardGameMoment.GAME_MOMENT_DRAW_FIRST_CARDS;
                _singleton.momentSubStep = 0;
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

                for (int i = 0; i < card_resource_pool.Count; i++)
                {
                    card_resource_pool[i].SetOnClickCallback(Card_Clicked);
                }

                playerScore = new int[GameFixedConfig.CARD_GAME_MAX_PLAYERS];
                remainingDeckCards = new List<CardInfo>((int)CardType.TOTAL_CARDS);
                placedBoardCards = new List<CardBoardInfo>(GameFixedConfig.CARD_GAME_MAX_PLAYERS);
                playerHandCards = new List<CardInfo>[GameFixedConfig.CARD_GAME_MAX_PLAYERS];
                playerScoredCards = new List<CardInfo>[GameFixedConfig.CARD_GAME_MAX_PLAYERS];
                card_instances_playerHand = new CardClass[GameFixedConfig.CARD_GAME_MAX_PLAYERS, MAX_HAND_CARDS];
                card_instances_placedBoard = new CardClass[GameFixedConfig.CARD_GAME_MAX_PLAYERS];
                initialTrickedCards = new HashSet<CardType>(MAX_HAND_CARDS + 1);
                imposedOtherPlayerCards = new CardType[MAX_HAND_CARDS];

                for (int i= 0; i < GameFixedConfig.CARD_GAME_MAX_PLAYERS; i++)
                {
                    playerHandCards[i] = new List<CardInfo>(MAX_HAND_CARDS);
                    playerScoredCards[i] = new List<CardInfo>((int)CardType.TOTAL_CARDS);

                    dequeuedInstance = card_instance_available.Dequeue();
                    card_instances_placedBoard[i] = dequeuedInstance;

                    RectTransform handPlacement;

                    if(i==0)
                    {
                        handPlacement = card_placement_p1_hand;
                    }
                    else
                    {
                        handPlacement = card_placement_p2_hand;
                    }

                    for (int e = 0; e < MAX_HAND_CARDS; e++)
                    {
                        dequeuedInstance = card_instance_available.Dequeue();
                        card_instances_playerHand[i,e] = dequeuedInstance;
                    }
                }

                dequeuedInstance = card_instance_available.Dequeue();
                card_instance_exposedSuit = dequeuedInstance;


                dequeuedInstance = card_instance_available.Dequeue();
                card_instance_deck = dequeuedInstance;
            }
        }

        // Start is called before the first frame update
        private void Start()
        {
            VARMAP_CardMaster.REG_GAMESTATUS(_GameStatusChanged);
            ResetGame();
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
                    if ((timestamp - lastActionTimestamp) > 3000)
                    {
                        Game_Moment_ComputeRound(timestamp);
                    }
                    break;
                case CardGameMoment.GAME_MOMENT_DECIDE_EXCHANGE:
                    if(currentPlayer != 0)
                    {
                        Game_Action_StartNextDrawRound(currentPlayer);
                    }
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
            if(momentSubStep < (numPlayers * MAX_HAND_CARDS))
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

                if (playerToDraw == 0)
                {
                    handInstance.SetCardType(drawnCard.cardType, card_resource_sprites[(int)drawnCard.cardType], card_resource_reverse_sprite);
                    handInstance.SetFrontal(true);
                }
                else
                {
                    handInstance.SetCardType(CardType.CARD_NONE, card_resource_reverse_sprite, card_resource_reverse_sprite);
                    handInstance.SetFrontal(false);
                }
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

            card_instance_exposedSuit.SetCardType(exposedSuitCard.cardType, card_resource_sprites[(int)exposedSuitCard.cardType], card_resource_reverse_sprite);
            card_instance_exposedSuit.SetVisible(true);
            card_instance_exposedSuit.SetFrontal(true);

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
                        Game_Action_AI_Turn(currentPlayer, timestamp);
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
                    winningSuit = cardBoardInfo.cardInfo.cardSuit;
                    Debug.Log("Won by first card for player " + cardBoardInfo.playerID);
                }
                /* Change from a loser suit to game suit */
                else if ((cardBoardInfo.cardInfo.cardSuit == gameSuit) && (winningSuit != gameSuit))
                {
                    Debug.Log("Won by game suit for player " + cardBoardInfo.playerID);
                    winCycle = true;
                    winningSuit = cardBoardInfo.cardInfo.cardSuit;
                }
                /* A card of actual winning suit */
                else if (cardBoardInfo.cardInfo.cardSuit == winningSuit)
                {
                    /* Card has more value than highest in board stack */
                    if (cardBoardInfo.cardInfo.cardScore > winningScore)
                    {
                        Debug.Log("Won by same suit and score for player " + cardBoardInfo.playerID);
                        winCycle = true;
                    }
                    /* Card has score 0, but its number is higher */
                    else if ((cardBoardInfo.cardInfo.cardScore == winningScore) && (cardBoardInfo.cardInfo.cardValue > winningValue))
                    {
                        Debug.Log("Won by same suit, same score and higher value for player " + cardBoardInfo.playerID);
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

            Debug.Log("Player " + winningPlayer + " wins the round with score " + sumScore + " and total score " + playerScore[winningPlayer]);

            for (int i = 0; i < placedBoardCards.Count; i++)
            {
                Debug.Log(placedBoardCards[i].cardInfo.cardType);
                playerScoredCards[winningPlayer].Add(placedBoardCards[i].cardInfo);
                card_instances_placedBoard[i].SetCardType(CardType.CARD_NONE, card_resource_reverse_sprite, card_resource_reverse_sprite);
                card_instances_placedBoard[i].SetFrontal(false);
                card_instances_placedBoard[i].SetVisible(false);
            }

            placedBoardCards.Clear();

            if((remainingDeckCards.Count > 0)||(!exposedSuitCardDrawn))
            {
                if ((remainingDeckCards.Count > 0) && isExchangeAvailable(winningPlayer, out _))
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
                int playerToDraw = (momentSubStep + currentPlayer) % numPlayers;

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

                    card_instance_exposedSuit.SetCardType(CardType.CARD_NONE, card_resource_reverse_sprite, card_resource_reverse_sprite);
                    card_instance_exposedSuit.SetFrontal(false);
                    card_instance_exposedSuit.SetVisible(false);
                }

                playerHandCards[playerToDraw].Add(drawnCard);
                CardClass handInstance = card_instances_playerHand[playerToDraw, playerHandCards[playerToDraw].Count-1];
                if (playerToDraw == 0)
                {
                    handInstance.SetCardType(drawnCard.cardType, card_resource_sprites[(int)drawnCard.cardType], card_resource_reverse_sprite);
                    handInstance.SetFrontal(true);
                }
                else
                {
                    handInstance.SetCardType(CardType.CARD_NONE, card_resource_reverse_sprite, card_resource_reverse_sprite);
                    handInstance.SetFrontal(false);
                }
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

        private void Game_Action_AI_Turn(int player, ulong timestamp)
        {
            bool done = false;

            while (!done)
            {
                for (int i = 0; i < playerHandCards[player].Count; i++)
                {
                    if (UnityEngine.Random.value > 0.5f)
                    {
                        Game_Action_PlaceCard(player, i, timestamp);
                        done = true;
                        break;
                    }
                }
            }
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
                            Game_Action_PlaceCard(currentPlayer, handIndex, VARMAP_CardMaster.GET_ELAPSED_TIME_MS());
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

        private void Game_Action_PlaceCard(int player, int handIndex, ulong timestamp)
        {
            if ((playerHandCards[player].Count > handIndex) && (currentPlayer == player) && (gameMoment == CardGameMoment.GAME_MOMENT_PLAY) && (momentSubStep == 0))
            {
                int actualBoardCards = placedBoardCards.Count;
                CardInfo usedCard = playerHandCards[player][handIndex];

                card_instances_placedBoard[actualBoardCards].SetCardType(usedCard.cardType, card_resource_sprites[(int)usedCard.cardType], card_resource_reverse_sprite);
                card_instances_placedBoard[actualBoardCards].SetFrontal(true);
                card_instances_placedBoard[actualBoardCards].SetVisible(true);

                float random = UnityEngine.Random.value - 0.5f;
                float random2 = UnityEngine.Random.value - 0.5f;
                Vector3 position = card_placement_board.anchoredPosition + random * card_placement_board.rect.size;
                card_instances_placedBoard[actualBoardCards].SetPositionAndRotation(position, Quaternion.Euler(0, 0, 10f * random2));

                CardBoardInfo cardBoardInfo = new CardBoardInfo(usedCard, player);
                placedBoardCards.Add(cardBoardInfo);
                playerHandCards[player].RemoveAt(handIndex);

                for(int i = handIndex; i < MAX_HAND_CARDS; i++)
                {
                    if ((playerHandCards[player].Count > i) && (i < (MAX_HAND_CARDS-1)))
                    {
                        if (player == 0)
                        {
                            CardClass dstInstance = card_instances_playerHand[player, i];
                            dstInstance.SetCardType(playerHandCards[player][i].cardType, card_resource_sprites[(int)playerHandCards[player][i].cardType], card_resource_reverse_sprite);
                            dstInstance.SetFrontal(true);
                        }
                    }
                    else
                    {
                        card_instances_playerHand[player, i].SetCardType(CardType.CARD_NONE, card_resource_reverse_sprite, card_resource_reverse_sprite);
                        card_instances_playerHand[player, i].SetFrontal(false);
                        card_instances_playerHand[player, i].SetVisible(false);
                    }
                }

                currentPlayer = (currentPlayer + 1) % numPlayers;

                lastActionTimestamp = timestamp;

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
            if ((currentPlayer == player) && (gameMoment == CardGameMoment.GAME_MOMENT_DECIDE_EXCHANGE) && (momentSubStep == 0))
            {
                if (isExchangeAvailable(player, out int handIndex))
                {
                    CardInfo handCardInfo = playerHandCards[player][handIndex];
                    playerHandCards[player][handIndex] = exposedSuitCard;
                    exposedSuitCard = handCardInfo;

                    card_instance_exposedSuit.SetCardType(handCardInfo.cardType, card_resource_sprites[(int)handCardInfo.cardType], card_resource_reverse_sprite);
                    card_instance_exposedSuit.SetFrontal(true);

                    if (player == 0)
                    {
                        card_instances_playerHand[player, handIndex].SetCardType(playerHandCards[player][handIndex].cardType,
                            card_resource_sprites[(int)playerHandCards[player][handIndex].cardType], card_resource_reverse_sprite);
                        card_instances_playerHand[player, handIndex].SetFrontal(true);
                    }

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



            for (int i = 0; i < card_resource_pool.Count; i++)
            {
                card_resource_pool[i].SetCardType(CardType.CARD_NONE, card_resource_reverse_sprite, card_resource_reverse_sprite);
                card_resource_pool[i].SetVisible(false);
                card_resource_pool[i].SetFrontal(false);
            }

            card_instance_deck.SetPositionAndRotation(card_placement_deck.anchoredPosition, Quaternion.identity);
            card_instance_exposedSuit.SetPositionAndRotation(card_placement_exposed.anchoredPosition, Quaternion.Euler(0, 0, -90));

            for (int i= 0; i < GameFixedConfig.CARD_GAME_MAX_PLAYERS; i++)
            {
                playerScore[i] = 0;
                playerHandCards[i].Clear();
                playerScoredCards[i].Clear();

                card_instances_placedBoard[i].SetPositionAndRotation(card_placement_board.anchoredPosition, Quaternion.identity);

                RectTransform handPlacement;
                float baseRotation;
                float rotMult;

                if(i == 0)
                {
                    handPlacement = card_placement_p1_hand;
                    baseRotation = 0;
                    rotMult = -1f;
                }
                else
                {
                    handPlacement = card_placement_p2_hand;
                    baseRotation = 180;
                    rotMult = 1f;
                }

                for (int e=0; e < MAX_HAND_CARDS; e++)
                {
                    Vector3 position = handPlacement.anchoredPosition + new Vector2((e - 1) * handPlacement.rect.size.x / 2f, 0);
                    card_instances_playerHand[i,e].SetPositionAndRotation(position, Quaternion.Euler(0,0,baseRotation + (rotMult*(e-1)*45)));
                }
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
            for(int i = 0; i < MAX_HAND_CARDS; i++)
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

        private bool isExchangeAvailable(int player, out int handIndex)
        {
            bool available = false;
            handIndex = -1;

            if ((!exposedSuitCardExchanged) && (!exposedSuitCardDrawn))
            {
                int neededValue;

                if(exposedSuitCard.cardScore == 0)
                {
                    neededValue = 2;
                }
                else
                {
                    neededValue = 7;
                }

                for (int i = 0; i < playerHandCards[player].Count; i++)
                {
                    CardInfo handCardInfo = playerHandCards[player][i];
                    if ((handCardInfo.cardSuit == exposedSuitCard.cardSuit)&&(handCardInfo.cardValue == neededValue))
                    {
                        handIndex = i;
                        available = true;
                        break;
                    }
                }
            }


            return available;
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
