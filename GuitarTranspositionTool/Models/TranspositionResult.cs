using System.Collections.Generic;
using System.Linq;

namespace GuitarTranspositionTool
{
    /// <summary>
    /// Contains the results of a transposition operation
    /// </summary>
    public class TranspositionResult
    {
        public List<GuitarNote> OriginalNotes { get; set; } = new();
        public List<GuitarNote> TransposedNotes { get; set; } = new();
        public double OriginalAverageFret { get; set; }
        public double TransposedAverageFret { get; set; }
        public double FretReduction => OriginalAverageFret - TransposedAverageFret;
        public double PercentageImprovement => OriginalAverageFret > 0 ? (FretReduction / OriginalAverageFret) * 100 : 0;

        public bool IsSuccessful => TransposedNotes.Count > 0;
    }
}
