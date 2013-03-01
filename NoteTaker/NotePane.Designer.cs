namespace NoteTaker
{
    partial class NotePane
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
            this.textBoxNoteText = new System.Windows.Forms.TextBox();
            this.checkBoxSelected = new System.Windows.Forms.CheckBox();
            this.radioButtonPriorityHigh = new System.Windows.Forms.RadioButton();
            this.radioButtonPriorityNormal = new System.Windows.Forms.RadioButton();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.radioButtonPriorityLow = new System.Windows.Forms.RadioButton();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // textBoxNoteText
            // 
            this.textBoxNoteText.Location = new System.Drawing.Point(33, 9);
            this.textBoxNoteText.Multiline = true;
            this.textBoxNoteText.Name = "textBoxNoteText";
            this.textBoxNoteText.Size = new System.Drawing.Size(262, 99);
            this.textBoxNoteText.TabIndex = 0;
            // 
            // checkBoxSelected
            // 
            this.checkBoxSelected.AutoSize = true;
            this.checkBoxSelected.Location = new System.Drawing.Point(12, 11);
            this.checkBoxSelected.Name = "checkBoxSelected";
            this.checkBoxSelected.Size = new System.Drawing.Size(15, 14);
            this.checkBoxSelected.TabIndex = 1;
            this.checkBoxSelected.UseVisualStyleBackColor = true;
            // 
            // radioButtonPriorityHigh
            // 
            this.radioButtonPriorityHigh.AutoSize = true;
            this.radioButtonPriorityHigh.Location = new System.Drawing.Point(6, 19);
            this.radioButtonPriorityHigh.Name = "radioButtonPriorityHigh";
            this.radioButtonPriorityHigh.Size = new System.Drawing.Size(47, 17);
            this.radioButtonPriorityHigh.TabIndex = 2;
            this.radioButtonPriorityHigh.TabStop = true;
            this.radioButtonPriorityHigh.Text = "High";
            this.radioButtonPriorityHigh.UseVisualStyleBackColor = true;
            // 
            // radioButtonPriorityNormal
            // 
            this.radioButtonPriorityNormal.AutoSize = true;
            this.radioButtonPriorityNormal.Location = new System.Drawing.Point(6, 42);
            this.radioButtonPriorityNormal.Name = "radioButtonPriorityNormal";
            this.radioButtonPriorityNormal.Size = new System.Drawing.Size(58, 17);
            this.radioButtonPriorityNormal.TabIndex = 3;
            this.radioButtonPriorityNormal.TabStop = true;
            this.radioButtonPriorityNormal.Text = "Normal";
            this.radioButtonPriorityNormal.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.radioButtonPriorityLow);
            this.groupBox1.Controls.Add(this.radioButtonPriorityNormal);
            this.groupBox1.Controls.Add(this.radioButtonPriorityHigh);
            this.groupBox1.Location = new System.Drawing.Point(301, 9);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(138, 99);
            this.groupBox1.TabIndex = 4;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Priority";
            // 
            // radioButtonPriorityLow
            // 
            this.radioButtonPriorityLow.AutoSize = true;
            this.radioButtonPriorityLow.Location = new System.Drawing.Point(6, 66);
            this.radioButtonPriorityLow.Name = "radioButtonPriorityLow";
            this.radioButtonPriorityLow.Size = new System.Drawing.Size(45, 17);
            this.radioButtonPriorityLow.TabIndex = 4;
            this.radioButtonPriorityLow.TabStop = true;
            this.radioButtonPriorityLow.Text = "Low";
            this.radioButtonPriorityLow.UseVisualStyleBackColor = true;
            // 
            // NotePane
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Window;
            this.ClientSize = new System.Drawing.Size(448, 117);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.checkBoxSelected);
            this.Controls.Add(this.textBoxNoteText);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "NotePane";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "NotePane";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBoxNoteText;
        private System.Windows.Forms.CheckBox checkBoxSelected;
        private System.Windows.Forms.RadioButton radioButtonPriorityHigh;
        private System.Windows.Forms.RadioButton radioButtonPriorityNormal;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton radioButtonPriorityLow;
    }
}