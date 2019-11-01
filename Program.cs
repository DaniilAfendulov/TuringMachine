//Автор: Афендулов Даниил 
//Реализация Машины Тьюринга

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace turing_machine
{
    class Program
    {
        static void Main(string[] args)
        {
            ParenthesesRemoval();
        }

        public class TuringMachine
        {
            char[] _word;
            int _index;
            int _q;
            Dictionary<int, Dictionary<char, Command>> _tacts;

            public TuringMachine(string word)
            {
                string emptyChars = "                 ";
                _word = (emptyChars + word + emptyChars).ToCharArray();
                _index = emptyChars.Length;
                _q = 1;
                _tacts = new Dictionary<int, Dictionary<char, Command>>();
            }

            public char[] Execute()
            {
                ExecuteCommand(_tacts[_q][_word[_index]]);
                return _word;
            }

            void ExecuteCommand(Command command)
            {
                _word[_index] = command.symbol;
                switch (command.direct)
                {
                    case Direct.R:
                        _index++;
                        break;
                    case Direct.L:
                        _index--;
                        break;
                    case Direct.N:
                        break;
                    default:
                        throw new Exception("invalid direct");
                }
                _q = command.q;
                if (_q == 0) return;
                ExecuteCommand(_tacts[_q][_word[_index]]);
            }

            public bool AddCommand(char s, int q, Command command)
            {
                if (!_tacts.ContainsKey(q)) _tacts.Add(q, new Dictionary<char, Command>());
                if (!_tacts[q].ContainsKey(s))
                {
                    _tacts[q].Add(s, command);
                    return true;
                }
                return false;
            }


        }
        public enum Direct { R, L, N }
        public class Command
        {
            public char symbol;
            public int q;
            public Direct direct;

            public Command(char s, int q, Direct d)
            {
                this.symbol = s;
                this.q = q;
                this.direct = d;
            }

        }


        static public void CharacterOffset()
        {
            TuringMachine turingMachine = new TuringMachine("abcbabbc");
            turingMachine.AddCommand('a', 1, new Command(' ', 2, Direct.R));
            turingMachine.AddCommand('b', 1, new Command(' ', 3, Direct.R));
            turingMachine.AddCommand('c', 1, new Command(' ', 4, Direct.R));
            turingMachine.AddCommand(' ', 1, new Command(' ', 0, Direct.N));
            turingMachine.AddCommand('a', 2, new Command('a', 2, Direct.R));
            turingMachine.AddCommand('b', 2, new Command('b', 2, Direct.R));
            turingMachine.AddCommand('c', 2, new Command('c', 2, Direct.R));
            turingMachine.AddCommand(' ', 2, new Command('a', 0, Direct.N));
            turingMachine.AddCommand('a', 3, new Command('a', 3, Direct.R));
            turingMachine.AddCommand('b', 3, new Command('b', 3, Direct.R));
            turingMachine.AddCommand('c', 3, new Command('c', 3, Direct.R));
            turingMachine.AddCommand(' ', 3, new Command('a', 0, Direct.N));
            turingMachine.AddCommand('a', 4, new Command('a', 4, Direct.R));
            turingMachine.AddCommand('b', 4, new Command('b', 4, Direct.R));
            turingMachine.AddCommand('c', 4, new Command('c', 4, Direct.R));
            turingMachine.AddCommand(' ', 4, new Command('a', 0, Direct.N));
            Console.WriteLine(turingMachine.Execute());
            Console.ReadLine();
        }

        static public void ParenthesesRemoval()
        {
            TuringMachine turingMachine = new TuringMachine(")(()(()");

            turingMachine.AddCommand('(', 1, new Command('(', 2, Direct.R));
            turingMachine.AddCommand(')', 1, new Command(')', 1, Direct.R));
            turingMachine.AddCommand('*', 1, new Command('*', 4, Direct.L));
            turingMachine.AddCommand(' ', 1, new Command(' ', 0, Direct.N));

            turingMachine.AddCommand('(', 2, new Command('(', 2, Direct.R));
            turingMachine.AddCommand(')', 2, new Command('*', 3, Direct.L));
            turingMachine.AddCommand('*', 2, new Command('*', 4, Direct.L));
            turingMachine.AddCommand(' ', 2, new Command(' ', 0, Direct.N));

            turingMachine.AddCommand('(', 3, new Command('*', 4, Direct.L));
            turingMachine.AddCommand('*', 3, new Command(' ', 1, Direct.R));

            turingMachine.AddCommand('(', 4, new Command('*', 5, Direct.R));
            turingMachine.AddCommand(')', 4, new Command('*', 6, Direct.R));
            turingMachine.AddCommand('*', 4, new Command('*', 4, Direct.L));
            turingMachine.AddCommand(' ', 4, new Command(' ', 3, Direct.R));

            turingMachine.AddCommand('*', 5, new Command('(', 4, Direct.L));

            turingMachine.AddCommand('*', 6, new Command(')', 4, Direct.L));


            Console.WriteLine(turingMachine.Execute());
            Console.ReadLine();
        }





    }
}
