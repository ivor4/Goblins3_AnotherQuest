using Gob3AQ.FixedConfig;
using Gob3AQ.VARMAP.CardMaster;
using Gob3AQ.VARMAP.Types;
using Gob3AQ.VARMAP.Types.Cards;
using System;
using System.Collections.Generic;
using Gob3AQ.GameElement.Item.Card;
using UnityEngine;

namespace Gob3AQ.CardMaster
{
    [Serializable]
    public class CardMasterClass : MonoBehaviour
    {
        private static readonly IReadOnlyList<GameSound> CARD_PLACE_SOUNDS = new List<GameSound>()
        {
            GameSound.SOUND_CARD_PLACE_1,
            GameSound.SOUND_CARD_PLACE_2,
            GameSound.SOUND_CARD_PLACE_3
        };
        private const int MAX_HAND_CARDS = 3;
        private const ulong COMPUTE_ROUND_DELAY_MS = 500;
        
        /* AI CONSTANTS */
        private const int AI_SCORE_TRUMP_PENALTY = 400;
        private const int AI_BASE_WORST_SCORE = -10000;
        private const int AI_HARD_GUARANTEED_PLAY_THRESHOLD = 2000;
        private const int AI_HARD_SUIT_PREFERENCE_MARGIN = 1000;

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
        
        private CardClass card_animated;

        private int exchange_hand_index;

        // Temporary instance for animations
        private CardClass card_instance_anim;

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
        private bool isExposedSuitCardExchanged;
        private bool isExposedSuitCardDrawn;
        private int currentPlayer;
        private byte momentSubStep;
        private int turnNum;
        private ulong lastActionTimestamp;

        private int round_winningPlayer;
        private CardSuit round_winningSuit;
        private int round_winningScore;
        private int round_winningValue;
        private int round_accumScore;


        /* GAME SETUP */
        private CardType imposedGameSuit;
        private CardType[] imposedOtherPlayerCards;
        private int imposedOtherPlayerCardNum;
        private int imposedStartingPlayer;
        private int gameDifficulty;
        private int numPlayers;


        public static void StartCardGameService(int difficulty, int startingPlayer, CardType othercard1, CardType othercard2, CardType othercard3, CardType gamesuit)
        {
            if (!_singleton) return;
            
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
                if (_singleton.imposedOtherPlayerCards[i] == CardType.CARD_NONE) continue;
                
                _singleton.initialTrickedCards.Add(_singleton.imposedOtherPlayerCards[i]);
                ++_singleton.imposedOtherPlayerCardNum;
            }

            _singleton.ShuffleDeck(_singleton.initialTrickedCards);

            _singleton.card_instance_deck.SetVisible(true);

            _singleton.gameMoment = CardGameMoment.GAME_MOMENT_RANDOM_FIRST_TURN;
            _singleton.momentSubStep = 0;
        }


