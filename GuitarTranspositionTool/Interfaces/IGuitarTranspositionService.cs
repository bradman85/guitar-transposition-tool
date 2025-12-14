using System.Collections.Generic;

namespace GuitarTranspositionTool
{
    /// <summary>
    /// Interface for guitar transposition services
    /// </summary>
    public interface IGuitarTranspositionService
    {
        List<GuitarNote> FindAllPositions(int midiNote);
        GuitarNote? FindLowestPosition(int midiNote);
        TranspositionResult TransposeNotes(List<GuitarNote> originalNotes);
        List<GuitarNote> ParseInputNotes(List<FretPosition> positions);
    }
}
