using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

namespace Drive56.Multiplayer
{
    public class PhotonLobbyManager : MonoBehaviourPunCallbacks
    {
        public string roomName = "Drive56_Room";
        public int defaultMaxPlayers = 4;
        public int maxPlayers = 4;
        public bool autoJoinLobbyOnStart = true;

        public static PhotonLobbyManager Instance { get; private set; }

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void Start()
        {
            if (PhotonNetwork.InLobby || PhotonNetwork.IsConnected) return;

            if (autoJoinLobbyOnStart)
            {
                PhotonNetwork.ConnectUsingSettings();
            }
        }

        public void SetPlayerLimit(int roomLimit)
        {
            maxPlayers = Mathf.Clamp(roomLimit, 2, 16);
        }

        public void CreateRoom(string roomId = "")
        {
            if (string.IsNullOrEmpty(roomId))
            {
                roomId = roomName;
            }

            var roomOptions = new RoomOptions
            {
                MaxPlayers = (byte)maxPlayers,
                PublishUserId = true,
            };

            PhotonNetwork.CreateRoom(roomId, roomOptions, TypedLobby.Default);
        }

        public void JoinRoom(string roomId)
        {
            PhotonNetwork.JoinRoom(roomId);
        }

        public void JoinRandomRoom()
        {
            PhotonNetwork.JoinRandomRoom();
        }

        public void LeaveRoom()
        {
            PhotonNetwork.LeaveRoom();
        }

        public override void OnConnectedToMaster()
        {
            Debug.Log("Connected to Photon master server.");
            PhotonNetwork.JoinLobby();
        }

        public override void OnJoinedLobby()
        {
            Debug.Log("Joined Photon lobby.");
        }

        public override void OnCreatedRoom()
        {
            Debug.Log($"Room created: {PhotonNetwork.CurrentRoom.Name} with max players {PhotonNetwork.CurrentRoom.MaxPlayers}");
        }

        public override void OnJoinedRoom()
        {
            Debug.Log($"Joined room: {PhotonNetwork.CurrentRoom.Name} ");
        }

        public override void OnJoinRoomFailed(short returnCode, string message)
        {
            Debug.LogError($"Join room failed: {returnCode} {message}");
        }

        public override void OnPlayerEnteredRoom(Player newPlayer)
        {
            Debug.Log($"Player entered room: {newPlayer.NickName}");
        }

        public override void OnPlayerLeftRoom(Player otherPlayer)
        {
            Debug.Log($"Player left room: {otherPlayer.NickName}");
        }

        public override void OnRoomListUpdate(List<RoomInfo> roomList)
        {
            foreach (var info in roomList)
            {
                if (info.RemovedFromList)
                {
                    continue;
                }

                Debug.Log($"Available room: {info.Name} ({info.PlayerCount}/{info.MaxPlayers})");
            }
        }
    }
}
