using System;

namespace pendulum.Model
{
    public class DoublePendulum
    {
        /// <summary>
        /// Length of inner and outer rod.
        /// </summary>
        public int r1, r2;

        /// <summary>
        /// Mass of inner and outer point masses.
        /// </summary>
        public int m1, m2;
        
        /// <summary>
        /// Angle of inner and outer rod.
        /// </summary>
        public double a1, a2;
        
        /// <summary>
        /// Angle velocity of inner and outer rod.
        /// </summary>
        public double v1, v2;
        
        /// <summary>
        /// Gravitational force.
        /// </summary>
        public double g;
        
        /// <summary>
        /// Point mass coordinates.
        /// </summary>
        public double x1, y1, x2, y2;

        public DoublePendulum(int m2, double a1, double a2, double g)
        {
            // Constants.
            this.SetupConstants();
            
            // Treatment.
            this.m2 = m2;
            this.a1 = a1;
            this.a2 = a2;
            this.g = g;
            
            // Starting position.
            this.GetPointMassPositions();
        }

        private void SetupConstants()
        {
            this.m1 = 10;
            this.r1 = 125;
            this.r2 = 125;
            this.v1 = 0;
            this.v2 = 0;
        }

        private double CalculateInnerAcceleration()
        {
            double _p = -g * (2 * m1 + m2) * Math.Sin(a1);
            double _q = -m2 * g * Math.Sin(a1 - 2 * a2);
            double _r = -2 * Math.Sin(a1 - a2) * m2;
            double _s = v2 * v2 * r2 + v1 * v1 * r1 * Math.Cos(a1 - a2);
            double _t = r1 * (2 * m1 + m2 - m2 * Math.Cos(2 * a1 - 2 * a2));
            return (_p + _q + _r * _s) / _t;
        }
        
        private double CalculateOuterAcceleration()
        {
            double _p = 2 * Math.Sin(a1 - a2);
            double _q = v1 * v1 * r1 * (m1 + m2);
            double _r = g * (m1 + m2) * Math.Cos(a1);
            double _s = v2 * v2 * r2 * m2 * Math.Cos(a1 - a2);
            double _t = r2 * (2 * m1 + m2 - m2 * Math.Cos(2 * a1 - 2 * a2));
            return _p * (_q + _r + _s) / _t;
        }

        private void GetPointMassPositions()
        {
            // Inner point mass position.
            this.x1 = this.r1 * Math.Sin(this.a1);
            this.y1 = this.r1 * Math.Cos(this.a1);
            
            // Outer point mass position.
            this.x2 = this.x1 + this.r2 * Math.Sin(this.a2);
            this.y2 = this.y1 + this.r2 * Math.Cos(this.a2);

        }

        private void UpdateRodAngles()
        {
            // Get acceleration.
            double innerAcceleration = this.CalculateInnerAcceleration();
            double outerAcceleration = this.CalculateOuterAcceleration();
            
            // Update velocity.
            this.v1 += innerAcceleration;
            this.v2 += outerAcceleration;
            
            // Update angle.
            this.a1 += this.v1;
            this.a2 += this.v2;
        }

        public void Step()
        {
            this.GetPointMassPositions();
            this.UpdateRodAngles();
        }

        public override string ToString()
        {
            return $"M1({Math.Round(this.x1, 2)} - {Math.Round(this.y1, 2)}) | M2({Math.Round(this.x2, 2)} - {Math.Round(this.y2, 2)})";
        }
    }
}