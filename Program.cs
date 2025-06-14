using System;
using System.Windows;

namespace MyWpfApp
{
    public class Program : System.Windows.Application
    {
        [STAThread]
        public static void Main()
        {
            var app = new Program();
            var window = new MainWindow(); 
            app.Run(window); // start the app with your custom window
        }
    }
}
