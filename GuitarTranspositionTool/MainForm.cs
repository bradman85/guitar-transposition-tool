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
		}

		// Event handler for Add Note button
		private void btnAddNote_Click(object sender, EventArgs e)
		{
			try
			{
				int stringNum = (int)numericString.Value;
				int fret = (int)numericFret.Value;

				int midiNote = _midiCalculator.GetMidiNote(stringNum, fret);
				var note = new GuitarNote(
					stringNum,
					fret,
					midiNote,
					_midiCalculator.GetNoteName(midiNote),
					_midiCalculator.GetFrequency(midiNote)
				);

				_inputNotes.Add(note);
				listBoxInput.Items.Add(note.ToString());

				// Clear the fret field for next input
				numericFret.Value = 0;
			}
			catch (ArgumentException ex)
			{
				MessageBox.Show(ex.Message, "Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		// Event handler for Transpose button
		private void btnTranspose_Click(object sender, EventArgs e)
		{
			if (_inputNotes.Count == 0)
			{
				MessageBox.Show("Please add some notes first.", "No Notes", MessageBoxButtons.OK, MessageBoxIcon.Warning);
				return;
			}

			var result = _transpositionService.TransposeNotes(_inputNotes);

			// Clear output list
			listBoxOutput.Items.Clear();
			_transposedNotes.Clear();

			// Add transposed notes to output
			foreach (var note in result.TransposedNotes)
			{
				_transposedNotes.Add(note);
				listBoxOutput.Items.Add(note.ToString());
			}

			// Update statistics
			if (result.IsSuccessful)
			{
				labelStats.Text = $"Average fret reduced from {result.OriginalAverageFret:F1} to {result.TransposedAverageFret:F1}\n" +
								 $"Fret reduction: {result.FretReduction:F1} positions\n" +
								 $"Percentage improvement: {result.PercentageImprovement:F1}%";
			}
		}

		// Event handler for Clear All button
		private void btnClear_Click(object sender, EventArgs e)
		{
			_inputNotes.Clear();
			_transposedNotes.Clear();
			listBoxInput.Items.Clear();
			listBoxOutput.Items.Clear();
			labelStats.Text = "Enter notes and click Transpose to see results.";
		}

		public List<GuitarNote> GetInputNotes() => new List<GuitarNote>(_inputNotes);
		public List<GuitarNote> GetTransposedNotes() => new List<GuitarNote>(_transposedNotes);
	}
}
