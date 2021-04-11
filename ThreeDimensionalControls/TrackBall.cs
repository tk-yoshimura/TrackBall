using System;
using System.Drawing;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Windows.Forms;

// Copyright (c) T.Yoshimura 2019-2021
// https://github.com/tk-yoshimura

namespace ThreeDimensionalControls {
    public class TrackBallRolledEventArgs : EventArgs {
        public Quaternion Quaternion { private set; get; }

        public TrackBallRolledEventArgs(Quaternion quaternion) {
            this.Quaternion = quaternion;
        }

        public override string ToString() {
            return Quaternion.ToString();
        }
    }

    public delegate void TrackBallRolledEventHandler(object sender, TrackBallRolledEventArgs tre);

    public class TrackBall : UserControl {
        bool is_manipulate = false;
        int pic_size;
        double quat_r = 1, quat_i = 0, quat_j = 0, quat_k = 0;
        double init_x, init_y, init_z;

        Point panel_pos, ball_pos;
        Size panel_size, ball_size;

        Bitmap panel, ball;

        public Quaternion Quaternion => new(new Vector3((float)quat_i, (float)quat_j, (float)quat_k), (float)quat_r);

        public event TrackBallRolledEventHandler ValueChanged;

        public TrackBall() {
            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            SetImage();
        }

        protected override void OnPaint(PaintEventArgs pe) {
            if (IsValidSize()) {
                Graphics g = pe.Graphics;

                if (ball is not null) {
                    g.DrawImageUnscaled(ball, ball_pos);
                }

                if (panel is not null) {
                    g.DrawImageUnscaled(panel, panel_pos);
                }
            }

            base.OnPaint(pe);
        }

        protected override void OnResize(EventArgs e) {
            SetImage();
            base.OnResize(e);
            Invalidate();
        }

        protected override void OnMove(EventArgs e) {
            is_manipulate = false;
            base.OnMove(e);
        }

        protected override void OnMouseDown(MouseEventArgs e) {
            if (e.Button == MouseButtons.Left && IsBallArea(e.X, e.Y)) {
                is_manipulate = false;

                double dx, dy, dz, norm_sq, center = (ball.Width - 1) * 0.5, inv_center = 1.0 / center;

                dx = (e.X - ball_pos.X - center) * inv_center;
                dy = (e.Y - ball_pos.Y - center) * inv_center;
                norm_sq = dx * dx + dy * dy;

                if (norm_sq <= 1) {
                    is_manipulate = true;

                    dz = Math.Sqrt(1 - norm_sq);

                    TransformCoord(dx, dy, dz, out init_x, out init_y, out init_z);
                }
            }
            base.OnMouseDown(e);
        }

        protected override void OnMouseUp(MouseEventArgs e) {
            if (e.Button == MouseButtons.Left && is_manipulate) {

                AcceptManipulateBall(e);

                is_manipulate = false;
            }
            base.OnMouseUp(e);
        }

        protected override void OnMouseLeave(EventArgs e) {
            is_manipulate = false;
            base.OnMouseLeave(e);
        }

        protected override void OnMouseMove(MouseEventArgs e) {
            if (e.Button == MouseButtons.Left && IsBallArea(e.X, e.Y)) {
                if (is_manipulate) {
                    AcceptManipulateBall(e);
                }
            }
            base.OnMouseMove(e);
        }

        protected override void OnMouseDoubleClick(MouseEventArgs e) {
            if (e.Button == MouseButtons.Left) {
                if (IsValidSize()) {
                    double dx = e.X - ball_pos.X - ball.Width * 0.5, dy = e.Y - ball_pos.Y - ball.Height * 0.5, norm = Math.Sqrt(dx * dx + dy * dy);
                    if (norm <= ball.Width * 0.125) {
                        Reset();
                        ValueChanged?.Invoke(this, new TrackBallRolledEventArgs(Quaternion));
                    }
                }
            }

            base.OnMouseDoubleClick(e);
        }

        protected override void OnHandleDestroyed(EventArgs e) {
            if (panel is not null) {
                panel.Dispose();
                panel = null;
            }
            if (ball is not null) {
                ball.Dispose();
                ball = null;
            }
            base.OnHandleDestroyed(e);
        }

