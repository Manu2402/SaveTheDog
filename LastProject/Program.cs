using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LastProject
{
    // Dependencies: OpenAL32.

    class Program
    {
        static void Main(string[] args)
        {
            Game.Init();
            Game.Play();
        }
    }
}
