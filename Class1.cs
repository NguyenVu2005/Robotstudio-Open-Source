using ABB.Robotics.Math;
using ABB.Robotics.RobotStudio;
using ABB.Robotics.RobotStudio.Environment;
using ABB.Robotics.RobotStudio.Stations;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
//using OpenAI.Chat;
//using OpenAI;

namespace RobotStudioEmptyAddin1
{
    public class Class1
    {
        private static ToolWindow chatWindow;
        private static TextBox chatLog;
        private static TextBox inputBox;
        public static void AddinMain()
        {
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

        }
        private static void KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true; 
                chatLog.AppendText("You: " + inputBox.Text + "\r\n"); 
                inputBox.Clear();

                //OpenAIClient client = new OpenAIClient(Environment.GetEnvironmentVariable("chatgpt_api"));
                string message = inputBox.Text;
               
                if (message == "Hello World")
                {
                    chatLog.AppendText("ChatGPT: Hello!\r\n");
                }
            }
        }
    }
}