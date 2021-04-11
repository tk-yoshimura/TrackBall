
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
            this.trackBall = new ThreeDimensionalControls.TrackBall();
            this.labelState = new System.Windows.Forms.Label();
            this.trackBarW = new System.Windows.Forms.TrackBar();
            this.trackBarX = new System.Windows.Forms.TrackBar();
            this.trackBarY = new System.Windows.Forms.TrackBar();
            this.trackBarZ = new System.Windows.Forms.TrackBar();
            this.labelW = new System.Windows.Forms.Label();
            this.labelX = new System.Windows.Forms.Label();
            this.labelY = new System.Windows.Forms.Label();
            this.labelZ = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarW)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarX)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarY)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarZ)).BeginInit();
            this.SuspendLayout();
            // 
            // trackBall
            // 
            this.trackBall.Location = new System.Drawing.Point(13, 12);
            this.trackBall.Name = "trackBall";
            this.trackBall.Size = new System.Drawing.Size(150, 150);
            this.trackBall.TabIndex = 0;
            this.trackBall.ValueChanged += new ThreeDimensionalControls.TrackBallRolledEventHandler(this.TrackBall_ValueChanged);
            // 
            // labelState
            // 
            this.labelState.AutoSize = true;
            this.labelState.Location = new System.Drawing.Point(13, 187);
            this.labelState.Name = "labelState";
            this.labelState.Size = new System.Drawing.Size(0, 15);
            this.labelState.TabIndex = 1;
            // 
            // trackBarW
            // 
            this.trackBarW.Location = new System.Drawing.Point(213, 12);
            this.trackBarW.Maximum = 100;
            this.trackBarW.Minimum = -100;
            this.trackBarW.Name = "trackBarW";
            this.trackBarW.Size = new System.Drawing.Size(166, 45);
            this.trackBarW.TabIndex = 2;
            this.trackBarW.TickFrequency = 10;
            this.trackBarW.Scroll += new System.EventHandler(this.TrackBar_Scroll);
            // 
            // trackBarX
            // 
            this.trackBarX.Location = new System.Drawing.Point(213, 55);
            this.trackBarX.Maximum = 100;
            this.trackBarX.Minimum = -100;
            this.trackBarX.Name = "trackBarX";
            this.trackBarX.Size = new System.Drawing.Size(166, 45);
            this.trackBarX.TabIndex = 3;
            this.trackBarX.TickFrequency = 10;
            this.trackBarX.Scroll += new System.EventHandler(this.TrackBar_Scroll);
            // 
            // trackBarY
            // 
            this.trackBarY.Location = new System.Drawing.Point(213, 98);
            this.trackBarY.Maximum = 100;
            this.trackBarY.Minimum = -100;
            this.trackBarY.Name = "trackBarY";
            this.trackBarY.Size = new System.Drawing.Size(166, 45);
            this.trackBarY.TabIndex = 4;
            this.trackBarY.TickFrequency = 10;
            this.trackBarY.Scroll += new System.EventHandler(this.TrackBar_Scroll);
            // 
            // trackBarZ
            // 
            this.trackBarZ.Location = new System.Drawing.Point(213, 141);
            this.trackBarZ.Maximum = 100;
            this.trackBarZ.Minimum = -100;
            this.trackBarZ.Name = "trackBarZ";
            this.trackBarZ.Size = new System.Drawing.Size(166, 45);
            this.trackBarZ.TabIndex = 5;
            this.trackBarZ.TickFrequency = 10;
            this.trackBarZ.Scroll += new System.EventHandler(this.TrackBar_Scroll);
            // 
            // labelW
            // 
            this.labelW.AutoSize = true;
            this.labelW.Location = new System.Drawing.Point(203, 12);
            this.labelW.Name = "labelW";
            this.labelW.Size = new System.Drawing.Size(18, 15);
            this.labelW.TabIndex = 6;
            this.labelW.Text = "W";
            // 
            // labelX
            // 
            this.labelX.AutoSize = true;
            this.labelX.Location = new System.Drawing.Point(203, 55);
            this.labelX.Name = "labelX";
            this.labelX.Size = new System.Drawing.Size(14, 15);
            this.labelX.TabIndex = 7;
            this.labelX.Text = "X";
            // 
            // labelY
            // 
            this.labelY.AutoSize = true;
            this.labelY.Location = new System.Drawing.Point(203, 98);
            this.labelY.Name = "labelY";
            this.labelY.Size = new System.Drawing.Size(14, 15);
            this.labelY.TabIndex = 8;
            this.labelY.Text = "Y";
            // 
            // labelZ
            // 
            this.labelZ.AutoSize = true;
            this.labelZ.Location = new System.Drawing.Point(203, 141);
            this.labelZ.Name = "labelZ";
            this.labelZ.Size = new System.Drawing.Size(14, 15);
            this.labelZ.TabIndex = 9;
            this.labelZ.Text = "Z";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(386, 214);
            this.Controls.Add(this.labelZ);
            this.Controls.Add(this.labelY);
            this.Controls.Add(this.labelX);
            this.Controls.Add(this.labelW);
            this.Controls.Add(this.trackBarZ);
            this.Controls.Add(this.trackBarY);
            this.Controls.Add(this.trackBarX);
            this.Controls.Add(this.trackBarW);
            this.Controls.Add(this.labelState);
            this.Controls.Add(this.trackBall);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.Text = "MainForm";
            ((System.ComponentModel.ISupportInitialize)(this.trackBarW)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarX)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarY)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarZ)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private ThreeDimensionalControls.TrackBall trackBall;
        private System.Windows.Forms.Label labelState;
        private System.Windows.Forms.TrackBar trackBarW;
        private System.Windows.Forms.TrackBar trackBarX;
        private System.Windows.Forms.TrackBar trackBarY;
        private System.Windows.Forms.TrackBar trackBarZ;
        private System.Windows.Forms.Label labelW;
        private System.Windows.Forms.Label labelX;
        private System.Windows.Forms.Label labelY;
        private System.Windows.Forms.Label labelZ;
    }
}

