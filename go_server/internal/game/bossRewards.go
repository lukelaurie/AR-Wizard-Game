package game

import (
	"fmt"
	"math"
	"math/rand"
)

const scalingFactor = 1.5
var creatures = map[string][2]int{
	"hydra":    {1000, 100},
	"basilisk": {2000, 200},
}

func GetReward(dragonName string, level int) (int, error) {
	bossRewards, ok := creatures[dragonName]
	if !ok {
		return -1, fmt.Errorf("dragon name does not exist")
	}

	base := float64(bossRewards[0])
	bonus := float64(rand.Intn(bossRewards[1]))

	levelCalc := float64(level - 1)

	bastTotal := base * math.Pow(scalingFactor, levelCalc)
	bonusTotal := bonus * math.Pow(scalingFactor, levelCalc)

	return int(bastTotal + bonusTotal), nil
}