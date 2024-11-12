package handler

import (
	"encoding/json"
	"fmt"
	"net/http"
	"slices"
	"strconv"

	"github.com/lukelaurie/AR-Wizard-Game/go_server/internal/middleware"
)

var nextRoomNumber = 1
var rooms = map[int][]string{} //map room number to slices of usernames

func CreateRoom(w http.ResponseWriter, r *http.Request) {
	username, ok := middleware.GetUsernameFromContext(r.Context())
	if !ok {
		http.Error(w, "Username not found in context", http.StatusInternalServerError)
		return
	}
	rooms[nextRoomNumber] = []string{username}
	fmt.Println(username)
	fmt.Println(rooms)
	fmt.Println(rooms[nextRoomNumber])
	json.NewEncoder(w).Encode(nextRoomNumber)

	nextRoomNumber++
}

func JoinRoom(w http.ResponseWriter, r *http.Request) {
	username, ok := middleware.GetUsernameFromContext(r.Context())
	if !ok {
		http.Error(w, "Username not found in context", http.StatusInternalServerError)
		return
	}

	roomNumber, err := strconv.Atoi(r.URL.Query().Get("roomNumber"))
	if err != nil {
		http.Error(w, "Room number is not a valid integer", http.StatusBadRequest)
		return
	}

	curRoom, ok := rooms[roomNumber]
	if !ok {
		http.Error(w, "Room does not exist", http.StatusNotFound)
		return
	}

	if slices.Contains(curRoom, username) {
		http.Error(w, "You are already in this room", http.StatusConflict)
		return
	}

	rooms[roomNumber] = append(curRoom, username)
	json.NewEncoder(w).Encode("joined")
	fmt.Println(rooms[roomNumber])
}
