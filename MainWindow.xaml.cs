using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CyberBabe_Part2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            //creating an instance for the class Array
            ArrayList reply = new ArrayList();
            ArrayList ignore = new ArrayList();
            user_names check_name = new user_names();

            // variables
            string username = string.Empty;
            string pre_question = string.Empty;
            int counting = 0;

            ChatBot bot;


        public MainWindow()
        {
            InitializeComponent();
            //creating a class with a constructor
            bot = new ChatBot(reply, ignore);

            //creating an instance for the class voice_greeting 
            //with an object name greet
            voice_greeting greet = new voice_greeting();

            //call the voice method
            greet.greet();
        }

        private void start_ai(object sender, RoutedEventArgs e)
        {
            //Hide home page grid and set Username grid visible
            start_grid.Visibility = Visibility.Hidden;
            username_grid.Visibility = Visibility.Visible;
        }

        private void submit_name(object sender, RoutedEventArgs e)
        {
            string inputName = users_input.Text.Trim();

            // check empty
            if (string.IsNullOrWhiteSpace(inputName))
            {
                error_method("ChatBot", "Please enter a username.");
                return;
            }

            // validation: only letters and spaces allowed
            if (!Regex.IsMatch(inputName, @"^[A-Za-z\s]+$"))
            {

                users_input.Clear();
                return;
            }

            // if valid, assign username
            username = check_name.submit_name(users_input, Conversation);

            // move to chat screen
            username_grid.Visibility = Visibility.Hidden;
            chatbot_grid.Visibility = Visibility.Visible;
        }

        private void btnSend_Click(object sender, RoutedEventArgs e)
        {
            // Get the question from the design and sanitize it
            string rawQuestion = questions.Text.Trim();

            if (string.IsNullOrWhiteSpace(rawQuestion))
            {
                error_method("ChatBot", "Please enter a question.");
                return;
            }

            // Remove special characters and clean the question
            string cleanQuestion = RemoveSpecialCharacters(rawQuestion);

            // Show what the user typed 
            error_method(username, rawQuestion);

            // ai chats and auto_show_interest
            auto_show_interest();

            // Pass cleaned string instead of TextBox
            ai_check(cleanQuestion);

            // Optional: clear textbox after sending
            questions.Clear();
        }

        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            Conversation.Items.Clear();
            questions.Clear();
        }

        private string RemoveSpecialCharacters(string input)
        {
            return Regex.Replace(input, @"[^a-zA-Z0-9\s]", "");
        }

        //start of ai_chat method
        private void ai_check(string questions)
        {


            // Check if user entered anything meaningful
            if (string.IsNullOrWhiteSpace(questions))
            {
                error_method("ChatBot", "Please enter a valid question.");
                this.questions.Clear();
                return;
            }



            // Check if the question contains only special characters or empty after cleaning
            if (questions.Length == 0 || string.IsNullOrWhiteSpace(questions))
            {
                error_method("ChatBot", "I couldn't understand that.");
                this.questions.Clear();
                return;
            }

            // Variables for processing
            string[] words = questions.ToLower().Split(new char[] { ' ', ',', '.', '?', '!', ';', ':' }, StringSplitOptions.RemoveEmptyEntries);

            //sentiment detection
            string sentiment = "";

            // check full sentence for emotions
            if (questions.ToLower().Contains("frustrated")) sentiment = "frustrated";
            else if (questions.ToLower().Contains("confused")) sentiment = "confused";
            else if (questions.ToLower().Contains("worried")) sentiment = "worried";
            else if (questions.ToLower().Contains("happy")) sentiment = "happy";
            else if (questions.ToLower().Contains("sad")) sentiment = "sad";
            else if (questions.ToLower().Contains("angry")) sentiment = "angry";

            //respond to sentiment immediately
            if (!string.IsNullOrEmpty(sentiment))
            {
                foreach (string item in reply)
                {
                    if (item.StartsWith(sentiment))
                    {
                        string sentiment_response = item.Substring(sentiment.Length).Trim();
                        error_method("ChatBot", sentiment_response);
                        return; // stop further processing
                    }
                }
            }

            string message = string.Empty;
            Random indexer = new Random();
            List<string> per_word = new List<string>();
            List<string> answers_found = new List<string>();

            // Process each word
            foreach (string word in words)
            {
                // Skip very short words or ignored words
                if (word.Length < 3 || ignore.Contains(word.ToLower()))
                    continue;

                per_word.Clear();

                //start of interests

                if (word.Contains("interested"))
                {
                    string store_interests = string.Empty;
                    bool found_interest = false;

                    HashSet<string> currentInterests = new HashSet<string>();

                    foreach (string interest in words)
                    {
                        // CLEAN INPUT
                        string clean = interest.ToLower().Trim();
                        clean = Regex.Replace(clean, @"[^a-zA-Z0-9\s]", "");

                        // FILTER NOISE WORDS
                        if (!ignore.Contains(clean) && clean != "interested" && clean != "and" && clean != "in" && clean.Length >= 3)
                        {
                            found_interest = true;
                            currentInterests.Add(clean);
                        }
                    }


                    // prepare interests
                    store_interests = string.Join(", ", currentInterests);

                    if (found_interest && !string.IsNullOrWhiteSpace(store_interests))
                    {
                        string filename = "interested_topic.txt";
                        bool userFound = false;

                        if (File.Exists(filename))
                        {
                            string[] lines = File.ReadAllLines(filename);

                            for (int i = 0; i < lines.Length; i++)
                            {
                                if (lines[i].StartsWith(username))
                                {
                                    userFound = true;

                                    //get all the interests
                                    string existing = lines[i].Replace(username + " interested in:", "").ToLower();

                                    HashSet<string> existingSet = new HashSet<string>(existing.Split(',').Select(x => x.Trim()).Where(x => x != ""));

                                    // remove dumplicates
                                    foreach (string item in currentInterests)
                                    {
                                        existingSet.Add(item);
                                    }

                                    string finalList = string.Join(", ", existingSet);

                                    lines[i] = username + " interested in: " + finalList;
                                    File.WriteAllLines(filename, lines);

                                    message += "great, i added " + store_interests + " to your interests and ";
                                    break;
                                }
                            }
                        }

                        if (!userFound)
                        {
                            File.AppendAllText(
                                filename,
                                username + " interested in: " + store_interests + "\n"
                            );

                            message += "great, i will remember that you are interested in " + store_interests + " and ";
                        }
                    }
                    else
                    {
                        message += "Please specify what you're interested in (e.g., 'I am interested in cybersecurity')";


                    }
                }
                if (!string.IsNullOrWhiteSpace(message))
                {
                    error_method("ChatBot", message);
                    return;
                }
                string response = bot.getResponse(questions, reply);
                error_method("ChatBot", response);
            }
        }

        private void auto_show_interest()
        {
            if (counting >= 3)
            {
                // Reset counting after threshold is reached
                counting = 0;
            }
            else
            {
                // Incrementing the interest counter
                counting += 1;
            }
        }



        // Updated error method with better formatting
        private void error_method(string name, string message)
        {
            // Create a border for chats
            Border messageBorder = new Border
            {
                Margin = new Thickness(0, 2, 0, 2),
                Padding = new Thickness(5, 3, 5, 3),
                CornerRadius = new CornerRadius(5)
            };

            // Set different background for user vs bot
            if (name.ToLower().Contains("chatbot") || name.ToLower().Contains("chat"))
            {// Light blue
                messageBorder.Background = new SolidColorBrush(Color.FromRgb(240, 248, 255));
                messageBorder.BorderBrush = new SolidColorBrush(Color.FromRgb(173, 216, 230));
            }
            else
            {    // Light gray
                messageBorder.Background = new SolidColorBrush(Color.FromRgb(245, 245, 245));
                messageBorder.BorderBrush = new SolidColorBrush(Color.FromRgb(211, 211, 211));
            }
            messageBorder.BorderThickness = new Thickness(1);

            TextBlock messageText = new TextBlock
            {
                TextWrapping = TextWrapping.Wrap,
                Margin = new Thickness(2)
            };

            // Set color based on sender
            Brush nameColor = (name.ToLower().Contains("chatbot") || name.ToLower().Contains("chat")) ?
                              Brushes.Magenta : Brushes.DarkMagenta;

            Brush messageColor = Brushes.Black;

            messageText.Inlines.Add(new Run
            {
                Text = name + ": ",
                Foreground = nameColor,
                FontWeight = FontWeights.Bold
            });

            messageText.Inlines.Add(new Run
            {
                Text = message,
                Foreground = messageColor
            });

            messageBorder.Child = messageText;
            Conversation.Items.Add(messageBorder);

            Conversation.ScrollIntoView(Conversation.Items[Conversation.Items.Count - 1]);
        }//end of error method

    }//end of class
}//end of namespace
