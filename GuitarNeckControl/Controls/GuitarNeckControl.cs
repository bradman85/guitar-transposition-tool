using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.ComponentModel;

namespace GuitarNeckControl
{
	/// <summary>
	/// Display modes for the guitar neck control
	/// </summary>
	public enum DisplayMode
	{
		Original,
		Transposed,
		Both
	}

	/// <summary>
	/// Enhanced guitar neck control with dual display modes for original/transposed notes
	/// </summary>
	public partial class GuitarNeckControl : UserControl
	{
		#region Constants
		private const int NUM_STRINGS = 6;
		private const int NUM_FRETS = 15; // Extended to 15 frets
		private const int FRET_WIDTH = 45;
		private const int STRING_HEIGHT = 28;
		private const int NECK_OFFSET = 40;
		#endregion

		#region Fields
		private FretPosition _selectedPosition = new FretPosition(1, 0);
		private readonly List<FretPosition> _selectedPositions = new List<FretPosition>();
		private readonly List<FretPosition> _originalNotes = new List<FretPosition>();
		private readonly List<FretPosition> _transposedNotes = new List<FretPosition>();
		private DisplayMode _displayMode = DisplayMode.Original;
		private bool _multiSelect = false;
		private bool _showBothModes = false; // Show both original and transposed
		#endregion

		#region Events
		public event EventHandler<FretPositionEventArgs> FretSelected;
		public event EventHandler<FretPositionEventArgs> FretDeselected;
		public event EventHandler<DisplayModeChangedEventArgs> DisplayModeChanged;
		#endregion

		#region Constructor
		public GuitarNeckControl()
		{
			InitializeComponent();
			SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.DoubleBuffer, true);
			BackColor = Color.FromArgb(139, 69, 19); // Guitar neck brown
			Size = new Size(NUM_FRETS * FRET_WIDTH + NECK_OFFSET * 2, NUM_STRINGS * STRING_HEIGHT + NECK_OFFSET * 2);
			MinimumSize = Size;

			// Fix the default selection issue
			ClearSelection();
		}
		#endregion

