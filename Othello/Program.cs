
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace Othello
{
    public class Program
    {
        [STAThread]
        public static void Main()
        {
            Application.EnableVisualStyles();
            GameManager newGame = new GameManager();
            newGame.InitGame();
        }
    }
}