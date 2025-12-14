namespace GuitarTranspositionTool
{
    /// <summary>
    /// Represents a specific position on the guitar fretboard
    /// </summary>
    public class FretPosition
    {
        public int String { get; set; }
        public int Fret { get; set; }

        public FretPosition(int stringNum, int fret)
        {
            String = stringNum;
            Fret = fret;
        }
    }
}
