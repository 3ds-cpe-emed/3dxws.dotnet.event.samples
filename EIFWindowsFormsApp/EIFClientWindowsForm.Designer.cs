//------------------------------------------------------------------------------------------------------------------------------------
// Copyright 2022 Dassault Systèmes - CPE EMED
//
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation
// files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify,
// merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished
// to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES 
// OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS
// BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//------------------------------------------------------------------------------------------------------------------------------------

namespace EIFWindowsFormsApp
{
   partial class EIFClientWindowsForm
   {
      /// <summary>
      /// Required designer variable.
      /// </summary>
      private System.ComponentModel.IContainer components = null;

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
         this.m_backgroundWorker = new System.ComponentModel.BackgroundWorker();
         this.m_startButton = new System.Windows.Forms.Button();
         this.m_stopButton = new System.Windows.Forms.Button();
         this.m_textBox = new System.Windows.Forms.TextBox();
         this.SuspendLayout();
         // 
         // m_backgroundWorker
         // 
         this.m_backgroundWorker.WorkerSupportsCancellation = true;
         this.m_backgroundWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.m_backgroundWorker_DoWork);
         this.m_backgroundWorker.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.m_backgroundWorker_RunWorkerCompleted);
         // 
         // m_startButton
         // 
         this.m_startButton.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
         this.m_startButton.Location = new System.Drawing.Point(12, 25);
         this.m_startButton.Name = "m_startButton";
         this.m_startButton.Size = new System.Drawing.Size(826, 37);
         this.m_startButton.TabIndex = 0;
         this.m_startButton.Text = "Start";
         this.m_startButton.UseVisualStyleBackColor = true;
         this.m_startButton.Click += new System.EventHandler(this.m_startButton_Click);
         // 
         // m_stopButton
         // 
         this.m_stopButton.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
         this.m_stopButton.Enabled = false;
         this.m_stopButton.Location = new System.Drawing.Point(12, 81);
         this.m_stopButton.Name = "m_stopButton";
         this.m_stopButton.Size = new System.Drawing.Size(826, 38);
         this.m_stopButton.TabIndex = 1;
         this.m_stopButton.Text = "Stop";
         this.m_stopButton.UseVisualStyleBackColor = true;
         this.m_stopButton.Click += new System.EventHandler(this.m_stopButton_Click);
         // 
         // m_textBox
         // 
         this.m_textBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
         this.m_textBox.Location = new System.Drawing.Point(12, 138);
         this.m_textBox.Multiline = true;
         this.m_textBox.Name = "m_textBox";
         this.m_textBox.ReadOnly = true;
         this.m_textBox.Size = new System.Drawing.Size(826, 294);
         this.m_textBox.TabIndex = 2;
         // 
         // EIFWindowsForm
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.ClientSize = new System.Drawing.Size(850, 444);
         this.Controls.Add(this.m_textBox);
         this.Controls.Add(this.m_stopButton);
         this.Controls.Add(this.m_startButton);
         this.MaximizeBox = false;
         this.MinimizeBox = false;
         this.Name = "EIFWindowsForm";
         this.Text = "EIF Client Windows Form Example";
         this.ResumeLayout(false);
         this.PerformLayout();

      }

      #endregion

      private System.ComponentModel.BackgroundWorker m_backgroundWorker;
      private System.Windows.Forms.Button m_startButton;
      private System.Windows.Forms.Button m_stopButton;
      private System.Windows.Forms.TextBox m_textBox;
   }
}

