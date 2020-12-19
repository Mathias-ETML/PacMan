using System;
using System.Windows.Forms;
using PacManGame.Controllers.GameControllerNS;

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
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            GameController gameController = new GameController();
        }
        #endregion Main entry
    }
}
