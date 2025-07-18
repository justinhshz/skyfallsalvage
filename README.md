# 🌌 Skyfall Salvage: Catch the Trash, Make a Splash

[![Unity Version](https://img.shields.io/badge/Unity-2022.3.42f1-blue?logo=unity)](https://unity.com/releases/editor/whats-new/2022.3.42)
[![WebGL](https://img.shields.io/badge/WebGL-Only-orange?logo=googlechrome&logoColor=white)](https://get.webgl.org/)
[![itch.io](https://img.shields.io/badge/itch.io-Play%20Now!-FA5C5C?logo=itchdotio&logoColor=white)](https://justin-hshz.itch.io/skyfall-salvage)
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg?logo=github)](LICENSE)

Welcome to **Skyfall Salvage** — a lighthearted 2D roguelike merge-and-upgrade puzzle game. Salvage falling parts, combine matching pieces, and unlock special effects to climb the score ladder and outsmart the junkstorm from above!

![---](https://raw.githubusercontent.com/andreasbm/readme/master/assets/lines/rainbow.png)

## 📄 Table of Contents

* 🎮 [Gameplay Overview](#-gameplay-overview)
* 🕹️ [Game Controls](#%EF%B8%8F-game-controls)
* 🧩 [Item Overview](#-item-overview)
    * 🧬 [Elements](#-elements)
    * 🧷 [Connectors](#-connectors)
    * ⚡ [Equipments](#-equipments)
* 🧪 [Crafting Recipes](#-crafting-recipes)
    * 🚀 [Equipment Lv.1](#-equipment-lv1)
    * 🚿 [Equipment Lv.2](#-equipment-lv2)
    * 🤖 [Equipment Lv.3](#-equipment-lv3)
    * 🦾 [Equipment Lv.4](#-equipment-lv4)
* 🎯 [Skill Choices](#-skill-choices)
* 🏷️ [Icon Credits](#%EF%B8%8F-icon-credits)

![---](https://raw.githubusercontent.com/andreasbm/readme/master/assets/lines/rainbow.png)

## 🎮 Gameplay Overview

In a dystopian future, the wealthy live in a majestic floating city known as Celestium, while the poor struggle to survive on the ground below. You are an engineer salvaging discarded components from above, merging and upgrading them to survive—and possibly rebel.

**Core Mechanics:**

* 🧬 **Falling Elements**\
    Elements fall from the top of the screen. When two identical elements collide, they merge into a higher-level element. Only the first few low-tier elements will fall. Higher-level elements must be obtained by merging—they will never drop directly.

* 🧷 **Connectors System**\
    Occasionally, connectors fall. When they contact **two specific types of different elements**, they combine to form a **equipment**.

* ⏱️ **Controlled Drop Timing**\
    Elements and connectors move back and forth horizontally across the screen while a countdown timer runs. The player must **click** or press **spacebar** before the timer expires to decide when and where to drop the item. If no input is given in time, the item will automatically fall at its current position.

* ⚡ **Activating Equipments**\
    Equipments are crafted during gameplay but remain inactive until your score reaches their activation threshold. Once unlocked, you can trigger them at any time for powerful effects like instant upgrades or element conversions.

* 🎯 **Skill Selection**\
    Skills are offered at regular score intervals, allowing you to choose effects like score multipliers for merges or extended countdown time before auto-drop. These choices shape each run and add variety to the roguelike experience.

* ⚠️ **Game Over & Rankings**\
    The game ends when items stack beyond the designated boundary line near the top of the field. Players must manage space carefully to avoid reaching this critical limit. After the game ends, your score is recorded and compared on a leaderboard, adding a competitive element to each run.

![---](https://raw.githubusercontent.com/andreasbm/readme/master/assets/lines/rainbow.png)

## 🕹️ Game Controls

This game is fully playable with a mouse or keyboard:

* **Left Click / Spacebar** – Drop the current element or connector at its horizontal position.
* **Click on Skills** – Choose one when the selection appears at specific score milestones.
* **Click on Equipments** – Activate them after unlocking, at any time during gameplay.

> [!TIP]
> No additional keyboard input required.

![---](https://raw.githubusercontent.com/andreasbm/readme/master/assets/lines/rainbow.png)

## 🧩 Item Overview

There are three core item types—**Elements**, **Connectors**, and **Equipments**—each with distinct mechanics and clear visual hierarchy. Below is an overview to help you quickly identify and understand the role of each category.

### 🧬 Elements

Elements are the fundamental items that fall from above. When two identical elements collide, they merge into a higher-level element. Only lower-tier elements drop; advanced levels must be achieved through merging.

<table>
    <tr>
        <td align="center">
            <img src="Assets/Development/Sprites/Entities/Elements/%5BElement%5D%20Level%201.png"
            width="60" />
            <div style="margin-top: 4px;">
                <strong>
                    Lv.1
                </strong>
            </div>
        </td>
        <td align="center">
            <img src="Assets/Development/Sprites/Entities/Elements/%5BElement%5D%20Level%202.png"
            width="60" />
            <div style="margin-top: 4px;">
                <strong>
                    Lv.2
                </strong>
            </div>
        </td>
        <td align="center">
            <img src="Assets/Development/Sprites/Entities/Elements/%5BElement%5D%20Level%203.png"
            width="60" />
            <div style="margin-top: 4px;">
                <strong>
                    Lv.3
                </strong>
            </div>
        </td>
        <td align="center">
            <img src="Assets/Development/Sprites/Entities/Elements/%5BElement%5D%20Level%204.png"
            width="60" />
            <div style="margin-top: 4px;">
                <strong>
                    Lv.4
                </strong>
            </div>
        </td>
        <td align="center">
            <img src="Assets/Development/Sprites/Entities/Elements/%5BElement%5D%20Level%205.png"
            width="60" />
            <div style="margin-top: 4px;">
                <strong>
                    Lv.5
                </strong>
            </div>
        </td>
        <td align="center">
            <img src="Assets/Development/Sprites/Entities/Elements/%5BElement%5D%20Level%206.png"
            width="60" />
            <div style="margin-top: 4px;">
                <strong>
                    Lv.6
                </strong>
            </div>
        </td>
        <td align="center">
            <img src="Assets/Development/Sprites/Entities/Elements/%5BElement%5D%20Level%207.png"
            width="60" />
            <div style="margin-top: 4px;">
                <strong>
                    Lv.7
                </strong>
            </div>
        </td>
        <td align="center">
            <img src="Assets/Development/Sprites/Entities/Elements/%5BElement%5D%20Level%208.png"
            width="60" />
            <div style="margin-top: 4px;">
                <strong>
                    Lv.8
                </strong>
            </div>
        </td>
        <td align="center">
            <img src="Assets/Development/Sprites/Entities/Elements/%5BElement%5D%20Level%209.png"
            width="60" />
            <div style="margin-top: 4px;">
                <strong>
                    Lv.9
                </strong>
            </div>
        </td>
        <td align="center">
            <img src="Assets/Development/Sprites/Entities/Elements/%5BElement%5D%20Level%2010.png"
            width="60" />
            <div style="margin-top: 4px;">
                <strong>
                    Lv.10
                </strong>
            </div>
        </td>
    </tr>
</table>

### 🧷 Connectors

Connectors are special items that trigger crafting when they contact two specific, compatible elements. They do not merge with other connectors and only appear occasionally.

<table>
    <tr>
        <td align="center">
            <img src="Assets/Development/Sprites/Entities/Connectors/%5BConnector%5D%20Level%201.png"
            width="60" />
            <div style="margin-top: 4px;">
                <strong>
                    Lv.1
                </strong>
            </div>
        </td>
        <td align="center">
            <img src="Assets/Development/Sprites/Entities/Connectors/%5BConnector%5D%20Level%202.png"
            width="60" />
            <div style="margin-top: 4px;">
                <strong>
                    Lv.2
                </strong>
            </div>
        </td>
    </tr>
</table>

### ⚡ Equipments

Equipments are the result of successful combinations involving a connector and two matching elements. They provide powerful effects but can only be activated once your score reaches the required threshold.

<table>
    <tr>
        <td align="center">
            <img src="Assets/Development/Sprites/Entities/Equipments/%5BEquipment%5D%20Level%201.png"
            width="60" />
            <div style="margin-top: 4px;">
                <strong>
                    Lv.1
                </strong>
            </div>
        </td>
        <td align="center">
            <img src="Assets/Development/Sprites/Entities/Equipments/%5BEquipment%5D%20Level%202.png"
            width="60" />
            <div style="margin-top: 4px;">
                <strong>
                    Lv.2
                </strong>
            </div>
        </td>
        <td align="center">
            <img src="Assets/Development/Sprites/Entities/Equipments/%5BEquipment%5D%20Level%203.png"
            width="60" />
            <div style="margin-top: 4px;">
                <strong>
                    Lv.3
                </strong>
            </div>
        </td>
        <td align="center">
            <img src="Assets/Development/Sprites/Entities/Equipments/%5BEquipment%5D%20Level%204.png"
            width="60" />
            <div style="margin-top: 4px;">
                <strong>
                    Lv.4
                </strong>
            </div>
        </td>
    </tr>
</table>

![---](https://raw.githubusercontent.com/andreasbm/readme/master/assets/lines/rainbow.png)

## 🧪 Crafting Recipes

The following recipes show how specific combinations of two elements and one connector create unique equipments, each with a powerful effect and activation score:

### 🚀 Equipment Lv.1

> [!NOTE]
> **Activation Score:** 30\
> **Effect:** Upgrades all elements it touches by one level (e.g., if two Lv.1 elements and one Lv.3 element are in contact, they will become Lv.2, Lv.2, and Lv.4 respectively).

<table>
    <tr>
        <td align="center">
            <img src="Assets/Development/Sprites/Entities/Elements/%5BElement%5D%20Level%202.png"
            width="60" />
            <br/>
            <strong>
                Element Lv.2
            </strong>
        </td>
        <td align="center">
            <strong>
                ＋
            </strong>
        </td>
        <td align="center">
            <img src="Assets/Development/Sprites/Entities/Elements/%5BElement%5D%20Level%203.png"
            width="60" />
            <br/>
            <strong>
                Element Lv.3
            </strong>
        </td>
        <td align="center">
            <strong>
                ＋
            </strong>
        </td>
        <td align="center">
            <img src="Assets/Development/Sprites/Entities/Connectors/%5BConnector%5D%20Level%201.png"
            width="60" />
            <br/>
            <strong>
                Connector Lv.1
            </strong>
        </td>
        <td align="center">
            <strong>
                ＝
            </strong>
        </td>
        <td align="center">
            <img src="Assets/Development/Sprites/Entities/Equipments/%5BEquipment%5D%20Level%201.png"
            width="60" />
            <br/>
            <strong>
                Equipment Lv.1
            </strong>
        </td>
    </tr>
</table>

### 🚿 Equipment Lv.2

> [!NOTE]
> **Activation Score:** 50\
> **Effect:** Converts all elements it touches into the one with the lowest tier among them (e.g., if it touches a Lv.2, Lv.3, and Lv.4 element, all will become Lv.2).

<table>
    <tr>
        <td align="center">
            <img src="Assets/Development/Sprites/Entities/Elements/%5BElement%5D%20Level%203.png"
            width="60" />
            <br/>
            <strong>
                Element Lv.3
            </strong>
        </td>
        <td align="center">
            <strong>
                ＋
            </strong>
        </td>
        <td align="center">
            <img src="Assets/Development/Sprites/Entities/Elements/%5BElement%5D%20Level%204.png"
            width="60" />
            <br/>
            <strong>
                Element Lv.4
            </strong>
        </td>
        <td align="center">
            <strong>
                ＋
            </strong>
        </td>
        <td align="center">
            <img src="Assets/Development/Sprites/Entities/Connectors/%5BConnector%5D%20Level%201.png"
            width="60" />
            <br/>
            <strong>
                Connector Lv.1
            </strong>
        </td>
        <td align="center">
            <strong>
                ＝
            </strong>
        </td>
        <td align="center">
            <img src="Assets/Development/Sprites/Entities/Equipments/%5BEquipment%5D%20Level%202.png"
            width="60" />
            <br/>
            <strong>
                Equipment Lv.2
            </strong>
        </td>
    </tr>
</table>

### 🤖 Equipment Lv.3

> [!NOTE]
> **Activation Score:** 100\
> **Effect:** Shrinks all items it touches to make space (e.g., affected items will occupy smaller physical space, allowing more room before hitting the stack limit).

<table>
    <tr>
        <td align="center">
            <img src="Assets/Development/Sprites/Entities/Elements/%5BElement%5D%20Level%203.png"
            width="60" />
            <br/>
            <strong>
                Element Lv.3
            </strong>
        </td>
        <td align="center">
            <strong>
                ＋
            </strong>
        </td>
        <td align="center">
            <img src="Assets/Development/Sprites/Entities/Elements/%5BElement%5D%20Level%204.png"
            width="60" />
            <br/>
            <strong>
                Element Lv.4
            </strong>
        </td>
        <td align="center">
            <strong>
                ＋
            </strong>
        </td>
        <td align="center">
            <img src="Assets/Development/Sprites/Entities/Connectors/%5BConnector%5D%20Level%202.png"
            width="60" />
            <br/>
            <strong>
                Connector Lv.2
            </strong>
        </td>
        <td align="center">
            <strong>
                ＝
            </strong>
        </td>
        <td align="center">
            <img src="Assets/Development/Sprites/Entities/Equipments/%5BEquipment%5D%20Level%203.png"
            width="60" />
            <br/>
            <strong>
                Equipment Lv.3
            </strong>
        </td>
    </tr>
</table>

### 🦾 Equipment Lv.4

> [!NOTE]
> **Activation Score:** 150\
> **Effect:** Transforms all connectors currently present on the field into the lowest-tier element it contacts (e.g., if it touches Lv.2 and Lv.5 elements, all connectors become Lv.2 elements).

<table>
    <tr>
        <td align="center">
            <img src="Assets/Development/Sprites/Entities/Elements/%5BElement%5D%20Level%204.png"
            width="60" />
            <br/>
            <strong>
                Element Lv.4
            </strong>
        </td>
        <td align="center">
            <strong>
                ＋
            </strong>
        </td>
        <td align="center">
            <img src="Assets/Development/Sprites/Entities/Elements/%5BElement%5D%20Level%205.png"
            width="60" />
            <br/>
            <strong>
                Element Lv.5
            </strong>
        </td>
        <td align="center">
            <strong>
                ＋
            </strong>
        </td>
        <td align="center">
            <img src="Assets/Development/Sprites/Entities/Connectors/%5BConnector%5D%20Level%202.png"
            width="60" />
            <br/>
            <strong>
                Connector Lv.2
            </strong>
        </td>
        <td align="center">
            <strong>
                ＝
            </strong>
        </td>
        <td align="center">
            <img src="Assets/Development/Sprites/Entities/Equipments/%5BEquipment%5D%20Level%204.png"
            width="60" />
            <br/>
            <strong>
                Equipment Lv.4
            </strong>
        </td>
    </tr>
</table>

![---](https://raw.githubusercontent.com/andreasbm/readme/master/assets/lines/rainbow.png)

## 🎯 Skill Choices

As your score increases, you'll be presented with a choice of skills that enhance your gameplay. These upgrades are stackable and help shape your strategy in each run.

### ✨ **Available Skills**

| 📋 Skill Name                  | 💥 Effect                                                                     |
| ------------------------------ | ----------------------------------------------------------------------------- |
| **Merge Bonus +1 / +2 / +3**   | Gain additional score (+1, +2, or +3) for every successful merge.             |
| **Countdown Duration +1 / +2** | Increases drop countdown by 1 or 2 seconds, offering more placement time.     |
| **Movement Speed +1**          | Speeds up horizontal movement, enabling faster traversal across the screen.   |

> [!TIP]
> Skill choices appear at regular score intervals (e.g., every 100 points). Choose wisely—your decisions stack and define your build.

![---](https://raw.githubusercontent.com/andreasbm/readme/master/assets/lines/rainbow.png)

## 🏷️ Icon Credits

Icons used in this project are sourced from [Flaticon](https://www.flaticon.com/) under the Free License:

- [Bolt](https://www.flaticon.com/free-icons/bolt), [Break](https://www.flaticon.com/free-icons/break), [Circuit board](https://www.flaticon.com/free-icons/circuit-board), [Engine](https://www.flaticon.com/free-icons/engine), [Gear](https://www.flaticon.com/free-icons/gear), [Microchip](https://www.flaticon.com/free-icons/microchip), [Modern](https://www.flaticon.com/free-icons/modern), [Optical fiber](https://www.flaticon.com/free-icons/optical-fiber), [Robot](https://www.flaticon.com/free-icons/robot), [Screw](https://www.flaticon.com/free-icons/screw), [Shafts](https://www.flaticon.com/free-icons/shafts), [Spring](https://www.flaticon.com/free-icons/spring), [Suspension](https://www.flaticon.com/free-icons/suspension), [System](https://www.flaticon.com/free-icons/system), [Water pump](https://www.flaticon.com/free-icons/water-pump) created by [Freepik](https://www.flaticon.com/authors/freepik)
- [Brakes](https://www.flaticon.com/free-icons/brakes) created by [Linector](https://www.flaticon.com/authors/linector)
- [Car parts](https://www.flaticon.com/free-icons/car-parts) created by [RaftelDesign](https://www.flaticon.com/authors/rafteldesign)
- [Radical](https://www.flaticon.com/free-icons/radical) created by [Three musketeers](https://www.flaticon.com/authors/three-musketeers)
