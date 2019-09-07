using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

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
            PopoverBackground.SetActive(false);
            EnterNicknamePopover.SetActive(false);
            WaitForOpponentPopover.SetActive(false);
            StartRoomButton.SetActive(false);
            Player1Portrait.SetActive(false);
            Player2Portrait.SetActive(false);
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
            PopoverBackground.SetActive(true);
            EnterNicknamePopover.SetActive(true);
        }

        /// <summary>
        /// Cancel button in the popover was clicked.
        /// </summary>
        public void OnCancelClicked()
        {
            Debug.Log("OnCancelClicked");
            // TODO: leave room.
            if (State == LobbyState.JoinedRoom)
            {

            }

            PopoverBackground.SetActive(false);
            EnterNicknamePopover.SetActive(false);
            WaitForOpponentPopover.SetActive(false);
        }

        /// <summary>
        /// Start button in the WaitForOpponentPopover was clicked.
        /// </summary>
        public void OnStartRoomClicked()
        {
            Debug.Log("OnStartRoomClicked");
            if (Debugging)
            {
                SceneManager.LoadScene("GameScene");
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
                EnterNicknamePopover.SetActive(false);
                WaitForOpponentPopover.SetActive(true);
                StartRoomButton.SetActive(true);
                Player1Portrait.SetActive(true);
                Player2Portrait.SetActive(true);
            }
            else
            {
                //TODO: Use nickname as player custom id to check into SocketWeaver.
            }
        }
    }
}