		#region Properties with Designer Attributes
		[Browsable(true)]
		[Category("Behavior")]
		[Description("Currently selected fret position")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
		public FretPosition SelectedPosition
		{
			get => _selectedPosition;
			set
			{
				_selectedPosition = value ?? new FretPosition(1, 0);
				Invalidate();
			}
		}

		[Browsable(true)]
		[Category("Behavior")]
		[Description("Display mode for showing original vs transposed notes")]
		[DefaultValue(DisplayMode.Original)]
		public DisplayMode CurrentDisplayMode
		{
			get => _displayMode;
			set
			{
				_displayMode = value;
				DisplayModeChanged?.Invoke(this, new DisplayModeChangedEventArgs(value));
				Invalidate();
			}
		}

		[Browsable(true)]
		[Category("Behavior")]
		[Description("Enable multi-selection of fret positions")]
		[DefaultValue(false)]
		public bool MultiSelect
		{
			get => _multiSelect;
			set
			{
				_multiSelect = value;
				if (!_multiSelect)
				{
					_selectedPositions.Clear();
				}
				Invalidate();
			}
		}

		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public IReadOnlyList<FretPosition> OriginalNotes => _originalNotes.AsReadOnly();

		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public IReadOnlyList<FretPosition> TransposedNotes => _transposedNotes.AsReadOnly();

		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public IReadOnlyList<FretPosition> SelectedPositions => _selectedPositions.AsReadOnly();
		#endregion

		#region ShouldSerialize Methods
		private bool ShouldSerializeSelectedPosition()
		{
			return _selectedPosition.String != 1 || _selectedPosition.Fret != 0;
		}

		private void ResetSelectedPosition()
		{
			SelectedPosition = new FretPosition(1, 0);
		}
		#endregion

		#region Note Management
		/// <summary>
		/// Set original notes to display
		/// </summary>
		public void SetOriginalNotes(IEnumerable<FretPosition> notes)
		{
			_originalNotes.Clear();
			if (notes != null)
			{
				_originalNotes.AddRange(notes);
			}
			Invalidate();
		}

		/// <summary>
		/// Set transposed notes to display
		/// </summary>
		public void SetTransposedNotes(IEnumerable<FretPosition> notes)
		{
			_transposedNotes.Clear();
			if (notes != null)
			{
				_transposedNotes.AddRange(notes);
			}
			Invalidate();
		}

		/// <summary>
		/// Clear all notes and selections
		/// </summary>
		public void ClearAllNotes()
		{
			_originalNotes.Clear();
			_transposedNotes.Clear();
			_selectedPositions.Clear();
			_selectedPosition = new FretPosition(1, 0);
			Invalidate();
		}

		/// <summary>
		/// Switch between display modes
		/// </summary>
		public void ToggleDisplayMode()
		{
			CurrentDisplayMode = CurrentDisplayMode switch
			{
				DisplayMode.Original => DisplayMode.Transposed,
				DisplayMode.Transposed => DisplayMode.Original,
				_ => DisplayMode.Original
			};
		}
		#endregion

		#region Painting
		protected override void OnPaint(PaintEventArgs e)
		{
			base.OnPaint(e);
			DrawGuitarNeck(e.Graphics);
		}

		private void DrawGuitarNeck(Graphics g)
		{
			// High quality rendering
			g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

			// Draw fretboard background
			using (var neckBrush = new SolidBrush(Color.FromArgb(139, 69, 19)))
			{
				g.FillRectangle(neckBrush, 0, 0, Width, Height);
			}

			// Draw frets (vertical lines)
			using (var fretPen = new Pen(Color.FromArgb(212, 175, 55), 3)) // Gold frets
			{
				for (int fret = 0; fret <= NUM_FRETS; fret++)
				{
					int x = NECK_OFFSET + (fret * FRET_WIDTH);
					g.DrawLine(fretPen, x, NECK_OFFSET, x, NECK_OFFSET + (NUM_STRINGS * STRING_HEIGHT));
				}
			}

			// Draw strings (horizontal lines)
			using (var stringPen = new Pen(Color.FromArgb(212, 175, 55), 2)) // Gold strings
			{
				for (int stringNum = 0; stringNum < NUM_STRINGS; stringNum++)
				{
					int y = NECK_OFFSET + (stringNum * STRING_HEIGHT) + (STRING_HEIGHT / 2);
					g.DrawLine(stringPen, NECK_OFFSET, y, NECK_OFFSET + (NUM_FRETS * FRET_WIDTH), y);
				}
			}

			// Draw fret markers
			DrawFretMarkers(g);

			// Draw notes based on display mode
			DrawNotes(g);

			// Draw selections
			DrawSelections(g);

			// Draw labels
			DrawLabels(g);

			// Draw display mode indicator
			DrawDisplayModeIndicator(g);
		}

		private void DrawFretMarkers(Graphics g)
		{
			int[] markerFrets = { 3, 5, 7, 9, 12 };
			using (var markerBrush = new SolidBrush(Color.FromArgb(212, 175, 55)))
			{
				foreach (int fret in markerFrets)
				{
					if (fret <= NUM_FRETS)
					{
						int x = NECK_OFFSET + (fret * FRET_WIDTH) - (FRET_WIDTH / 2);
						int y = NECK_OFFSET + (NUM_STRINGS * STRING_HEIGHT) + 10;
						g.FillEllipse(markerBrush, x - 4, y - 4, 8, 8);
					}
				}
			}
		}

		private void DrawNotes(Graphics g)
		{
			switch (CurrentDisplayMode)
			{
				case DisplayMode.Original:
					DrawNoteList(g, _originalNotes, Color.LightBlue, "Original");
					break;

				case DisplayMode.Transposed:
					DrawNoteList(g, _transposedNotes, Color.LightGreen, "Transposed");
					break;

				case DisplayMode.Both:
					DrawNoteList(g, _originalNotes, Color.LightBlue, "Original");
					DrawNoteList(g, _transposedNotes, Color.LightGreen, "Transposed");
					break;
			}
		}

		private void DrawNoteList(Graphics g, List<FretPosition> notes, Color color, string label)
		{
			using (var noteBrush = new SolidBrush(Color.FromArgb(180, color))) // Semi-transparent
			using (var borderPen = new Pen(Color.DarkGray, 2))
			using (var textBrush = new SolidBrush(Color.Black))
			using (var textFont = new Font("Arial", 8))
			{
				foreach (var pos in notes)
				{
					DrawFretHighlight(g, pos.String, pos.Fret, noteBrush, borderPen);

					// Draw note label
					int x = NECK_OFFSET + (pos.Fret * FRET_WIDTH) - (FRET_WIDTH / 2) + 8;
					int y = NECK_OFFSET + ((pos.String - 1) * STRING_HEIGHT) + 8;
					g.DrawString(label.Substring(0, 3), textFont, textBrush, x, y);
				}
			}
		}

		private void DrawSelections(Graphics g)
		{
			// Draw multi-selected positions
			using (var selectedBrush = new SolidBrush(Color.FromArgb(150, 255, 255, 0))) // Yellow with transparency
			using (var selectedBorderPen = new Pen(Color.Orange, 2))
			{
				foreach (var pos in _selectedPositions)
				{
					DrawFretHighlight(g, pos.String, pos.Fret, selectedBrush, selectedBorderPen);
				}
			}

			// Draw current selection (only if not showing both modes and not in multi-select)
			if (_selectedPosition != null && CurrentDisplayMode != DisplayMode.Both && !_multiSelect)
			{
				using (var currentBrush = new SolidBrush(Color.Red))
				using (var currentBorderPen = new Pen(Color.DarkRed, 3))
				{
					DrawFretHighlight(g, _selectedPosition.String, _selectedPosition.Fret, currentBrush, currentBorderPen);
				}
			}
		}

		private void DrawLabels(Graphics g)
		{
			// String labels (E, B, G, D, A, E)
			using (var labelBrush = new SolidBrush(Color.White))
			using (var labelFont = new Font("Arial", 10, FontStyle.Bold))
			{
				string[] stringLabels = { "E", "B", "G", "D", "A", "E" };
				for (int stringNum = 0; stringNum < NUM_STRINGS; stringNum++)
				{
					int y = NECK_OFFSET + (stringNum * STRING_HEIGHT) + (STRING_HEIGHT / 2);
					g.DrawString(stringLabels[stringNum], labelFont, labelBrush, 5, y - 8);
				}
			}

			// Fret numbers
			using (var fretLabelBrush = new SolidBrush(Color.White))
			using (var fretLabelFont = new Font("Arial", 8))
			{
				for (int fret = 0; fret <= NUM_FRETS; fret++)
				{
					int x = NECK_OFFSET + (fret * FRET_WIDTH) - 8;
					g.DrawString(fret.ToString(), fretLabelFont, fretLabelBrush, x, NECK_OFFSET - 20);
				}
			}
		}

		private void DrawDisplayModeIndicator(Graphics g)
		{
			// Draw current mode indicator
			string modeText = CurrentDisplayMode switch
			{
				DisplayMode.Original => "ORIGINAL NOTES",
				DisplayMode.Transposed => "TRANSPOSED NOTES",
				DisplayMode.Both => "BOTH MODES",
				_ => "UNKNOWN"
			};

			using (var modeBrush = new SolidBrush(Color.White))
			using (var modeFont = new Font("Arial", 10, FontStyle.Bold))
			{
				SizeF textSize = g.MeasureString(modeText, modeFont);
				int x = Width - (int)textSize.Width - 10;
				int y = 5;

				// Draw background rectangle
				using (var bgBrush = new SolidBrush(Color.FromArgb(200, Color.DarkBlue)))
				{
					g.FillRectangle(bgBrush, x - 5, y - 2, (int)textSize.Width + 10, (int)textSize.Height + 4);
				}

				g.DrawString(modeText, modeFont, modeBrush, x, y);
			}
		}

		private void DrawFretHighlight(Graphics g, int stringNum, int fret, Brush brush, Pen borderPen = null)
		{
			if (stringNum >= 1 && stringNum <= NUM_STRINGS && fret >= 0 && fret <= NUM_FRETS)
			{
				int x = NECK_OFFSET + (fret * FRET_WIDTH) - (FRET_WIDTH / 2) + 5;
				int y = NECK_OFFSET + ((stringNum - 1) * STRING_HEIGHT) + 5;

				// Fill ellipse
				g.FillEllipse(brush, x, y, FRET_WIDTH - 10, STRING_HEIGHT - 10);

				// Draw border if provided
				if (borderPen != null)
				{
					g.DrawEllipse(borderPen, x, y, FRET_WIDTH - 10, STRING_HEIGHT - 10);
				}
			}
		}
		#endregion

		#region Mouse Handling
		protected override void OnMouseClick(MouseEventArgs e)
		{
			base.OnMouseClick(e);

			var clickedPosition = GetClickedPosition(e.Location);
			if (clickedPosition.String >= 1 && clickedPosition.String <= NUM_STRINGS &&
				clickedPosition.Fret >= 0 && clickedPosition.Fret <= NUM_FRETS)
			{
				HandleFretClick(clickedPosition);
			}
		}

		private void HandleFretClick(FretPosition clickedPosition)
		{
			if (_multiSelect)
			{
				if (_selectedPositions.Contains(clickedPosition))
				{
					_selectedPositions.Remove(clickedPosition);
					FretDeselected?.Invoke(this, new FretPositionEventArgs(clickedPosition));
				}
				else
				{
					_selectedPositions.Add(clickedPosition);
					FretSelected?.Invoke(this, new FretPositionEventArgs(clickedPosition));
				}
			}
			else
			{
				_selectedPosition = clickedPosition;
				FretSelected?.Invoke(this, new FretPositionEventArgs(clickedPosition));
			}

			Invalidate();
		}

		public FretPosition GetClickedPosition(Point location)
		{
			int fret = (location.X - NECK_OFFSET + (FRET_WIDTH / 2)) / FRET_WIDTH;
			int stringNum = (location.Y - NECK_OFFSET + (STRING_HEIGHT / 2)) / STRING_HEIGHT + 1;

			if (fret >= 0 && fret <= NUM_FRETS && stringNum >= 1 && stringNum <= NUM_STRINGS)
			{
				return new FretPosition(stringNum, fret);
			}
			return new FretPosition(1, 0);
		}
		#endregion

		#region Public Methods
		public void ClearSelection()
		{
			_selectedPositions.Clear();
			_selectedPosition = new FretPosition(1, 0);
			Invalidate();
		}

		public void SelectPosition(int stringNum, int fret)
		{
			if (stringNum >= 1 && stringNum <= NUM_STRINGS && fret >= 0 && fret <= NUM_FRETS)
			{
				var position = new FretPosition(stringNum, fret);

				if (_multiSelect)
				{
					if (!_selectedPositions.Contains(position))
					{
						_selectedPositions.Add(position);
					}
				}
				else
				{
					_selectedPosition = position;
				}

				Invalidate();
			}
		}
		#endregion
	}
}
