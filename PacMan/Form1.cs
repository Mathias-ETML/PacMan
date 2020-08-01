using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PacMan
{
    public partial class frm_FormPrincipal : Form
    {
        public List<PacMan> g_listPlayerPacMan = new List<PacMan>();

        public frm_FormPrincipal()
        {
            InitializeComponent();
        }

        private void pan_PanGame_Click(object sender, EventArgs e)
        {
            PacMan b = new PacMan();

            g_listPlayerPacMan.Add(b);

            b.Spawn((Panel)sender, e);
        }
    }
}
