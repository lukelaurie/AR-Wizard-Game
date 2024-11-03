#!/bin/bash

# Define the PostgreSQL data directory
DATA_DIR="/var/lib/postgresql/16/main"

# Check if PostgreSQL is running
echo "Starting PostgreSQL..."
sudo systemctl start postgresql

echo "Starting Go Sever..."
go build ./cmd/app/main.go
./main