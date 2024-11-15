package database

import (
	"net/http"
	"strings"

	"github.com/lukelaurie/AR-Wizard-Game/go_server/internal/model"
)

func RetrieveUser(username string) (model.Player, error) {
	var user model.Player

	err := DB.First(&user, "username = ?", username).Error
	if err != nil {
		return user, err
	}
	return user, nil
}

func RegisterUser(username string, password string, w http.ResponseWriter) bool {
	newUser := model.Player{
		Username: username,
		Password: password,
	}

	// insert the user into the database
	err := DB.Create(&newUser).Error
	if err == nil {
		return false
	}

	if isUniqueViolation(err, "users_pkey") {
		http.Error(w, "username already exists", http.StatusConflict)
	} else {
		http.Error(nil, "database insert error", http.StatusInternalServerError)
	}
	return true
}

func UpdateUserCoins(username string, coins int) error {
	var player model.Player
	err := DB.First(&player, "username = ?", username).Error
	if err != nil {
		return err
	} 

	player.Coins += coins

	// update the player to have the new coins
	err = DB.Save(&player).Error
	if err != nil {
		return err
	}

	return nil
}

func UpgradeUserSpell(username string, spellName string) error {
	var playerSpell model.PlayerSpell
	err := DB.Where("username = ? AND spell_name = ?", username, spellName).First(&playerSpell).Error
	if err != nil {
		return err
	} 

	// update the player to have the new level
	err = DB.Model(&playerSpell).
	Where("username = ? AND spell_name = ?", username, spellName).
	Update("level", playerSpell.Level + 1).Error
	
	return err
}

func RetrieveUserCoins(username string) (int, error) {
	var player model.Player
	err := DB.First(&player, "username = ?", username).Error
	if err != nil {
		return -1, err
	} 

	return player.Coins, nil
}

func RetrieveUserSpells(username string) ([]model.PlayerSpell, error) {
	var playerSpells []model.PlayerSpell
	err := DB.Find(&playerSpells, "username = ?", username).Error
	if err != nil {
		return playerSpells, err
	} 

	return playerSpells, nil
}

func AddNewUserSpell(username string, spellName string) error {
	newSpell := model.PlayerSpell{
		SpellName: spellName,
		Username: username,
		Level: 1,
	}

	// insert the spell into the database 
	return DB.Create(&newSpell).Error
}

func isUniqueViolation(err error, constraint string) bool {
	return strings.Contains(err.Error(), "unique constraint") &&
		strings.Contains(err.Error(), constraint)
}
