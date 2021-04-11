using System.Windows.Forms;

namespace ThreeDimensionalControlsTests {
    public partial class MainForm : Form {
        public MainForm() {
            InitializeComponent();
        }

        private void TrackBall1_ValueChanged(object sender, ThreeDimensionalControls.TrackBallRolledEventArgs tre) {
            label1.Text = tre.Quaternion.ToString();
        }
    }
}
