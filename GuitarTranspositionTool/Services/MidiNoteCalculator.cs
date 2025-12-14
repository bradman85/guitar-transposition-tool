using System;

namespace GuitarTranspositionTool
{
    /// <summary>
    /// Calculates MIDI notes, note names, and frequencies for guitar
    /// </summary>
    public class MidiNoteCalculator : IMidiNoteCalculator
    {
        // Standard tuning MIDI notes for open strings (1st to 6th string)
        private readonly int[] _openStringMidi = { 64, 59, 55, 50, 45, 40 }; // E4, B3, G3, D3, A2, E2
        private readonly string[] _noteNames = { "C", "C#", "D", "D#", "E", "F", "F#", "G", "G#", "A", "A#", "B" };
        private const double A4_FREQUENCY = 440.0;
        private const int A4_MIDI = 69;

        public int GetMidiNote(int stringNum, int fret)
        {
            if (stringNum < 1 || stringNum > 6)
                throw new ArgumentException("String must be between 1 and 6");
            if (fret < 0 || fret > 24)
                throw new ArgumentException("Fret must be between 0 and 24");

            return _openStringMidi[stringNum - 1] + fret;
        }

        public string GetNoteName(int midiNote)
        {
            int octave = (midiNote / 12) - 1;
            int noteIndex = midiNote % 12;
            return $"{_noteNames[noteIndex]}{octave}";
        }

        public double GetFrequency(int midiNote)
        {
            return A4_FREQUENCY * Math.Pow(2.0, (midiNote - A4_MIDI) / 12.0);
        }
    }
}
