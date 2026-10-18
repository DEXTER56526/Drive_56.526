# Prototype Scene Setup

This document shows step-by-step instructions to assemble the prototype scene in Unity using primitives and the provided scripts.

1) Create a new Scene
- File -> New Scene. Save it as Assets/Scenes/Prototype.unity (you can create the Scenes folder).

2) Add ground
- GameObject -> 3D Object -> Plane (rename to Ground)
- Scale the plane to e.g. 10x10 (Scale: 10,1,10) or larger to create an open area.
- Add a Physics Material to the ground if you want low friction.

3) Create a Player vehicle (using simple primitives)
- GameObject -> 3D Object -> Cube. Rename to PlayerCar.
- Add a Rigidbody component.
- Add the script Assets/Scripts/VehicleController.cs to PlayerCar.
- Optionally create child Cubes for wheels and visuals.
- Tweak Rigidbody mass (e.g., 1200) and the VehicleController parameters (maxMotorForce, topSpeed).

4) Camera setup
- Main Camera: remove existing camera or use the Main Camera.
- Attach Assets/Scripts/CameraFollow.cs to the Main Camera.
- In the inspector assign the PlayerCar transform to the target field and set offset to (0,4,-6).

5) SpawnManager
- Create an empty GameObject named SpawnManager and put it at the center of the map.
- Attach Assets/Scripts/SpawnManager.cs.
- For prototype vehiclePrefabs, you can create simple prefab copies of PlayerCar with different colors and drag them to the vehiclePrefabs array.

6) Driving loop / Free-roam environment
- Use a combination of planes, cubes, and ramps to make a small driving playground.
- Place some static obstacles (cubes) and plates to act as roads.

7) Play and test
- Press Play and control the PlayerCar with WASD / arrow keys. Use Space for an emergency brake (handbrake).

Notes
- The provided VehicleController is a simplified prototype. For more realistic behavior, replace with WheelCollider-based setup or a vehicle package from the Asset Store.
- If you want, I can add a simple WheelCollider-based controller next.
