package handler

import (
	"encoding/json"
	"fmt"
	"net/http"
	"slices"
	"strconv"

	"github.com/lukelaurie/AR-Wizard-Game/go_server/internal/database"
	"github.com/lukelaurie/AR-Wizard-Game/go_server/internal/game"
	"github.com/lukelaurie/AR-Wizard-Game/go_server/internal/middleware"
	"github.com/lukelaurie/AR-Wizard-Game/go_server/internal/model"
	"github.com/lukelaurie/AR-Wizard-Game/go_server/internal/utils"
)

var nextRoomNumber = 1000
var rooms = map[int][]string{} //map room number to slices of usernames
var userRooms = map[string]int{} //map each player to the room they are currently in

func CreateRoom(w http.ResponseWriter, r *http.Request) {
	username, ok := middleware.GetUsernameFromContext(r.Context())
	if !ok {
		http.Error(w, "Username not found in context", http.StatusInternalServerError)
		return
	}
	rooms[nextRoomNumber] = []string{username}
	userRooms[username] = nextRoomNumber

	json.NewEncoder(w).Encode(nextRoomNumber)

	nextRoomNumber++
}

func GetUsersInRoom(w http.ResponseWriter, r *http.Request) {
	username, ok := middleware.GetUsernameFromContext(r.Context())
	if !ok {
		http.Error(w, "Username not found in context", http.StatusInternalServerError)
		return
	}

	roomNumber, ok := userRooms[username]
	if !ok {
		http.Error(w, "User not in a room", http.StatusNotFound)
		return
	}


	curRoom, ok := rooms[roomNumber]
	if !ok {
		http.Error(w, "Room does not exist", http.StatusNotFound)
		return
	}

	json.NewEncoder(w).Encode(curRoom)
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
	userRooms[username] = roomNumber

	json.NewEncoder(w).Encode("joined")
	fmt.Println(rooms[roomNumber])
}

func EndGame(w http.ResponseWriter, r *http.Request) {
	username, ok := middleware.GetUsernameFromContext(r.Context())
	if !ok {
		http.Error(w, "Username not found in context", http.StatusInternalServerError)
		return
	}

	var reqBody model.EndGameRequest
	err := json.NewDecoder(r.Body).Decode(&reqBody)
	if err != nil {
		utils.LogAndAddServerError(fmt.Errorf("request decode error: %v", err), w)
		return
	}

	// determine the room number the host is a part of 
	roomNumber, ok := userRooms[username]
	if !ok {
		http.Error(w, "The player is not yet a part of a room", http.StatusNotFound)
		return
	}

	curRoom, ok := rooms[roomNumber]
	if !ok {
		http.Error(w, "Room does not exist", http.StatusNotFound)
		return
	}

	if !reqBody.WinStatus {
		return
	}

	for _, player := range curRoom {
		// get the reward of the player for defeating the boss
		playerCoins, err := game.GetReward(reqBody.BossName, reqBody.Level)
		if err != nil {
			utils.LogAndAddServerError(err, w)
			return
		}

		err = database.UpdateUserCoins(player, playerCoins)
		if err != nil {
			utils.LogAndAddServerError(err, w)
			return
		}
	}

	json.NewEncoder(w).Encode("game has ended")
}
