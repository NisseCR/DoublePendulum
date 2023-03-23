using System;

namespace pendulum.Model
{
    public class Simulation
    {
        // Write to file
        // NaN caused by infinity
        // Visualisations
        // Friction
        // Runge Kutta?
        
        public Simulation()
        {
            
        }

        public void Run()
        {
            DoublePendulum pen = new DoublePendulum(0.15f, Math.PI / 2, Math.PI / 2, 9.81);

            int n = 0;
            while (true)
            {
                if (n == 9)
                {
                    Console.WriteLine();
                }
                pen.Step();
                n++;

                if (Double.IsNaN(pen.x1))
                {
                    break;
                }
            }
        }
    }
}