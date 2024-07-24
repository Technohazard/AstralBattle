# AstrobloxBattle

Welcome to the AstrobloxBattle Code Test:

Author: Justin Smith, technohazard@gmail.com

Setup:
1. Load the Main game scene
2. Edit the starting stats on the GameController object.
3. Hit Play to begin battle.

Play:
This Battle simulator runs automatically when you press Play in the Unity Editor.
There is no quit button.
The battle will end when one side or the other runs out of HP.
Agents with Red background info are dead and can’t act.
All Agents spawn within their minimum to maximum level range.
Minimum level is 0, maximum level is unlimited, but enemies only get unique names until level 100.
You can click and drag the scroll windows to view your party member info - theoretically unlimited size.

Development process
1. Read thoroughly the take-home project description
2. Break it down into key design documents.
    1. A master TODO list of meta-tasks, like setting up the git repo, etc.  A copy of the project requrements with check boxes underneath each point and some reordering.
    2. Development Master, for code and architecture notes A list of Systems needed to produce the minimum viable runtime, checkboxes.
    3. Design Master for gameplay and UX elements Gameplay states, UI display choices
3. Code, test, and iterate features on the list according to the document.
4. Once I get to minimum viable gameplay loop, commit and push
5. Finish the additional behaviors or bugs with incremental commits. 
Design choices 
Use Unity UI for 100% of the game display. 
2D only!
Feature Completion:
Everything should be in the requirements except additional behaviors:
DOT, HEAL, HOT, Buff, Debuff
However, the system supports these actions fully, which should go in the next(?) updates.

Time spent on each section
Sunday: 1-2h reading design docs and planning, 1h code.
Monday: 2h code initial systems and project setup/UI
Weds: 2h Finish coding Damage system and core game loop
+1h bug fix and polish
- Added levels to monsters for some variety
- Fixed bug where if no agents were alive to take an action the game would never quit.
