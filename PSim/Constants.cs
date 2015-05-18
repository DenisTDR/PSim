using System;

namespace PSim
{
	internal static class Constants
	{

	}
    public enum LimitType
    {
        Wall,
        Border,
        Free
    }

    public enum Direction
    {
        Left,
        Straight,
        Right
    }
    public enum MoveAction
    {
        Forward,
        Backward,
        SmartMovement
    }
    public enum Sensor
    {
        FrontRight,
        FrontLeft,
        SideRight,
        SideLeft
    }

    public enum Sens
    {
        SensFata,
        SensSpate
    }

    public enum Engines
    {
        RightEngines,
        LeftEngines
    }

}