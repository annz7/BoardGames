/*ToDo: взять карту со стола, положить на стол, положить в колоду, посмотреть карты на столе*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Threading;
using GameBox.Models;

namespace GameBoxConsole
{
    public class Program
    {
        static void Main(string[] args)
        {
            ConsoleCommands.NewGame(new Board());
        }
    }

    public class ConsoleCommands
    {
        public static void NewGame(Board board)
        {    
            ConsoleSettings();

            board = CreateBoard();

            string[] menu = 
            { 
                "Взять карту", 
                "Положить карту", 
                "Посмотреть на предметы", 
                "Сменить игрока", 
                "Бросить кубики",
                "Перемешать колоду", 
                "Новая игра", 
                "Закончить и выйти из игры"
            };

            var menuActions = new GameActions[]
            {
                TakeCard, 
                PutCard, 
                See, 
                ChangeArm, 
                TrowDice, 
                ShuffleDeck, 
                NewGame, 
                GameOver
            };

            var position = 0;
            var check = true;

            while (check)
            {
                WriteMenu(menu, position);
                ChooseActionAndDoIt(ref position, ref check, menu, menuActions, board);
            }
        }

        private static void ChooseActionAndDoIt(ref int position, ref bool check, IReadOnlyList<string> menu, IReadOnlyList<GameActions> menuActions, Board board)
        {
           var q = Console.ReadKey();

           switch (q.Key)
           {
               case ConsoleKey.UpArrow when position != 0:
                   position--;
                   break;
               case ConsoleKey.DownArrow when position != menu.Count - 1:
                   position++;
                   break;
               case ConsoleKey.Enter:
                   DoAction(menu[position], menuActions[position], board, ref check);
                   break;
           }
        }

        private static void DoAction(string actionName, GameActions action, Board board, ref bool check)
        {
           Console.Clear();
           Console.WriteLine(actionName + ": \n");
           Thread.Sleep(1000);
           action(board);

           if (actionName == "Новая игра" || actionName == "Закончить и выйти из игры")
           {
               check = false;
               return;
           }

           WaitForKey();
        }


        private static void WaitForKey()
        {
           Console.WriteLine("Для продолжения нажмите любую клавишу.\n");
           Console.ReadKey();
           Console.Clear();
        }

        private static void GameOver(Board board)
        {
           Console.Clear();
           Console.WriteLine("Пока((\n");
        }

        private static void ShuffleDeck(Board board)
        {
           var decks = board.GetItems<Deck>();
           Console.Clear();
           Console.WriteLine("Какую колоду?");
           Thread.Sleep(1000);
           var deck = ChooseDeck(decks);
           Actions.ShuffleDeck(deck);

           Console.Clear();
           Console.WriteLine("Колода перемешана");
        }

        private static Deck ChooseDeck(List<Deck> decks)
        {
           var position = 0;
           var check = true;
           var menu = new string[decks.Count];
           for (var i = 0; i < decks.Count; i++)
               menu[i] = i.ToString();
           
           while (check)
           {
               WriteMenu(menu, position);

               var q = Console.ReadKey();
               switch (q.Key)
               {
                   case ConsoleKey.UpArrow when position != 0:
                       position--;
                       break;
                   case ConsoleKey.DownArrow when position != menu.Count() - 1:
                       position++;
                       break;
                   case ConsoleKey.Enter:
                       return decks[position];
               }
           }
           return decks[0];
        }

        private static void TrowDice(Board board)
        {
            var dices = board.GetItems<Dice>();
            dices = ChooseDices(dices);

            foreach (var dice in dices)
                dice.ThrowDice();

            Console.Clear();
            Console.WriteLine("Кубики брошены");

        }

        private static List<Dice> ChooseDices(List<Dice> dices)
        {
           var isCorrectData = false;
           string[] indexes = { };
           while (!isCorrectData)
           {
               Console.WriteLine("Доступные кубики:\n");
               for (var i = 0; i < dices.Count; i++)
                   Console.WriteLine(dices[i] + i.ToString());
               Console.WriteLine("Введите номера кубиков через пробел, которые надо бросить:\n");
               indexes = Console.ReadLine()?.Split(' ');
               isCorrectData = CheckData(indexes, dices.Count);
               Console.Clear();

                if (!isCorrectData)
                   Console.WriteLine("Неверный формат. Попробуй ещё.");
           }

           return (indexes ?? Array.Empty<string>()).Select(index => dices[Convert.ToInt32(index)]).ToList();
        }

        private static bool CheckData(string[] indexes, int dicesCount)
        {
           return indexes.All(index => Convert.ToInt32(index) < dicesCount);
        }

        private delegate void GameActions(Board board);

        public static void TakeCard(Board board)
        {
            var deckCount = board.GetItems<Deck>().Count;
            Console.Clear();
            Console.WriteLine("Откуда вы хотите взять карту?");
            Thread.Sleep(1000);
            string[] menu = { "Из колоды.", "Со стола." };

            var position = 0;
            Console.ForegroundColor = ConsoleColor.White;
            var check = true;
            while (check)
            {
                WriteMenu(menu, position);

                var q = Console.ReadKey();

                switch (q.Key)
                {
                    case ConsoleKey.UpArrow when position != 0:
                        position--;
                        break;
                    case ConsoleKey.DownArrow when position != menu.Length - 1:
                        position++;
                        break;
                    case ConsoleKey.Enter:
                        switch (position)
                        {
                            case 0://"Из колоды."
                                TakeCardFromDeck(board);
                                break;
                            case 1://"Со стола."
                                TakeCardFromBoard(board);
                                break;
                        }
                        check = false;
                        break;
                }
            }
        }

        public static void TakeCardFromBoard(Board board)
        {
            var cards = board.GetItems<Card>();
            if (cards.Count == 0)
            {
                Console.WriteLine("Доска пуста");
                return;
            }

            var card = ChooseCard(cards);
            Actions.TakeCardAtArmFromBoard(board, card);
        }

        private static Card ChooseCard(List<Card> cards)
        {
            Console.Clear();
            Console.WriteLine("Выберите карту по индексу");
            for(var i = 0;i <cards.Count;i++)
                Console.WriteLine(cards[i].ToString() + i);
            var index = Convert.ToInt32(Console.ReadLine());
            return cards[index];
        }

        public static void TakeCardFromDeck(Board board)
        {
            var deck = ChooseDeck(board.GetItems<Deck>());

            if (deck.IsEmpty())
            {
                Console.WriteLine("Колода пуста");
                return;
            }

            Actions.TakeCardAtArmFromDeck(deck, board.CurrentArm);
            Console.WriteLine($"Вы взяли карту из колоды\n");
        }

        public static void PutCard(Board board)
        {
            if (board.CurrentArm.IsEmpty())
            {
                Console.WriteLine("Ваша рука пуста");
                return;
            }

            var card = ChooseCard(board.CurrentArm.Cards);

            Console.WriteLine("Куда вы хотите положить карту?");
            Thread.Sleep(1000);
            string[] menu = { "На стол.", "В колоду." };

            var position = 0;
            var check = true;
            while (check)
            {
                WriteMenu(menu, position);

                var q = Console.ReadKey();

                switch (q.Key)
                {
                    case ConsoleKey.UpArrow when position != 0:
                        position--;
                        break;
                    case ConsoleKey.DownArrow when position != menu.Length - 1:
                        position++;
                        break;
                    case ConsoleKey.Enter:
                        switch (position)
                        {
                            case 0://"На стол."
                                Actions.PutCardOnBoardFromArm(board, card);
                                Console.WriteLine("Вы положили карту на стол\n");
                                break;
                            case 1://"В колоду."
                                var deck = ChooseDeck(board.GetItems<Deck>());
                                Console.WriteLine($"В начало, конец или середину?(число от 0 до {deck.Cards.Count})");
                                var index = Convert.ToInt32(Console.ReadLine());
                                Actions.PutCardAtDeckFromArm(board.CurrentArm, deck, card, index);
                                Console.WriteLine("Вы положили карту в колоду\n");
                                break;
                        }
                        check = false;
                        break;
                }
            }
        }

        public static void See(Board board)
        {
            Console.Clear();
            Console.WriteLine("На что вы хотите посмотреть?");
            Thread.Sleep(1000);
            string[] menu = { "На карты в руке.", "На стол.", "На кубики." };

            var position = 0;
            var check = true;
            while (check)
            {
                WriteMenu(menu, position);

                var q = Console.ReadKey();

                switch (q.Key)
                {
                    case ConsoleKey.UpArrow when position != 0:
                        position--;
                        break;
                    case ConsoleKey.DownArrow when position != menu.Length - 1:
                        position++;
                        break;
                    case ConsoleKey.Enter:
                        switch (position)
                        {
                            case 0: //"На карты в руке."
                                if (board.CurrentArm.IsEmpty())
                                {
                                    Console.WriteLine("Рука пуста");
                                    break;
                                }
                                Console.WriteLine("Ваши карты:\n");
                                PrintItemsDescriptions<Card>(board.CurrentArm.GetCards());
                                break;
                            case 1: //"На стол."
                                var cards = board.GetItems<Card>();
                                if (!cards.Any())
                                {
                                    Console.WriteLine("На столе нет карт");
                                    break;
                                }
                                Console.WriteLine("Вы посмотрели на стол\n");
                                PrintItemsDescriptions(cards);
                                break;
                            case 2: //"На кубики."
                                var dices = board.GetItems<Dice>();
                                if (!dices.Any())
                                {
                                    Console.WriteLine("На столе нет кубиков");
                                    break;
                                }
                                Console.WriteLine("Все кубики:\n");
                                PrintItemsDescriptions(dices);
                                break;
                        }
                        check = false;
                        break;
                }
            }
        }

        public static void ChangeArm(Board board)
        {
            var arms = board.GetItems<Arm>();
            var position = 0;
            Console.ForegroundColor = ConsoleColor.White;
            var check = true;
            while (check)
            {
                Console.Clear();
                for (int i = 0; i < arms.Count; i++)
                {
                    Console.Write(i == position ? "> " : "  ");
                    Console.WriteLine(i);
                }

                var q = Console.ReadKey();

                switch (q.Key)
                {
                    case ConsoleKey.UpArrow when position != 0:
                        position--;
                        break;
                    case ConsoleKey.DownArrow when position != arms.Count - 1:
                        position++;
                        break;
                    case ConsoleKey.Enter:
                        board.ChangeArm(arms[position]);
                        check = false;
                        break;
                }
            }

            Console.Clear();
            Console.WriteLine($"Ход у игрока {position}");
            Thread.Sleep(1000);
        }

        public static void PrintItemsDescriptions<T>(List<T> items) where T : Item
        {
            for(var i = 0; i < items.Count; i++)
                Console.WriteLine(items[i].ToString() + i);
        }

        private static Board CreateBoard()
        {
            var armCount = 0;
            var deckCount = 0;
            var diceCount = 0;

            SetParameters(ref armCount, ref deckCount, ref diceCount);

            var board = new Board();
            board.AddItem(new Deck());

            for (var i = 0; i < deckCount; i++)
            {
                var deck = new Deck();
                deck.CreateDeck();
                board.AddItem(deck);
            }

            for (var i = 0; i < diceCount; i++)
                board.AddItem(new Dice());

            for (var i = 0; i < armCount; i++)
                board.AddItem(new Arm());

            board.CurrentArm = board.GetItems<Arm>()[0];
            return board;
        }

        private static void SetParameters(ref int armCount, ref int deckCount, ref int diceCount)
        {
            var gameParameters = new[] {
                "Введите колличество игроков:",
                "Введите колличество колод в игре:",
                "Введите колличество кубиков в игре:"};

            foreach (var request in gameParameters)
            {
                var checkCorrectData = true;
                while (checkCorrectData)
                {
                    try
                    {
                        Console.WriteLine(request);
                        switch (request)
                        {
                            case "Введите колличество игроков:":
                                armCount = Convert.ToInt32(Console.ReadLine());
                                break;
                            case "Введите колличество колод в игре:":
                                deckCount = Convert.ToInt32(Console.ReadLine());
                                break;
                            case "Введите колличество кубиков в игре:":
                                diceCount = Convert.ToInt32(Console.ReadLine());
                                break;
                        }

                        checkCorrectData = false;
                    }
                    catch
                    {
                        Console.WriteLine("Неверные данные. Попробуйте еще:");
                    }
                }
            }
        }


        private static void ConsoleSettings()
        {
            Console.ForegroundColor = ConsoleColor.White;
        }
        private static void WriteMenu(string[] menu, int position)
        {
            Console.Clear();
            for (var i = 0; i < menu.Length; i++)
            {
                Console.Write(i == position ? "> " : "  ");
                Console.WriteLine(menu[i]);
            }
        }
    }
}


#region ForFuture

//public static void settings()
//{
//    Console.Clear();
//    string[] menu = { "Настройка 1.", "Настройка 2.", "Настройка 3.", "Настройка 4." };
//    ConsoleKeyInfo q;
//    var position = 0;
//    Console.ForegroundColor = ConsoleColor.White;
//    var check = true;
//    while (check)
//    {
//        Console.Clear();
//        for (var i = 0; i < menu.Length; i++)
//        {
//            Console.Write(i == position ? "> " : "  ");
//            Console.WriteLine(menu[i]);
//        }

//        q = Console.ReadKey();

//        switch (q.Key)
//        {
//            case ConsoleKey.UpArrow when position != 0:
//                position--;
//                break;
//            case ConsoleKey.DownArrow when position != menu.Length - 1:
//                position++;
//                break;
//            case ConsoleKey.Enter:
//                switch (position)
//                {
//                    case 0:
//                        Console.WriteLine("Настройка 1:\n");
//                        Thread.Sleep(1000);
//                        break;
//                    case 1:
//                        Console.WriteLine("Настройка 2:\n");
//                        Thread.Sleep(1000);
//                        break;
//                    case 2:
//                        Console.WriteLine("Настройка 3:\n");
//                        Thread.Sleep(1000);
//                        break;
//                    case 3:
//                        Console.WriteLine("Настройка 4:\n");
//                        Thread.Sleep(1000);
//                        break;
//                }
//                check = false;
//                break;
//        }
//    }
//}

//public static void Start()
//{
//    Console.Clear();
//    Console.ForegroundColor = ConsoleColor.White;

//    string[] menu = { "Начать новую игру.", "Настройки." };
//    var position = 0;
//    var check = true;
//    while (check)
//    {
//        WriteMenu(menu, position);

//        var q = Console.ReadKey();

//        switch (q.Key)
//        {
//            case ConsoleKey.UpArrow when position != 0:
//                position--;
//                break;
//            case ConsoleKey.DownArrow when position != menu.Length - 1:
//                position++;
//                break;
//            case ConsoleKey.Enter:
//                switch (position)
//                {
//                    case 0:
//                        Console.WriteLine("Начало новой игры:\n");
//                        Thread.Sleep(1000);
//                        //Console.ReadKey();
//                        NewGame();
//                        return;
//                    case 1:
//                        Console.WriteLine("Настройки:\n");
//                        Thread.Sleep(1000);
//                        settings();
//                        //Console.ReadKey();
//                        break;
//                }
//                //check = false;
//                break;
//        }
//    }
//}

//public static void YesOrNo()
//{
//    Console.Clear();
//    string[] menu = { "Да.", "Нет." };
//    ConsoleKeyInfo q;
//    var position = 0;
//    Console.ForegroundColor = ConsoleColor.White;
//    var check = true;
//    while (check)
//    {
//        Console.Clear();
//        for (int i = 0; i < menu.Length; i++)
//        {
//            Console.Write(i == position ? "> " : "  ");
//            Console.WriteLine(menu[i]);
//        }

//        q = Console.ReadKey();

//        switch (q.Key)
//        {
//            case ConsoleKey.UpArrow when position != 0:
//                position--;
//                break;
//            case ConsoleKey.DownArrow when position != menu.Length - 1:
//                position++;
//                break;
//            case ConsoleKey.Enter:
//                switch (position)
//                {
//                    case 0://"Да."
//                        Console.WriteLine("Хорошо.\n");
//                        Thread.Sleep(1000);
//                        break;
//                    case 1://"Нет."
//                        Console.WriteLine("Хорошо.\n");
//                        Thread.Sleep(1000);
//                        NewGame(new Board());
//                        break;
//                }
//                check = false;
//                break;
//        }
//    }
//}
#endregion