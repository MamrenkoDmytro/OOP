namespace lab2
{
    public static class GameFactory
    {
        public static Game CommonGameCreation()
        {
            return new CommonGame();
        }

        public static Game TrainingGameCreation()
        {
            return new TrainingGame();
        }

        public static Game OneRaitGameCreation()
        {
            return new OneRaitGame();
        }

        public static Game GenerateRandomGame(int randomGame)
        {
            switch (randomGame)
            {
                case 1:
                    return CommonGameCreation();
                case 2:
                    return TrainingGameCreation();
                default:
                    return OneRaitGameCreation();
            }
        }
    }
}