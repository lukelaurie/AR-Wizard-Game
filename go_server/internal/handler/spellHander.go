package handler

import (
	"encoding/json"
	"fmt"
	"net/http"

	"github.com/lukelaurie/AR-Wizard-Game/go_server/internal/database"
	"github.com/lukelaurie/AR-Wizard-Game/go_server/internal/middleware"
	"github.com/lukelaurie/AR-Wizard-Game/go_server/internal/model"
	"github.com/lukelaurie/AR-Wizard-Game/go_server/internal/utils"
)

var spellPrices = map[string]int{
	"fireball":  1000,
	"lightning": 2000,
	"destroy":   10000,
}

var levelPrices = map[int]int{
	1: 1000,
	2: 1500,
	3: 3000,
	4: 5000,
	5: 9000,
	6: 15000,
}

func PurchaseSpell(w http.ResponseWriter, r *http.Request) {
	username, ok := middleware.GetUsernameFromContext(r.Context())
	if !ok {
		http.Error(w, "Username not found in context", http.StatusInternalServerError)
		return
	}

	var reqBody model.SpellRequest
	err := json.NewDecoder(r.Body).Decode(&reqBody)
	if err != nil {
		utils.LogAndAddServerError(fmt.Errorf("request decode error: %v", err), w)
		return
	}

	userCoins, playerSpells, err := getPlayerSpellData(username)
	if err != nil {
		utils.LogAndAddServerError(err, w)
		return
	}

	// check if the spell is valid
	spellPrice, err := validateUserSpellPurchase(reqBody, playerSpells, userCoins)
	if err != nil {
		utils.LogAndAddServerError(err, w)
		return
	}

	// add the spell for the user to the database
	err = database.AddNewUserSpell(username, reqBody.SpellName)
	if err != nil {
		utils.LogAndAddServerError(err, w)
		return
	}

	// update the user coins to have purchased the spell
	err = database.UpdateUserCoins(username, -spellPrice)
	if err != nil {
		utils.LogAndAddServerError(err, w)
		return
	}

	json.NewEncoder(w).Encode("user has purchased the spell")
}

func UpgradeSpell(w http.ResponseWriter, r *http.Request) {
	username, ok := middleware.GetUsernameFromContext(r.Context())
	if !ok {
		http.Error(w, "Username not found in context", http.StatusInternalServerError)
		return
	}

	var reqBody model.SpellRequest
	err := json.NewDecoder(r.Body).Decode(&reqBody)
	if err != nil {
		utils.LogAndAddServerError(fmt.Errorf("request decode error: %v", err), w)
		return
	}

	userCoins, playerSpells, err := getPlayerSpellData(username)
	if err != nil {
		utils.LogAndAddServerError(err, w)
		return
	}

	// check if the spell is valid
	upgradePrice, err := validateUserSpellUpdate(reqBody, playerSpells, userCoins)
	if err != nil {
		utils.LogAndAddServerError(err, w)
		return
	}

	// upgrade the spell in the database
	err = database.UpgradeUserSpell(username, reqBody.SpellName)
	if err != nil {
		utils.LogAndAddServerError(err, w)
		return
	}

	// update the user coins to have purchased the spell
	err = database.UpdateUserCoins(username, -upgradePrice)
	if err != nil {
		utils.LogAndAddServerError(err, w)
		return
	}

	json.NewEncoder(w).Encode("user has upgraded the spell")
}

func GetSpells(w http.ResponseWriter, r *http.Request) {
	username, ok := middleware.GetUsernameFromContext(r.Context())
	if !ok {
		http.Error(w, "Username not found in context", http.StatusInternalServerError)
		return
	}

	playerSpells, err := database.RetrieveUserSpells(username)
	if err != nil {
		utils.LogAndAddServerError(err, w)
		return
	}

	json.NewEncoder(w).Encode(playerSpells)
}

func validateUserSpellPurchase(reqBody model.SpellRequest, playerSpells []model.PlayerSpell, userCoins int) (int, error) {
	// check if the spell is valid
	spellPrice, ok := spellPrices[reqBody.SpellName]
	if !ok {
		return -1, fmt.Errorf("the spell is invalid")
	}

	if doesPlayerOwnSpell(playerSpells, reqBody.SpellName) {
		return -1, fmt.Errorf("user already owns the spell")
	}

	if userCoins < spellPrice {
		return -1, fmt.Errorf("user does not have enough coins for this spell")
	}

	return spellPrice, nil
}

func validateUserSpellUpdate(reqBody model.SpellRequest, playerSpells []model.PlayerSpell, userCoins int) (int, error) {
	if !doesPlayerOwnSpell(playerSpells, reqBody.SpellName) {
		return -1, fmt.Errorf("user does not own the spell")
	}

	upgradePrice, ok := levelPrices[playerSpells[0].Level + 1]
	if !ok {
		return -1, fmt.Errorf("the spell is already max level")
	}

	if userCoins < upgradePrice {
		return -1, fmt.Errorf("user does not have enough coins for this spell")
	}

	return upgradePrice, nil
}

func doesPlayerOwnSpell(playerSpells []model.PlayerSpell, purchaseSpellName string) bool {
	// check if the player already owns the spell
	for _, playerSpell := range playerSpells {
		if playerSpell.SpellName == purchaseSpellName {
			return true
		}
	}

	return false
}

func getPlayerSpellData(username string) (int, []model.PlayerSpell, error) {
	playerSpells, err := database.RetrieveUserSpells(username)
	if err != nil {
		return -1, playerSpells, fmt.Errorf(err.Error())
	}

	userCoins, err := database.RetrieveUserCoins(username)
	if err != nil {
		return -1, playerSpells, fmt.Errorf(err.Error())
	}

	return userCoins, playerSpells, nil
}