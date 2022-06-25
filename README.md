# Example ECS Application
Goal behind this project was to learn how to work with ECS and to get some experience with DOTS. At that time, ECS was at version 0.17 so recently I've got time
to update it to actual version 0.51@preview-32. Back then it lacked documentation, writing code without it was diffucult, 
I never had good understanding of what I was doing etc. so I've left that project till better days.

From what I have seen recently, I may tell that documentation now is better, ECS has got new features and Unity team is definitely moving in the right direction.

## Features

Project has not too much of them, features:
* player input system
* ball movement system
* prefab spawning in job, using command buffer I think
* working example of OnTrigger event
* usage of tags
* FPS Cap to 60

## Productivity
I've created project with main goal - to test out, how many small objects colliding ECS can handle, and I was kind of disappointed 
since I've never got more >750 entities without serious framedrop on my PC, not even mentioning my phone. Probably I did something wrong, most likely that 
I haven't even thought about easiest optimization techniques but.. you know, some projects just wait till you get better mood to finish them.

## Used assets
* [Graphy](https://assetstore.unity.com/packages/tools/gui/graphy-ultimate-fps-counter-stats-monitor-debugger-105778) - great FPS counter, which helped me to test application productivity
