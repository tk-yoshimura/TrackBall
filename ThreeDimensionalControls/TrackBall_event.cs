using System;
using System.Drawing;
using System.Windows.Forms;

// Copyright (c) T.Yoshimura 2019-2021
// https://github.com/tk-yoshimura

namespace ThreeDimensionalControls {
    public partial class TrackBall {

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
            DrawImage();
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
                        ValueChanged?.Invoke(this, new TrackBallRolledEventArgs(Value));
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
    }
}
