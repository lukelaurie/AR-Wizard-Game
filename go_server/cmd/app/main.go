package main

import (
	"log"
	"net/http"

	"github.com/joho/godotenv"
	"github.com/lukelaurie/AR-Wizard-Game/go_server/internal/database"
	route "github.com/lukelaurie/AR-Wizard-Game/go_server/internal/routes"
)

func init() {
	// load in the .env file
	err := godotenv.Load("./cmd/app/.env")
	if err != nil {
		log.Fatalf("Error loading the .env file: %v", err)
	}
}

func main() {
	// initialize the database connection
	database.InitDb()
	defer database.SqlDB.Close()

	// set up the router
	router := route.InitializeRoutes()
	log.Println("Server started on port 3030")
	log.Fatal(http.ListenAndServe(":3030", router))
}
