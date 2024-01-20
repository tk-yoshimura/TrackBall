using System;
using System.Numerics;

// Copyright (c) T.Yoshimura 2019-2024
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
}
