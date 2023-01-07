using System;
using System.Collections.Generic;
using System.Linq;

namespace TicTacToe
{
    [Serializable]
    public class DB
    {
        public List<User> Users { get; set; }
        public List<Game> Games { get; set; }

        public DB()
        {
            Users = new List<User>();
            Games = new List<Game>();
        }

        //Дпук статистики усіх користувачів
        public void PrintStats()
        {
            if (IsEmptyUsers()) return;
            foreach (var user in Users)
                Console.WriteLine($"Игрок: {user.Name}. Рейтинг: {user.Raiting}.");
        }

        //Дпук статистики обраного користувача
        public void PrintStats(string player)
        {
            if (IsEmptyUsers())
                return;
            foreach (var user in Users.Where(user => user.Name == player))
            {
                Console.WriteLine($"Игрок: {user.Name}. Рейтинг: {user.Raiting}.");
                return;
            }

            Console.WriteLine("Такого пользователя не существует.");
        }

        //Друк історії усіх ігор
        public void PrintHistory()
        {
            var gameCount = 1;
            if (IsEmptyHistory()) return;
            Console.WriteLine("История игр.");
            foreach (var game in Games)
            {
                Console.WriteLine(
                    "----------------------------------------------------------------------------------------------------------------------");
                Console.WriteLine($"№{gameCount}. X: {game.Player1.Name}. O: {game.Player2.Name}.");
                if (game.WinCondition("X") || game.WinCondition("O"))
                    Console.WriteLine($"Победитель ({game.Winner.GameRait}): {game.Winner.Name}. Проигравший ({game.LoseRaiting}): {game.Looser.Name}.");
                else
                {
                    Console.WriteLine("Ничья");
                }
                if (game == Games[Games.Count - 1])
                {
                    Console.WriteLine(
                        "----------------------------------------------------------------------------------------------------------------------");
                    return;
                }
                gameCount++;
            }
        }

        //Друк історії усіх ігор, обраного користувача
        public void PrintHistory(string player)
        {
            if (IsEmptyUsers()) return;
            foreach (var user in Users.Where(user => user.Name == player))
            {
                user.PrintHistory();
                return;
            }

            Console.WriteLine("Такого пользователя не существует.");
        }

        //Перевірка, чи існують користувачи
        private bool IsEmptyUsers()
        {
            if (Users.Count != 0) return false;
            Console.WriteLine("Не зарегестрировано ни одного пользователя.");
            return true;
        }

        //Перевірка, чи була зіграна хоча б одна гра
        private bool IsEmptyHistory()
        {
            if (Games.Count != 0) return false;
            Console.WriteLine("Ещё не сыграно ни одной игры");
            return true;
        }
    }
}