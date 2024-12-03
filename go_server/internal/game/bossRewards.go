package game

import (
	"math"
	"math/rand"
)

const scalingFactor = 1.8

func GetReward(level int) (int, error) {
	var base float64 = 1000
	var bonus float64 = float64(rand.Intn(150))

	levelCalc := float64(level)

	bastTotal := base * math.Pow(scalingFactor, levelCalc)
	bonusTotal := bonus * math.Pow(scalingFactor, levelCalc)

	return int(bastTotal + bonusTotal), nil
}