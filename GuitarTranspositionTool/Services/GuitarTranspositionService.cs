using System;
using System.Collections.Generic;
using System.Linq;

namespace GuitarTranspositionTool
{
    /// <summary>
    /// Handles guitar note transposition logic
    /// </summary>
    public class GuitarTranspositionService : IGuitarTranspositionService
    {
        private readonly IMidiNoteCalculator _midiCalculator;

        public GuitarTranspositionService(IMidiNoteCalculator midiCalculator)
        {
            _midiCalculator = midiCalculator;
        }

        public List<GuitarNote> FindAllPositions(int midiNote)
        {
            var positions = new List<GuitarNote>();

            for (int stringNum = 1; stringNum <= 6; stringNum++)
            {
                for (int fret = 0; fret <= 24; fret++)
                {
                    if (_midiCalculator.GetMidiNote(stringNum, fret) == midiNote)
                    {
                        var note = new GuitarNote(
                            stringNum, 
                            fret, 
                            midiNote,
                            _midiCalculator.GetNoteName(midiNote),
                            _midiCalculator.GetFrequency(midiNote)
                        );
                        positions.Add(note);
                    }
                }
            }

            return positions;
        }

        public GuitarNote? FindLowestPosition(int midiNote)
        {
            var allPositions = FindAllPositions(midiNote);
            return allPositions.OrderBy(p => p.Fret).FirstOrDefault();
        }

        public TranspositionResult TransposeNotes(List<GuitarNote> originalNotes)
        {
            var result = new TranspositionResult
            {
                OriginalNotes = new List<GuitarNote>(originalNotes)
            };

            foreach (var originalNote in originalNotes)
            {
                var lowestPosition = FindLowestPosition(originalNote.MidiNote);
                if (lowestPosition != null)
                {
                    result.TransposedNotes.Add(lowestPosition);
                }
            }

            if (result.OriginalNotes.Count > 0)
            {
                result.OriginalAverageFret = result.OriginalNotes.Average(n => n.Fret);
            }

            if (result.TransposedNotes.Count > 0)
            {
                result.TransposedAverageFret = result.TransposedNotes.Average(n => n.Fret);
            }

            return result;
        }

        public List<GuitarNote> ParseInputNotes(List<FretPosition> positions)
        {
            var notes = new List<GuitarNote>();

            foreach (var position in positions)
            {
                try
                {
                    int midiNote = _midiCalculator.GetMidiNote(position.String, position.Fret);
                    var note = new GuitarNote(
                        position.String,
                        position.Fret,
                        midiNote,
                        _midiCalculator.GetNoteName(midiNote),
                        _midiCalculator.GetFrequency(midiNote)
                    );
                    notes.Add(note);
                }
                catch (ArgumentException)
                {
                    // Skip invalid positions
                    continue;
                }
            }

            return notes;
        }
    }
}
