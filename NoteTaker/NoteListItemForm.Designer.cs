﻿namespace NoteTaker
{
    partial class NoteListItemForm
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
            this.checkBoxIsSelected = new System.Windows.Forms.CheckBox();
            this.textBoxContent = new System.Windows.Forms.TextBox();
            this.pictureBox = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // checkBoxIsSelected
            // 
            this.checkBoxIsSelected.AutoSize = true;
            this.checkBoxIsSelected.Location = new System.Drawing.Point(3, 3);
            this.checkBoxIsSelected.Name = "checkBoxIsSelected";
            this.checkBoxIsSelected.Size = new System.Drawing.Size(15, 14);
            this.checkBoxIsSelected.TabIndex = 0;
            this.checkBoxIsSelected.UseVisualStyleBackColor = true;
            // 
            // textBoxContent
            // 
            this.textBoxContent.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxContent.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBoxContent.Location = new System.Drawing.Point(21, 3);
            this.textBoxContent.Name = "textBoxContent";
            this.textBoxContent.Size = new System.Drawing.Size(340, 13);
            this.textBoxContent.TabIndex = 1;
            // 
            // pictureBox
            // 
            this.pictureBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureBox.Location = new System.Drawing.Point(366, 2);
            this.pictureBox.Name = "pictureBox";
            this.pictureBox.Size = new System.Drawing.Size(16, 16);
            this.pictureBox.TabIndex = 2;
            this.pictureBox.TabStop = false;
            // 
            // NoteListItemForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Window;
            this.ClientSize = new System.Drawing.Size(385, 19);
            this.Controls.Add(this.pictureBox);
            this.Controls.Add(this.textBoxContent);
            this.Controls.Add(this.checkBoxIsSelected);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "NoteListItemForm";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "NoteListItemForm";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox checkBoxIsSelected;
        private System.Windows.Forms.TextBox textBoxContent;
        private System.Windows.Forms.PictureBox pictureBox;
    }
}