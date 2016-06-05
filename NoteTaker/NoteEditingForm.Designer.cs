namespace NoteTaker
{
    partial class NoteEditingForm
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
            this.label1 = new System.Windows.Forms.Label();
            this.checkBoxIsSelected = new System.Windows.Forms.CheckBox();
            this.textBoxContent = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.radioButtonHigh = new System.Windows.Forms.RadioButton();
            this.radioButtonNormal = new System.Windows.Forms.RadioButton();
            this.radioButtonLow = new System.Windows.Forms.RadioButton();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(44, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Content";
            // 
            // checkBoxIsSelected
            // 
            this.checkBoxIsSelected.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.checkBoxIsSelected.AutoSize = true;
            this.checkBoxIsSelected.Location = new System.Drawing.Point(384, 32);
            this.checkBoxIsSelected.Name = "checkBoxIsSelected";
            this.checkBoxIsSelected.Size = new System.Drawing.Size(68, 17);
            this.checkBoxIsSelected.TabIndex = 1;
            this.checkBoxIsSelected.Text = "Selected";
            this.checkBoxIsSelected.UseVisualStyleBackColor = true;
            // 
            // textBoxContent
            // 
            this.textBoxContent.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxContent.Location = new System.Drawing.Point(62, 6);
            this.textBoxContent.Name = "textBoxContent";
            this.textBoxContent.Size = new System.Drawing.Size(390, 20);
            this.textBoxContent.TabIndex = 2;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 36);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(38, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Priority";
            // 
            // radioButtonHigh
            // 
            this.radioButtonHigh.AutoSize = true;
            this.radioButtonHigh.Location = new System.Drawing.Point(62, 34);
            this.radioButtonHigh.Name = "radioButtonHigh";
            this.radioButtonHigh.Size = new System.Drawing.Size(47, 17);
            this.radioButtonHigh.TabIndex = 4;
            this.radioButtonHigh.TabStop = true;
            this.radioButtonHigh.Text = "High";
            this.radioButtonHigh.UseVisualStyleBackColor = true;
            // 
            // radioButtonNormal
            // 
            this.radioButtonNormal.AutoSize = true;
            this.radioButtonNormal.Location = new System.Drawing.Point(62, 55);
            this.radioButtonNormal.Name = "radioButtonNormal";
            this.radioButtonNormal.Size = new System.Drawing.Size(58, 17);
            this.radioButtonNormal.TabIndex = 5;
            this.radioButtonNormal.TabStop = true;
            this.radioButtonNormal.Text = "Normal";
            this.radioButtonNormal.UseVisualStyleBackColor = true;
            // 
            // radioButtonLow
            // 
            this.radioButtonLow.AutoSize = true;
            this.radioButtonLow.Location = new System.Drawing.Point(62, 76);
            this.radioButtonLow.Name = "radioButtonLow";
            this.radioButtonLow.Size = new System.Drawing.Size(45, 17);
            this.radioButtonLow.TabIndex = 6;
            this.radioButtonLow.TabStop = true;
            this.radioButtonLow.Text = "Low";
            this.radioButtonLow.UseVisualStyleBackColor = true;
            // 
            // NoteEditingForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Window;
            this.ClientSize = new System.Drawing.Size(464, 112);
            this.Controls.Add(this.radioButtonLow);
            this.Controls.Add(this.radioButtonNormal);
            this.Controls.Add(this.radioButtonHigh);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.textBoxContent);
            this.Controls.Add(this.checkBoxIsSelected);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "NoteEditingForm";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "NoteEditingForm";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox checkBoxIsSelected;
        private System.Windows.Forms.TextBox textBoxContent;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.RadioButton radioButtonHigh;
        private System.Windows.Forms.RadioButton radioButtonNormal;
        private System.Windows.Forms.RadioButton radioButtonLow;
    }
}