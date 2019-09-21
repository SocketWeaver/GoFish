using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SWNetwork;

namespace GoFish
{
    public class MultiplayerGame : Game
    {
        protected new void Awake()
        {
            base.Awake();

            NetworkClient.Lobby.GetPlayersInRoom((successful, reply, error) =>
            {
                if (successful)
                {
                    foreach(SWPlayer swPlayer in reply.players)
                    {
                        string playerName = swPlayer.GetCustomDataString();
                        string playerId = swPlayer.id;

                        if (playerId.Equals(NetworkClient.Instance.PlayerId))
                        {
                            localPlayer.PlayerId = playerId;
                            localPlayer.PlayerName = playerName;
                        }
                        else
                        {
                            remotePlayer.PlayerId = playerId;
                            remotePlayer.PlayerName = playerName;
                        }
                    }

                    gameDataManager = new GameDataManager(localPlayer, remotePlayer);
                }
                else
                {
                    Debug.Log("Failed to get players in room.");
                }

            });
        }
    }
}
