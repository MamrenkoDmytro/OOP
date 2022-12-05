using System;
using System.Collections.Generic;


namespace lab2
{
    public abstract class GameAccount
    {
        private readonly string _userName;
        protected string AccountType;
        protected uint CurrentRaiting = 1;
        private readonly List<Game> _gamesHistory = new List<Game>();
        private uint _gameCount;
        protected static int ComboCounter;
        protected GameAccount(string userName) => _userName = userName;

        public virtual void WinGame(GameAccount opponentName, Game match)
        {
            Game game = null;
            switch (match.GameType)
            {
                case "Common Game":
                    game = GameFactory.CommonGameCreation();
                    break;
                case "Training Game":
                    game = GameFactory.TrainingGameCreation();
                    break;
                case "One Rait Game":
                    game = GameFactory.OneRaitGameCreation();
                    break;
                default:
                    Console.WriteLine("Такого типу гри нема.");
                    break;
            }

            if (game == null) return;
            game.OpponentName = opponentName;
            game.PlayerStatus = "Win";
            game.GameId = _gameCount;
            game.Raiting = match.Raiting;
            _gamesHistory.Add(game);
            _gameCount++;
        }

        public virtual void LoseGame(GameAccount opponentName, Game match)
        {
            Game game = null;
            switch (match.GameType)
            {
                case "Common Game":
                    game = GameFactory.CommonGameCreation();
                    break;
                case "Training Game":
                    game = GameFactory.TrainingGameCreation();
                    break;
                case "One Rait Game":
                    game = GameFactory.OneRaitGameCreation();
                    game.OpponentName = opponentName;
                    break;
                default:
                    Console.WriteLine("Такого типу гри нема.");
                    break;
            }

            if (game == null) return;
            game.OpponentName = opponentName;
            game.PlayerStatus = "Lose";
            game.GameId = _gameCount;
            _gamesHistory.Add(game);
            _gameCount++;
        }

        public void GetStats()
        {
            Console.WriteLine("Гравець: " + _userName);
            Console.WriteLine("Тип акаунту: " + AccountType);
            Console.WriteLine("Кiлькiсть зiграних iгор: " + _gameCount);
            Console.WriteLine("Рейтинг: " + CurrentRaiting);
            Console.WriteLine("Iсторiя iгор:");
            Console.WriteLine("|№ Гри \t|Статус   \t|Опонент \t|Рейтинг \t|Тип гри \t|");
            foreach (var match in _gamesHistory)
                switch (match.PlayerStatus)
                {
                    case "Win":
                            WinStats(match.GameId, match.OpponentName._userName, (int)match.Raiting, match.GameType);
                        break;
                    case "Lose":
                        if (match.GameType.Equals("One Rait Game") || match.GameType.Equals("Training Game"))
                            LoseStats(match.GameId, match.OpponentName._userName, 0, match.GameType);
                        else if (AccountType.Equals("DoubleSaveAccount"))
                            LoseStats(match.GameId, match.OpponentName._userName, 0 - (int)match.Raiting / 2,
                                match.GameType);
                        else
                            LoseStats(match.GameId, match.OpponentName._userName, 0 - (int)match.Raiting, match.GameType);
                        break;
                }
        }

        private static void WinStats(uint gameId, string opponentsName, int raiting, string gameType) =>
            Console.WriteLine($"|№{gameId + 1} \t|Перемога \t|{opponentsName}    \t|{raiting}\t\t|{gameType}\t|");

        private static void LoseStats(uint gameId, string opponentsName, int raiting, string gameType) =>
            Console.WriteLine($"|№{gameId + 1} \t|Поразка \t|{opponentsName}    \t|{raiting}\t\t|{gameType}\t|");
    }

    //Звичайний акаунт
    public class BasicAccount : GameAccount
    {
        public BasicAccount(string userName) : base(userName)
        {
            AccountType = "BasicAccount";
        }

        public override void WinGame(GameAccount opponentName, Game match)
        {
            CurrentRaiting += match.Raiting;
            base.WinGame(opponentName, match);
        }

        public override void LoseGame(GameAccount opponentName, Game match)
        {
            var raiting = match.Raiting;
            if (match.GameType.Equals("One Rait Game")) CurrentRaiting += 0;
            else if (CurrentRaiting < raiting) CurrentRaiting = 1;
            else CurrentRaiting -= raiting;
            base.LoseGame(opponentName, match);
        }
    }

    //Акаунт, у якому після поразки втрачений рейтинг зменшується у 2 рази
    public class DoubleSaveAccount : GameAccount
    {
        public DoubleSaveAccount(string userName) : base(userName)
        {
            AccountType = "DoubleSaveAccount";
        }

        public override void WinGame(GameAccount opponentName, Game match)
        {
            CurrentRaiting += match.Raiting;
            base.WinGame(opponentName, match);
        }

        public override void LoseGame(GameAccount opponentName, Game match)
        {
            var raiting = match.Raiting / 2;
            if (match.GameType.Equals("One Rait Game")) CurrentRaiting -= 0;
            else if (CurrentRaiting < raiting) CurrentRaiting = 1;
            else CurrentRaiting -= raiting;
            base.LoseGame(opponentName, match);
        }
    }

    //Акаунт у якому після серії перемог подвоюється здобутий рейтинг
    public class ComboAccount : GameAccount
    {

        public ComboAccount(string userName) : base(userName)
        {
            AccountType = "ComboAccount";
        }

        private static bool Combo() => ComboCounter > 1;

        public override void WinGame(GameAccount opponentName, Game match)
        {
            if (!match.GameType.Equals("Training Game")) ComboCounter++;
            if (Combo())
            {
                match.Raiting *= 2;
                CurrentRaiting += match.Raiting;
            }
            else CurrentRaiting += match.Raiting;
            
            
            base.WinGame(opponentName, match);
        }

        public override void LoseGame(GameAccount opponentName, Game match)
        {
            var raiting = match.Raiting;
            if (match.GameType.Equals("One Rait Game") || match.GameType.Equals("Training Game")) CurrentRaiting += 0;
            else if (CurrentRaiting < raiting) CurrentRaiting = 1;
            else CurrentRaiting -= raiting;
            if (!match.GameType.Equals("Training Game")) ComboCounter = 0;
            base.LoseGame(opponentName, match);
        }
    }
}