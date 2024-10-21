#!/bin/bash

BRANCH_NAME="$1"
cd AR-Wizard-Game/
git checkout "$BRANCH_NAME"
git pull

cd unity/game_build/

chmod +x game.x86_64

# kill old server and start the new one 
pkill -f game.x86_64
nohup ./game.x86_64 -batchmode -nographics &