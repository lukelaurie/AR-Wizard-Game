#!/bin/bash

# Define the PostgreSQL data directory
# DATA_DIR="/Library/PostgreSQL/16/data"
# DATA_DIR="C:/Program Files/PostgreSQL/16/data"
DATA_DIR="/var/lib/postgresql/16/main"

# Check if PostgreSQL is running
echo "Starting PostgreSQL..."
# pg_ctl -D "$DATA_DIR" start > /dev/null 2>&1
sudo systemctl start postgresql

echo "Starting Go Sever..."
go build ./cmd/app/main.go
./main.exe