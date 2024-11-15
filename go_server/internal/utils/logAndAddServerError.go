package utils

import (
	"log"
	"net/http"
)

func LogAndAddServerError(err error, w http.ResponseWriter) {
	log.Print(err.Error())
	// http.Error(w, "Internal Server Error", http.StatusInternalServerError)
	http.Error(w, err.Error(), http.StatusInternalServerError)
}