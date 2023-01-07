using System;
using System.Linq;

namespace TicTacToe
{
    [Serializable]
    public class Game
    {
        public User Player1 { get; set; }
        public User Player2 { get; set; }
        private string[] _gameField = { "1", "2", "3", "4", "5", "6", "7", "8", "9" };
        public int WinRaiting = 2;
        public int LoseRaiting = -1;
        public int Player1CurrRait;
        public int Player2CurrRait;
        public User Winner { get; set; }
        public User Looser { get; set; }
        private int _turn = 1;
        private bool _end;

        public Game(User player1, User player2)
        {
            Player1 = player1;
            Player2 = player2;
        }


        //Початок гри
        public void PlayGame()
        {
            var who = _turn % 2 != 0 ? "X" : "O";
            _turn++;
            if (!_end)
                Choose(who);
        }

        //Вибір клітини на ігровому полі
        private void Choose(string who)
        {
            try
            {
                Console.WriteLine("Выберите свободную ячейку, чтобы сделать ход.");
                PrintGameField();
                var cell = int.Parse(Console.ReadLine());
                Console.Clear();
                if (cell != int.Parse(_gameField[cell - 1])) return;
                _gameField[cell - 1] = who;
                if (!EndGame())
                    PlayGame();
            }
            catch
            {
                Console.Clear();
                Console.WriteLine("Введите число от 1 до 9");
                Choose(who);
            }
        }

        //Перевірка, чи закінчилася гра
        private bool EndGame()
        {
            if (WinCondition("X"))
            {
                WinGame(Player1);
                _end = true;
                return true;
            }

            if (WinCondition("O"))
            {
                WinGame(Player2);
                _end = true;
                return true;
            }

            if (!Draw()) return false;
            PrintGameField();
            Console.WriteLine("Ничья.");
            Player1.GameResult(this);
            Player2.GameResult(this);
            return true;
        }

        //Визначення переможця
        private void WinGame(User user)
        {
            PrintGameField();
            if (Player1 == user)
            {
                Winner = Player1;
                Looser = Player2;
                WriteWinner(Player1, Player2, "X");
            }
            else
            {
                Winner = Player2;
                Looser = Player1;
                WriteWinner(Player2, Player1, "O");
            }
        }

        //Запис даних, після перемоги
        private void WriteWinner(User winner, User loser, string who)
        {
            Console.WriteLine("Победа игрока " + who + ": " + winner.Name);
            winner.WinGame(this);
            loser.LoseGame(this);
        }

        //Умови перемоги
        public bool WinCondition(string condition)
        {
            return _gameField[0].Equals(condition) && _gameField[4].Equals(condition) && _gameField[8].Equals(condition) ||
                   _gameField[2].Equals(condition) && _gameField[4].Equals(condition) && _gameField[6].Equals(condition) ||
                   _gameField[0].Equals(condition) && _gameField[1].Equals(condition) && _gameField[2].Equals(condition) ||
                   _gameField[3].Equals(condition) && _gameField[4].Equals(condition) && _gameField[5].Equals(condition) ||
                   _gameField[6].Equals(condition) && _gameField[7].Equals(condition) && _gameField[8].Equals(condition) ||
                   _gameField[0].Equals(condition) && _gameField[3].Equals(condition) && _gameField[6].Equals(condition) ||
                   _gameField[1].Equals(condition) && _gameField[4].Equals(condition) && _gameField[7].Equals(condition) ||
                   _gameField[2].Equals(condition) && _gameField[5].Equals(condition) && _gameField[8].Equals(condition);
        }

        //Перевірка на нічию
        public bool Draw() => _gameField.All(field => field.Equals("X") || field.Equals("O"));

        //Друк ігрового поля
        public void PrintGameField()
        {
            for (var i = 1; i < _gameField.Length + 1; i++)
            {
                Console.Write("|" + _gameField[i - 1] + "|");
                if (i % 3 == 0)
                    Console.WriteLine();
            }
        }
    }
}