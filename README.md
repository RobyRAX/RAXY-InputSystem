# RAXY Input System

RAXY Input System provides a flexible touch gesture module and an event-driven workflow managed through the `Raiser` class.

It is designed to unify and streamline both touch and keyboard inputs, allowing you to switch or expand input methods without modifying your core gameplay logic.

Virtual on-screen joystick support is provided through **`JoystickInputActionRaiser`** and integrates with **[Joystick Pack](https://assetstore.unity.com/packages/tools/input-management/joystick-pack-107631)**. Joystick Pack **prefabs and sprites** ship with the **Basic Setup** sample (import from Package Manager); runtime Joystick scripts remain in the package core.

## Features
- Touch gesture recognition (tap, swipe, pinch)
- Centralized event-based input handling
- Easy expansion using the `Raiser` class
- Clean separation between input detection and gameplay logic
- On-screen joystick integration via `JoystickInputActionRaiser` (see **Basic Setup** sample)

## Samples

Import **Basic Setup** from Package Manager → RAXY Input System → Samples.

After import it lands at:

`Assets/Samples/RAXY Input System/1.1.1/Basic Setup/`

Contents:

- **Move / Jump / Interact / Show Cursor** `InputActionEventSO` assets
- **Sample Input Manager** prefab — `InputActionEventBindingManager` (action refs left empty), `CursorManager`, and `SampleInputDebugListener`
- **Sample On-Screen Controls** prefab — EventSystem + Canvas with joystick + Jump/Interact button raisers
- **Joystick Pack** prefabs and sprites (third-party Asset Store assets) — see `Joystick Pack/README.md` in the imported sample folder

### Quick verify

1. Drop **Sample Input Manager** and **Sample On-Screen Controls** into a scene.
2. Enter Play Mode.
3. Drag the on-screen joystick and press Jump / Interact — Console should log events from `SampleInputDebugListener`.

### Optional: Unity Input Actions

Assign your project's `InputActionReference`s to the BindingManager rows on **Sample Input Manager**. Keyboard/gamepad will raise the same event SOs as the on-screen controls.
