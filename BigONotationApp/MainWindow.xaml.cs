using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Windows;

namespace BigONotationApp
{
    public partial class MainWindow : Window
    {
        private int currentCardIndex = 0;
        private List<FlashCard> flashCards = new List<FlashCard>();
        private bool isShowingAnswer = false;

        public MainWindow()
        {
            InitializeComponent();
            LoadFlashCardsFromDatabase();
            DisplayCard();
        }

        private void LoadFlashCardsFromDatabase()
        {
            string connectionString = "Data Source=C:\\Users\\Christian\\Desktop\\Projects\\BigONotationApp\\BigONotationApp\\flashcards.db;Version=3;";
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT * FROM FlashCards";
                using (var command = new SQLiteCommand(query, connection))
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        flashCards.Add(new FlashCard
                        {
                            Question = reader["Question"]?.ToString() ?? "Unknown Question",
                            CorrectAnswer = reader["CorrectAnswer"]?.ToString() ?? "Unknown Answer",
                            IncorrectAnswer1 = reader["IncorrectAnswer1"]?.ToString() ?? "Unknown Answer",
                            IncorrectAnswer2 = reader["IncorrectAnswer2"]?.ToString() ?? "Unknown Answer",
                            Hint = reader["Hint"]?.ToString() ?? "No Hint Available"
                        });
                    }
                }
            }
        }

        private void DisplayCard()
        {
            if (flashCards.Count == 0)
            {
                MessageBox.Show("No flashcards found.");
                return;
            }

            var currentCard = flashCards[currentCardIndex];

            if (!isShowingAnswer)
            {
                TextBlockQuestionAnswer.Text = currentCard.Question;
            }
            else
            {
                TextBlockQuestionAnswer.Text = $"Answer: {currentCard.CorrectAnswer}";
            }

            var answers = new List<string>
            {
                currentCard.CorrectAnswer,
                currentCard.IncorrectAnswer1,
                currentCard.IncorrectAnswer2
            };
            answers.Shuffle();

            TextBoxAnswer1.Text = answers[0];
            TextBoxAnswer2.Text = answers[1];
            TextBoxAnswer3.Text = answers[2];
        }


        private void ButtonAnswer_Click(object sender, RoutedEventArgs e)
        {
            isShowingAnswer = !isShowingAnswer;
            DisplayCard();
        }

        private void ButtonNext_Click(object sender, RoutedEventArgs e)
        {
            currentCardIndex = (currentCardIndex + 1) % flashCards.Count;
            isShowingAnswer = false;
            DisplayCard();
        }

        private void ButtonPrevious_Click(object sender, RoutedEventArgs e)
        {
            currentCardIndex = (currentCardIndex - 1 + flashCards.Count) % flashCards.Count;
            isShowingAnswer = false;
            DisplayCard();
        }

        private void ButtonHint_Click(object sender, RoutedEventArgs e)
        {
            var currentCard = flashCards[currentCardIndex];
            MessageBox.Show(currentCard.Hint, "Hint");
        }
    }

    public class FlashCard
    {
        public string? Question { get; set; } = string.Empty;
        public string? CorrectAnswer { get; set; } = string.Empty;
        public string? IncorrectAnswer1 { get; set; } = string.Empty;
        public string? IncorrectAnswer2 { get; set; } = string.Empty;
        public string? Hint { get; set; } = string.Empty;
    }


    public static class Extensions
    {
        private static Random rng = new Random();

        public static void Shuffle<T>(this IList<T> list)
        {
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }
    }
}