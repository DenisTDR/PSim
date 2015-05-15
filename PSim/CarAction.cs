using System;
using System.Runtime.CompilerServices;

namespace PSim
{
	public class CarAction
	{
		public double Data
		{
			get;
			set;
		}

		public Direction Direction
		{
			get;
			set;
		}

		public double Duration
		{
			get;
			set;
		}

		public MoveAction MoveAction
		{
			get;
			set;
		}

		public CarAction()
		{
			this.MoveAction = MoveAction.Forward;
			this.Direction = Direction.Straight;
			this.Duration = 0;
			this.Data = 1;
		}
	}
}