using PudelkoLibrary;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleApp1
{
    public static class NewPudelko
    {
        public static Pudelko Kompresuj(this Pudelko pudelko)
        {
            double size = Math.Cbrt(pudelko.Objetosc);
            return new Pudelko(size, size, size);
        }
    }
}