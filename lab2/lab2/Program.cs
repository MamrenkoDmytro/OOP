using System;
using System.Collections.Generic;

namespace lab2
{
    internal static class Program
    {
        public static void Main(string[] args)
        {
            var players = new List<GameAccount>();
            GameAccount logan = new BasicAccount("Логан");
            GameAccount deadpool = new BasicAccount("Дедпул");
            GameAccount jocker = new ComboAccount("Джокер");
            GameAccount hulk = new DoubleSaveAccount("Халк");
            players.Add(logan);
            players.Add(deadpool);
            players.Add(jocker);
            players.Add(hulk);
            var i = 60;
            var rand = new Random();
            while (i != 0)
            {
                var randomGame = rand.Next(1, 4);
                var randomWinner = rand.Next(0, 2);
                var player1 = GetPlayer(players);
                var player2 = GetPlayer(players);
                while (player1.Equals(player2))
                    player2 = GetPlayer(players);
                PlayGame(player1, player2, GameFactory.GenerateRandomGame(randomGame), randomWinner);
                i--;
            }

            foreach (var player in players)
                player.GetStats();
        }

        //Обирається випадковий гравець зі списку
        private static GameAccount GetPlayer(IReadOnlyList<GameAccount> players)
        {
            var rand = new Random();
            var randomPlayer = rand.Next(players.Count);
            return players[randomPlayer];
        }

        //імітація гри
        private static void PlayGame(GameAccount player1, GameAccount player2, Game match, int randomWinner)
        {
            if (randomWinner < 1)
            {
                player2.LoseGame(player1, match);
                player1.WinGame(player2, match);
            }
            else
            {
                player1.LoseGame(player2, match);
                player2.WinGame(player1, match);
            }
        }
    }
}