#!/bin/bash

BRANCH_NAME = "$1"
cd AR-Wizard-Game/unity/game_build/

git checkout $BRANCH_NAME
git pull

chmod +x game.x86_64

# kill old server and start the new one 
pkill -f game.x86_64
nohup ./game.x86_64 -batchmode -nographics &