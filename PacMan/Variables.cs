using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PacMan
{
    public class Variables
    {
        public static readonly byte G_BYTESIZEOFSQUARE = 40;
        public static readonly byte G_BYTETIMEBETWENGAMETICK = 100; // in milliseconds
        public static readonly byte G_NUMBEROFGHOST = 4;
        public static byte g_byteNumberOfPlayer = 1;
        public static byte g_byteNumberOfPacMan = 0;
        public static byte g_byteNumberOfSpawnedGhost = 0;
        public static Ghost[] g_tab_ghosts;
        public static PacMan[] g_tab_pacMans;
    }
}
