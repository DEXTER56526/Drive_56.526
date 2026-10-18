# Demo Scene & Prefab Setup

This document explains how the demo scene and prefabs are arranged for a playable cross-platform prototype.

Files added in this commit:
- Assets/Scripts/InputAdapter.cs
- Assets/Scripts/MobileTouchController.cs
- Assets/Scripts/DamageController.cs
- Assets/Scripts/GasStation.cs
- Assets/Scripts/CameraManager.cs
- Assets/Scripts/VehicleReset.cs
- Assets/Scripts/WheelVehicleController.cs (updated to use InputAdapter and DamageController)

Create the demo scene (manually in Unity):
1. Create a new Scene and save as Assets/Scenes/Demo.unity (create Scenes folder).
2. Create an empty GameObject "GameManager" and attach InputAdapter. Also add MobileTouchController as a child if testing mobile.
3. Create a Plane (scale large) for the ground.
4. Create a Car prefab:
   - Empty GameObject "CarRoot" with Rigidbody and VehicleReset.
   - Add WheelVehicleController and DamageController to CarRoot.
   - Create four WheelCollider child objects at wheel positions and assign them in WheelVehicleController.
   - Create four visual wheel meshes (cylinders) and assign transforms in WheelVehicleController.
   - Set mass to ~1200, set drag low.
   - Make the CarRoot into a prefab and place an instance in the scene.
5. Camera setup:
   - Create empty GameObjects under the car as camera points: "Cam_3rd", "Cam_Hood", "Cam_Top" with appropriate positions/rotations.
   - Create a CameraManager object, assign the main camera and the camera points (absolute transforms or child transforms of the car).
6. Gas Station:
   - Create a BoxCollider with isTrigger=true and attach GasStation.
   - Place in the world where you want automatic repair behavior.
7. UI:
   - Create a Canvas and add Text for speed (hook HUDManager), create a RawImage for minimap, and create UI buttons for mobile.
   - Hook up MobileTouchController to UI button callbacks.

Notes:
- I did not commit binary scene/prefab files to keep repo size small. If you want, I can add a sample scene and prefabs (Unity YAML) but recommend you create them via the editor to ensure correct GUIDs.
- If you prefer I can generate basic prefab YAML and a scene file; tell me and I will add them.
