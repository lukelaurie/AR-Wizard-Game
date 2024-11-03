package model

type Player struct {
	Username string `gorm:"primaryKey"`
	Password string `gorm:"not null"`
}
