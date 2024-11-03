#!/bin/bash

BRANCH_NAME="$1"
git checkout "$BRANCH_NAME"
git pull


# Start up the unity server
cd unity/game_build/
chmod +x game.x86_64

# kill old server and start the new one 
sudo pkill -f game.x86_64
nohup ./game.x86_64 -batchmode -nographics > /dev/null 2>&1 &


# start up the go server
cd ../../go_server

# Check if PostgreSQL is running
echo "Starting PostgreSQL..."
sudo systemctl start postgresql

echo "Starting Go Sever..."
sudo pkill -f main
go build ./cmd/app/main.go
nohup ./main

disown
# Exit the script
exit 0