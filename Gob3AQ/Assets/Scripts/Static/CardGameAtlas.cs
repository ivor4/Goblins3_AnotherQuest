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
                new Dictionary<CardGameEvent, DialogPhrase>()
                {
                    { CardGameEvent.GAME_EVENT_LOSE_HIGH_END_TURN, DialogPhrase.PHRASE_CARDS1_ARTURO_TAUNT_1 },
                    { CardGameEvent.GAME_EVENT_LOSE_BIG_COMBO , DialogPhrase.PHRASE_CARDS1_ARTURO_TAUNT_2 },
                    { CardGameEvent.GAME_EVENT_EXCHANGE_SUIT_OTHER, DialogPhrase.PHRASE_CARDS1_ARTURO_TAUNT_3 },
                    { CardGameEvent.GAME_EVENT_WIN_BIG_COMBO, DialogPhrase.PHRASE_CARDS1_MAINCHAR_TAUNT_1},
                    { CardGameEvent.GAME_EVENT_EXCHANGE_SUIT_OWN, DialogPhrase.PHRASE_CARDS1_MAINCHAR_TAUNT_2},
                    { CardGameEvent.GAME_EVENT_WIN_NOTHING_END_TURN, DialogPhrase.PHRASE_CARDS1_MAINCHAR_TAUNT_3},
                    { CardGameEvent.GAME_EVENT_WIN_LOW_END_TURN, DialogPhrase.PHRASE_CARDS1_MAINCHAR_TAUNT_4},
                },
                0, -1, GameItem.ITEM_NPC_ARTURO_EXTRAPERLO, Array.Empty<CardType>(), CardType.CARD_NONE
            )
        };
    }
}
