using System;
using System.Drawing;
using System.Numerics;
using System.Windows.Forms;

// Copyright (c) T.Yoshimura 2019-2024
// https://github.com/tk-yoshimura

namespace ThreeDimensionalControls {

    public partial class TrackBall : UserControl {
        bool is_manipulate = false;
        int pic_size;
        double quat_r = 1, quat_i = 0, quat_j = 0, quat_k = 0;
        double init_x, init_y, init_z;

        Point panel_pos, ball_pos;
        Size panel_size, ball_size;

        public TrackBall() {
            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            DrawImage();
        }

        public event TrackBallRolledEventHandler ValueChanged;

        public Quaternion Value {
            get {
                return new(new Vector3((float)quat_i, (float)quat_j, (float)quat_k), (float)quat_r);
            }
            set {
                quat_r = value.W / value.Length();
                quat_i = value.X / value.Length();
                quat_j = value.Y / value.Length();
                quat_k = value.Z / value.Length();

                if (double.IsNaN(quat_r)) {
                    Reset();
                }
                else {
                    DrawBall();
                }
            }
        }

        protected void Roll(double r, double i, double j, double k) {
            double nr, ni, nj, nk, norm, inv_norm;

            nr = r * quat_r - i * quat_i - j * quat_j - k * quat_k;
            ni = r * quat_i + i * quat_r + j * quat_k - k * quat_j;
            nj = r * quat_j - i * quat_k + j * quat_r + k * quat_i;
            nk = r * quat_k + i * quat_j - j * quat_i + k * quat_r;

            norm = Math.Sqrt(nr * nr + ni * ni + nj * nj + nk * nk);

            if (!double.IsNaN(norm)) {
                inv_norm = 1.0 / norm;

                quat_r = nr * inv_norm;
                quat_i = ni * inv_norm;
                quat_j = nj * inv_norm;
                quat_k = nk * inv_norm;

                DrawBall();
            }
            else {
                Reset();
            }

            ValueChanged?.Invoke(this, new TrackBallRolledEventArgs(Value));
        }

        protected void Roll(double dx, double dy) {
            double dz, norm_sq;
            double nx, ny, nz;
            double len, theta, c, s;

            norm_sq = dx * dx + dy * dy;

            if (norm_sq <= 1) {
                dz = Math.Sqrt(1 - norm_sq);

                TransformCoord(0, 0, 1, out double prev_x, out double prev_y, out double prev_z);
                TransformCoord(dx, dy, dz, out double next_x, out double next_y, out double next_z);

                nx = next_y * prev_z - next_z * prev_y;
                ny = next_z * prev_x - next_x * prev_z;
                nz = next_x * prev_y - next_y * prev_x;

                len = Math.Sqrt(nx * nx + ny * ny + nz * nz);
                theta = Math.Acos(prev_x * next_x + prev_y * next_y + prev_z * next_z);

                c = Math.Cos(theta / 2);
                s = Math.Sin(theta / 2) / len;

                if (double.IsNaN(c) || double.IsNaN(s)) {
                    return;
                }

                Roll(c, nx * s, ny * s, nz * s);
            }
        }

        protected void Reset() {
            is_manipulate = false;

            quat_r = 1;
            quat_i = 0;
            quat_j = 0;
            quat_k = 0;

            DrawBall();
        }

        private void AcceptManipulateBall(MouseEventArgs e) {
            double dx, dy, dz, norm_sq, center = (ball.Width - 1) * 0.5, inv_center = 1.0 / center;
            double nx, ny, nz;
            double len, theta, c, s;

            dx = (e.X - ball_pos.X - center) * inv_center;
            dy = (e.Y - ball_pos.Y - center) * inv_center;
            norm_sq = dx * dx + dy * dy;

            if (norm_sq <= 1) {
                dz = Math.Sqrt(1 - norm_sq);

                TransformCoord(dx, dy, dz, out double move_x, out double move_y, out double move_z);

                nx = move_y * init_z - move_z * init_y;
                ny = move_z * init_x - move_x * init_z;
                nz = move_x * init_y - move_y * init_x;

                len = Math.Sqrt(nx * nx + ny * ny + nz * nz);
                theta = Math.Acos(init_x * move_x + init_y * move_y + init_z * move_z);

                c = Math.Cos(theta / 2);
                s = Math.Sin(theta / 2) / len;

                if (double.IsNaN(c) || double.IsNaN(s)) {
                    return;
                }

                Roll(c, nx * s, ny * s, nz * s);
            }
        }

        private bool IsBallArea(int x, int y) {
            if (!IsValidSize()) {
                return false;
            }

            double dx = x - ball_pos.X - ball.Width * 0.5, dy = y - ball_pos.Y - ball.Height * 0.5, norm = Math.Sqrt(dx * dx + dy * dy);
            return (norm <= ball.Width);
        }

        private bool IsValidSize() {
            return pic_size >= 50;
        }

        private void TransformCoord(double dx, double dy, double dz, out double x, out double y, out double z) {
            double r, i, j, k;

            r = -quat_i * dx - quat_j * dy - quat_k * dz;
            i = +quat_r * dx - quat_k * dy + quat_j * dz;
            j = +quat_k * dx + quat_r * dy - quat_i * dz;
            k = -quat_j * dx + quat_i * dy + quat_r * dz;

            x = quat_r * i - quat_i * r + quat_j * k - quat_k * j;
            y = quat_r * j - quat_i * k - quat_j * r + quat_k * i;
            z = quat_r * k + quat_i * j - quat_j * i - quat_k * r;
        }
    }
}
