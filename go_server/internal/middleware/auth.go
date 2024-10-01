package middleware

import (
	"context"
	"fmt"
	"net/http"
	"os"

	"github.com/golang-jwt/jwt/v5"
)

// create the key to be able to access username from the context 
type contextKey string
const userContextKey = contextKey("username")

func CheckAuthMiddleware(next http.Handler) http.Handler {
	return http.HandlerFunc(func(w http.ResponseWriter, r *http.Request) {
		// extract the JWT from the cookie
		cookie, err := r.Cookie("token")
		if err != nil {
			http.Error(w, "Unauthorized: Missing or invalid token", http.StatusUnauthorized)
			return
		}
		tokenString := cookie.Value

		// parse out the JWT and verify decryption was valid
		jwtSecretKey := os.Getenv("JWT_SECRET_KEY")
		token, err := jwt.Parse(tokenString, func(token *jwt.Token) (interface{}, error) {
			// set the method that will be used to decrypt the token
			_, ok := token.Method.(*jwt.SigningMethodHMAC)
			if !ok {
				return nil, fmt.Errorf("unexpected signing method: %v", token.Header["alg"])
			}
			// use the bytes from the secret key in the decryption
			return []byte(jwtSecretKey), nil
		})

		// check that the decryption was valid
		if err != nil || !token.Valid {
			http.Error(w, "Unauthorized: Invalid token", http.StatusUnauthorized)
			return
		}

		// extract the username from the parsed out jwt
		claims, ok := token.Claims.(jwt.MapClaims)  // sets claims if jwt.MapClaims exists within it
		if !ok {
			http.Error(w, "Unauthorized: Invalid claims", http.StatusUnauthorized)
			return
		}

		username := claims["username"].(string)
		ctx := context.WithValue(r.Context(), userContextKey, username) // create a new instance of context with added username
		// apply the context to the request body
		next.ServeHTTP(w, r.WithContext(ctx))
	})
}

func GetUsernameFromContext(ctx context.Context) (string, bool) {
	username, ok := ctx.Value(userContextKey).(string)
	return username, ok
}