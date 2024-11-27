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
var rooms = map[int][]string{}       //map room number to slices of usernames
var userRooms = map[string]int{}     //map each player to the room they are currently in
var activeRooms = make(map[int]bool) // a set to keep track of the current active rooms

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

	deleteRoom(roomNumber, curRoom)

	if !reqBody.WinStatus {
		json.NewEncoder(w).Encode("game ended")
		return
	}

	rewardMap := map[string]int{}

	for _, player := range curRoom {
		delete(userRooms, player)
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

		rewardMap[player] = playerCoins
	}

	
	json.NewEncoder(w).Encode(rewardMap)
}

func deleteRoom(roomNumber int, curRoom []string) {
	for _, player := range curRoom {
		delete(userRooms, player)
	} 

	delete(rooms, roomNumber)
	delete(activeRooms, roomNumber)
}

func StartGame(w http.ResponseWriter, r *http.Request) {
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

	_, ok = activeRooms[roomNumber]
	if !ok {
		activeRooms[roomNumber] = true
		json.NewEncoder(w).Encode("game has started")
		return
	}

	json.NewEncoder(w).Encode("game already started")
}

func IsGameStarted(w http.ResponseWriter, r *http.Request) {
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

	_, ok = activeRooms[roomNumber]
	if !ok {
		http.Error(w, "game not started", http.StatusNotFound)
		return
	}

	json.NewEncoder(w).Encode("game has started")
}

func LeaveGame(w http.ResponseWriter, r *http.Request) {
	username, ok := middleware.GetUsernameFromContext(r.Context())
	if !ok {
		http.Error(w, "Username not found in context", http.StatusInternalServerError)
		return
	}

	roomNumber, ok := userRooms[username]
	if !ok {
		http.Error(w, "The player is not yet a part of a room", http.StatusNotFound)
		return
	}

	// check if user already has left the room 
	if !isUserInGame(rooms[roomNumber], username) {
		http.Error(w, "user does not exist in game", http.StatusNotFound)
		return
	}

	rooms[roomNumber] = removeUser(rooms[roomNumber], username)

	delete(userRooms, username)
	json.NewEncoder(w).Encode("user has left game")
}

func isUserInGame(users []string, checkUser string) bool {
	for _, user := range users {
		if user == checkUser {
			return true
		}
	}
	return false
}

func removeUser(users []string, deleteUser string) []string {
	retval := make([]string, len(users)-1)
	
	i := 0
	for _, user := range users {
		if user == deleteUser {
			continue
		}
		retval[i] = user 
		i++
	}

	return retval
}