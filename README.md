# WASTELAND Jam 02

Game project created for a game jam (WASTELAND Jam 02). Contains scenes, assets and Unity scripts ready to open in the Editor.

**Overview**
- Type: Game prototype (shooter / survival) with weapon assets and zombie animations.
- Structure: contains `Assets`, `Packages` and `ProjectSettings`, configured for URP and the new Input System.

**Unity Version**
- Registered editor version: m_EditorVersion: 6000.3.5f2 (see [ProjectSettings/ProjectVersion.txt](ProjectSettings/ProjectVersion.txt)).

**Requirements**
- Unity Editor (use the version above or the closest supported version).
- Key packages (see `Packages/manifest.json`):
	- com.unity.render-pipelines.universal 17.3.0 (URP)
	- com.unity.inputsystem 1.17.0
	- com.unity.visualscripting 1.9.9
	- com.unity.ai.navigation 2.0.9

**How to open the project**
1. Open Unity Hub.
2. Click "Add" and select the project folder (the root of this repository).
3. Open the project with a compatible Editor version.
4. In the Project window, open a scene to test (for example [Assets/Low Poly Weapons VOL.1/Sample_Scene.unity](Assets/Low%20Poly%20Weapons%20VOL.1/Sample_Scene.unity) or scenes under [Assets/Scenes](Assets/Scenes)).

**How to run**
- With the Editor open, select the desired scene and press Play.
- If needed, open `Window > Package Manager` to ensure packages listed in `Packages/manifest.json` are installed/updated.

**Controls / Input**
- Input actions are defined in `Assets/InputSystem_Actions.inputactions` (using the new Input System).


**Gameplay**
- Setting: the game takes place in an apocalyptic factory arena — a ruined industrial environment with machinery, corridors and cover points.
- Objective: the player must eliminate all zombies present in the arena to win. Clear every enemy in the area to complete the level.
- Player role: a lone survivor armed with weapons found in the environment; use movement and cover to survive while managing ammo and positioning.
- Tips: use available cover, prioritize headshots, and control choke points to avoid being overwhelmed.



**Important notes**
- There is no license file included in this repository. If you plan to publish or collaborate, add an appropriate `LICENSE` file.
- Third-party assets are included in `Assets/Low Poly Weapons VOL.1`, `Assets/Mixamo`, `Assets/SkySeries Freebie`, etc. Check credits and permissions before redistributing.

**Contributing**
- To contribute: create a branch, make changes, and open a pull request describing the updates.

**Contact / Next steps**
- I kept the project information concise. If you'd like, I can add build instructions (Windows/macOS), detailed control mappings, or replace the placeholder screenshots with real captures—tell me which scenes you want captured.
