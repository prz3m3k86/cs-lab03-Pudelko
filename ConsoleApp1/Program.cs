using PudelkoLibrary;
using System;
using System.Collections.Generic;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            List<Pudelko> pudelka = new List<Pudelko>();
            pudelka.Add(new Pudelko(2.5, 9.321, 0.1, UnitOfMeasure.meter));
            pudelka.Add(new Pudelko(623.76, 153.34, 123.654, UnitOfMeasure.centimeter));
            pudelka.Add(new Pudelko(4500, 2525, null, UnitOfMeasure.milimeter));
            pudelka.Add(new Pudelko(45, null, null, UnitOfMeasure.milimeter));
            pudelka.Add(new Pudelko(0.45, null, 0.5982));
            pudelka.Add(
                new Pudelko(987, 634, 876, UnitOfMeasure.milimeter).Kompresuj()
            );

            pudelka.Sort();

            pudelka.ForEach(
                (pudelko) => {
                    Console.WriteLine(pudelko.ToString());
                }
            );
        }
    }
}
