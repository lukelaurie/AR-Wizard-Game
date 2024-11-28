package handler

import (
	"encoding/json"
	"net/http"
	"os"
	"time"

	"github.com/golang-jwt/jwt/v5"
	"github.com/lukelaurie/AR-Wizard-Game/go_server/internal/database"
	"github.com/lukelaurie/AR-Wizard-Game/go_server/internal/utils"
)

func LoginUser(w http.ResponseWriter, r *http.Request) {
	username := r.URL.Query().Get("username")
	password := r.URL.Query().Get("password")
	// verify the query parameters where passed
	if username == "" || password == "" {
		http.Error(w, "Username and password are required", http.StatusBadRequest)
		return
	}

	// search for the user in the database
	user, err := database.RetrieveUser(username)
	if err != nil {
		http.Error(w, "invalid username", http.StatusConflict)
		return
	}

	// check that the user passed in the correct password
	if !utils.CheckPasswordhash(password, user.Password) {
		http.Error(w, "password invalid", http.StatusConflict)
		return
	}

	// create the JWT token
	jwtToken := jwt.NewWithClaims(jwt.SigningMethodHS256, jwt.MapClaims{
		"username": user.Username,
		"exp":      time.Now().Add(time.Hour * 72).Unix(), // set how long token lasts
	})

	// sign the token with a secret key
	jwtSecretKey := os.Getenv("JWT_SECRET_KEY")

	// encodes both the username and exp into tokenString
	tokenString, err := jwtToken.SignedString([]byte(jwtSecretKey))
	if err != nil {
		http.Error(w, "unable to generate the jwt token", http.StatusInternalServerError)
		return
	}

	// place the token in a cookie in the request
	http.SetCookie(w, &http.Cookie{
		Name:     "token",
		Value:    tokenString,
		Expires:  time.Now().Add(time.Hour * 72),
		HttpOnly: true,
		Secure:   false,
		Path:     "/",
	})

	json.NewEncoder(w).Encode("login successful")
}
