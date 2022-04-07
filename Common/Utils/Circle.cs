using System;
using Microsoft.Xna.Framework;

namespace CNC.Common.Utils
{
    public struct Circle : IEquatable<Circle>
    {
        public Vector2 center;
        public float radius;
        public float Area => MyMath.pi * this.radius * this.radius;
        public float Long => MyMath.pi * this.radius * 2;

        public static Circle Empty() => new Circle(0, new Vector2());

        public Circle(float radius = 0, Vector2? center = null)
        {
            this.center = center ?? new Vector2();
            this.radius = Math.Max(0, radius);
        }

        public Circle(float radius = 0, float x = 0, float y = 0)
        {
            this.center = new Vector2(x, y);
            this.radius = Math.Max(0, radius);
        }

        public bool Equals(Circle obj) => this.Equals(obj);

        public override bool Equals(object obj)
        {
            if (obj is Circle other) return this.radius == other.radius && this.center == other.center;
            return base.Equals(obj);
        }

        public override int GetHashCode() => new { this.radius, this.center }.GetHashCode();

        public override string ToString() => "{" + this.center + ", " + this.radius + "}";
        
        public bool Contains(float x, float y) => this.Contains(new Vector2(x, y));
        public bool Contains(Vector2 value) => Vector2.Distance(value, this.center) < this.radius;
        public bool Contains(Circle circle) => Vector2.Distance(circle.center, this.center) + circle.radius < this.radius;
        public bool Tangency(float x, float y, double eps = 0) => this.Tangency(new Vector2(x, y), eps);
        public bool Tangency(Vector2 value, double eps = 0) => Math.Abs(Vector2.Distance(value, this.center) - this.radius) <= eps;
        public bool Tangency(Circle circle, double eps = 0) => Math.Abs(Vector2.Distance(circle.center, this.center) - circle.radius - this.radius) <= eps || Math.Abs(Vector2.Distance(circle.center, this.center) + circle.radius - this.radius) <= eps;
        public bool Intersects(Circle circle) => Vector2.Distance(this.center, circle.center) < this.radius + circle.radius && Vector2.Distance(this.center, circle.center) > this.radius - circle.radius;
        public bool Separates(Circle circle) => Vector2.Distance(this.center, circle.center) > this.radius + circle.radius; //!this.Contains(circle) && !this.Tangency(circle) && !this.Intersects(circle);

        public static bool operator ==(Circle a, Circle b) => a.Equals(b);
        public static bool operator !=(Circle a, Circle b) => !a.Equals(b);
    }
}