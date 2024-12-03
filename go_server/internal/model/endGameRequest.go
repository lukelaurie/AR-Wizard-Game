package model

type EndGameRequest struct {
	WinStatus bool   `json:"winStatus"`
	Level     int `json:"level"`
}
