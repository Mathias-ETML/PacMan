using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace PacManGame.Test
{
#pragma warning disable 0169
    public abstract class RoundPanel
    {
        // todo : re coder rectangle en circle
        // todo : re coder les parents de panel avec la classe circle
        // todo : implémenter round region
        // todo : implémenter cirleF
        // todo : implémenter marshal by ref object
        // todo : regarder le source code microsoft

        private Panel Panel;
        private Rectangle Rectangle;
        private RectangleF RectangleF;

        //
        private Region Region;
        private MarshalByRefObject marshalByRefObject; // for region
        //

        public RoundPanel()
        {
            throw new NotImplementedException("2021");
        }
    }
#pragma warning restore 0169
}
