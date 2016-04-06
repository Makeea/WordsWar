using BoardConstruction;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoardConstructionRunner
{
    class Program
    {
        static void Main(string[] args)
        {
            BoardSolution sol = BoardConstructor.Build_Solve_Publish(4, "en-us", "english-words");

            Console.WriteLine(sol.ToString());
            string json = JsonConvert.SerializeObject(sol, Formatting.Indented);
            Console.WriteLine(json);
        }
    }
}
