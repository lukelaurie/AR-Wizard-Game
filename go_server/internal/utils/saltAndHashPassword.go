package utils

import (
	"fmt"

	"golang.org/x/crypto/bcrypt"
)

func SaltAndHashPassword(password string) (string, error) {
	// generate the salt and hashed password using bcrypt library
	saltHashedPassword, err := bcrypt.GenerateFromPassword([]byte(password), bcrypt.DefaultCost)
	if err != nil {
		return "", fmt.Errorf("error encrypting the password: %v", err)
	}
	// Convert the byte slice to a string and return it
	return string(saltHashedPassword), nil
}

func CheckPasswordhash(password string, hashedPassword string) bool {
	err := bcrypt.CompareHashAndPassword([]byte(hashedPassword), []byte(password))
	return err == nil
}
