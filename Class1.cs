using ABB.Robotics.Math;
using ABB.Robotics.RobotStudio;
using ABB.Robotics.RobotStudio.Environment;
using ABB.Robotics.RobotStudio.Stations;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using OpenAI.Chat;
using OpenAI;

namespace ChatAddIn
{
    public class Class1
    {
        private static ToolWindow chatWindow;
        private static TextBox chatLog;
        private static TextBox inputBox;

        private static OpenAIClient client;
        public static void AddinMain()
        {
            string apikey = Environment.GetEnvironmentVariable("chatgpt_api");
            if (string.IsNullOrEmpty(apikey))
            {
                MessageBox.Show("Please set the environment variable 'chatgpt_api' with your OpenAI API key.");
                return;
            }

            client = new OpenAIClient(apikey);

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

            inputBox.KeyUp += KeyDown;
            chatControl.Controls.Add(chatLog);
            chatControl.Controls.Add(inputBox);

            chatWindow = new ToolWindow("ChatGPT", chatControl);
            UIEnvironment.Windows.Add(chatWindow);
            chatWindow.Visible = true;

            inputBox.Focus();

        }
        private static void KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                MessageBox.Show("KeyDown fired");
                e.SuppressKeyPress = true;

                string message = inputBox.Text;
                chatLog.AppendText("You: " + inputBox.Text + "\r\n");
                inputBox.Clear();
                

                if (client != null)
                {
                    chatLog.AppendText("ChatGPT client intialized \r\n");
                }
                else
                {
                    chatLog.AppendText("ChatGPT client is null \r\n");
                }
            }
        }
    }
}