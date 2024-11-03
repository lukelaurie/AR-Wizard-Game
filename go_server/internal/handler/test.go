package handler

import (
	"encoding/json"
	"net/http"
)

func Test(writter http.ResponseWriter, res *http.Request) {
	json.NewEncoder(writter).Encode("testing23")
}
