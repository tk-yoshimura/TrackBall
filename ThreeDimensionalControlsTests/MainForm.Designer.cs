
namespace ThreeDimensionalControlsTests {
    partial class MainForm {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            this.trackBall1 = new ThreeDimensionalControls.TrackBall();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // trackBall1
            // 
            this.trackBall1.Location = new System.Drawing.Point(171, 12);
            this.trackBall1.Name = "trackBall1";
            this.trackBall1.Size = new System.Drawing.Size(150, 150);
            this.trackBall1.TabIndex = 0;
            this.trackBall1.ValueChanged += new ThreeDimensionalControls.TrackBallRolledEventHandler(this.TrackBall1_ValueChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 187);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(0, 15);
            this.label1.TabIndex = 1;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(501, 214);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.trackBall1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.Text = "MainForm";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private ThreeDimensionalControls.TrackBall trackBall1;
        private System.Windows.Forms.Label label1;
    }
}

