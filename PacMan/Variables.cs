namespace PacMan
{
    /// <summary>
    /// Global variables class
    /// </summary>
    public static class Variables
    {
        #region static variables
        public static readonly byte G_BYTESIZEOFSQUARE = 40;
        public static readonly byte G_BYTETIMEBETWENGAMETICK = 150; // in milliseconds
        public static readonly byte G_NUMBEROFGHOST = 1;
        public static byte G_numberOfPlayer = 1;
        public static byte G_numberOfPacMan = 0;
        public static byte G_numberOfSpawnedGhost = 0;
        public static Ghost[] G_ghosts;
        public static PacMan[] G_pacMans;
        public static bool G_lightMode = false;
        public static bool G_RicardoMode = false;
        #endregion static variables
    }
}
