namespace OOP
{
    class Program
    {


        class GameAccount
        {
            public string UserName;
            private int CurrentRating = 1;
            public int getCurrentRating() { return CurrentRating; }
            public string getUserName() { return UserName; }
            uint GamesCount;
            List<Game> Played = new List<Game>();

            public GameAccount(string UserName)
            {
                this.UserName = UserName;
            }

            public void WinGame(GameAccount EnemyName, int Rating)
            {
                Game game = new Game(this, EnemyName, Rating);
                Played.Add(game);
                EnemyName.LoseGame(this, Rating, game);
                CurrentRating += Rating;
                GamesCount++;
            }

            public void LoseGame(GameAccount EnemyName, int Rating, Game game)
            {
                if (Rating > CurrentRating) CurrentRating = 1;
                else CurrentRating -= Rating;

                Played.Add(game);
                GamesCount++;
            }

            public void GetStats()
            {
                Console.WriteLine("Гравець:" + UserName);
                Console.WriteLine("Поточний рейтинг: " + CurrentRating);
                Console.WriteLine("Зіграно партій: " + GamesCount);
                Console.WriteLine("Історія партій:");
                foreach (Game Game in Played)
                {
                    Console.Write("|Гра №" + (Game.GameID + 1));
                    Console.Write("  \tПеремога:");
                    Console.Write(Game.Winns.getUserName());
                    Console.Write("  \tПоразка:");
                    Console.Write(Game.Loses.getUserName());
                    Console.WriteLine("     \tРейтинг - " + Game.Rating + "|");
                }
                Console.WriteLine();
            }
        }
        class Game
        {
            public GameAccount Winns;
            public GameAccount Loses;
            public int Rating;
            //static uint GlobID;
            public uint GameID;

            public Game(GameAccount firstPlayer, GameAccount secondPlayer, int Rating)
            {
                Winns = firstPlayer;
                Loses = secondPlayer;
                this.Rating = Rating;
                GameID++;
            }

        }
        static void Main(string[] args)
        {
            GameAccount Logan = new GameAccount("Логан");
            GameAccount Deadpool = new GameAccount("Дедпул");
            Random rnd = new Random();
            for (int i = 0; i < 15; i++)
            {
                if (rnd.Next(0, 11) < 6)
                    Logan.WinGame(Deadpool, rnd.Next(25, 35));
                else
                    Deadpool.WinGame(Logan, rnd.Next(25, 35));
            }
            Logan.GetStats();
            Deadpool.GetStats();
        }
    }
}