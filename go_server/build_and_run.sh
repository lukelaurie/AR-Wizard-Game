#!/bin/bash

# Define the PostgreSQL data directory
DATA_DIR="/Library/PostgreSQL/16/data"

# Check if PostgreSQL is running
# echo "Starting PostgreSQL..."
pg_ctl -D "$DATA_DIR" start > /dev/null 2>&1

go build ./cmd/app/main.go
./main
