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
                    {CardGameEvent.GAME_EVENT_LOSE_HIGH_END_TURN, DialogPhrase.PHRASE_NONSENSE}
                },
                0, -1, Array.Empty<CardType>(), CardType.CARD_NONE
            )
        };
    }
}
