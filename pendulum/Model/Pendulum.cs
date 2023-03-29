using System;
using System.IO;
using Microsoft.VisualBasic.CompilerServices;

namespace pendulum.Model
{
    public class Pendulum
    {
        private double _gravity;

        private double _mass1, _mass2;

        private double _length1, _length2;

        private double _theta1, _theta2;

        private double _omega1, _omega2;

        private int _stateSize;

        private double Theta1Dot()
        {
            return _omega1;
        }

        private double Theta2Dot()
        {
            return _omega2;
        }

        private double Omega1Dot()
        {
            double numerator = -_gravity * (2 * _mass1 + _mass2) * Math.Sin(_theta1) - _gravity * _mass2 * Math.Sin(_theta1 - 2 * _theta2);
            numerator = numerator - 2 * _mass2 * Math.Sin(_theta1 - _theta2) * (_length2 * Math.Pow(_omega2, 2) + _length1 * Math.Pow(_omega1, 2) * Math.Cos(_theta1-_theta2));
            double denominator =_length1 * (2 * _mass1 + _mass2 * (1 - Math.Cos(2 * (_theta1 - _theta2))));
            return numerator / denominator;
        }

        private double Omega2Dot()
        {
            double numerator = 2 * Math.Sin(_theta1 - _theta2);
            numerator *= (_length1 * Math.Pow(_omega1, 2) * (_mass1 + _mass2) * _gravity * Math.Cos(_theta1) * (_mass1 + _mass2) + _length2 * _mass2 * Math.Pow(_omega2, 2) * Math.Cos(_theta1 - _theta2));
            double denominator = _length2 * (2 * _mass1 + _mass2 * (1 - Math.Cos(2 * (_theta1 - _theta2))));
            return numerator / denominator;
        }

        private double[] GetValues(double[] state)
        {
            _theta1 = state[0];
            _theta2 = state[1];
            _omega1 = state[2];
            _omega2 = state[3];

            double[] values = new double[_stateSize];
            values[0] = this.Theta1Dot();
            values[1] = this.Theta2Dot();
            values[2] = this.Omega1Dot();
            values[3] = this.Omega2Dot();

            return values;
        }

        private double[] UpdateState(double[] baseState, double[] updateState, double factor, double disc)
        {
            double[] newState = new double[_stateSize];

            for (int i = 0; i < _stateSize; i++)
            {
                newState[i] = baseState[i] + updateState[i] * factor * disc;
            }

            return newState;
        }

        private double[] Sum(double[][] states)
        {
            double[] sumState = new double[_stateSize];

            for (int i = 0; i < _stateSize; i++)
            {
                double n = 0;
                
                for (int j = 0; j < states.Length; j++)
                {
                    n += states[j][i];
                }

                sumState[i] = n;
            }

            return sumState;
        }

        private double[] RungeKutta(double disc, double[] state)
        {
            double[] k1 = this.GetValues(state);
            double[] k2 = this.GetValues(this.UpdateState(state, k1, 0.5, disc));
            double[] k3 = this.GetValues(this.UpdateState(state, k2, 0.5, disc));
            double[] k4 = this.GetValues(this.UpdateState(state, k3, 1, disc));

            double[][] states = {k1, k2, k2, k3, k3, k4};
            double[] sumState = this.Sum(states);
            return this.UpdateState(state, sumState, 1 / 6f, disc);
        }

        public void Step(double disc)
        {
            
        }
    }
}