

using System;

namespace Gob3AQ.VARMAP.Types.Cards
{
    public enum CardType
    {
        CARD_NONE = -1,

        AS_DE_OROS,
        DOS_DE_OROS,
        TRES_DE_OROS,
        CUATRO_DE_OROS,
        CINCO_DE_OROS,
        SEIS_DE_OROS,
        SIETE_DE_OROS,
        SOTA_DE_OROS,
        CABALLO_DE_OROS,
        REY_DE_OROS,

        AS_DE_COPAS,
        DOS_DE_COPAS,
        TRES_DE_COPAS,
        CUATRO_DE_COPAS,
        CINCO_DE_COPAS,
        SEIS_DE_COPAS,
        SIETE_DE_COPAS,
        SOTA_DE_COPAS,
        CABALLO_DE_COPAS,
        REY_DE_COPAS,

        AS_DE_BASTOS,
        DOS_DE_BASTOS,
        TRES_DE_BASTOS,
        CUATRO_DE_BASTOS,
        CINCO_DE_BASTOS,
        SEIS_DE_BASTOS,
        SIETE_DE_BASTOS,
        SOTA_DE_BASTOS,
        CABALLO_DE_BASTOS,
        REY_DE_BASTOS,

        AS_DE_ESPADAS,
        DOS_DE_ESPADAS,
        TRES_DE_ESPADAS,
        CUATRO_DE_ESPADAS,
        CINCO_DE_ESPADAS,
        SEIS_DE_ESPADAS,
        SIETE_DE_ESPADAS,
        SOTA_DE_ESPADAS,
        CABALLO_DE_ESPADAS,
        REY_DE_ESPADAS,

        TOTAL_CARDS
    }

    public enum CardSuit
    {
        SUIT_NONE = -1,

        OROS,
        COPAS,
        BASTOS,
        ESPADAS,
    }

    public enum CardGameMoment
    {
        GAME_MOMENT_STOP,
        GAME_MOMENT_DRAW_FIRST_CARDS,
        GAME_MOMENT_EXPOSE_SUIT_CARD,
        GAME_MOMENT_RANDOM_FIRST_TURN,
        GAME_MOMENT_PLAY,
        GAME_MOMENT_DRAW,
        GAME_MOMENT_FINAL_RESULT
    }

    public readonly struct CardInfo
    {
        public readonly CardType cardType;
        public readonly CardSuit cardSuit;
        public readonly byte cardValue;
        public readonly byte cardScore;

        public static ReadOnlySpan<CardInfo> GAME_CARDS => _GAME_CARDS;

        public static CardInfo UNDEFINED_CARD = new CardInfo(CardType.CARD_NONE, CardSuit.SUIT_NONE, 0, 0);

        private static readonly CardInfo[] _GAME_CARDS = new CardInfo[]
        {
            new CardInfo(CardType.AS_DE_OROS, CardSuit.OROS, 1, 11),
            new CardInfo(CardType.DOS_DE_OROS, CardSuit.OROS, 2, 0),
            new CardInfo(CardType.TRES_DE_OROS, CardSuit.OROS, 3, 10),
            new CardInfo(CardType.CUATRO_DE_OROS, CardSuit.OROS, 4, 0),
            new CardInfo(CardType.CINCO_DE_OROS, CardSuit.OROS, 5, 0),
            new CardInfo(CardType.SEIS_DE_OROS, CardSuit.OROS, 6, 0),
            new CardInfo(CardType.SIETE_DE_OROS, CardSuit.OROS, 7, 0),
            new CardInfo(CardType.SOTA_DE_OROS, CardSuit.OROS, 10, 2),
            new CardInfo(CardType.CABALLO_DE_OROS, CardSuit.OROS, 11, 3),
            new CardInfo(CardType.REY_DE_OROS, CardSuit.OROS, 12 , 4),

            new CardInfo(CardType.AS_DE_COPAS, CardSuit.COPAS, 1, 11),
            new CardInfo(CardType.DOS_DE_COPAS, CardSuit.COPAS, 2, 0),
            new CardInfo(CardType.TRES_DE_COPAS, CardSuit.COPAS, 3, 10),
            new CardInfo(CardType.CUATRO_DE_COPAS, CardSuit.COPAS, 4, 0),
            new CardInfo(CardType.CINCO_DE_COPAS, CardSuit.COPAS, 5, 0),
            new CardInfo(CardType.SEIS_DE_COPAS, CardSuit.COPAS, 6, 0),
            new CardInfo(CardType.SIETE_DE_COPAS, CardSuit.COPAS, 7, 0),
            new CardInfo(CardType.SOTA_DE_COPAS, CardSuit.COPAS, 10, 2),
            new CardInfo(CardType.CABALLO_DE_COPAS, CardSuit.COPAS, 11, 3),
            new CardInfo(CardType.REY_DE_COPAS, CardSuit.COPAS, 12 , 4),

            new CardInfo(CardType.AS_DE_BASTOS, CardSuit.BASTOS, 1, 11),
            new CardInfo(CardType.DOS_DE_BASTOS, CardSuit.BASTOS, 2, 0),
            new CardInfo(CardType.TRES_DE_BASTOS, CardSuit.BASTOS, 3, 10),
            new CardInfo(CardType.CUATRO_DE_BASTOS, CardSuit.BASTOS, 4, 0),
            new CardInfo(CardType.CINCO_DE_BASTOS, CardSuit.BASTOS, 5, 0),
            new CardInfo(CardType.SEIS_DE_BASTOS, CardSuit.BASTOS, 6, 0),
            new CardInfo(CardType.SIETE_DE_BASTOS, CardSuit.BASTOS, 7, 0),
            new CardInfo(CardType.SOTA_DE_BASTOS, CardSuit.BASTOS, 10, 2),
            new CardInfo(CardType.CABALLO_DE_BASTOS, CardSuit.BASTOS, 11, 3),
            new CardInfo(CardType.REY_DE_BASTOS, CardSuit.BASTOS, 12 , 4),

            new CardInfo(CardType.AS_DE_ESPADAS, CardSuit.ESPADAS, 1, 11),
            new CardInfo(CardType.DOS_DE_ESPADAS, CardSuit.ESPADAS, 2, 0),
            new CardInfo(CardType.TRES_DE_ESPADAS, CardSuit.ESPADAS, 3, 10),
            new CardInfo(CardType.CUATRO_DE_ESPADAS, CardSuit.ESPADAS, 4, 0),
            new CardInfo(CardType.CINCO_DE_ESPADAS, CardSuit.ESPADAS, 5, 0),
            new CardInfo(CardType.SEIS_DE_ESPADAS, CardSuit.ESPADAS, 6, 0),
            new CardInfo(CardType.SIETE_DE_ESPADAS, CardSuit.ESPADAS, 7, 0),
            new CardInfo(CardType.SOTA_DE_ESPADAS, CardSuit.ESPADAS, 10, 2),
            new CardInfo(CardType.CABALLO_DE_ESPADAS, CardSuit.ESPADAS, 11, 3),
            new CardInfo(CardType.REY_DE_ESPADAS, CardSuit.ESPADAS, 12 , 4)
        };

        public CardInfo(CardType cardType, CardSuit cardSuit, byte cardValue, byte cardScore)
        {
            this.cardType = cardType;
            this.cardSuit = cardSuit;
            this.cardValue = cardValue;
            this.cardScore = cardScore;
        }
    }


}
