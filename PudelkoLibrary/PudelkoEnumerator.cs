using System;
using System.Collections.Generic;
using System.Text;

namespace PudelkoLibrary
{
    class PudelkoEnumerator : IEnumerator<double>
    {
        private readonly Pudelko pudelko;

        private int index = 0;

        public PudelkoEnumerator(Pudelko Pudelko)
        {
            this.pudelko = Pudelko;
        }

        public object Current => pudelko[index++];

        double IEnumerator<double>.Current => pudelko[index++];


        public bool MoveNext()
        {
            return index <= 1;
        }

        public void Reset()
        {
            index = 0;
        }

        public void Dispose()
        {
        }
    }
}
