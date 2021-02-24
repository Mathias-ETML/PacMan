using System;
using System.Windows.Forms;
using PacManGame.Controllers;

namespace PacManGame
{
    /// <summary>
    /// Default class
    /// </summary>
    static class Program
    {
        /// <summary>
        /// Point d'entrée principal de l'application.
        /// </summary>
        [STAThread]
        #region Main entry
        static void Main()
        {

            // ArgIterator()

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            UpdateController updateController = new UpdateController();
            updateController.OnStart();
        }
        #endregion Main entry
    }
}
