#!/bin/bash

git pull
cd AR-Wizard-Game/unity/game_build/

chmod +x game.x86_64

# kill old server and start the new one 
pkill -f game.x86_64
nohup ./game.x86_64 -batchmode -nographics &