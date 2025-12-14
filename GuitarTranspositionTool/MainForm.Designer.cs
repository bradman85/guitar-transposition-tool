using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Drawing;

namespace GuitarTranspositionTool
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        // Control declarations - only declare once
        private System.Windows.Forms.NumericUpDown numericString;
        private System.Windows.Forms.NumericUpDown numericFret;
        private System.Windows.Forms.Button btnAddNote;
        private System.Windows.Forms.Button btnTranspose;
        private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.ListBox listBoxInput;
        private System.Windows.Forms.ListBox listBoxOutput;
        private System.Windows.Forms.Label labelStats;
        private System.Windows.Forms.Label labelInput;
        private System.Windows.Forms.Label labelOutput;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.numericString = new System.Windows.Forms.NumericUpDown();
            this.numericFret = new System.Windows.Forms.NumericUpDown();
            this.btnAddNote = new System.Windows.Forms.Button();
            this.btnTranspose = new System.Windows.Forms.Button();
            this.btnClear = new System.Windows.Forms.Button();
            this.listBoxInput = new System.Windows.Forms.ListBox();
            this.listBoxOutput = new System.Windows.Forms.ListBox();
            this.labelStats = new System.Windows.Forms.Label();
            this.labelInput = new System.Windows.Forms.Label();
            this.labelOutput = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.numericString)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericFret)).BeginInit();
            this.SuspendLayout();
            
            // String selection
            this.numericString.Location = new System.Drawing.Point(20, 20);
            this.numericString.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            this.numericString.Maximum = new decimal(new int[] { 6, 0, 0, 0 });
            this.numericString.Value = new decimal(new int[] { 1, 0, 0, 0 });
            this.numericString.Size = new System.Drawing.Size(60, 27);
            
            // Fret selection
            this.numericFret.Location = new System.Drawing.Point(100, 20);
            this.numericFret.Minimum = new decimal(new int[] { 0, 0, 0, 0 });
            this.numericFret.Maximum = new decimal(new int[] { 24, 0, 0, 0 });
            this.numericFret.Value = new decimal(new int[] { 12, 0, 0, 0 });
            this.numericFret.Size = new System.Drawing.Size(60, 27);
            
            // Add Note button
            this.btnAddNote.Location = new System.Drawing.Point(180, 20);
            this.btnAddNote.Size = new System.Drawing.Size(100, 30);
            this.btnAddNote.Text = "Add Note";
            this.btnAddNote.UseVisualStyleBackColor = true;
            
            // Transpose button
            this.btnTranspose.Location = new System.Drawing.Point(300, 20);
            this.btnTranspose.Size = new System.Drawing.Size(120, 30);
            this.btnTranspose.Text = "Transpose";
            this.btnTranspose.UseVisualStyleBackColor = true;
            
            // Clear button
            this.btnClear.Location = new System.Drawing.Point(440, 20);
            this.btnClear.Size = new System.Drawing.Size(100, 30);
            this.btnClear.Text = "Clear All";
            this.btnClear.UseVisualStyleBackColor = true;
            
            // Input list label
            this.labelInput.AutoSize = true;
            this.labelInput.Location = new System.Drawing.Point(20, 70);
            this.labelInput.Text = "Original Notes:";
            
            // Input list
            this.listBoxInput.FormattingEnabled = true;
            this.listBoxInput.ItemHeight = 20;
            this.listBoxInput.Location = new System.Drawing.Point(20, 100);
            this.listBoxInput.Size = new System.Drawing.Size(300, 200);
            
            // Output list label
            this.labelOutput.AutoSize = true;
            this.labelOutput.Location = new System.Drawing.Point(340, 70);
            this.labelOutput.Text = "Transposed Notes:";
            
            // Output list
            this.listBoxOutput.FormattingEnabled = true;
            this.listBoxOutput.ItemHeight = 20;
            this.listBoxOutput.Location = new System.Drawing.Point(340, 100);
            this.listBoxOutput.Size = new System.Drawing.Size(300, 200);
            
            // Statistics label
            this.labelStats.AutoSize = true;
            this.labelStats.Location = new System.Drawing.Point(20, 320);
            this.labelStats.Size = new System.Drawing.Size(600, 100);
            this.labelStats.Text = "Enter notes and click Transpose to see results.";
            
            // Form settings
            this.ClientSize = new System.Drawing.Size(660, 450);
            this.Controls.Add(this.numericString);
            this.Controls.Add(this.numericFret);
            this.Controls.Add(this.btnAddNote);
            this.Controls.Add(this.btnTranspose);
            this.Controls.Add(this.btnClear);
            this.Controls.Add(this.listBoxInput);
            this.Controls.Add(this.listBoxOutput);
            this.Controls.Add(this.labelStats);
            this.Controls.Add(this.labelInput);
            this.Controls.Add(this.labelOutput);
            this.Text = "Guitar Transposition Tool";
            ((System.ComponentModel.ISupportInitialize)(this.numericString)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericFret)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion
    }
}
