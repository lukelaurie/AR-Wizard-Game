package handler

import (
	"encoding/json"
	"net/http"

	"github.com/lukelaurie/AR-Wizard-Game/go_server/internal/database"
	"github.com/lukelaurie/AR-Wizard-Game/go_server/internal/middleware"
	"github.com/lukelaurie/AR-Wizard-Game/go_server/internal/utils"
)

func GetCoins(w http.ResponseWriter, r *http.Request) {
	username, ok := middleware.GetUsernameFromContext(r.Context())
	if !ok {
		http.Error(w, "Username not found in context", http.StatusInternalServerError)
		return
	}

	userCoins, err := database.RetrieveUserCoins(username)
	if err != nil {
		utils.LogAndAddServerError(err, w)
		return
	}

	json.NewEncoder(w).Encode(userCoins)
}
