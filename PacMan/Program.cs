using System;
using System.Windows.Forms;

namespace PacMan
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
            Application.Run(new frm_FormPrincipal());
        }
        #endregion Main entry
    }
}
