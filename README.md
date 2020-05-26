# RNGDungeon
RNGDungeon is a simple roguelike game made in Unity version 2019.2.17.f1. The object of the game is to survive through four randomly generated floors of a dungeon and defeat a boss that appears after descending the stairs on the fourth floor. Each new run is started from scratch.

## Releases
Version 1.0 is available under the releases page. It was built and tested only on Windows 10 with a 1080p monitor, so other platforms or resolutions may or may not work. I haven't tried.

## How to build yourself
If you want to build or modify the game yourself, you can clone this repo and open it in Unity. Newer versions of Unity may require updates that I haven't tested myself. To change the build settings or build for different platforms, go to File -> Build Settings in Unity. Note that I have only run this game on my personal computer, so I can't make any guarantees on its ability to look remotely reasonable on different resolutions or platforms.

# Game overview
This section describes the game with some screenshots to give an idea of how it looks and plays. Please don't laugh at my programmer "art".

## Controls
The character is controlled on the map using WASD, and movement is tile-based. The inventory screen, battle screen, and menus are controlled using the mouse. When on the map the 'I' button can be used to open the inventory screen, and can be pressed again to return to the map. The 'P' and 'Esc' buttons toggle the pause menu.

## The map
![Floor 1](Docs/Screenshots/completemap.png?raw=true)
On each floor, there will be a few points of interest on the map. Red question marks indicate items the player (the smiley face I drew the day I started this game) can pick up. The blue vampire is the "Collector", and defeating him is the only way for the player to obtain keys. The Collector appears on each floor and will be more powerful on later floors. Chests on the map will be opened when the player walks over one while holding at least one key, and have a greater chance for more powerful items than enemy item drops or normal map item drops. When the player steps on the stairs tile they will move to the next floor (or in the case of the fourth floor, immediately face the boss). As the player moves on the map, enemies have a random chance of appearing and starting a battle, with higher level enemies appearing on later floors.

## Battle
![Battle](Docs/Screenshots/battle.png?raw=true)
Battles take place with the player and enemy essentially rolling dice against one another, based on each actor's Roll, which is a range of possible roll values. For example, the player starts each game with a base Roll of [1-5]. When one actor rolls higher than the other, damage equal to the difference is dealt to the actor's opponent. These rolls take place in real time, with one roll occuring every couple seconds (the fast-forward button in the top right can speed up the pace of battles).

Roll values are displayed in the center, with resulting damage or healing displayed on the player or enemy. Battle effects like buffs, debuffs, or statuses (Break, Enraged, and Poisoned) are displayed above each actor's health bar. At the bottom of the screen, the player has access to any usable items available in their inventory.

## Techs
![Techs](Docs/Screenshots/battletech.png?raw=true)
During battle, the player may use techs, which are displayed at the top of the screen. By default the player will only use a simple attack, which is displayed at the center of tech selection. When the player selects a tech by clicking on it, it will be highlighted and used during the next roll. After a tech is used it goes on cooldown, which is indicated by a number in the center of the tech. Tech cooldowns count down by one after each roll, and a tech can't be used again until the cooldown reaches zero. In the above screenshot the player has just used the Topple tech and has the Crit tech selected for the next roll.

## Inventory
![Inventory](Docs/Screenshots/inventory.png?raw=true)
The player can toggle their inventory by pressing 'I' from the map screen (the inventory cannot be accessed during battle). The inventory screen displays the player's status, including health, Roll, abilities, techs, and items. Items displayed in the inventory column include a character indicating whether they are a (W)eapon, (T)rinket, or (U)sable item. Double-clicking an item in the inventory column equips or uses that item, and double-clicking an item in the equipped column unequips that item. Note that most usable items are usable only in battle, with the exception of healing items.

Only one weapon can be equipped at a time, but any number of trinkets can be equipped at a time. Multiples of the same trinket can be equipped to have their effects stack, which can frequently lead to some pretty powerful builds. Note that when new items are picked up from the map or from a chest, they are not automatically equipped - the player must equip them manually.

## Abilities
![AbilityAndTechSelection](Docs/Screenshots/abilityandtechselection.png?raw=true)
When starting a new game and before each subsequent floor, the player will have an opportunity to choose a new ability and a new tech. Hovering over the options will describe each one. The player is able to view their inventory using 'I' during ability and tech selection if necessary. Abilities are generally permanent effects that stay in place for the rest of the current run. Different abilities and techs can synergize with each other to create powerful combinations. Choosing abilities and techs wisely is the key to developing a strong build.

## Game over
The game is over after the player either loses all their health or defeats the boss at the end of the dungeon.

Hope you enjoy the game!
