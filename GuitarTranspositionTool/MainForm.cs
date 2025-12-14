using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace GuitarTranspositionTool
{
    public partial class MainForm : Form
    {
        private readonly IGuitarTranspositionService _transpositionService;
        private readonly IMidiNoteCalculator _midiCalculator;
        private readonly List<GuitarNote> _inputNotes;
        private readonly List<GuitarNote> _transposedNotes;

        public MainForm()
        {
            InitializeComponent();
            
            // Initialize services
            _midiCalculator = new MidiNoteCalculator();
            _transpositionService = new GuitarTranspositionService(_midiCalculator);
            
            _inputNotes = new List<GuitarNote>();
            _transposedNotes = new List<GuitarNote>();
            
            InitializeControls();
        }

        private void InitializeControls()
        {
            Text = "Guitar Transposition Tool";
            Size = new System.Drawing.Size(800, 600);
            StartPosition = FormStartPosition.CenterScreen;
        }

        public void AddNote(int stringNum, int fret)
        {
            try
            {
                int midiNote = _midiCalculator.GetMidiNote(stringNum, fret);
                var note = new GuitarNote(
                    stringNum, 
                    fret, 
                    midiNote,
                    _midiCalculator.GetNoteName(midiNote),
                    _midiCalculator.GetFrequency(midiNote)
                );

                _inputNotes.Add(note);
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show(ex.Message, "Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public TranspositionResult TransposeNotes()
        {
            if (_inputNotes.Count == 0)
            {
                MessageBox.Show("Please add some notes first.", "No Notes", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return new TranspositionResult();
            }

            return _transpositionService.TransposeNotes(_inputNotes);
        }

        public void ClearAll()
        {
            _inputNotes.Clear();
            _transposedNotes.Clear();
        }

        public List<GuitarNote> GetInputNotes() => new List<GuitarNote>(_inputNotes);
        public List<GuitarNote> GetTransposedNotes() => new List<GuitarNote>(_transposedNotes);
    }
}
