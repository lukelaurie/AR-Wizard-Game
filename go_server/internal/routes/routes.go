package route

import (
	"github.com/gorilla/mux"
	"github.com/lukelaurie/AR-Wizard-Game/go_server/internal/handler"
	"github.com/lukelaurie/AR-Wizard-Game/go_server/internal/middleware"
)

func InitializeRoutes() *mux.Router {
	router := mux.NewRouter()

	// create a subrouter for all routes to start with /api
	apiRouter := router.PathPrefix("/api").Subrouter()

	// public routes with no middleware
	apiRouter.HandleFunc("/register-user", handler.RegisterUser).Methods("POST")
	apiRouter.HandleFunc("/login", handler.LoginUser).Methods("GET")

	// private routes that require middleware
	protectedRouter := apiRouter.PathPrefix("/protected").Subrouter()
	protectedRouter.Use(middleware.CheckAuthMiddleware) // apply the middleware to first authorize

	protectedRouter.HandleFunc("/test", handler.Test).Methods("GET")
	protectedRouter.HandleFunc("/createRoom", handler.CreateRoom).Methods("GET")
	protectedRouter.HandleFunc("/joinRoom", handler.JoinRoom).Methods("GET")

	return router
}