        protected void DrawPanel() {
            if (panel is not null) {
                panel.Dispose();
            }
            if (IsValidSize()) {
                panel = new Bitmap(panel_size.Width, panel_size.Height, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            }
            else {
                panel = new Bitmap(1, 1);
                return;
            }

            int width = panel.Width, height = panel.Height;
            double dx, dy, norm_sq, alpha, pic_center = (pic_size - 1) * 0.5;
            double thr1_sq = (pic_center * 0.98 + 1.414) * (pic_center * 0.98 + 1.414), thr2_sq = pic_center * pic_center * 0.98 * 0.98;
            double thr3_sq = pic_center * pic_center * 0.96 * 0.96, thr4_sq = pic_center * pic_center * 0.10 * 0.10;
            double inv_thr12 = 1.0 / (Math.Sqrt(thr1_sq) - Math.Sqrt(thr2_sq)), inv_thr34 = 1.0 / (Math.Sqrt(thr3_sq) - Math.Sqrt(thr4_sq));

            byte[] buf = new byte[width * height * 4];

            unsafe {
                fixed (byte* c = buf) {
                    for (int x, y = 0, i = 0; y < height; y++) {
                        dy = y - pic_center;
                        for (x = 0; x < width; x++, i += 4) {
                            dx = x - pic_center;
                            norm_sq = dx * dx + dy * dy;

                            if (thr1_sq < norm_sq) {
                                alpha = 0;
                            }
                            else if (thr2_sq < norm_sq) {
                                alpha = 1 - (Math.Sqrt(norm_sq) - Math.Sqrt(thr2_sq)) * inv_thr12;
                            }
                            else if (thr3_sq < norm_sq) {
                                alpha = 1;
                            }
                            else if (thr4_sq < norm_sq) {
                                alpha = (Math.Sqrt(norm_sq) - Math.Sqrt(thr4_sq)) * inv_thr34;
                                alpha = alpha * alpha * alpha * alpha;
                            }
                            else {
                                alpha = 0;
                            }

                            c[i] = c[i + 1] = c[i + 2] = 0;
                            c[i + 3] = (byte)(alpha * 255);
                        }
                    }
                }
            }

            var bmpdata = panel.LockBits(new Rectangle(0, 0, width, height), System.Drawing.Imaging.ImageLockMode.WriteOnly, panel.PixelFormat);
            Marshal.Copy(buf, 0, bmpdata.Scan0, buf.Length);
            panel.UnlockBits(bmpdata);
        }

        protected void DrawBall() {
            if (ball is not null)
                ball.Dispose();

            if (IsValidSize()) {
                ball = new Bitmap(ball_size.Width, ball_size.Height, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            }
            else {
                ball = new Bitmap(1, 1);
                return;
            }

            bool flag;
            byte cr;
            int width = ball.Width, height = ball.Height, scanline_num = width / 8;
            double dx, dy, dz, norm_sq, center = (ball.Width - 1) * 0.5, inv_center = 1.0 / center;
            double ball_x, ball_y, ball_z;

            int[] x_scanline = new int[scanline_num + 1], y_scanline = new int[scanline_num + 1];
            int[,] scan = new int[scanline_num + 1, scanline_num + 1];
            byte[] buf = new byte[width * height * 4];

            for (int i = 0; i <= scanline_num; i++) {
                x_scanline[i] = (width - 1) * i / scanline_num;
                y_scanline[i] = (height - 1) * i / scanline_num;
            }

            for (int i, j = 0; j <= scanline_num; j++) {
                dy = (y_scanline[j] - center) * inv_center;

                for (i = 0; i <= scanline_num; i++) {
                    dx = (x_scanline[i] - center) * inv_center;
                    norm_sq = dx * dx + dy * dy;

                    if (norm_sq > 1) {
                        scan[i, j] = -1;
                        continue;
                    }

                    dz = Math.Sqrt(1 - norm_sq);

                    TransformCoord(dx, dy, dz, out ball_x, out ball_y, out ball_z);

                    scan[i, j] = 0;
                    if (ball_x < 0)
                        scan[i, j] += 1;
                    if (ball_y < 0)
                        scan[i, j] += 2;
                    if (ball_z < 0)
                        scan[i, j] += 4;
                }
            }

            unsafe {
                fixed (byte* c = buf) {
                    for (int i, j = 1; j <= scanline_num; j++) {
                        for (i = 1; i <= scanline_num; i++) {
                            if (scan[i, j] == scan[i - 1, j] && scan[i, j] == scan[i, j - 1] && scan[i, j] == scan[i - 1, j - 1]) {

                                if (scan[i, j] == -1) {
                                    continue;
                                }

                                cr = (scan[i, j] == 0 || scan[i, j] == 3 || scan[i, j] == 5 || scan[i, j] == 6) ? (byte)193 : (byte)255;

                                for (int x, y = y_scanline[j - 1]; y < y_scanline[j]; y++) {
                                    for (x = x_scanline[i - 1]; x < x_scanline[i]; x++) {
                                        int k = 4 * (x + y * width);

                                        c[k] = c[k + 1] = c[k + 2] = cr;
                                        c[k + 3] = 255;
                                    }
                                }
                            }
                            else {
                                for (int x, y = y_scanline[j - 1]; y < y_scanline[j]; y++) {
                                    dy = (y - center) * inv_center;
                                    for (x = x_scanline[i - 1]; x < x_scanline[i]; x++) {
                                        dx = (x - center) * inv_center;
                                        norm_sq = dx * dx + dy * dy;

                                        if (norm_sq > 1) {
                                            continue;
                                        }

                                        dz = Math.Sqrt(1 - norm_sq);

                                        TransformCoord(dx, dy, dz, out ball_x, out ball_y, out ball_z);

                                        flag = false;
                                        if (ball_x < 0)
                                            flag = !flag;
                                        if (ball_y < 0)
                                            flag = !flag;
                                        if (ball_z < 0)
                                            flag = !flag;

                                        int k = 4 * (x + y * width);

                                        c[k] = c[k + 1] = c[k + 2] = flag ? (byte)255 : (byte)193;
                                        c[k + 3] = 255;
                                    }
                                }
                            }
                        }
                    }
                }
            }

            var bmpdata = ball.LockBits(new Rectangle(0, 0, width, height), System.Drawing.Imaging.ImageLockMode.WriteOnly, ball.PixelFormat);
            Marshal.Copy(buf, 0, bmpdata.Scan0, buf.Length);
            ball.UnlockBits(bmpdata);

            Invalidate();
        }

        protected void SetImage() {
            pic_size = Math.Max(Math.Min(this.Width, this.Height) / 2 * 2, 50);

            panel_size = new Size(pic_size, pic_size);
            ball_size = new Size((pic_size * 19 / 20) / 2 * 2, (pic_size * 19 / 20) / 2 * 2);

            panel_pos = new Point((this.Width - panel_size.Width) / 2, (this.Height - panel_size.Height) / 2);
            ball_pos = new Point(panel_pos.X + (panel_size.Width - ball_size.Width) / 2, panel_pos.Y + (panel_size.Height - ball_size.Height) / 2);

            DrawPanel();
            DrawBall();
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

            ValueChanged?.Invoke(this, new TrackBallRolledEventArgs(Quaternion));
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
