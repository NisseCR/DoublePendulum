using System;
using System.Transactions;

namespace pendulum.Model
{
    public class DoublePendulum
    {
        public double f;
        
        /// <summary>
        /// Length of inner and outer rod.
        /// </summary>
        public float r1, r2;

        /// <summary>
        /// Mass of inner and outer point masses.
        /// </summary>
        public float m1, m2;
        
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

        public DoublePendulum(float m2, double a1, double a2, double g)
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
            this.f = 1 / 200f;
            this.m1 = 10;
            this.r1 = 125;
            this.r2 = 125;
            this.v1 = 0;
            this.v2 = 0;
        }
        
        /// <summary>
        /// Theta`` = acc
        /// 
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        private double F1(double v)
        {
            return 
                (
                    -g * (2 * m1 + m2) * Math.Sin(a1)
                    + -m2 * g * Math.Sin(a1 - 2 * a2)
                    + -2 * Math.Sin(a1 - a2) * m2
                    * (v2 * v2 * r2 + v * v * r1 * Math.Cos(a1 - a2))
                )
                / (r1 * (2 * m1 + m2 - m2 * Math.Cos(2 * a1 - 2 * a2)));
        }
        
        private double F2(double v)
        {
            return
                2 * Math.Sin(a1 - a2)
                  * (
                      v1 * v1 * r1 * (m1 + m2)
                      + g * (m1 + m2) * Math.Cos(a1)
                      + v * v * r2 * m2 * Math.Cos(a1 - a2)
                  )
                / (r2 * (2 * m1 + m2 - m2 * Math.Cos(2 * a1 - 2 * a2)));
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
        
        private double RungeKuttaV1()
        {
            double a = this.F1(this.v1);
            double b = this.F1(this.v1 + 0.5 * a);
            double c = this.F1(this.v1 + 0.5 * b);
            double d = this.F1(this.v1 + c);

            return (1 / 6f) * (a + 2 * b + 2 * c + d);
        }
        
        private double RungeKuttaV2()
        {
            double a = this.F2(this.v2);
            double b = this.F2(this.v2 + 0.5 * a);
            double c = this.F2(this.v2 + 0.5 * b);
            double d = this.F2(this.v2 + c);

            return (1 / 6f) * (a + 2 * b + 2 * c + d);
        }

        private double ApplyFriction(double v)
        {
            if (v < 0)
            {
                return v += v * v * this.f;
            }
            
            return v -= v * v * this.f;
        }

        private void ApplyRungeKutta()
        {
            // Update velocity.
            double r1 = this.RungeKuttaV1();
            double r2 = this.RungeKuttaV2();

            this.v1 += r1;
            this.v2 += r2;

            // Friction
            this.v1 = this.ApplyFriction(this.v1);
            this.v2 = this.ApplyFriction(this.v2);

            // Update angle.
            this.a1 += this.v1 * (1 / 100f);
            this.a2 += this.v2 * (1 / 100f);
        }

        public void Step()
        {
            this.GetPointMassPositions();
            this.ApplyRungeKutta();
        }

        public double[] ToArray()
        {
            return new[] {this.x1, this.y1, this.x2, this.y2};
        }

        public override string ToString()
        {
            return $"{Math.Round(this.x1, 2)};{Math.Round(this.y1, 2)};{Math.Round(this.x2, 2)};{Math.Round(this.y2, 2)};";
        }
    }
}