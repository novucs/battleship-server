using System;

namespace Battleships
{
	public class Ship
	{
		public bool inuse;

		public int X;

		public int Y;

		public int speedX;

		public int speedY;

		public bool fire;

		public int fireX;

		public int fireY;

		public int Health;

		public void setX(int newX)
		{
			this.X = newX;
		}

		public void setY(int newY)
		{
			this.Y = newY;
		}
	}
}
