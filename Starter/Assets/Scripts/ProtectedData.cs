using System;
using System.Collections.Generic;
using UnityEngine;

namespace GoFish
{
    /// <summary>
    /// Stores the important data of the game
    /// We will encypt the fields in a multiplayer game.
    /// </summary>
    [Serializable]
    public class ProtectedData
    {
        [SerializeField]
        List<byte> poolOfCards = new List<byte>();
        [SerializeField]
        List<byte> player1Cards = new List<byte>();
        [SerializeField]
        List<byte> player2Cards = new List<byte>();
        [SerializeField]
        int numberOfBooksForPlayer1;
        [SerializeField]
        int numberOfBooksForPlayer2;
        [SerializeField]
        string player1Id;
        [SerializeField]
        string player2Id;

        public ProtectedData(string p1Id, string p2Id)
        {
            player1Id = p1Id;
            player2Id = p2Id;
        }

        public void SetPoolOfCards(List<byte> cardValues)
        {
            poolOfCards = cardValues;
        }

        public List<byte> GetPoolOfCards()
        {
            return poolOfCards;
        }

        public List<byte> PlayerCards(Player player)
        {
            if (player.PlayerId.Equals(player1Id))
            {
                return player1Cards;
            }
            else
            {
                return player2Cards;
            }
        }

        public void AddCardValuesToPlayer(Player player, List<byte> cardValues)
        {
            if (player.PlayerId.Equals(player1Id))
            {
                player1Cards.AddRange(cardValues);
                player1Cards.Sort();
            }
            else
            {
                player2Cards.AddRange(cardValues);
                player2Cards.Sort();
            }
        }

        public void AddCardValueToPlayer(Player player, byte cardValue)
        {
            if (player.PlayerId.Equals(player1Id))
            {
                player1Cards.Add(cardValue);
                player1Cards.Sort();
            }
            else
            {
                player2Cards.Add(cardValue);
                player2Cards.Sort();
            }
        }

        public void RemoveCardValuesFromPlayer(Player player, List<byte> cardValuesToRemove)
        {
            if (player.PlayerId.Equals(player1Id))
            {
                player1Cards.RemoveAll(cv => cardValuesToRemove.Contains(cv));
            }
            else
            {
                player2Cards.RemoveAll(cv => cardValuesToRemove.Contains(cv));
            }
        }

        public void AddBooksForPlayer(Player player, int numberOfNewBooks)
        {
            if (player.PlayerId.Equals(player1Id))
            {
                numberOfBooksForPlayer1 += numberOfNewBooks;
            }
            else
            {
                numberOfBooksForPlayer2 += numberOfNewBooks;
            }
        }

        public bool GameFinished()
        {
            if (poolOfCards.Count == 0)
            {
                return true;
            }

            if (player1Cards.Count == 0)
            {
                return true;
            }

            if (player2Cards.Count == 0)
            {
                return true;
            }

            return false;
        }

        public string WinnerPlayerId()
        {
            if (numberOfBooksForPlayer1 > numberOfBooksForPlayer2)
            {
                return player1Id;
            }
            else
            {
                return player2Id;
            }
        }
    }
}