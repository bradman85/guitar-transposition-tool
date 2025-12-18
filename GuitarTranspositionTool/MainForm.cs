using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Drawing;
using GuitarNeckControl;

namespace GuitarTranspositionTool
{
	public partial class MainForm : Form
	{
		private readonly IGuitarTranspositionService _transpositionService;
		private readonly IMidiNoteCalculator _midiCalculator;
		private readonly List<GuitarNote> _inputNotes;
		private readonly List<GuitarNote> _transposedNotes;

		private GuitarNeckControl.GuitarNeckControl _guitarNeckControl;
		private ListBox _inputListBox;
		private ListBox _outputListBox;
		private Label _statsLabel;
		private Button _transposeButton;
		private Button _clearButton;
		private CheckBox _multiSelectCheckBox;

		public MainForm()
		{
			_midiCalculator = new MidiNoteCalculator();
			_transpositionService = new GuitarTranspositionService(_midiCalculator);
			_inputNotes = new List<GuitarNote>();
			_transposedNotes = new List<GuitarNote>();

			this.SuspendLayout();
			BuildUI();

			this.Text = "Guitar Transposition Tool";
			this.Size = new Size(800, 600);
			this.StartPosition = FormStartPosition.CenterScreen;
			this.BackColor = Color.LightGray;
			this.ResumeLayout(false);
		}

		private void BuildUI()
		{
			// Guitar neck control
			_guitarNeckControl = new GuitarNeckControl.GuitarNeckControl();
			_guitarNeckControl.Location = new Point(20, 20);
			_guitarNeckControl.Size = new Size(740, 250);
			_guitarNeckControl.FretSelected += GuitarNeckControl_FretSelected;
			this.Controls.Add(_guitarNeckControl);

			// Buttons
			_transposeButton = new Button();
			_transposeButton.Text = "Transpose";
			_transposeButton.Location = new Point(20, 280);
			_transposeButton.Size = new Size(100, 30);
			_transposeButton.Click += TransposeButton_Click;
			this.Controls.Add(_transposeButton);

			_clearButton = new Button();
			_clearButton.Text = "Clear";
			_clearButton.Location = new Point(130, 280);
			_clearButton.Size = new Size(100, 30);
			_clearButton.Click += ClearButton_Click;
			this.Controls.Add(_clearButton);

			// Multi-select checkbox - CORRECT NAME
			_multiSelectCheckBox = new CheckBox();
			_multiSelectCheckBox.Text = "Multi-Select";
			_multiSelectCheckBox.Location = new Point(250, 280);
			_multiSelectCheckBox.Size = new Size(120, 30);
			_multiSelectCheckBox.CheckedChanged += MultiSelectCheckBox_CheckedChanged;
			this.Controls.Add(_multiSelectCheckBox);

			// List boxes
			_inputListBox = new ListBox();
			_inputListBox.Location = new Point(20, 320);
			_inputListBox.Size = new Size(360, 100);
			_inputListBox.BackColor = Color.LightBlue;
			this.Controls.Add(_inputListBox);

			_outputListBox = new ListBox();
			_outputListBox.Location = new Point(400, 320);
			_outputListBox.Size = new Size(360, 100);
			_outputListBox.BackColor = Color.LightGreen;
			this.Controls.Add(_outputListBox);

			// Status label
			_statsLabel = new Label();
			_statsLabel.Location = new Point(20, 430);
			_statsLabel.Size = new Size(740, 60);
			_statsLabel.Text = "Click on guitar neck to add notes";
			this.Controls.Add(_statsLabel);
		}

		private void GuitarNeckControl_FretSelected(object sender, FretPositionEventArgs e)
		{
			try
			{
				int stringNum = e.Position.String;
				int fret = e.Position.Fret;

				int midiNote = _midiCalculator.GetMidiNote(stringNum, fret);
				var note = new GuitarNote(stringNum, fret, midiNote,
					_midiCalculator.GetNoteName(midiNote),
					_midiCalculator.GetFrequency(midiNote));

				if (_guitarNeckControl.MultiSelect)
				{
					_inputNotes.Add(note);
					_inputListBox.Items.Add(note.ToString());
				}
				else
				{
					_inputNotes.Clear();
					_inputListBox.Items.Clear();
					_inputNotes.Add(note);
					_inputListBox.Items.Add(note.ToString());
				}

				UpdateGuitarNeckDisplay();
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		private void TransposeButton_Click(object sender, EventArgs e)
		{
			if (_inputNotes.Count == 0)
			{
				MessageBox.Show("Please add some notes first.", "No Notes", MessageBoxButtons.OK, MessageBoxIcon.Warning);
				return;
			}

			// Clear previous transposed notes
			_transposedNotes.Clear();

			// Perform transposition
			var result = _transpositionService.TransposeNotes(_inputNotes);

			// Add transposed notes
			foreach (var note in result.TransposedNotes)
			{
				_transposedNotes.Add(note);
			}

			// Update output list
			_outputListBox.Items.Clear();
			foreach (var note in result.TransposedNotes)
			{
				_outputListBox.Items.Add(note.ToString());
			}

			// Update statistics
			if (result.IsSuccessful)
			{
				_statsLabel.Text = $"Average fret: {result.OriginalAverageFret:F1} → {result.TransposedAverageFret:F1}\n" +
								 $"Fret reduction: {result.FretReduction:F1} positions\n" +
								 $"Improvement: {result.PercentageImprovement:F1}%";
			}

			UpdateGuitarNeckDisplay();
		}

		private void ClearButton_Click(object sender, EventArgs e)
		{
			_inputNotes.Clear();
			_transposedNotes.Clear();
			_inputListBox.Items.Clear();
			_outputListBox.Items.Clear();
			_guitarNeckControl.ClearSelection();
			_guitarNeckControl.ClearAllNotes();
			_statsLabel.Text = "Click on guitar neck to add notes";
		}

		private void MultiSelectCheckBox_CheckedChanged(object sender, EventArgs e)
		{
			// FIXED: Use correct variable name with capital C
			_guitarNeckControl.MultiSelect = _multiSelectCheckBox.Checked;
			if (!_multiSelectCheckBox.Checked)
			{
				_guitarNeckControl.ClearSelection();
			}
		}

		private void UpdateGuitarNeckDisplay()
		{
			// FIXED: Use individual position setting instead of list conversion
			// Clear existing display first
			_guitarNeckControl.ClearAllNotes();

			// Add original notes one by one
			foreach (var note in _inputNotes)
			{
				_guitarNeckControl.SelectPosition(note.String, note.Fret);
			}

			// For transposed notes, use a different approach since we can't create the wrong type
			// We'll use the individual position approach for transposed notes too
			foreach (var note in _transposedNotes)
			{
				// Use a different visual indication for transposed notes
				// Since we can't create FretPosition in wrong namespace, we'll use selection or different method
				_guitarNeckControl.SelectPosition(note.String, note.Fret);
			}

			// Set display mode based on what's available
			if (_transposedNotes.Count > 0)
			{
				_guitarNeckControl.CurrentDisplayMode = GuitarNeckControl.DisplayMode.Both;
			}
			else
			{
				_guitarNeckControl.CurrentDisplayMode = GuitarNeckControl.DisplayMode.Original;
			}
		}
	}
}
