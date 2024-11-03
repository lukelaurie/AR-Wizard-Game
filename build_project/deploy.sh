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
sudo pkill -f main
echo "Starting PostgreSQL..."
sudo systemctl start postgresql

# Wait for PostgreSQL to be fully ready before proceeding
echo "Checking PostgreSQL status..."

if pg_isready -h 127.0.0.1 -p 5432 -U postgres; then
    echo "PostgreSQL is ready!"
    break
else
  echo "Waiting for PostgreSQL to start..."
  sleep 2
fi

echo "Starting Go Sever..."
go build ./cmd/app/main.go
nohup ./main > /dev/null 2>&1 &

disown
# Exit the script
exit 0