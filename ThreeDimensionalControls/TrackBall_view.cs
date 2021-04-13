using System;
using System.Drawing;
using System.Runtime.InteropServices;

// Copyright (c) T.Yoshimura 2019-2021
// https://github.com/tk-yoshimura

namespace ThreeDimensionalControls {
    public partial class TrackBall {

        Bitmap panel, ball;

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

        protected void DrawImage() {
            pic_size = Math.Max(Math.Min(this.Width, this.Height) / 2 * 2, 50);

            panel_size = new Size(pic_size, pic_size);
            ball_size = new Size((pic_size * 19 / 20) / 2 * 2, (pic_size * 19 / 20) / 2 * 2);

            panel_pos = new Point((this.Width - panel_size.Width) / 2, (this.Height - panel_size.Height) / 2);
            ball_pos = new Point(panel_pos.X + (panel_size.Width - ball_size.Width) / 2, panel_pos.Y + (panel_size.Height - ball_size.Height) / 2);

            DrawPanel();
            DrawBall();
        }
    }
}
