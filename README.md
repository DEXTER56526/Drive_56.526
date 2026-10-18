# Drive_56.526

A Unity-based free-roam driving game inspired by Driving Empire (Roblox). This repository contains the initial project scaffold and prototype scripts to get a basic vehicle driving prototype running.

Goals
- Prototype a physics-driven vehicle you can drive around an open scene.
- Spawn random vehicles for free-roam play.
- Keep art and code separate: Assets/ for Unity content, Assets/Scripts/ for C# scripts.

Unity version
- Recommended: Unity 2021.3 LTS or newer (2022.3 LTS is fine). Use the version you have installed.

What I added in branch `feature/init-project`
- README.md (this file)
- ROADMAP.md (project milestones and next steps)
- .gitignore (Unity-specific)
- Assets/Scripts/VehicleController.cs (prototype Rigidbody-based car controller)
- Assets/Scripts/CameraFollow.cs (smooth third-person camera follow)
- Assets/Scripts/SpawnManager.cs (simple vehicle spawner manager)
- docs/SCENE_SETUP.md (step-by-step to assemble the prototype scene using primitives)

How to open
1. Clone the repo and open it in Unity.
2. Create a new Unity project (if you prefer) and copy the Assets and docs folders into the Unity project folder.
3. Follow docs/SCENE_SETUP.md to create the prototype scene and assign the scripts.

Next
- If you want, I can add prefab examples (JSON-like placeholders) or a minimal Unity scene file. Unity scenes are larger and binary; I can add them if you prefer.
- I can also open a PR or commit directly to `main` — currently changes are on `feature/init-project`.

Status: prototype scripts added. Test the car with a Rigidbody on a GameObject and follow the scene setup guide.
