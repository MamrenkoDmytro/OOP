using System;
using System.Collections.Generic;

namespace TicTacToe
{
    [Serializable]
    public abstract class User
    {
        private int Id { get; }
        public string Name { get; set; }
        public int Raiting { get; set; }
        public int GameRait;
        public string Password { get; set; }
        public List<Game> History;
        private static int _idGeneration;

        protected User(string name, string password)
        {
            Name = name;
            Password = password;
            Id = _idGeneration++;
            History = new List<Game>();
        }

        //Абстрактна реалізація методу, що повертатиме рейтинг на який грали у грі
        protected abstract int GetRaiting(Game game);

        //Запис перемоги у історію гравця
        public virtual void WinGame(Game game)
        {
            game.Player1CurrRait = game.Player1.Raiting;
            game.Player2CurrRait = game.Player2.Raiting;
            History.Add(game);
        }

        //Запис поразки у історію граіця
        public virtual void LoseGame(Game game)
        {
            game.Player1CurrRait = game.Player1.Raiting;
            game.Player2CurrRait = game.Player2.Raiting;
            History.Add(game);
        }

        //Запис нічиєї у історію гравця
        public void GameResult(Game game)
        {
            History.Add(game);
        }

        //Друк історії ігор користувача
        public void PrintHistory()
        {
            if (History.Count == 0)
            {
                Console.WriteLine("Пользователь " + Name + " не сыграл ни одной игры.");
                return;
            }

            var gameCount = 1;
            foreach (var game in History)
            {
                if (game.WinCondition("X") || game.WinCondition("O"))
                {
                    Console.WriteLine($"№{gameCount}. Игроки: {game.Player1.Name}(X), {game.Player2.Name}(O). " +
                                      $"Победитель: {game.Winner.Name} +{game.Winner.GameRait}.\nРейтинг первого " +
                                      $"игрока({game.Player1.Name}): {game.Player1CurrRait}\nРейтинг второго игрока(" +
                                      $"{game.Player2.Name}): {game.Player2CurrRait}");
                    gameCount++;
                }
                else
                {
                    Console.WriteLine($"№{gameCount}. Игроки: {game.Player1.Name}(X), {game.Player2.Name}(O). Ничья.");
                    gameCount++;
                }
            }
        }
    }

    [Serializable]
    //Клас звичайного користувача
    public class BasicUser : User
    {
        public BasicUser(string name, string password) : base(name, password)
        {
        }

        //Реалізований абстрактний метод
        protected override int GetRaiting(Game game) => GameRait = game.WinRaiting;

        public override void WinGame(Game game)
        {
            GetRaiting(game);
            Raiting += GameRait;
            base.WinGame(game);
        }

        public override void LoseGame(Game game)
        {
            if (Raiting + game.LoseRaiting < 0) Raiting = 0;
            else Raiting += game.LoseRaiting;
            base.LoseGame(game);
        }
    }

    [Serializable]
    //Клас користувача, що отримує подвійний рейтинг при перемозі
    public class DoubleRaitingUser : User
    {
        public DoubleRaitingUser(string name, string password) : base(name, password)
        {
        }

        protected override int GetRaiting(Game game) => GameRait = 2 * game.WinRaiting;

        public override void WinGame(Game game)
        {
            GetRaiting(game);
            Raiting += GameRait;
            base.WinGame(game);
        }

        public override void LoseGame(Game game)
        {
            if (Raiting + game.LoseRaiting < 0) Raiting = 0;
            else Raiting += game.LoseRaiting;
            base.LoseGame(game);
        }
    }

    [Serializable]
    //Клас користувача, який не втрачає рейтинг при поразці
    public class NoLoseUser : User
    {
        public NoLoseUser(string name, string password) : base(name, password)
        {
        }

        protected override int GetRaiting(Game game) => GameRait = game.WinRaiting;

        public override void WinGame(Game game)
        {
            GetRaiting(game);
            Raiting += GameRait;
            base.WinGame(game);
        }

        public override void LoseGame(Game game)
        {
            game.LoseRaiting = 0;
            base.LoseGame(game);
        }
    }
}