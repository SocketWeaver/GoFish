using System.ComponentModel;
using UnityEngine;

namespace GoFish
{
    public static class Constants
    {
        public const float  PLAYER_CARD_POSITION_OFFSET = 1.2f;
        public const float  PLAYER_BOOK_POSITION_OFFSET = 2f;
        public const float  DECK_CARD_POSITION_OFFSET = 0.2f;
        public const string CARD_BACK_SPRITE = "cardBack_red5";
        public const float  CARD_SELECTED_OFFSET = 0.3f;
        public const int    PLAYER_INITIAL_CARDS = 7;
        public const float  CARD_MOVEMENT_SPEED = 25.0f;
        public const float  CARD_SNAP_DISTANCE = 0.01f;
        public const float  CARD_ROTATION_SPEED = 8f;
        public const float  BOOK_MAX_RANDOM_ROTATION = 15f;
        public const byte   POOL_IS_EMPTY = 255;
    }

    public enum Suits
    {
        NoSuits = -1,
        Spades = 0,
        Clubs = 1,
        Diamonds = 2,
        Hearts = 3,
    }

    public enum Ranks
    {
        [Description("No Ranks")]
        NoRanks = -1,
        [Description("A")]
        Ace = 1,
        [Description("2")]
        Two = 2,
        [Description("3")]
        Three = 3,
        [Description("4")]
        Four = 4,
        [Description("5")]
        Five = 5,
        [Description("6")]
        Six = 6,
        [Description("7")]
        Seven = 7,
        [Description("8")]
        Eight = 8,
        [Description("9")]
        Nine = 9,
        [Description("10")]
        Ten = 10,
        [Description("J")]
        Jack = 11,
        [Description("Q")]
        Queen = 12,
        [Description("K")]
        King = 13,
    }
}
