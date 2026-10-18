# Roadmap

This file outlines an initial roadmap for the Drive_56.526 project.

MVP (Minimum Viable Product)
- Player: drive a single vehicle (Rigidbody-based) with keyboard controls.
- Camera: third-person follow camera.
- World: a flat terrain / plane with primitive obstacles and a driving loop for free-roam.
- Vehicle spawner: spawn random vehicles using placeholder models or primitives.
- UI: speedometer and simple HUD (optional for first playtests).

Phase 2 (Features to add after MVP)
- Traffic AI: NPC vehicles driving routes and reacting to the player.
- Missions & progression: objectives, currency, and unlockable vehicles.
- Garage: vehicle selection and upgrades.
- Damage model: visual and physics-based damage.
- Multiplayer: networked driving sessions.
- Optimization: LOD, pooling, and physics tuning.

Assets & Tools
- Use free assets from Unity Asset Store, Kenney, or OpenGameArt for models and sounds.
- Placeholder prototypes will use Unity primitives (Cube, Capsule) or free low-poly car models.

Development workflow
- Prototype rapidly in a single scene.
- Refactor into modular systems: VehicleController, SpawnManager, WorldGenerator, UI.
- Keep code under Assets/Scripts and art under Assets/Art or Assets/Models.

Contributing
- Use feature branches for new systems (e.g. feature/traffic-ai).
- Open PRs to `main` from feature branches.
