using System;

namespace lab2
{
    public abstract class Game
    {
        public GameAccount OpponentName;
        public string GameType;
        public uint GameId;
        public uint Raiting;
        public string PlayerStatus;
        protected readonly Random RandomRaiting = new Random();
        public abstract uint GetRaiting();
    }

    //Звичайна рейтингова гра
    public class CommonGame : Game
    {
        public CommonGame()
        {
            Raiting = GetRaiting();
            GameType = "Common Game";
        }

        public sealed override uint GetRaiting() => (uint)RandomRaiting.Next(25, 36);
    }

    //Тренувальна гра без втрати рейтингу
    public class TrainingGame : Game
    {
        public TrainingGame()
        {
            Raiting = GetRaiting();
            GameType = "Training Game";
        }

        public sealed override uint GetRaiting() => 0;
    }

    //Гра у якій програвший гравець не втрачає рейтинг
    public class OneRaitGame : Game
    {
        public OneRaitGame()
        {
            Raiting = GetRaiting();
            GameType = "One Rait Game";
        }

        public sealed override uint GetRaiting() => (uint)RandomRaiting.Next(25, 36);
    }
}