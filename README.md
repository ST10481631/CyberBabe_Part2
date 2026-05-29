# CyberBabe_Part2


## Overview
CyberBabe is a simple WPF based chatbot application built in C#  
It simulates a conversational AI that responds to cybersecurity topics such as phishing malware passwords and safe browsing while also detecting user sentiment and remembering basic user information

## Features

### Chatbot Responses
- Responds to cybersecurity keywords such as
  - Passwords
  - Phishing
  - Malware
  - Firewalls
  - Encryption
  - Data breaches
- Uses a keyword matching system to generate responses

### Sentiment Detection
- Detects emotions in user input
  - frustrated
  - confused
  - worried
  - happy
  - sad
  - angry
- Responds with supportive messages based on detected sentiment

### Username System
- Prompts user to enter a username on startup
- Validates input letters and spaces only
- Stores usernames in a text file user_names.txt
- Greets returning users differently from new users

### Chat Interface
- Displays conversation in a styled chat format using WPF ListView
- Differentiates between user and chatbot messages using colors and borders
- Auto scrolls to the latest message

### Interest Tracking
- Detects when users mention interests such as I am interested in cybersecurity
- Saves interests to interested_topic.txt
- Prevents duplicate interests from being stored

## Project Structure

Practise_Part2

MainWindow.xaml.cs Main UI logic and event handling  
ChatBot.cs Chatbot response engine and keyword handling  
user_names.cs Username handling and file storage  
user_names.txt Stores registered usernames  
interested_topic.txt Stores user interests  

## How It Works

1 User starts the application  
2 System greets user via voice and UI  
3 User enters username  
   - New users are stored in a file  
   - Returning users get a welcome back message  
4 Chat interface opens  
5 User types messages  
6 System processes input  
   - Cleans input  
   - Detects sentiment  
   - Checks keywords  
   - Generates responses  
   - Stores interests if mentioned  

## Technologies Used
- C#
- WPF Windows Presentation Foundation
- File IO
- Regular Expressions
- ArrayList and Lists

## Future Improvements
- Replace ArrayList with Dictionary for faster lookup
- Improve natural language processing
- Add database storage instead of text files
- Add speech to text input
- Improve chatbot learning capabilities

## Author
CyberBabe Chatbot Project built for educational purposes in C# WPF development
