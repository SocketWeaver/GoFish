using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using SWNetwork;

namespace GoFish
{
    public class Lobby : MonoBehaviour
    {
        public enum LobbyState
        {
            Default,
            JoinedRoom,
        }
        public LobbyState State = LobbyState.Default;
        public bool Debugging = false;

        public GameObject PopoverBackground;
        public GameObject EnterNicknamePopover;
        public GameObject WaitForOpponentPopover;
        public GameObject StartRoomButton;
        public InputField NicknameInputField;

        public GameObject Player1Portrait;
        public GameObject Player2Portrait;

        string nickname;

        private void Start()
        {
            // disable all online UI elements
            HideAllPopover();
            NetworkClient.Lobby.OnLobbyConnectedEvent += OnLobbyConnected;
            NetworkClient.Lobby.OnNewPlayerJoinRoomEvent += OnNewPlayerJoinRoomEvent;
            NetworkClient.Lobby.OnRoomReadyEvent += OnRoomReadyEvent;
        }

        private void OnDestroy()
        {
            if (NetworkClient.Lobby != null)
            {
                NetworkClient.Lobby.OnLobbyConnectedEvent -= OnLobbyConnected;
                NetworkClient.Lobby.OnNewPlayerJoinRoomEvent -= OnNewPlayerJoinRoomEvent;
            }
        }

        void ShowEnterNicknamePopover()
        {
            PopoverBackground.SetActive(true);
            EnterNicknamePopover.SetActive(true);
        }

        void ShowJoinedRoomPopover()
        {
            EnterNicknamePopover.SetActive(false);
            WaitForOpponentPopover.SetActive(true);
            StartRoomButton.SetActive(false);
            Player1Portrait.SetActive(false);
            Player2Portrait.SetActive(false);
        }

        void ShowReadyToStartUI()
        {
            StartRoomButton.SetActive(true);
            Player1Portrait.SetActive(true);
            Player2Portrait.SetActive(true);
        }

        void HideAllPopover()
        {
            PopoverBackground.SetActive(false);
            EnterNicknamePopover.SetActive(false);
            WaitForOpponentPopover.SetActive(false);
            StartRoomButton.SetActive(false);
            Player1Portrait.SetActive(false);
            Player2Portrait.SetActive(false);
        }

		//****************** Matchmaking *********************//
        void Checkin()
		{
			NetworkClient.Instance.CheckIn(nickname, (bool successful, string error) =>
			{
				if (!successful)
				{
					Debug.LogError(error);
				}
			});
		}

        void RegisterToTheLobbyServer()
        {
            NetworkClient.Lobby.Register(nickname, (successful, reply, error) => {
                if (successful)
                {
                    Debug.Log("Lobby registered " + reply);
                    if (string.IsNullOrEmpty(reply.roomId))
                    {
                        JoinOrCreateRoom();
                    }
                    else if (reply.started)
                    {
                        State = LobbyState.JoinedRoom;
                        ConnectToRoom();
                    }
                    else
                    {
                        State = LobbyState.JoinedRoom;
                        ShowJoinedRoomPopover();
                        GetPlayersInTheRoom();
                    }
                }
                else
                {
                    Debug.Log("Lobby failed to register " + reply);
                }
            });
        }

        void JoinOrCreateRoom()
        {
            NetworkClient.Lobby.JoinOrCreateRoom(false, 2, 60, (successful, reply, error) => {
                if (successful)
                {
                    Debug.Log("Joined or created room " + reply);
                    State = LobbyState.JoinedRoom;
                    ShowJoinedRoomPopover();
                    GetPlayersInTheRoom();
                }
                else
                {
                    Debug.Log("Failed to join or create room " + error);
                }
            });
        }

        void GetPlayersInTheRoom()
        {
            NetworkClient.Lobby.GetPlayersInRoom((successful, reply, error) => {
                if (successful)
                {
                    Debug.Log("Got players " + reply);
                    if(reply.players.Count == 1)
                    {
                        Player1Portrait.SetActive(true);
                    }
                    else
                    {
                        Player1Portrait.SetActive(true);
                        Player2Portrait.SetActive(true);

                        if (NetworkClient.Lobby.IsOwner)
                        {
                            ShowReadyToStartUI();
                        }
                    }
                }
                else
                {
                    Debug.Log("Failed to get players " + error);
                }
            });
        }

        void LeaveRoom()
        {
            NetworkClient.Lobby.LeaveRoom((successful, error) => {
                if (successful)
                {
                    Debug.Log("Left room");
                    State = LobbyState.Default;
                }
                else
                {
                    Debug.Log("Failed to leave room " + error);
                }
            });
        }

        void StartRoom()
        {
            NetworkClient.Lobby.StartRoom((successful, error) => {
                if (successful)
                {
                    Debug.Log("Started room.");
                }
                else
                {
                    Debug.Log("Failed to start room " + error);
                }
            });
        }

        void ConnectToRoom()
        {
            // connect to the game server of the room.
            NetworkClient.Instance.ConnectToRoom((connected) =>
            {
                if (connected)
                {
                    SceneManager.LoadScene("MultiplayerGameScene");
                }
                else
                {
                    Debug.Log("Failed to connect to the game server.");
                }
            });
        }

        //****************** Lobby events *********************//
        void OnLobbyConnected()
		{
            RegisterToTheLobbyServer();
		}

        void OnNewPlayerJoinRoomEvent(SWJoinRoomEventData eventData)
        {
            if (NetworkClient.Lobby.IsOwner)
            {
                ShowReadyToStartUI();
            }
        }

        void OnRoomReadyEvent(SWRoomReadyEventData eventData)
        {
            ConnectToRoom();
        }

        //****************** UI event handlers *********************//
        /// <summary>
        /// Practice button was clicked.
        /// </summary>
        public void OnPracticeClicked()
        {
            Debug.Log("OnPracticeClicked");
            SceneManager.LoadScene("GameScene");
        }

        /// <summary>
        /// Online button was clicked.
        /// </summary>
        public void OnOnlineClicked()
        {
            Debug.Log("OnOnlineClicked");
            ShowEnterNicknamePopover();
        }

        /// <summary>
        /// Cancel button in the popover was clicked.
        /// </summary>
        public void OnCancelClicked()
        {
            Debug.Log("OnCancelClicked");

            if (State == LobbyState.JoinedRoom)
            {
                // TODO: leave room.
                LeaveRoom();
            }

            HideAllPopover();
        }

        /// <summary>
        /// Start button in the WaitForOpponentPopover was clicked.
        /// </summary>
        public void OnStartRoomClicked()
        {
            Debug.Log("OnStartRoomClicked");
            // players are ready to player now.
            if (Debugging)
            {
                SceneManager.LoadScene("GameScene");
            }
            else
            {
                // Start room
                StartRoom();
            }
        }

        /// <summary>
        /// Ok button in the EnterNicknamePopover was clicked.
        /// </summary>
        public void OnConfirmNicknameClicked()
        {
            nickname = NicknameInputField.text;
            Debug.Log($"OnConfirmNicknameClicked: {nickname}");

            if (Debugging)
            {
                ShowJoinedRoomPopover();
                ShowReadyToStartUI();
            }
            else
            {
				//Use nickname as player custom id to check into SocketWeaver.
				Checkin();
            }
        }
    }
}
