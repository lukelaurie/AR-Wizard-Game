package model

type Player struct {
	Username string `gorm:"primaryKey"`
	Password string `gorm:"not null"`
	Coins    int    `gorm:"default:0"`
}

type PlayerSpell struct {
	SpellName string `gorm:"not null"`
	Username  string `gorm:"not null"`
	Level     int    `gorm:"default:1"`
}
