This demo currently requires Unity Pro due to the Oculus Rift features. Unity offers a 1 month free trial of Unity Pro. We are currently working implementing a Non-Oculus Mode that is compatible with Unity Free.

The 'PlayerManager' gameobject is will generate a list of players and set one as the active player. 

'Space' moves the player forward
'Left Shit' activates jump (if it has been implemented)
'z' brings up DebugGui (currently, it only displays the name of the player controller)
'x' cycles through to the next player controller




The one that my team is the most fond of is the one we call 'No Friction With Drift'. It is a very awkward name. We do need a much better name for it. 

My thinking is that the best mechanics for motions are the ones based on real world physics. For example the motion of a rolling ball, or the ice on a low friction surface. Here are some of the mechanics we implemented.

Ball - The physics of a rolling ball. There is a really nice smooth motion but the player has less control.

Basic - Movement based on Character Controller which runs at a constant velocity. 

Floating Sphere - Same Ball physics but with gravity turned off. You can float in the direction of whereever you are looking. Be careful not to fly off to far.

No Friction - Physics based on ice physics or frictionless physics. 

No Friction With Drift - A more enhanced version of the previous player controller. This most closely matched what the team collectively thought the mechanic might be