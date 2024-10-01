package model

type User struct {
	Username string `gorm:"primaryKey"`
	Password string `gorm:"not null"`
}
