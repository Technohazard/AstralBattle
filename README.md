# AstralBattle - War in the Heavens

Author: Justin Smith  
email: technohazard@gmail.com  
twitter/x: [https://twitter.com/odd_dimensions](https://twitter.com/odd_dimensions)  
linkedIn: [https://www.linkedin.com/in/justinsmithgamedesign/](https://www.linkedin.com/in/justinsmithgamedesign/)  

Summary:
This was originally a code test for a swell startup.  
I need to put some code samples on GitHub, this is a good start.

Goals:
Improve this, make it example-terrific.

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

Second Commit:
- Added levels to monsters for some variety  
- Fixed bug where if no agents were alive to take an action the game would never quit.

Future Commits:
Not sure what sort of design direction to take with this, but for now I'll just improve things until it gets fun, or I get feedback.
