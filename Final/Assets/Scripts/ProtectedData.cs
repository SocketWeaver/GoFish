using System;
using System.Collections.Generic;
using System.Linq;
using SWNetwork;
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
        [SerializeField]
        string currentTurnPlayerId;
        [SerializeField]
        int currentGameState;
        [SerializeField]
        int selectedRank;

        public ProtectedData(string p1Id, string p2Id)
        {
            player1Id = p1Id;
            player2Id = p2Id;
            currentTurnPlayerId = "";
            selectedRank = (int)Ranks.NoRanks;
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

        public void SetCurrentTurnPlayerId(string playerId)
        {
            currentTurnPlayerId = playerId;
        }

        public string GetCurrentTurnPlayerId()
        {
            return currentTurnPlayerId;
        }

        public void SetGameState(int gameState)
        {
            currentGameState = gameState;
        }
        public int GetGameState()
        {
            return currentGameState;
        }

        public void SetSelectedRank(int rank)
        {
            selectedRank = rank;
        }

        public int GetSelectedRank()
        {
            return selectedRank;
        }

        public Byte[] ToArray()
        {
            SWNetworkMessage message = new SWNetworkMessage();
            message.Push((Byte)poolOfCards.Count);
            message.PushByteArray(poolOfCards.ToArray());

            message.Push((Byte)player1Cards.Count);
            message.PushByteArray(player1Cards.ToArray());

            message.Push((Byte)player2Cards.Count);
            message.PushByteArray(player2Cards.ToArray());

            message.Push(numberOfBooksForPlayer1);
            message.Push(numberOfBooksForPlayer2);

            message.PushUTF8ShortString(player1Id);
            message.PushUTF8ShortString(player2Id);

            message.PushUTF8ShortString(currentTurnPlayerId);
            message.Push(currentGameState);

            message.Push(selectedRank);

            return message.ToArray();
        }

        public void ApplyByteArray(Byte[] byteArray)
        {
            SWNetworkMessage message = new SWNetworkMessage(byteArray);
            byte poolOfCardsCount = message.PopByte();
            poolOfCards = message.PopByteArray(poolOfCardsCount).ToList();

            byte player1CardsCount = message.PopByte();
            player1Cards = message.PopByteArray(player1CardsCount).ToList();

            byte player2CardsCount = message.PopByte();
            player2Cards = message.PopByteArray(player2CardsCount).ToList();

            numberOfBooksForPlayer1 = message.PopInt32();
            numberOfBooksForPlayer2 = message.PopInt32();

            player1Id = message.PopUTF8ShortString();
            player2Id = message.PopUTF8ShortString();

            currentTurnPlayerId = message.PopUTF8ShortString();
            currentGameState = message.PopInt32();

            selectedRank = message.PopInt32();
        }
    }
}