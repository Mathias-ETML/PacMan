using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PacMan
{
    public class Variables
    {
        public static byte G_BYTESIZEOFSQUARE { get; } = 40;
        public static byte G_BYTETIMEBETWENGAMETICK { get; } = 250; // in milliseconds
        public static byte G_NUMBEROFGHOST { get; } = 4;
        public static byte g_byteNumberOfPlayer { get; set; } = 1;
        public static byte g_byteNumberOfPacMan { get; set; } = 0;
        public static byte g_byteNumberOfSpawnedGhost { get; set; } = 0;
        public static Ghost[] g_ghosts { get; set; } = new Ghost[G_NUMBEROFGHOST];
        public static PacMan[] g_pacMans { get; set; }
    }
}
