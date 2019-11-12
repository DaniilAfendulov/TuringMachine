//Автор: Афендулов Даниил 
//Реализация Машины Тьюринга


using System.IO;

namespace turing_machine
{
    partial class Program
    {
        static void Main(string[] args)
        {
            string path = @"..\..\TuringMachinePrograms\" + File.ReadAllText(@"..\..\name.txt");
            TuringMachine.StartTuringProgramFromFile(path);
        }
        public enum Direct { R, L, N }
        public class Command
        {
            public char symbol;
            public int q;
            public Direct direct;

            public Command(char s, int q, Direct d)
            {
                symbol = s;
                this.q = q;
                direct = d;
            }

        }


    }
}
