# Drive_56.526 Multiplayer + Save MVP Setup

This branch contains the Unity-ready foundation for the multiplayer MVP:
- Firebase Auth + Firestore save wrappers
- Photon room creation and join flow
- chat box with hide/show toggle
- voice toggle button using Photon Voice
- player profile and vehicle save flow

## Required Unity packages
Install these Unity packages in Package Manager (or via the Unity Package Manager UI):
- Firebase Authentication
- Firebase Firestore
- Firebase App
- Photon PUN 2
- Photon Voice 2

## Required project configuration
1. Create a Firebase project in the Firebase Console.
2. Add an Android app with your Android package ID.
3. Add an iOS app if you need iOS support.
4. Download the config files:
   - google-services.json (Android)
   - GoogleService-Info.plist (iOS)
5. Place them in the Unity project using the Firebase Unity setup flow.
6. Add your Photon App ID to the Photon PUN setup.

## Important: keep secrets out of the repository
Do not commit:
- google-services.json
- GoogleService-Info.plist
- Photon App ID secrets
- service account keys

Use a local project folder or secure CI/secrets flow instead.

## Features included
- Player profile save system (Firestore)
- Player money / owned vehicles / unlocked cars
- Lobby player-limit setting by room creator
- Room join/create in Photon
- Chat panel in the top-left of the screen with hide/show toggle
- Voice toggle button with mic activation/deactivation logic
- Extension points for the garage and the driving gameplay

## Example usage
- Open the Firebase bootstrap flow before first scene load.
- When a user signs in, call FirebaseProfileService.LoadProfileAsync().
- When a player buys a vehicle, call FirebaseProfileService.BuyVehicleAsync().
- Use PhotonLobbyManager.CreateRoom() and PhotonLobbyManager.JoinRoom() from the lobby UI.
- Use ChatController to send and receive room messages.
- Use VoiceChatController to toggle microphone transmit state.

## Notes
This is a multiplayer-ready Unity skeleton and should be treated as the foundation for your actual game flow. The real Firebase project configuration and the Photon App ID must still be supplied for online play to work.
