package handler

import (
	"encoding/json"
	"fmt"
	"net/http"

	"github.com/lukelaurie/AR-Wizard-Game/go_server/internal/database"
	"github.com/lukelaurie/AR-Wizard-Game/go_server/internal/model"
	"github.com/lukelaurie/AR-Wizard-Game/go_server/internal/utils"
)

func RegisterUser(w http.ResponseWriter, r *http.Request) {
	var reqBody model.RegisterRequest

	// Decode the body of the request into the struct
	err := json.NewDecoder(r.Body).Decode(&reqBody)
	if err != nil {
		utils.LogAndAddServerError(fmt.Errorf("request decode error: %v", err), w)
		return
	}

	// get the encoded password so not stored in plaintext in the database
	password, err := utils.SaltAndHashPassword(reqBody.Password)
	if err != nil {
		utils.LogAndAddServerError(err, w)
		return
	}

	// execute the query in the database
	wasErr := database.RegisterUser(reqBody.Username, password, w)
	if wasErr {
		//TODO send back an actual response
		return
	}

	// // add new entry for preference tracker for the new user to manage next preference
	// err = database.AddNewUserPreferenceTracker(reqBody.Username)
	// if err != nil {
	// 	utils.LogAndAddServerError(err, w)
	// 	return
	// }

	//give the new user a lvl 1 fireball
	err = database.AddNewUserSpell(reqBody.Username, "fireball")
	if err != nil {
		utils.LogAndAddServerError(err, w)
		return
	}

	json.NewEncoder(w).Encode("user registered")
}
