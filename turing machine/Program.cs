﻿//Автор: Афендулов Даниил 
//Реализация Машины Тьюринга


using System.IO;

namespace turing_machine
{
    partial class Program
    {
        static void Main(string[] args)
        {
            string path = @"..\..\TuringMachinePrograms\" + File.ReadAllText(@"..\..\name.txt");
            TuringMachine.StartFromFile(path);
        }



    }
}
