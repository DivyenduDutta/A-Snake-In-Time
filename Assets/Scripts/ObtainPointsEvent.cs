using System;

namespace CustomEvent
{
    public class ObtainPointsEvent : EventArgs
    {
        public float PointX { get; set; }
        public float PointY { get; set; }
    }
}
