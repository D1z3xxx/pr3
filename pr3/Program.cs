using System;
using System.Collections.Generic;
using System.Linq;

namespace TextAnalyzer
{

    public class TextStatistics
    {
        public int WordCount { get; set; }
        public string ShortestWord { get; set; }
        public string LongestWord { get; set; }
        public int SentenceCount { get; set; }
        public int VowelCount { get; set; }
        public int ConsonantCount { get; set; }


        public Dictionary<char, int> LetterFrequency { get; set; }

        public TextStatistics()
        {
            LetterFrequency = new Dictionary<char, int>();
        }


        public void Print()
        {

            Console.WriteLine($"Количество слов: {WordCount}");
            Console.WriteLine($"Самое короткое слово: {ShortestWord}");
            Console.WriteLine($"Самое длинное слово: {LongestWord}");
            Console.WriteLine($"Количество предложений: {SentenceCount}");
            Console.WriteLine($"Гласных букв: {VowelCount}");
            Console.WriteLine($"Согласных букв: {ConsonantCount}");

            Console.WriteLine("\nЧастота букв:");


            int columns = 5;
            int counter = 0;

            foreach (var pair in LetterFrequency)
            {

                string output = $"  '{pair.Key}': {pair.Value,-4}";

                Console.Write(output);
                counter++;


                if (counter % columns == 0)
                {
                    Console.WriteLine();
                }
            }


            if (counter % columns != 0)
            {
                Console.WriteLine();
            }



        }
    }

    class Program
    {
        static List<TextStatistics> history = new List<TextStatistics>();

        static void Main(string[] args)
        {
            Console.WriteLine("Добро пожаловать в программу анализа текста!");

            Console.OutputEncoding = System.Text.Encoding.UTF8;

            bool continueProgram = true;

            while (continueProgram)
            {
                Console.WriteLine("\nВыберите действие:");
                Console.WriteLine("1. Ввести новый текст для анализа");
                Console.WriteLine("2. Показать статистику по прошлым текстам");
                Console.WriteLine("3. Выход");
                Console.Write("Ваш выбор: ");

                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        ProcessNewText();
                        break;
                    case "2":
                        ShowHistory();
                        break;
                    case "3":
                        continueProgram = false;
                        break;
                    default:
                        Console.WriteLine("Некорректный ввод. Попробуйте снова.");
                        break;
                }
            }
        }

        static void ProcessNewText()
        {
            Console.WriteLine("\nВведите текст (минимум 100 символов):");
            string inputText = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(inputText) || inputText.Length < 100)
            {
                Console.WriteLine($"Ошибка: Текст слишком короткий ({inputText?.Length ?? 0} символов). Нужно минимум 100.");
                return;
            }

            TextStatistics stats = new TextStatistics();


            int sentenceCount = 0;
            foreach (char c in inputText)
            {
                if (c == '.' || c == '!' || c == '?')
                {
                    sentenceCount++;
                }
            }
            if (sentenceCount == 0 && inputText.Length > 0) sentenceCount = 1;
            stats.SentenceCount = sentenceCount;


            char[] delimiters = new char[] { ' ', '.', ',', '!', '?', ';', ':', '-', '\n', '\r', '(', ')' };
            string[] words = inputText.Split(delimiters, StringSplitOptions.RemoveEmptyEntries);
            stats.WordCount = words.Length;

            // 3. Поиск самого короткого и длинного слова
            string shortest = null;
            string longest = null;

            foreach (string word in words)
            {
                string cleanWord = word.Trim();
                if (string.IsNullOrEmpty(cleanWord)) continue;

                if (shortest == null || cleanWord.Length < shortest.Length) shortest = cleanWord;
                if (longest == null || cleanWord.Length > longest.Length) longest = cleanWord;
            }
            stats.ShortestWord = shortest ?? "Нет слов";
            stats.LongestWord = longest ?? "Нет слов";

            // 4. Подсчет гласных, согласных и частоты букв
            string vowels = "аеёиоуыэюяАЕЁИОУЫЭЮЯ";
            string consonants = "бвгджзйклмнпрстфхцчшщБВГДЖЗЙКЛМНПРСТФХЦЧШЩ";

            int vowelCount = 0;
            int consonantCount = 0;
            string lowerText = inputText.ToLower();

            foreach (char c in lowerText)
            {
                if (char.IsLetter(c))
                {
                    if (vowels.Contains(c)) vowelCount++;
                    else if (consonants.Contains(c)) consonantCount++;

                    if (stats.LetterFrequency.ContainsKey(c))
                        stats.LetterFrequency[c]++;
                    else
                        stats.LetterFrequency[c] = 1;
                }
            }

            stats.VowelCount = vowelCount;
            stats.ConsonantCount = consonantCount;
            history.Add(stats);

            Console.WriteLine("\nАнализ завершен!");
            stats.Print();
        }

        static void ShowHistory()
        {
            if (history.Count == 0)
            {
                Console.WriteLine("История пуста. Сначала введите текст.");
                return;
            }

            Console.WriteLine($"\n=== Статистика по прошлым текстам (всего: {history.Count}) ===");
            for (int i = 0; i < history.Count; i++)
            {
                Console.WriteLine($"\n--- Текст №{i + 1} ---");
                history[i].Print();
            }
        }
    }
}
//У лукоморья дуб зелёный; Златая цепь на дубе том: И днём и ночью кот учёный Всё ходит по цепи кругом; Идёт направо — песнь заводит, Налево — сказку говорит. Там чудеса: там леший бродит, Русалка на ветвях сидит; Там на неведомых дорожках Следы невиданных зверей; Избушка там на курьих ножках Стоит без окон, без дверей; Там лес и дол видений полны;