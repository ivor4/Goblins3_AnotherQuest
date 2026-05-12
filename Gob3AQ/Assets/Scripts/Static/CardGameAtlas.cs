using System;
using System.Collections.Generic;
using Gob3AQ.VARMAP.Types;
using UnityEngine;
using Gob3AQ.VARMAP.Types.Cards;

namespace Gob3AQ.CardGameAtlas
{
    public static class CardGameAtlasClass
    {
        public static ref readonly CardGameInfo GetCardGameInfo(CardGameID id)
        {
            if ((uint)id < (uint)CardGameID.CARD_GAME_TOTAL)
            {
                return ref _CARD_GAME_INFO_ARRAY[(int)id];
            }

            return ref CardGameInfo.EMPTY;
        }
        
        private static readonly CardGameInfo[] _CARD_GAME_INFO_ARRAY = new CardGameInfo[(int)CardGameID.CARD_GAME_TOTAL]
        {
            /* CARD_GAME_TEST */
            new (
                new Dictionary<CardGameEvent, TauntInfo>()
                {
                    { CardGameEvent.GAME_EVENT_LOSE_HIGH_CARD_END_TURN, new TauntInfo(DialogPhrase.PHRASE_CARDS1_ARTURO_TAUNT_1, GameItem.ITEM_NPC_ARTURO_EXTRAPERLO) },
                    { CardGameEvent.GAME_EVENT_LOSE_BIG_COMBO , new TauntInfo(DialogPhrase.PHRASE_CARDS1_ARTURO_TAUNT_2, GameItem.ITEM_NPC_ARTURO_EXTRAPERLO) },
                    { CardGameEvent.GAME_EVENT_EXCHANGE_SUIT_OTHER, new TauntInfo(DialogPhrase.PHRASE_CARDS1_ARTURO_TAUNT_3, GameItem.ITEM_NPC_ARTURO_EXTRAPERLO) },
                    { CardGameEvent.GAME_EVENT_LOSE_HIGH_END_TURN, new TauntInfo(DialogPhrase.PHRASE_CARDS1_ARTURO_TAUNT_4, GameItem.ITEM_NPC_ARTURO_EXTRAPERLO) },
                    { CardGameEvent.GAME_EVENT_LOSE_NOTHING, new TauntInfo(DialogPhrase.PHRASE_CARDS1_ARTURO_TAUNT_5, GameItem.ITEM_NPC_ARTURO_EXTRAPERLO) },
                    { CardGameEvent.GAME_EVENT_LOSE_LOW_END_TURN, new TauntInfo(DialogPhrase.PHRASE_CARDS1_ARTURO_TAUNT_6, GameItem.ITEM_NPC_ARTURO_EXTRAPERLO) },
                    { CardGameEvent.GAME_EVENT_LOSE_GAME, new TauntInfo(DialogPhrase.PHRASE_CARDS1_ARTURO_TAUNT_WIN, GameItem.ITEM_NPC_ARTURO_EXTRAPERLO) },
                    { CardGameEvent.GAME_EVENT_WIN_BIG_COMBO, new TauntInfo(DialogPhrase.PHRASE_CARDS1_MAINCHAR_TAUNT_1, GameItem.ITEM_PLAYER_MAIN)},
                    { CardGameEvent.GAME_EVENT_EXCHANGE_SUIT_OWN, new TauntInfo(DialogPhrase.PHRASE_CARDS1_MAINCHAR_TAUNT_2, GameItem.ITEM_PLAYER_MAIN)},
                    { CardGameEvent.GAME_EVENT_WIN_NOTHING, new TauntInfo(DialogPhrase.PHRASE_CARDS1_MAINCHAR_TAUNT_3, GameItem.ITEM_PLAYER_MAIN)},
                    { CardGameEvent.GAME_EVENT_WIN_LOW_END_TURN, new TauntInfo(DialogPhrase.PHRASE_CARDS1_MAINCHAR_TAUNT_4, GameItem.ITEM_PLAYER_MAIN)},
                    { CardGameEvent.GAME_EVENT_WIN_HIGH_END_TURN, new TauntInfo(DialogPhrase.PHRASE_CARDS1_MAINCHAR_TAUNT_5, GameItem.ITEM_PLAYER_MAIN) },
                    { CardGameEvent.GAME_EVENT_WIN_GAME, new TauntInfo(DialogPhrase.PHRASE_CARDS1_MAINCHAR_TAUNT_WIN, GameItem.ITEM_PLAYER_MAIN) },
                },
                0, -1,GameAction.ACTION_EVENT_WIN_CARDS_ARTURO, GameAction.ACTION_NONE, Array.Empty<CardType>(), CardType.CARD_NONE
            )
        };
    }
}