        private void Awake()
        {
            if (_singleton)
            {
                Destroy(gameObject);
            }
            else
            {
                CardClass dequeuedInstance;
                _singleton = this;

                card_instance_available = new Queue<CardClass>(card_resource_pool);

                foreach (var t in card_resource_pool)
                {
                    t.SetOnClickCallback(Card_Clicked);
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

                /* First assign board cards. They will be placed in deepest draw order */
                for (int i= 0; i < GameFixedConfig.CARD_GAME_MAX_PLAYERS; i++)
                {
                    dequeuedInstance = card_instance_available.Dequeue();
                    card_instances_placedBoard[i] = dequeuedInstance;
                }

                for(int i = 0; i < GameFixedConfig.CARD_GAME_MAX_PLAYERS; i++)
                {
                    playerHandCards[i] = new List<CardInfo>(MAX_HAND_CARDS);
                    playerScoredCards[i] = new List<CardInfo>((int)CardType.TOTAL_CARDS);

                    for (int e = 0; e < MAX_HAND_CARDS; e++)
                    {
                        dequeuedInstance = card_instance_available.Dequeue();
                        card_instances_playerHand[i, e] = dequeuedInstance;
                    }
                }

                dequeuedInstance = card_instance_available.Dequeue();
                card_instance_exposedSuit = dequeuedInstance;


                dequeuedInstance = card_instance_available.Dequeue();
                card_instance_deck = dequeuedInstance;

                dequeuedInstance = card_instance_available.Dequeue();
                card_instance_anim = dequeuedInstance;
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
            if (_singleton != this) return;
            
            _singleton = null;
            VARMAP_CardMaster.UNREG_GAMESTATUS(_GameStatusChanged);
        }


        private void Update()
        {
            Game_Status status = VARMAP_CardMaster.GET_GAMESTATUS();

            if (status != Game_Status.GAME_STATUS_PLAY_CARDS) return;
            
            ulong timestamp = VARMAP_CardMaster.GET_ELAPSED_TIME_MS();
            Game_Cycle(timestamp);
        }

        private void Game_Cycle(ulong timestamp)
        {
            if (card_animated && card_animated.IsAnimating)
            {
                card_animated.DoAnimationStep();
                return;
            }

            card_animated = null;
            
            switch(gameMoment)
            {
                case CardGameMoment.GAME_MOMENT_RANDOM_FIRST_TURN:
                    Game_Moment_RandomFirstTurn(timestamp);
                    break;
                case CardGameMoment.GAME_MOMENT_DRAW_FIRST_CARDS:
                    Game_Moment_DrawFirstCards(timestamp);
                    break;
                case CardGameMoment.GAME_MOMENT_EXPOSE_SUIT_CARD:
                    Game_Moment_ExposeSuitCard(timestamp);
                    break;
                case CardGameMoment.GAME_MOMENT_PLAY:
                    Game_Moment_Play(timestamp);
                    break;
                case CardGameMoment.GAME_MOMENT_PLACE_CARD_ANIM:
                {
                    int actualBoardCards = placedBoardCards.Count - 1;
                    CardInfo usedCard = placedBoardCards[actualBoardCards].cardInfo;
                    card_instances_placedBoard[actualBoardCards]
                        .SetSprites(card_resource_sprites[(int)usedCard.cardType], card_resource_reverse_sprite);
                    card_instances_placedBoard[actualBoardCards].SetFrontalAndStopMotion(true);
                    card_instances_placedBoard[actualBoardCards].SetVisible(true);
                    card_instance_anim.SetVisible(false);

                    gameMoment = CardGameMoment.GAME_MOMENT_PLAY;
                }
                    break;
                case CardGameMoment.GAME_MOMENT_COMPUTE_ROUND:
                    if ((timestamp - lastActionTimestamp) > COMPUTE_ROUND_DELAY_MS)
                    {
                        Game_Moment_ComputeRound(timestamp);
                    }
                    break;
                case CardGameMoment.GAME_MOMENT_DECIDE_EXCHANGE:
                    if(currentPlayer != 0)
                    {
                        Game_Action_ExchangeExposedSuitCard(currentPlayer);
                    }
                    break;
                case CardGameMoment.GAME_MOMENT_EXCHANGE_ANIM:
                    Game_Moment_Exchange_Anim(timestamp);
                    break;
                case CardGameMoment.GAME_MOMENT_DRAW:
                    Game_Moment_DrawNextRound(timestamp);
                    break;

                default:
                    /* Final result */
                    break;
            }
        }

        private void Game_Moment_RandomFirstTurn(ulong timestamp)
        {
            if (imposedStartingPlayer != -1)
            {
                currentPlayer = imposedStartingPlayer;
            }
            else
            {
                currentPlayer = (int)(UnityEngine.Random.value * numPlayers) % numPlayers;
            }

            /* Animate to show first player */

            /**/
            gameMoment = CardGameMoment.GAME_MOMENT_DRAW_FIRST_CARDS;
            momentSubStep = 0;
        }

        private void Game_Moment_DrawFirstCards(ulong timestamp)
        {
            /* After animation pop-up */
            if (momentSubStep > 0)
            {
                /* Retrieve last operation info */
                int prevMomentSubStep = momentSubStep - 1;
                int playerToDraw = (currentPlayer + prevMomentSubStep) % numPlayers;
                int cardNumToDraw = prevMomentSubStep / numPlayers;
                CardClass handInstance = card_instances_playerHand[playerToDraw, cardNumToDraw];
                
                /* Make hand instance appear */
                CardInfo prevCard = playerHandCards[playerToDraw][cardNumToDraw];
                handInstance.SetSprites( playerToDraw == 0 ? card_resource_sprites[(int)prevCard.cardType] : card_resource_reverse_sprite, card_resource_reverse_sprite);
                handInstance.SetFrontalAndStopMotion(playerToDraw == 0);
                handInstance.SetVisible(true);
                
                card_instance_anim.SetVisible(false);
            }

            /* One card per cycle */
            if(momentSubStep < (numPlayers * MAX_HAND_CARDS))
            {
                CardInfo drawnCard;
                int playerToDraw = (currentPlayer + momentSubStep) % numPlayers;
                int cardNumToDraw = momentSubStep / numPlayers;

                /* Tricked Cards */
                if ((playerToDraw == 1) && (cardNumToDraw < imposedOtherPlayerCardNum))
                {
                    drawnCard = CardInfo.GAME_CARDS[(int)imposedOtherPlayerCards[cardNumToDraw]];
                }
                /* Non-tricked Cards */
                else
                {
                    drawnCard = remainingDeckCards[^1];
                    remainingDeckCards.RemoveAt(remainingDeckCards.Count - 1);
                }

                playerHandCards[playerToDraw].Add(drawnCard);


                card_animated = card_instance_anim;
                card_instance_anim.SetPositionAndRotation(card_instance_deck.AnchoredPosition, card_instance_deck.ActualQuaternion);
                card_instance_anim.SetVisible(true);
                card_instance_anim.SetFrontalAndStopMotion(false);
                Sprite frontalSprite;
                if(playerToDraw == 0)
                {
                    card_instance_anim.DoFlip(true, 0.5f);
                    frontalSprite = card_resource_sprites[(int)drawnCard.cardType];
                }
                else
                {
                    frontalSprite = card_resource_reverse_sprite;
                }
                card_instance_anim.SetSprites(frontalSprite, card_resource_reverse_sprite);
                card_instance_anim.SetTargetPositionAndRotation(card_instances_playerHand[playerToDraw, cardNumToDraw].AnchoredPosition, 
                    card_instances_playerHand[playerToDraw, cardNumToDraw].ActualQuaternion, 0.5f);

                PlayRandomCardPlaceSound();

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
            switch (momentSubStep)
            {
                case 0:
                {
                    if (imposedGameSuit != CardType.CARD_NONE)
                    {
                        exposedSuitCard = CardInfo.GAME_CARDS[(int)imposedGameSuit];
                    }
                    else
                    {
                        exposedSuitCard = remainingDeckCards[^1];
                        remainingDeckCards.RemoveAt(remainingDeckCards.Count - 1);
                    }

                    gameSuit = exposedSuitCard.cardSuit;
                    
                    card_animated = card_instance_anim;
                    card_instance_anim.SetPositionAndRotation(card_instance_deck.AnchoredPosition, card_instance_deck.ActualQuaternion);
                    card_instance_anim.SetVisible(true);
                    card_instance_anim.SetFrontalAndStopMotion(false);
                    card_instance_anim.SetSprites(card_resource_sprites[(int)exposedSuitCard.cardType], card_resource_reverse_sprite);
                    Vector2 finalPosition =
                        (card_placement_deck.anchoredPosition + card_placement_board.anchoredPosition) * 0.5f;
                    card_instance_anim.SetTargetPositionAndRotation(finalPosition, card_instance_exposedSuit.ActualQuaternion, 0.5f);
                    card_instance_anim.DoFlip(true, 0.5f);
                    
                    PlayRandomCardPlaceSound();
                    
                    ++momentSubStep;
                } break;
                
                case 1:
                {
                    card_instance_anim.SetVisible(false);
                    
                    card_animated = card_instance_exposedSuit;
                    card_instance_exposedSuit.SetSprites( card_resource_sprites[(int)exposedSuitCard.cardType], card_resource_reverse_sprite);
                    card_instance_exposedSuit.SetVisible(true);
                    card_instance_exposedSuit.SetFrontalAndStopMotion(true);
                    card_instance_exposedSuit.SetPositionAndRotation(card_instance_anim.AnchoredPosition, card_instance_exposedSuit.ActualQuaternion);
                    card_instance_exposedSuit.SetTargetPositionAndRotation(card_placement_exposed.anchoredPosition, card_instance_exposedSuit.ActualQuaternion, 0.5f);
                    ++momentSubStep;
                }
                break;

                default:
                {
                    NextRound();
                } break;
            }
        }

        private void Game_Moment_Play(ulong timestamp)
        {
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
        }

        private void Game_Moment_ComputeRound(ulong timestamp)
        {
            _ = timestamp;

            int indexToRemove = placedBoardCards.Count - 1;
            
            if (indexToRemove >= 0)
            {
                CardType removedCardType = placedBoardCards[indexToRemove].cardInfo.cardType;
                placedBoardCards.RemoveAt(indexToRemove);
                
                card_animated = card_instance_anim;
                card_instance_anim.SetPositionAndRotation(card_instances_placedBoard[indexToRemove].AnchoredPosition,
                    card_instances_placedBoard[indexToRemove].ActualQuaternion);
                card_instance_anim.SetSprites(card_resource_sprites[(int)removedCardType], card_resource_reverse_sprite);
                card_instance_anim.SetFrontalAndStopMotion(true);
                card_instance_anim.SetVisible(true);
                Vector2 finalPosition = 3f * card_placement_board.anchoredPosition;
                if (round_winningPlayer != 0) finalPosition.y *= -1;
                card_instance_anim.SetTargetPositionAndRotation(finalPosition, card_instances_placedBoard[indexToRemove].ActualQuaternion, 0.75f);
                card_instance_anim.DoFlip(false, 0.5f);
                
                card_instances_placedBoard[indexToRemove].SetSprites(card_resource_reverse_sprite, card_resource_reverse_sprite);
                card_instances_placedBoard[indexToRemove].SetFrontalAndStopMotion(false);
                card_instances_placedBoard[indexToRemove].SetVisible(false);

                VARMAP_CardMaster.PLAY_SOUND(GameSound.SOUND_CARD_TAKEBACK, null, false);
            }
            else
            {
                card_instance_anim.SetVisible(false);
                
                playerScore[round_winningPlayer] += round_accumScore;
                
                if((remainingDeckCards.Count > 0)||(!isExposedSuitCardDrawn))
                {
                    if ((remainingDeckCards.Count > 0) && IsExchangeAvailable(round_winningPlayer, out _))
                    {
                        gameMoment = CardGameMoment.GAME_MOMENT_DECIDE_EXCHANGE;
                        momentSubStep = 0;
                    }
                    else
                    {
                        gameMoment = CardGameMoment.GAME_MOMENT_DRAW;
                        momentSubStep = 0;
                    }
                    currentPlayer = round_winningPlayer;
                }
                else
                {
                    int handCards = 0;
                    int maxScore = -1;
                    int playerWinner = -1;

                    for(int i=0; i < numPlayers; i++)
                    {
                        handCards += playerHandCards[i].Count;
                        if (playerScore[i] > maxScore)
                        {
                            maxScore = playerScore[i];
                            playerWinner = i;
                        }
                    }

                    if (handCards > 0)
                    {
                        currentPlayer = round_winningPlayer;
                        NextRound();
                    }
                    else
                    {
                        gameMoment = CardGameMoment.GAME_MOMENT_FINAL_RESULT;
                        momentSubStep = 0;
                        Debug.Log("Winner is player: " + playerWinner + " with score of: " + maxScore);
                    }
                }
            }

        }

        private void Game_Moment_DrawNextRound(ulong timestamp)
        {
            /* Make appear previous player hand card */
            if (momentSubStep > 0)
            {
                int playerToDraw = (momentSubStep - 1 + currentPlayer) % numPlayers;
                CardClass handInstance = card_instances_playerHand[playerToDraw, playerHandCards[playerToDraw].Count-1];
                if (playerToDraw == 0)
                {
                    handInstance.SetSprites(card_resource_sprites[(int)playerHandCards[playerToDraw][^1].cardType], card_resource_reverse_sprite);
                    handInstance.SetFrontalAndStopMotion(true);
                }
                else
                {
                    handInstance.SetSprites(card_resource_reverse_sprite, card_resource_reverse_sprite);
                    handInstance.SetFrontalAndStopMotion(false);
                }
                handInstance.SetVisible(true);
                card_instance_anim.SetVisible(false);
            }
            
            /* Suit card */
            if (((remainingDeckCards.Count > 0) || (!isExposedSuitCardDrawn)) && (momentSubStep < numPlayers))
            {
                CardInfo drawnCard;
                int playerToDraw = (momentSubStep + currentPlayer) % numPlayers;

                Vector2 posOrigin;
                Quaternion quaternionOrigin;
                bool fromExposedSuit;
                
                if (remainingDeckCards.Count > 0)
                {
                    drawnCard = remainingDeckCards[^1];
                    remainingDeckCards.RemoveAt(remainingDeckCards.Count - 1);
                    posOrigin = card_placement_deck.anchoredPosition;
                    quaternionOrigin = card_instance_deck.ActualQuaternion;
                    fromExposedSuit = false;

                    if(remainingDeckCards.Count == 0)
                    {
                        card_instance_deck.SetVisible(false);
                    }
                }
                else
                {
                    drawnCard = exposedSuitCard;
                    isExposedSuitCardDrawn = true;
                    posOrigin = card_placement_exposed.anchoredPosition;
                    quaternionOrigin = card_instance_exposedSuit.ActualQuaternion;
                    fromExposedSuit = true;

                    card_instance_exposedSuit.SetSprites(card_resource_reverse_sprite, card_resource_reverse_sprite);
                    card_instance_exposedSuit.SetFrontalAndStopMotion(false);
                    card_instance_exposedSuit.SetVisible(false);
                }

                playerHandCards[playerToDraw].Add(drawnCard);
                
                card_animated = card_instance_anim;
                card_instance_anim.SetPositionAndRotation(posOrigin, quaternionOrigin);
                card_instance_anim.SetSprites(
                    playerToDraw == 0 ? card_resource_sprites[(int)drawnCard.cardType] : card_resource_reverse_sprite, card_resource_reverse_sprite);
                card_instance_anim.SetFrontalAndStopMotion(fromExposedSuit);
                card_instance_anim.SetVisible(true);
                card_instance_anim.SetTargetPositionAndRotation(card_instances_playerHand[playerToDraw, playerHandCards[playerToDraw].Count-1].AnchoredPosition,
                    card_instances_playerHand[playerToDraw, playerHandCards[playerToDraw].Count-1].ActualQuaternion, 0.5f);
                if(((playerToDraw == 0) && (!fromExposedSuit))||((playerToDraw != 0) && fromExposedSuit))
                {
                    card_instance_anim.DoFlip(playerToDraw == 0, 0.5f);
                }
                
                PlayRandomCardPlaceSound();
                
                ++momentSubStep;
            }
            else
            {
                NextRound();
            }
        }

        private void Game_Moment_Exchange_Anim(ulong timestamp)
        {
            switch (momentSubStep)
            {
                case 0:
                {
                    card_instances_playerHand[currentPlayer, exchange_hand_index].SetVisible(false);
                    
                    card_animated = card_instance_anim;
                    card_instance_anim.SetPositionAndRotation(card_instances_playerHand[currentPlayer, exchange_hand_index].AnchoredPosition,
                        card_instances_playerHand[currentPlayer, exchange_hand_index].ActualQuaternion);
                    card_instance_anim.SetSprites(card_resource_sprites[(int)exposedSuitCard.cardType], card_resource_reverse_sprite);
                    card_instance_anim.SetFrontalAndStopMotion(currentPlayer == 0);
                    card_instance_anim.SetVisible(true);
                    card_instance_anim.SetTargetPositionAndRotation(card_instance_exposedSuit.AnchoredPosition,
                        card_instance_exposedSuit.ActualQuaternion, 0.5f);
                    if(currentPlayer != 0) card_instance_anim.DoFlip(true, 0.5f);
                    
                    PlayRandomCardPlaceSound();

                    ++momentSubStep;
                    break;
                }
                case 1:
                {
                    card_instance_exposedSuit.SetSprites(card_resource_sprites[(int)exposedSuitCard.cardType], card_resource_reverse_sprite);
                    card_instance_exposedSuit.SetFrontalAndStopMotion(true);
                
                    card_animated = card_instance_anim;
                    card_instance_anim.SetPositionAndRotation(card_instance_exposedSuit.AnchoredPosition,
                        card_instance_exposedSuit.ActualQuaternion);
                    card_instance_anim.SetSprites(card_resource_sprites[(int)playerHandCards[currentPlayer][exchange_hand_index].cardType], card_resource_reverse_sprite);
                    card_instance_anim.SetFrontalAndStopMotion(true);
                    card_instance_anim.SetVisible(true);
                    card_instance_anim.SetTargetPositionAndRotation(card_instances_playerHand[currentPlayer, exchange_hand_index].AnchoredPosition,
                        card_instances_playerHand[currentPlayer, exchange_hand_index].ActualQuaternion, 0.5f);
                    if(currentPlayer != 0) card_instance_anim.DoFlip(false, 0.5f);
                    
                    PlayRandomCardPlaceSound();
                
                    ++momentSubStep;
                    break;
                }
                case 2:
                {
                    card_instance_anim.SetVisible(false);
                    
                    if (currentPlayer == 0)
                    {
                        card_instances_playerHand[currentPlayer, exchange_hand_index].SetSprites(
                            card_resource_sprites[(int)playerHandCards[currentPlayer][exchange_hand_index].cardType], card_resource_reverse_sprite);
                        card_instances_playerHand[currentPlayer, exchange_hand_index].SetFrontalAndStopMotion(true);
                    }
                
                    card_instances_playerHand[currentPlayer, exchange_hand_index].SetVisible(true);

                    card_animated = card_instance_exposedSuit;
                    card_instance_exposedSuit.SetTargetPositionAndRotation(card_placement_exposed.anchoredPosition, card_instance_exposedSuit.ActualQuaternion, 0.5f);
                    
                    ++momentSubStep;
                    break;
                }
                default:
                {
                    gameMoment = CardGameMoment.GAME_MOMENT_DRAW;
                    momentSubStep = 0;
                    break;
                }
            }
        }

        private void Game_Action_AI_Turn(int player, ulong timestamp)
        {
            switch(gameDifficulty)
            {
                case 0:
                    Game_Action_AI_Easy(player, timestamp);
                    break;
                case 1:
                    Game_Action_AI_Medium(player, timestamp);
                    break;
                default:
                    Game_Action_AI_Hard(player, timestamp);
                    break;
            }
        }

        private void Game_Action_AI_Easy(int player, ulong timestamp)
        {
            int chosenIndex = UnityEngine.Random.Range(0, playerHandCards[player].Count);
            Game_Action_PlaceCard(player, chosenIndex, timestamp);
        }

        private void Game_Action_AI_Medium(int player, ulong timestamp)
        {
            List<CardInfo> handCards = playerHandCards[player];

            /* Blind situation. Throw lowest card (prefer unsuited) */
            if (turnNum == 0)
            {
                Game_Action_PlaceCard(player, AI_GetLowestLossScore(handCards), timestamp);
            }
            /* Already present cards in board */
            else
            {
                int handIndex_suit = AI_GetHighestScoreMove(playerHandCards[player], true, out int potentialDeltaScore_suit);
                Game_Action_PlaceCard(player, handIndex_suit, timestamp);
            }
        }

        private void Game_Action_AI_Hard(int player, ulong timestamp)
        {
            List<CardInfo> handCards = playerHandCards[player];

            /* Blind situation. Throw lowest card (prefer unsuited) */
            if(turnNum == 0)
            {
                Game_Action_PlaceCard(player, AI_GetLowestLossScore(handCards), timestamp);
            }
            /* Already present cards in board */
            else
            {
                int handIndex_suit = AI_GetHighestScoreMove(playerHandCards[player], true, out int potentialDeltaScore_suit);
                int handIndex_woutsuit = AI_GetHighestScoreMove(playerHandCards[player], false, out int potentialDeltaScore_woutsuit);

                /* Unconditional */
                if (potentialDeltaScore_suit >= AI_HARD_GUARANTEED_PLAY_THRESHOLD)
                {
                    Game_Action_PlaceCard(player, handIndex_suit, timestamp);
                }
                else if (handIndex_woutsuit == -1)
                {
                    Game_Action_PlaceCard(player, AI_GetLowestLossScore(handCards), timestamp);
                }
                else if (remainingDeckCards.Count == 0)
                {
                    Game_Action_PlaceCard(player, handIndex_suit, timestamp);
                }
                else if(round_winningSuit == gameSuit)
                {
                    /* King vs horse or similar */
                    if((round_winningScore > 0) && (potentialDeltaScore_suit > round_winningScore))
                    {
                        Game_Action_PlaceCard(player, handIndex_suit, timestamp);
                    }
                    else
                    {
                        /* Use lowest */
                        Game_Action_PlaceCard(player, AI_GetLowestLossScore(handCards), timestamp);
                    }
                }
                /* A suit which is not game suit */
                else
                {
                    if (round_winningScore > 0)
                    {
                        if ((potentialDeltaScore_woutsuit > potentialDeltaScore_suit)||((potentialDeltaScore_suit - potentialDeltaScore_woutsuit) < AI_HARD_SUIT_PREFERENCE_MARGIN))
                        {
                            Game_Action_PlaceCard(player, handIndex_woutsuit, timestamp);
                        }
                        else
                        {
                            Game_Action_PlaceCard(player, handIndex_suit, timestamp);
                        }
                    }
                    else
                    {
                        if(potentialDeltaScore_woutsuit > 0)
                        {
                            Game_Action_PlaceCard(player, handIndex_woutsuit, timestamp);
                        }
                        else
                        {
                            /* Use lowest */
                            Game_Action_PlaceCard(player, AI_GetLowestLossScore(handCards), timestamp);
                        }
                    }
                }
            }
        }

        private int AI_GetLowestLossScore(IReadOnlyList<CardInfo> handCards)
        {
            int bestLossScore = AI_BASE_WORST_SCORE;
            int handIndex = -1;

            for(int i=0; i < handCards.Count; ++i)
            {
                CardInfo cardInfo = handCards[i];
                int negScore = -cardInfo.cardScore*100;
                negScore += -cardInfo.cardValue;
                if(cardInfo.cardSuit == gameSuit)
                {
                    negScore -= AI_SCORE_TRUMP_PENALTY;
                }

                if(negScore > bestLossScore)
                {
                    bestLossScore = negScore;
                    handIndex = i;
                }
            }

            return handIndex;
        }

        private int AI_GetHighestScoreMove(IReadOnlyList<CardInfo> handCards, bool withGameSuite, out int potentialDeltaScore)
        {
            potentialDeltaScore = AI_BASE_WORST_SCORE;
            int index = -1;

            for(int i=0; i < handCards.Count; ++i)
            {
                CardInfo cardInfo = handCards[i];

                if((cardInfo.cardSuit == gameSuit) && (!withGameSuite))
                {
                    continue;
                }

                int candidateScore = -cardInfo.cardScore - round_winningScore;
                candidateScore *= 100;

                if (round_winningSuit == cardInfo.cardSuit)
                {
                    if(cardInfo.cardScore > round_winningScore)
                    {
                        candidateScore *= -1;
                    }
                    else if(cardInfo.cardScore == round_winningScore)
                    {
                        if(cardInfo.cardValue > round_winningValue)
                        {
                            candidateScore *= -1;
                        }
                        else
                        {
                            /* Lose */
                        }
                    }
                    else
                    {
                        /* Lose */
                    }
                }
                else if(((round_winningSuit != gameSuit) && (cardInfo.cardSuit == gameSuit))||(round_winningSuit == CardSuit.SUIT_NONE))
                {
                    candidateScore *= -1;
                }
                else
                {
                    /* Lose */
                }

                candidateScore -= cardInfo.cardValue;

                if(candidateScore > potentialDeltaScore)
                {
                    potentialDeltaScore = candidateScore;
                    index = i;
                }
            }

            return index;
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
                        if (IsCardInPlayerHand(instance, currentPlayer, out int handIndex))
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
                            dstInstance.SetSprites(card_resource_sprites[(int)playerHandCards[player][i].cardType], card_resource_reverse_sprite);
                            dstInstance.SetFrontalAndStopMotion(true);
                        }
                    }
                    else
                    {
                        card_instances_playerHand[player, i].SetSprites( card_resource_reverse_sprite, card_resource_reverse_sprite);
                        card_instances_playerHand[player, i].SetFrontalAndStopMotion(false);
                        card_instances_playerHand[player, i].SetVisible(false);
                    }
                }
                
                card_animated = card_instance_anim;
                card_instance_anim.SetPositionAndRotation(card_instances_playerHand[player, handIndex].AnchoredPosition,
                    card_instances_playerHand[player, handIndex].ActualQuaternion);
                card_instance_anim.SetSprites(card_resource_sprites[(int)usedCard.cardType], card_resource_reverse_sprite);
                card_instance_anim.SetFrontalAndStopMotion(player == 0);
                card_instance_anim.SetVisible(true);
                card_instance_anim.SetTargetPositionAndRotation(card_instances_placedBoard[actualBoardCards].AnchoredPosition,
                    card_instances_placedBoard[actualBoardCards].ActualQuaternion, 0.5f);
                if(player != 0)
                {
                    card_instance_anim.DoFlip(true, 0.5f);
                }
                
                PlayRandomCardPlaceSound();

                bool isWinning;

                if(round_winningSuit == CardSuit.SUIT_NONE)
                {
                    isWinning = true;
                }
                else if((round_winningSuit != gameSuit) && (usedCard.cardSuit == gameSuit))
                {
                    isWinning = true;
                }
                else if(usedCard.cardSuit == round_winningSuit)
                {
                    if(usedCard.cardScore > round_winningScore)
                    {
                        isWinning = true;
                    }
                    else if(usedCard.cardScore == round_winningScore)
                    {
                        isWinning = usedCard.cardValue > round_winningValue;
                    }
                    else
                    {
                        isWinning = false;
                    }
                }
                else
                {
                    isWinning = false;
                }

                round_accumScore += usedCard.cardScore;

                if(isWinning)
                {
                    round_winningPlayer = player;
                    round_winningScore = usedCard.cardScore;
                    round_winningValue = usedCard.cardValue;
                    round_winningSuit = usedCard.cardSuit;
                }

                currentPlayer = (currentPlayer + 1) % numPlayers;

                lastActionTimestamp = timestamp;

                gameMoment = CardGameMoment.GAME_MOMENT_PLACE_CARD_ANIM;

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
            if ((currentPlayer != player) || (gameMoment != CardGameMoment.GAME_MOMENT_DECIDE_EXCHANGE) ||
                (momentSubStep != 0)) return;
            
            if (!IsExchangeAvailable(player, out int handIndex)) return;
                
            (playerHandCards[player][handIndex], exposedSuitCard) = (exposedSuitCard, playerHandCards[player][handIndex]);
            exchange_hand_index = handIndex;
            
            card_animated = card_instance_exposedSuit;
            Vector2 finalPos = (card_placement_deck.anchoredPosition + card_placement_board.anchoredPosition) * 0.5f;
            card_instance_exposedSuit.SetTargetPositionAndRotation(finalPos, card_instance_exposedSuit.ActualQuaternion, 0.5f);


            isExposedSuitCardExchanged = true;
            gameMoment = CardGameMoment.GAME_MOMENT_EXCHANGE_ANIM;
            momentSubStep = 0;
            
            PlayRandomCardPlaceSound();
        }

