# Advanced Setup & Cross-platform Guidance

This guide explains how to wire the realistic vehicle controller, garage, world and HUD, and what to consider for cross-platform support.

Wiring the WheelCollider car
1. Create an empty GameObject named "CarRoot".
2. Create 4 child GameObjects for visual wheels and 4 empty children for colliders (or use the same transforms).
3. Add WheelCollider components to the collider placeholders and position them at wheel contact points.
4. Add the `WheelVehicleController` script to CarRoot and assign the WheelColliders and visual wheel transforms.
5. Add a Rigidbody to CarRoot. Set center of mass lower for stability. Typical mass: 1200.

Minimap (RenderTexture)
1. Create a new Camera and set it to Orthographic.
2. Position it above the player (e.g., Y = 50) and point straight down.
3. Create a RenderTexture (Assets -> Create -> Render Texture) and assign it to the minimap camera Target Texture.
4. Create a RawImage in the UI and assign the RenderTexture to it.

Garage UI
- Create a Canvas -> Panel, add a Vertical Layout Group and ContentSizeFitter to hold buttons.
- Create a Button prefab with a Text child and assign it as buttonTemplate in GarageManager.
- GarageManager will populate the list automatically on Start.

Road & World
- Add an empty GameObject with RoadGenerator and assign a roadPrefab (a long cube scaled along X) and intersectionPrefab.
- Add WorldGenerator and assign props (tree prefabs, rocks). Set terrainSize and propCount.

Cross-platform build considerations
- Unity supports many platforms. Some require additional setup:
  - Mobile (iOS/Android): Input must support touch. Use Unity's Input System or map virtual joystick/buttons. Optimize physics and texture sizes, reduce draw calls, and use mobile-friendly shaders.
  - WebGL: Avoid heavy physics and large textures. Use lower physics time step and bake static props. WebGL does not support threads or some native plugins.
  - Consoles (PS, Xbox, Nintendo): You need platform licenses and dev kits; Unity supports console deployment but setup is per-platform and usually not available for indie/personal accounts without approval.
  - PC: Windows/Mac/Linux builds are straightforward.

Input abstraction
- Use Unity's new Input System or write an InputAdapter so you can switch between keyboard, touch, and gamepad easily.

Performance tips
- Use object pooling for vehicles and props.
- Use LOD groups for meshes and reduce physics layers for distant actors.
- Optimize FixedUpdate frequency for mobile.

Next steps I can implement for you
- Create prefab templates for a WheelCollider car and a simple UI prefab for the garage.
- Implement mobile touch controls and gamepad mapping.
- Add a small sample scene (binary) if you want the full Unity scene committed — note big binary files will increase repo size.


