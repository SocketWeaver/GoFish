using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
        List<byte> booksForPlayer1 = new List<byte>();
        [SerializeField]
        List<byte> booksForPlayer2 = new List<byte>();
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

        byte[] encryptionKey;
        byte[] safeData;

        public ProtectedData(string p1Id, string p2Id, string roomId)
        {
            player1Id = p1Id;
            player2Id = p2Id;
            currentTurnPlayerId = "";
            selectedRank = (int)Ranks.NoRanks;
            CalculateKey(roomId);
            Encrypt();
        }

        public void SetPoolOfCards(List<byte> cardValues)
        {
            Decrypt();
            poolOfCards = cardValues;
            Encrypt();
        }

        public List<byte> GetPoolOfCards()
        {
            List<byte> result;
            Decrypt();
            result = poolOfCards;
            Encrypt();
            return result;
        }

        public List<byte> PlayerCards(Player player)
        {
            List<byte> result;
            Decrypt();
            if (player.PlayerId.Equals(player1Id))
            {
                result = player1Cards;
            }
            else
            {
                result = player2Cards;
            }
            Encrypt();
            return result;
        }

        public List<byte> PlayerBooks(Player player)
        {
            List<byte> result;
            Decrypt();
            if (player.PlayerId.Equals(player1Id))
            {
                result = booksForPlayer1;
            }
            else
            {
                result = booksForPlayer2;
            }
            Encrypt();
            return result;
        }

        public void AddCardValuesToPlayer(Player player, List<byte> cardValues)
        {
            Decrypt();
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
            Encrypt();
        }

        public void AddCardValueToPlayer(Player player, byte cardValue)
        {
            Decrypt();
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
            Encrypt();
        }

        public void RemoveCardValuesFromPlayer(Player player, List<byte> cardValuesToRemove)
        {
            Decrypt();
            if (player.PlayerId.Equals(player1Id))
            {
                player1Cards.RemoveAll(cv => cardValuesToRemove.Contains(cv));
            }
            else
            {
                player2Cards.RemoveAll(cv => cardValuesToRemove.Contains(cv));
            }
            Encrypt();
        }

        public void AddBooksForPlayer(Player player, Ranks ranks)
        {
            Decrypt();
            if (player.PlayerId.Equals(player1Id))
            {
                booksForPlayer1.Add((byte)ranks);
            }
            else
            {
                booksForPlayer2.Add((byte)ranks);
            }
            Encrypt();
        }

        public bool GameFinished()
        {
            bool result = false;
            Decrypt();
            if (poolOfCards.Count == 0)
            {
                result = true;
            }

            if (player1Cards.Count == 0)
            {
                result = true;
            }

            if (player2Cards.Count == 0)
            {
                result = true;
            }
            Encrypt();

            return result;
        }

        public string WinnerPlayerId()
        {
            string result;
            Decrypt();
            if (booksForPlayer1.Count > booksForPlayer2.Count)
            {
                result = player1Id;
            }
            else
            {
                result = player2Id;
            }
            Encrypt();
            return result;
        }

        public void SetCurrentTurnPlayerId(string playerId)
        {
            Decrypt();
            currentTurnPlayerId = playerId;
            Encrypt();
        }

        public string GetCurrentTurnPlayerId()
        {
            string result;
            Decrypt();
            result = currentTurnPlayerId;
            Encrypt();
            return result;
        }

        public void SetGameState(int gameState)
        {
            Decrypt();
            currentGameState = gameState;
            Encrypt();
        }
        public int GetGameState()
        {
            int result;
            Decrypt();
            result = currentGameState;
            Encrypt();
            return result;
        }

        public void SetSelectedRank(int rank)
        {
            Decrypt();
            selectedRank = rank;
            Encrypt();
        }

        public int GetSelectedRank()
        {
            int result;
            Decrypt();
            result = selectedRank;
            Encrypt();
            return result;
        }

        public Byte[] ToArray()
        {
            return safeData;
        }

        public void ApplyByteArray(Byte[] byteArray)
        {
            safeData = byteArray;
        }

        void CalculateKey(string roomId)
        {
            string roomIdSubString = roomId.Substring(0, 16);
            encryptionKey = Encoding.UTF8.GetBytes(roomIdSubString);
        }

        void Encrypt()
        {
            SWNetworkMessage message = new SWNetworkMessage();
            message.Push((Byte)poolOfCards.Count);
            message.PushByteArray(poolOfCards.ToArray());

            message.Push((Byte)player1Cards.Count);
            message.PushByteArray(player1Cards.ToArray());

            message.Push((Byte)player2Cards.Count);
            message.PushByteArray(player2Cards.ToArray());

            message.Push((Byte)booksForPlayer1.Count);
            message.PushByteArray(booksForPlayer1.ToArray());

            message.Push((Byte)booksForPlayer2.Count);
            message.PushByteArray(booksForPlayer2.ToArray());

            message.PushUTF8ShortString(player1Id);
            message.PushUTF8ShortString(player2Id);

            message.PushUTF8ShortString(currentTurnPlayerId);
            message.Push(currentGameState);

            message.Push(selectedRank);

            safeData = AES.EncryptAES128(message.ToArray(), encryptionKey);

            poolOfCards = new List<byte>();
            player1Cards = new List<byte>();
            player2Cards = new List<byte>();
            booksForPlayer1 = new List<byte>();
            booksForPlayer2 = new List<byte>();
            player1Id = null;
            player2Id = null;
            currentTurnPlayerId = null;
            currentGameState = 0;
            selectedRank = 0;
        }

        void Decrypt()
        {
            byte[] byteArray = AES.DecryptAES128(safeData, encryptionKey);

            SWNetworkMessage message = new SWNetworkMessage(byteArray);
            byte poolOfCardsCount = message.PopByte();
            poolOfCards = message.PopByteArray(poolOfCardsCount).ToList();

            byte player1CardsCount = message.PopByte();
            player1Cards = message.PopByteArray(player1CardsCount).ToList();

            byte player2CardsCount = message.PopByte();
            player2Cards = message.PopByteArray(player2CardsCount).ToList();

            byte booksForPlayer1Count = message.PopByte();
            booksForPlayer1 = message.PopByteArray(booksForPlayer1Count).ToList();

            byte booksForPlayer2Count = message.PopByte();
            booksForPlayer2 = message.PopByteArray(booksForPlayer2Count).ToList();

            player1Id = message.PopUTF8ShortString();
            player2Id = message.PopUTF8ShortString();

            currentTurnPlayerId = message.PopUTF8ShortString();
            currentGameState = message.PopInt32();

            selectedRank = message.PopInt32();
        }
    }
}