using System.Numerics;
using System.Windows.Forms;

namespace ThreeDimensionalControlsTests {
    public partial class MainForm : Form {
        public MainForm() {
            InitializeComponent();
        }

        private void TrackBall_ValueChanged(object sender, ThreeDimensionalControls.TrackBallRolledEventArgs tre) {
            Quaternion quaternion = tre.Quaternion;
            
            labelState.Text = quaternion.ToString();
            trackBarW.Value = (int)(quaternion.W * 100);
            trackBarX.Value = (int)(quaternion.X * 100);
            trackBarY.Value = (int)(quaternion.Y * 100);
            trackBarZ.Value = (int)(quaternion.Z * 100);
        }

        private void TrackBar_Scroll(object sender, System.EventArgs e) {
            Quaternion quaternion = new(
                trackBarX.Value * 0.01f, trackBarY.Value * 0.01f, trackBarZ.Value * 0.01f, trackBarW.Value * 0.01f
            );

            trackBall.Quaternion = quaternion;

            labelState.Text = trackBall.Quaternion.ToString();
        }
    }
}
