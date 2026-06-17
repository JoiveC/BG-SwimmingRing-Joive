# Changelog

## 1.0.16

- Split world item visual, player visual, and shared visual preparation into separate classes.
- Renamed the world visual transform method to final production naming.
- Shortened config descriptions.

## 1.0.15

- Removed temporary debug visual fallback code from the swimming ring visual path.
- Renamed the world visual transform method to final production naming.
- Simplified AssetBundle prefab loading to the expected prefab path.
- Simplified runtime material shader lookup to the expected Standard shader.

## 1.0.14

- Split runtime visual material handling into a dedicated game helper.
- Kept Unity image loading reflection inside the game API layer.
- Reduced diagnostics to debug logging unless a warning is actionable.
- Updated the README to match the current DLL, embedded assets, localization, and config.

## 1.0.13

- Replaced the generated inventory icon with an embedded rendered swimming ring icon.

## 1.0.12

- Added localized swimming ring name and description entries for all available game locales.

## 1.0.11

- Restored the swimming ring as wearable equipment.
- Ignored swimming ring activation in the game's wearable equipment handler to avoid resetting real wearable equipment.
- Applied swimming behavior and player visual only while the swimming ring is in a wearable slot.
- Increased the player visual size.

## 1.0.10

- Restricted the swimming ring to normal inventory slots so it cannot occupy wearable slots.
- Increased the default player visual size and stripped gameplay components from the player visual.

## 1.0.9

- Made the swimming ring a regular inventory item so it does not conflict with wearable equipment.
- Added a player visual while the swimming ring is in inventory.

## 1.0.8

- Preserved the bundle texture color by using white runtime material tint when a texture is available.

## 1.0.7

- Added configurable visual scale and height offset.
- Added a game-compatible orange runtime material fallback for the swimming ring visual.

## 1.0.6

- Removed the temporary checker material override so the prefab material renders in game.

## 1.0.5

- Added a generated swimming ring icon for inventory UI.

## 1.0.4

- Added a temporary bright debug material and renderer diagnostics for the swimming ring visual.

## 1.0.3

- Added runtime diagnostics for embedded bundle loading.
- Applied the swimming ring visual to spawned item instances.

## 1.0.2

- Embedded the swimming ring bundle into the plugin DLL.

## 1.0.1

- Added the swimming ring visual prefab from `swimmingring.bundle`.

## 1.0.0

- Added the craftable swimming ring equipment item.
- Added water movement support while the item is equipped.
