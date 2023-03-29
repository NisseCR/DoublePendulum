using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace pendulum.Model
{
    public class Simulation
    {
        /// <summary>
        /// Gathered data from simulation.
        /// </summary>
        private List<double[]> results;
        
        /// <summary>
        /// Project directory.
        /// </summary>
        private static string dir = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName;
        
        /// <summary>
        /// Log file path.
        /// </summary>
        private static string filePath = @$"{dir}\Results\log.txt";
        
        public Simulation()
        {
            this.results = new List<double[]>();
        }

        private string FormatValue(double v)
        {
            return $"{Math.Round(v, 2)};".Replace(',', '.');
        }
        
        private string FormatCollection(double[] data)
        {
            return
                this.FormatValue(data[0])
                + this.FormatValue(data[1])
                + this.FormatValue(data[2])
                + this.FormatValue(data[3]);
        }
        
        /// <summary>
        /// Export the log date of h-values to file.
        /// </summary>
        private void Export()
        {
            List<string> data = this.results.Select(this.FormatCollection).ToList();
            File.WriteAllLines(Simulation.filePath, data);
        }

        public void Run()
        {
            Pendulum pen = new Pendulum();

            int n = 0;
            while (true)
            {
                this.results.Add(pen.ToArray());
                Console.WriteLine(pen);
                pen.Step();
                n++;

                if (Double.IsNaN(pen.x1))
                {
                    Console.WriteLine(n);
                    break;
                }

                if (n > 10000)
                {
                    break;
                }
            }
            
            this.Export();
        }
    }
}