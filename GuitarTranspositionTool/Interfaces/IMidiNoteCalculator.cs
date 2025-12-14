namespace GuitarTranspositionTool
{
    /// <summary>
    /// Interface for MIDI note calculations
    /// </summary>
    public interface IMidiNoteCalculator
    {
        int GetMidiNote(int stringNum, int fret);
        string GetNoteName(int midiNote);
        double GetFrequency(int midiNote);
    }
}
