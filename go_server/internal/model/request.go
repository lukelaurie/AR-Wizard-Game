package model

type RegisterRequest struct {
	Username string `json:"username"`
	Password string `json:"password"`
}

type SpellRequest struct {
	SpellName string `json:"spellName"`
}
