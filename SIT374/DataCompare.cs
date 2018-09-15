using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataApi
{
    public class DataCompare
    {
        public static bool AccelarationPedal(double data)
        {
            return (data > 0 && data < 100);
        }

        public static bool RPM(double data)
        {
            return (data > 0 && data < 10000);
        }

        public static bool Power(double data)
        {
            return (data > 0 && data < 1000);
        }

        public static bool Torque(double data)
        {
            return (data > 1 && data < 2000);
        }


        public static bool Cylinders(double data)
        {
            return (data > 1 && data < 20);
        }

        public static bool ValvesperCylinder(double data)
        {
            return (data > 2 && data < 10);
        }

        public static bool CylinderCapacity(double data)
        {
            return (data > 1 && data < 10000);
        }

        public static bool TopSpeed(double data)
        {
            return (data > 1 && data < 600);
        }


        public static bool Acceleration(double data)
        {
            return (data > 1 && data < 60);
        }

        public static bool FuelConsumption(double data)
        {
            return (data > 1 && data < 80);
        }
        public static bool CO2Emissions(double data)
        {
            return (data > 0 && data < 800);
        }

        public static bool Weight(double data)
        {
            return (data > 500 && data < 2000);
        }
    }
}
