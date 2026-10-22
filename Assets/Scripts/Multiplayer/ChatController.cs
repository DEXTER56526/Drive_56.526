using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

namespace Drive56.Multiplayer
{
    public class ChatController : MonoBehaviourPunCallbacks
    {
        public GameObject chatPanel;
        public Text chatText;
        public InputField chatInput;
        public Button sendButton;
        public Button toggleButton;

        private const string CHAT_EVENT = "ChatEvent";
        private readonly List<string> _messages = new List<string>();

        private void Awake()
        {
            if (toggleButton != null)
            {
                toggleButton.onClick.AddListener(ToggleChatPanel);
            }

            if (sendButton != null)
            {
                sendButton.onClick.AddListener(SendCurrentMessage);
            }
        }

        public void ToggleChatPanel()
        {
            if (chatPanel == null) return;
            chatPanel.SetActive(!chatPanel.activeSelf);
        }

        public void SendCurrentMessage()
        {
            if (chatInput == null || string.IsNullOrWhiteSpace(chatInput.text)) return;
            SendMessageToRoom(chatInput.text);
            chatInput.text = string.Empty;
        }

        public void SendMessageToRoom(string message)
        {
            if (!PhotonNetwork.InRoom)
            {
                Debug.LogWarning("Cannot send chat while not in a Photon room.");
                return;
            }

            var payload = PhotonNetwork.LocalPlayer.NickName + ": " + message;
            object[] data = { payload };
            PhotonNetwork.RaiseEvent(CHAT_EVENT, data, new Photon.Realtime.RaiseEventOptions { Receivers = Photon.Realtime.ReceiverGroup.All }, new Photon.Realtime.SendOptions { Reliability = true });
        }

        public override void OnEnable()
        {
            base.OnEnable();
            PhotonNetwork.NetworkingClient.EventReceived += HandleEvent;
        }

        public override void OnDisable()
        {
            base.OnDisable();
            PhotonNetwork.NetworkingClient.EventReceived -= HandleEvent;
        }

        private void HandleEvent(EventData photonEvent)
        {
            if (photonEvent.Code != CHAT_EVENT) return;

            object[] data = (object[])photonEvent.CustomData;
            if (data == null || data.Length == 0) return;

            var text = data[0].ToString();
            _messages.Add(text);
            UpdateChatLabel();
        }

        private void UpdateChatLabel()
        {
            if (chatText == null) return;

            const int maxMessages = 12;
            var visibleMessages = _messages.Count > maxMessages ? _messages.GetRange(_messages.Count - maxMessages, maxMessages) : _messages;
            chatText.text = string.Join("\n", visibleMessages);
        }
    }
}
