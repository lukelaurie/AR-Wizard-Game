package model

type EndGameRequest struct {
	BossName  string `json:"bossName"`
	WinStatus bool   `json:"winStatus"`
	Level     int `json:"level"`
}
