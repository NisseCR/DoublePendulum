using System;

namespace pendulum.Model
{
    public class Simulation
    {
        public Simulation()
        {
            
        }

        public void Run()
        {
            DoublePendulum pen = new DoublePendulum(15, Math.PI / 2, Math.PI / 2, 1);
            Console.WriteLine(pen);
        }
    }
}