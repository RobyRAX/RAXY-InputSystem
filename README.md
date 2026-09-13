# RAXY Input System

RAXY Input System provides a flexible touch gesture module and an event-driven workflow managed through the `Raiser` class.

It is designed to unify and streamline both touch and keyboard inputs, allowing you to switch or expand input methods without modifying your core gameplay logic.

This package uses the **[Joystick Pack](https://assetstore.unity.com/packages/tools/input-management/joystick-pack-107631)** for virtual joystick support, and the asset is already included inside the package.

## Features
- Touch gesture recognition (tap, swipe, pinch)
- Centralized event-based input handling
- Easy expansion using the `Raiser` class
- Clean separation between input detection and gameplay logic
- Built-in virtual joystick support via Joystick Pack

## Samples

Import **Basic Setup** from Package Manager → RAXY Input System → Samples.

After import it lands at:

`Assets/Samples/RAXY Input System/1.1.0/Basic Setup/`

Contents:

- **Move / Jump / Interact / Show Cursor** `InputActionEventSO` assets
- **Sample Input Manager** prefab — `InputActionEventBindingManager` (action refs left empty), `CursorManager`, and `SampleInputDebugListener`
- **Sample On-Screen Controls** prefab — EventSystem + Canvas with joystick + Jump/Interact button raisers

### Quick verify

1. Drop **Sample Input Manager** and **Sample On-Screen Controls** into a scene.
2. Enter Play Mode.
3. Drag the on-screen joystick and press Jump / Interact — Console should log events from `SampleInputDebugListener`.

### Optional: Unity Input Actions

Assign your project's `InputActionReference`s to the BindingManager rows on **Sample Input Manager**. Keyboard/gamepad will raise the same event SOs as the on-screen controls.
