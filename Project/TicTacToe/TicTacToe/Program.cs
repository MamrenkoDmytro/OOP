using System;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography;
using System.Text;

namespace TicTacToe
{
    public static class Program
    {
        private static DB _myDb = new DB();

        static void Main()
        {
            //Серіализация .bin форматом
            var binFormatter = new BinaryFormatter();
            using (var file = new FileStream("DB.bin", FileMode.OpenOrCreate))
            {
                if (binFormatter.Deserialize(file) is DB newDb)
                    _myDb = newDb;
            }

            MainMenu();
            using (var file = new FileStream("DB.bin", FileMode.OpenOrCreate))
            {
                binFormatter.Serialize(file, _myDb);
            }
        }

        //Головне меню з усіма доступними функціями
        private static void MainMenu()
        {
            Console.WriteLine("Выберите одну из опций игрового меню");
            Console.WriteLine(
                "1. Начать игру.\n" +
                "2. Статистика всех игроков.\n" +
                "3. Статистика пользователя.\n" +
                "4. История игр.\n" +
                "5. История игр пользователя.\n" +
                "9. Регистрация нового пользователя.\n" +
                "0. Закрыть игру.");
            try
            {
                var choice = int.Parse(Console.ReadLine());
                Console.Clear();
                switch (choice)
                {
                    case 0:
                        Console.WriteLine("Выключение игры.");
                        return;
                    case 1:
                        ChoosePlayers();
                        MainMenu();
                        break;
                    case 2:
                        _myDb.PrintStats();
                        MainMenu();
                        break;
                    case 3:
                        Console.Write("Введите имя пользователя: ");
                        _myDb.PrintStats(Console.ReadLine());
                        MainMenu();
                        break;
                    case 4:
                        _myDb.PrintHistory();
                        MainMenu();
                        break;
                    case 5:
                        Console.Write("Введите имя пользователя: ");
                        _myDb.PrintHistory(Console.ReadLine());
                        MainMenu();
                        break;
                    case 9:
                        Reg();
                        MainMenu();
                        break;
                    default:
                        /*_myDb.Users.Clear();
                        _myDb.Games.Clear();*/
                        Console.WriteLine("Такой опции не существует");
                        MainMenu();
                        break;
                }
            }
            catch
            {
                Console.Clear();
                Console.WriteLine("Такой опции не существует");
                MainMenu();
            }
        }

        //Реєстрація користувача
        private static void Reg()
        {
            Console.Write(
                "Выберите тип пользователя:\n" +
                "1. Обычный аккаунт.\n" +
                "2. Аккаунт с двойным рейтингом.\n" +
                "3. Аккаунт без проигрышей.\n" +
                "Вернитесь в меню (любая другая кнопка).");
            var choose = 0;
            try
            {
                var option = int.Parse(Console.ReadLine());
                switch (option)
                {
                    case 1:
                        choose = 1;
                        break;
                    case 2:
                        choose = 2;
                        break;
                    case 3:
                        choose = 3;
                        break;
                    case 0:
                        Console.Clear();
                        return;
                }
            }
            catch
            {
                Console.Clear();
                return;
            }

            Console.WriteLine("Регистрация нового пользователя.");
            Console.Write("Введите логин: ");
            var newUserName = Console.ReadLine();
            if (_myDb.Users.Any(user => newUserName == user.Name))
            {
                Console.WriteLine("Пользователь с таким именем уже существует.");
                return;
            }

            Console.Write("Придумайте пароль: ");
            var newUserPassword = GetHash(Console.ReadLine());
            Console.Write("Подтвердите пароль: ");
            var checkPassword = GetHash(Console.ReadLine());
            var tries = 3;
            while ((checkPassword == null || checkPassword != newUserPassword) && tries > 0)
            {
                Console.WriteLine(
                    $"Пароль введён неверно, повторите попытку (осталось попыток: {tries}) или отмените регистрацию: ");
                checkPassword = GetHash(Console.ReadLine());
                tries--;
                if (tries != 0) continue;
                Console.WriteLine("Попытки закончились.");
                return;
            }

            Console.Clear();
            Console.WriteLine("Пользователь успешно зарегестрирован.");
            switch (choose)
            {
                case 1:
                    _myDb.Users.Add(new BasicUser(newUserName, newUserPassword));
                    break;
                case 2:
                    _myDb.Users.Add(new DoubleRaitingUser(newUserName, newUserPassword));
                    break;
                case 3:
                    _myDb.Users.Add(new NoLoseUser(newUserName, newUserPassword));
                    break;
            }
        }

        //Авторизація користувача
        private static User LogIn()
        {
            try
            {
                Console.Write("Введите логин:");
                var name = Console.ReadLine();
                Console.Write("Введите пароль:");
                var password = GetHash(Console.ReadLine());
                foreach (var user in _myDb.Users.Where(user => user.Name == name && user.Password == password))
                    return user;
            }
            catch
            {
                Console.WriteLine("Неправильный логин или пароль, попробуйте ещё раз.");
                return null;
            }

            return null;
        }

        //Вибір гравців для гри
        private static void ChoosePlayers()
        {
            if (_myDb.Users.Count < 2)
            {
                Console.WriteLine("Зарегестрировано менее 2 пользователей, начать игру невозможно.");
                return;
            }

            Console.WriteLine("Выберите первого игрока(X).");
            var player1 = LogIn();
            while (player1 == null)
            {
                Console.Clear();
                Console.WriteLine("Такого пользователя не существует, попробуйте ещё раз.");
                player1 = LogIn();
            }

            Console.Clear();
            Console.WriteLine("Выберите второго игрока(O).");
            var player2 = LogIn();
            while (player1 == player2 || player2 == null)
            {
                Console.Clear();
                Console.WriteLine(player1 == player2 ? "Этот игрок уже в игре!" : "Такого пользователя не существует");
                player2 = LogIn();
            }

            var game = new Game(player1, player2);
            Console.Clear();
            Console.WriteLine("Игра начинается");
            game.PlayGame();
            _myDb.Games.Add(game);
        }

        //хешування паролю, за допомогою алгоритму хешування MD5
        private static string GetHash(string password)
        {
            var md5 = MD5.Create();
            var hash = md5.ComputeHash(Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(hash);
        }
    }
}