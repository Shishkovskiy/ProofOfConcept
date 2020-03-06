using ProofOfConcept.Abstractions;
using System;

namespace ProofOfConcept
{
    internal class ConsolePrinter : IPrinter
    {
        public void Print(string message)
        {
            Console.WriteLine(message);
        }
    }
}
