Project Wisp - Combat Prototype Ver. 0.1.5
Developed by Nathan Warren-Acord

Controls --

WASD - Movement
Left click - Melee (Main) attack
Shift + Left click - Psionic Shard (Projectile)
Left-Ctrl + Left click - Poison bottle (Thrown)
Escape - Quits to main menu from level and quits game from main menu

Gameplay Notes --
An indicator appears over enemies when they "spot" the player and engage in combat.

When in combat, actors and objects only move when the player does. Outside of combat, actors and objects return
to real-time movement.

Projectiles and Thrown attacks are directional (performed in direction of mouse cursor).

Projectiles can be fired continuously, but will damage player if used more than once ever 1.5 seconds.
(This serves as a soft cooldown, allowing the player to sacrifice health for damage, if needed).

Poison bottles travel 4 tiles or until they hit a wall / enemy. The impact does damage and a 3x3 square is
created at impact that will do 1 dmg per tick to enemies standing on it when the tick occurs (lasts 3 seconds).

Poison bottles have a 4 second cooldown.

Enemies drop health. Plain health (heals 1 hp) and Glowing health (heals 4 hp).

Credits --

Developed by Nathan Warren-Acord
Art assets by Nathan Warren-Acord
A* Pathfinding Project by Aron Granburg
Music track "Flaming Soul" by Marcelo Fernandez

Additional sound clips:
"Door, Wooden, Close, A (H1).wav" by InspectorJ (www.jshaw.co.uk) of Freesound.org CC-Attribution (modified)
"glass shatter.wav" by datasoundsample, Freesound.org CC-Attribution (modified)
"Grunts.wav" by bennychico11, Freesound.org CC-Attribution (sampled and modified)
"night_in_the_forest.wav" by cormi, Freesound.org CC-Attribution Noncommercial
"weirdbreath.wav" by Wolfsinger, Freesound.org CC-Attribution (modified)
Player hit souund performed by Jessica Warren-Acord

Developed in Unity Ver. 2019.3.0f6
Art developed in Aseprite
Audio modified in Audacity
Code editor used - Visual Studio Code
Version control with GitHub