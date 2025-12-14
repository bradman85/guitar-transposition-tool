using System;

namespace GuitarTranspositionTool
{
    /// <summary>
    /// Represents a guitar note with string, fret, and musical information
    /// </summary>
    public class GuitarNote
    {
        public int String { get; set; } // 1-6 (1st to 6th string)
        public int Fret { get; set; }   // 0-24
        public int MidiNote { get; set; }
        public string NoteName { get; set; } = string.Empty;
        public double Frequency { get; set; }

        public GuitarNote(int stringNum, int fret, int midiNote, string noteName, double frequency)
        {
            String = stringNum;
            Fret = fret;
            MidiNote = midiNote;
            NoteName = noteName;
            Frequency = frequency;
        }

        public override string ToString()
        {
            return $"{NoteName} (String {String}, Fret {Fret})";
        }
    }
}
