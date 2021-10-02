using Sexy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


static class Program
{
    static void Main(string[] args)
    {
        using (Main game = new Main())
        {
            game.Run();
        }
    }
}