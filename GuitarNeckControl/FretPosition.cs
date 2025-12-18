using System;

namespace GuitarNeckControl
{
	/// <summary>
	/// Represents a position on the guitar fretboard
	/// </summary>
	public class FretPosition
	{
		public int String { get; set; }
		public int Fret { get; set; }

		public FretPosition(int stringNum, int fret)
		{
			String = stringNum;
			Fret = fret;
		}

		public override bool Equals(object obj)
		{
			if (obj is FretPosition other)
				return String == other.String && Fret == other.Fret;
			return false;
		}

		public override int GetHashCode()
		{
			// Simple but effective hash for .NET 6.0
			return (String.ToString() + "|" + Fret.ToString()).GetHashCode();
		}
	}

	/// <summary>
	/// Event arguments for fret selection events
	/// </summary>
	public class FretPositionEventArgs : EventArgs
	{
		public FretPosition Position { get; }

		public FretPositionEventArgs(FretPosition position)
		{
			Position = position;
		}
	}

	/// <summary>
	/// Event arguments for display mode changed events
	/// </summary>
	public class DisplayModeChangedEventArgs : EventArgs
	{
		public DisplayMode NewMode { get; }

		public DisplayModeChangedEventArgs(DisplayMode newMode)
		{
			NewMode = newMode;
		}
	}
}
