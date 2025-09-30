using ABB.Robotics.Math;
using ABB.Robotics.RobotStudio;
using ABB.Robotics.RobotStudio.Environment;
using ABB.Robotics.RobotStudio.Stations;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using OpenAI;
using OpenAI.Chat;

namespace RobotStudioTextAddin
{
    public class Class1
    {
      

        private static ToolWindow chatWindow;
        private static TextBox chatLog;
        private static TextBox inputBox;

        // Conversation memory
        private static List<ChatMessage> messages = new List<ChatMessage>();

        public static void AddinMain()
        {
            // Create main chat UI
            UserControl chatControl = new UserControl
            {
                Dock = DockStyle.Fill
            };

            chatLog = new TextBox
            {
                Multiline = true,
                ReadOnly = true,
                Dock = DockStyle.Fill,
                ScrollBars = ScrollBars.Vertical
            };

            inputBox = new TextBox
            {
                Dock = DockStyle.Bottom
            };

            inputBox.KeyDown += KeyDown;

            chatControl.Controls.Add(chatLog);
            chatControl.Controls.Add(inputBox);

            chatWindow = new ToolWindow("ChatGPT", chatControl);
            UIEnvironment.Windows.Add(chatWindow);
            chatWindow.Visible = true;

            // Add system role at start
            messages.Add(ChatMessage.CreateSystemMessage("You are a helpful assistant inside ABB RobotStudio."));
        }

        private static async void KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;

                string userMessage = inputBox.Text.Trim();
                if (string.IsNullOrEmpty(userMessage)) return;

                chatLog.AppendText("You: " + userMessage + "\r\n");
                inputBox.Clear();

                // Add user message
                messages.Add(ChatMessage.CreateUserMessage(userMessage));

                try
                {
                    // Create OpenAI client with environment key
                    //var client = new OpenAIClient(Environment.GetEnvironmentVariable("chatgpt_api"));

                    var apiKey = "";//luffysolosyonko's personal api key
                    var client = new OpenAIClient(apiKey);

                    // Get a chat client for the model
                    var chatClient = client.GetChatClient("gpt-4o-mini");

                    // Send conversation
                    var response = await chatClient.CompleteChatAsync(messages);

                    // Extract reply
                    string reply = response.Value.Content[0].Text;


                    // Show reply
                    chatLog.AppendText("ChatGPT: " + reply + "\r\n");

                    // Add assistant reply
                    messages.Add(ChatMessage.CreateAssistantMessage(reply));
                }
                catch (Exception ex)
                {
                    chatLog.AppendText("Error: " + ex.Message + "\r\n");
                }
            }
        }
    }
}
