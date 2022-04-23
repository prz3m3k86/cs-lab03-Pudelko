using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Text.RegularExpressions;

namespace PudelkoLibrary
{
    public sealed class Pudelko : IFormattable, IEquatable<Pudelko>, IEnumerable<double>, IComparable<Pudelko>
    {
        public double A {
            get => Convert.ToDouble(a.ToString("0.000"));
            private set { this.a = value; }
        }
        public double B
        {
            get => Convert.ToDouble(b.ToString("0.000"));
            private set { b = value; }
        }

        public double C
        {
            get => Convert.ToDouble(c.ToString("0.000"));
            private set { c = value; }
        }

        private double a, b, c;

        private UnitOfMeasure unit { get; set; }

        public double Objetosc { get => Math.Round((this.A * this.B * this.C), 9); }

        public double Pole { get => Math.Round(2 * (this.A * this.B + this.A * this.C + this.B * this.C), 6); }

        public Pudelko(double? a = null, double? b = null, double? c = null, UnitOfMeasure unit = UnitOfMeasure.meter)
        {
            this.A = (double)(a != null ? (roundNumber((double)a / (ushort)unit)) : 0.1);
            this.B = (double)(b != null ? (roundNumber((double)b / (ushort)unit)) : 0.1);
            this.C = (double)(c != null ? (roundNumber((double)c / (ushort)unit)) : 0.1);

            if (this.A <= 0 | this.A > 10 | this.B <= 0 | this.B > 10 | this.C <= 0 | this.C > 10)
            {
                throw new ArgumentOutOfRangeException();
            }

            this.unit = unit;
        }

        private double roundNumber(double number)
        {
            return Math.Floor(number * Math.Pow(10, 3)) / Math.Pow(10, 3);
        }

        public override string ToString()
        {
            return this.ToString("m");
        }


        public string ToString(string format)
        {
            if (format == null)
                format = "m";

            return ToString(format, CultureInfo.CurrentCulture);
        }

        public string ToString(string format, IFormatProvider formatProvider)
        {
            ushort unit = (ushort) Pudelko.GetUnitForFormat(format);
            int digits = 3;

            if (format == "cm")
            {
                digits = 1;
            }
            else if (format == "mm")
            {
                digits = 0;
            }

            return $"{(this.A * unit).ToString("0." +new String('0', digits))} {format} \u00D7 {(this.B * unit).ToString("0." + new String('0', digits))} {format} \u00D7 {(this.C * unit).ToString("0." + new String('0', digits))} {format}";
        }

        public override bool Equals(object obj)
        {
            if (obj is Pudelko)
            {
                return Equals((Pudelko) obj);
            }

            return base.Equals(obj);
        }
        public bool Equals(Pudelko pudelko)
        {
            return (Pole == pudelko.Pole && Objetosc == pudelko.Objetosc);
        }

        public override int GetHashCode()
        {
            return A.GetHashCode() + B.GetHashCode() + C.GetHashCode() + unit.GetHashCode();
        }

        public static bool operator == (Pudelko pudelko1, Pudelko pudelko2) => pudelko1.Equals(pudelko2);
        public static bool operator != (Pudelko pudelko1, Pudelko pudelko2) => !pudelko1.Equals(pudelko2);

        public static explicit operator double[](Pudelko pudelko) => new double[] { pudelko.A, pudelko.B, pudelko.C };

        public static implicit operator Pudelko(ValueTuple<double, double, double> pudelko) => new Pudelko(pudelko.Item1, pudelko.Item2, pudelko.Item3, UnitOfMeasure.milimeter);

        public static Pudelko operator + (Pudelko pudelko1, Pudelko pudelko2)
        {
            double[] doublePudelko1 = (double[])pudelko1, doublePudelko2 = (double[])pudelko2;

            Array.Sort(doublePudelko1);
            Array.Sort(doublePudelko2);

            return new Pudelko(
                doublePudelko1[0] + doublePudelko2[0],
                doublePudelko1[1] + doublePudelko2[1],
                doublePudelko1[2] + doublePudelko2[2]
            );
        }

        public double this[int index]
        {
            get
            {
                switch (index)
                {
                    case 0:
                        return this.A;
                    case 1:
                        return this.B;
                    case 2:
                        return this.C;
                    default:
                        throw new IndexOutOfRangeException();
                }
            }
        }

        public IEnumerator<double> GetEnumerator()
        {
            return new PudelkoEnumerator(this);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public static Pudelko Parse(string strinToParse)
        {
            Regex reg = new Regex(@"(?<a>\d+\.?\d*)\s(?<format>\w+).*?(?<b>\d+\.?\d*)\s\w+.*?(?<c>\d+\.?\d*)\s\w+");
            Match match = reg.Match(strinToParse);

            string format = match.Groups["format"].Value;

            return new Pudelko(
                double.Parse(match.Groups["a"].Value),
                double.Parse(match.Groups["b"].Value),
                double.Parse(match.Groups["c"].Value),
                Pudelko.GetUnitForFormat(format)
            );
        }

        private static UnitOfMeasure GetUnitForFormat(string format)
        {
            if (format == "m")
            {
               return UnitOfMeasure.meter;
            }
            else if (format == "cm")
            {
                return UnitOfMeasure.centimeter;
            }
            else if (format == "mm")
            {
                return UnitOfMeasure.milimeter;
            }

            throw new FormatException();
        }

        public int CompareTo(Pudelko pudelko)
        {
            double objetoscPudelka1 = this.Objetosc;
            double objetoscPudelka2 = pudelko.Objetosc;

            if (objetoscPudelka1 == objetoscPudelka2)
            {
                double polePudelko1 = this.Pole;
                double polePudelko2 = pudelko.Pole;

                if (polePudelko1 == polePudelko2)
                {
                    double sumABCPudelko1 = this.A + this.B + this.C;
                    double sumABCPudelko2 = pudelko.A + pudelko.B + pudelko.C;

                    if (sumABCPudelko1 == sumABCPudelko2)
                        return 0;

                    return sumABCPudelko1 < sumABCPudelko2 ? 1 : -1;
                }

                return polePudelko1 < polePudelko2 ? 1 : -1;
            }

            return (objetoscPudelka1 < objetoscPudelka2) ? 1 : -1;
        }
    }
}