        private void ResetGame()
        {
            gameSuit = CardSuit.SUIT_NONE;
            exposedSuitCard = CardInfo.UNDEFINED_CARD;
            isExposedSuitCardExchanged = false;
            isExposedSuitCardDrawn = false;
            currentPlayer = 0;
            numPlayers = 2;
            turnNum = 0;
            round_winningPlayer = -1;
            round_winningScore = -1;
            round_winningValue = -1;
            round_accumScore = 0;
            round_winningSuit = CardSuit.SUIT_NONE;
            gameMoment = CardGameMoment.GAME_MOMENT_STOP;
            momentSubStep = 0;
            card_animated = null;
            remainingDeckCards.Clear();
            initialTrickedCards.Clear();
            placedBoardCards.Clear();



            for (int i = 0; i < card_resource_pool.Count; i++)
            {
                card_resource_pool[i].SetSprites(card_resource_reverse_sprite, card_resource_reverse_sprite);
                card_resource_pool[i].SetVisible(false);
                card_resource_pool[i].SetFrontalAndStopMotion(false);
            }

            card_instance_deck.SetPositionAndRotation(card_placement_deck.anchoredPosition, Quaternion.identity);
            card_instance_exposedSuit.SetPositionAndRotation(card_placement_exposed.anchoredPosition, Quaternion.Euler(0, 0, -90));
            
            card_instance_anim.SetVisible(false);
            card_instance_anim.SetSprites(card_resource_reverse_sprite, card_resource_reverse_sprite);
            card_instance_anim.SetFrontalAndStopMotion(false);
            

            for (int i= 0; i < GameFixedConfig.CARD_GAME_MAX_PLAYERS; i++)
            {
                playerScore[i] = 0;
                playerHandCards[i].Clear();
                playerScoredCards[i].Clear();

                Vector2 p2_position;
                float angle;
                RectTransform handPlacement;
                float baseRotation;
                float rotMult;

                if(i == 0)
                {
                    handPlacement = card_placement_p1_hand;
                    baseRotation = 0;
                    rotMult = -1f;
                    p2_position = card_placement_board.anchoredPosition + new Vector2(card_placement_board.rect.size.x * 0.5f, 0);
                    angle = -30f;
                }
                else
                {
                    handPlacement = card_placement_p2_hand;
                    baseRotation = 180;
                    rotMult = 1f;
                    p2_position = card_placement_board.anchoredPosition - new Vector2(card_placement_board.rect.size.x * 0.5f, 0);
                    angle = 30f;
                }

                card_instances_placedBoard[i].SetPositionAndRotation(p2_position, Quaternion.Euler(0, 0, angle));

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
            for (int i = 0; i < (int)CardType.TOTAL_CARDS; i++)
            {
                if (!alreadyTakenCards.Contains((CardType)i))
                {
                    int position = Mathf.RoundToInt(UnityEngine.Random.value * remainingDeckCards.Count);
                    if (position >= remainingDeckCards.Count)
                    {
                        remainingDeckCards.Add(CardInfo.GAME_CARDS[i]);
                    }
                    else
                    {
                        remainingDeckCards.Insert(position, CardInfo.GAME_CARDS[i]);
                    }
                }
            }
        }

        private bool IsCardInPlayerHand(CardClass instance, int player, out int handIndex)
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

        private bool IsExchangeAvailable(int player, out int handIndex)
        {
            bool available = false;
            handIndex = -1;

            if ((!isExposedSuitCardExchanged) && (!isExposedSuitCardDrawn))
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

        private void PlayRandomCardPlaceSound()
        {
            int soundIndex = UnityEngine.Random.Range(0, CARD_PLACE_SOUNDS.Count);
            VARMAP_CardMaster.PLAY_SOUND(
                CARD_PLACE_SOUNDS[soundIndex], null, false);
        }

        private void NextRound()
        {
            gameMoment = CardGameMoment.GAME_MOMENT_PLAY;
            momentSubStep = 0;
            turnNum = 0;
            round_winningPlayer = -1;
            round_winningScore = -1;
            round_winningValue = -1;
            round_winningSuit = CardSuit.SUIT_NONE;
            round_accumScore = 0;
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
