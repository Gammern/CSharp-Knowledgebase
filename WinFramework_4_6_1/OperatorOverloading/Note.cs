using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OperatorOverloading
{
    public struct Note
    {
        int value;
        public Note(int semitonesFromA) { value = semitonesFromA; }
        public static Note operator +(Note x, int semitones) => new Note(x.value + semitones);
        // convert to Hz
        public static implicit operator double(Note x) => 440 * Math.Pow(2, (double)x.value / 12);
        // convert from Hz
        public static explicit operator Note(double x) => new Note((int)(0.5 + 12 * (Math.Log(x / 440) / Math.Log(2))));
    }
}
