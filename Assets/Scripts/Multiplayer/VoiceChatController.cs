using Photon.Pun;
using Photon.Voice.PUN;
using UnityEngine;
using UnityEngine.UI;

namespace Drive56.Multiplayer
{
    public class VoiceChatController : MonoBehaviour
    {
        public Button voiceToggleButton;
        public Text voiceButtonText;
        public PhotonVoiceView voiceView;

        private bool _micEnabled;

        private void Start()
        {
            if (voiceToggleButton != null)
            {
                voiceToggleButton.onClick.AddListener(ToggleMicrophone);
            }

            UpdateVoiceButtonVisual();
        }

        public void ToggleMicrophone()
        {
            _micEnabled = !_micEnabled;

            if (voiceView != null)
            {
                // Photon Voice handles recording via the PhotonVoiceView / Recorder component.
                // If the Recorder is assigned, toggle its recorder state here.
                var recorder = voiceView.GetComponent<Recorder>();
                if (recorder != null)
                {
                    recorder.Transmit = _micEnabled;
                }
            }

            UpdateVoiceButtonVisual();
        }

        private void UpdateVoiceButtonVisual()
        {
            if (voiceButtonText != null)
            {
                voiceButtonText.text = _micEnabled ? "Voice: ON" : "Voice: OFF";
            }

            if (voiceToggleButton != null)
            {
                var color = _micEnabled ? Color.green : Color.gray;
                var img = voiceToggleButton.targetGraphic as UnityEngine.UI.Image;
                if (img != null)
                {
                    img.color = color;
                }
            }
        }
    }
}
