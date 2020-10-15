namespace PacMan
{
    /// <summary>
    /// Global variables class
    /// </summary>
    public static class Variables
    {
        #region static variables
        public static readonly byte G_BYTESIZEOFSQUARE = 40;
        public static readonly byte G_BYTETIMEBETWENGAMETICK = 100; // in milliseconds
        public static readonly byte G_NUMBEROFGHOST = 4;
        public static byte G_numberOfPlayer = 1;
        public static byte G_numberOfPacMan = 0;
        public static byte G_numberOfSpawnedGhost = 0;
        public static Ghost[] G_ghosts;
        public static PacMan[] G_pacMans;
        #endregion static variables
    }
}
