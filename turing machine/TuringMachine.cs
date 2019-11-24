//Автор: Афендулов Даниил 
//Реализация Машины Тьюринга


using System;
using System.Collections.Generic;
using System.IO;

namespace turing_machine
{
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
    public class TuringMachine
    {
        List<char> _word;
        int _index;
        int _q;
        Dictionary<int, Dictionary<char, Command>> _tacts;

        public TuringMachine(string word)
        {
            _word = new List<char>();
            _word.AddRange((" " + word + " ").ToCharArray());
            _index = 1;
            _q = 1;
            _tacts = new Dictionary<int, Dictionary<char, Command>>();
        }

        public char[] Execute()
        {
            ExecuteCommand(_tacts[_q][_word[_index]]);
            return _word.ToArray();
        }

        void ExecuteCommand(Command command)
        {
            _word[_index] = command.symbol;
            switch (command.direct)
            {
                case Direct.R:
                    _index++;
                    if (_index == _word.Count) _word.Add(' ');
                    break;
                case Direct.L:
                    _index--;
                    if (_index == 0)
                    {
                        _word.Insert(0, ' ');
                        _index++;
                    }
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

        static public void StartFromFile(string filePath)
        {
            TuringMachine turingMachine = null;
            using (StreamReader file = new StreamReader(filePath))
            {
                while (!file.EndOfStream)
                {
                    if ((char)file.Peek() == '#') file.ReadLine();
                    else
                    {
                        string line = file.ReadLine();
                        if (!string.IsNullOrWhiteSpace(line))
                        {
                            if (line.Contains("word"))
                            {
                                int index = line.IndexOf('=');
                                turingMachine = new TuringMachine(line.Substring(++index));
                            }
                            else if (turingMachine != null)
                            {
                                string[] parts = line.Split(new char[] { ',', ':' });
                                int current_state = Convert.ToInt32(parts[1]);
                                char current_symbol = parts[0].ToCharArray()[0];

                                char new_symbol = parts[2].ToCharArray()[0];
                                int new_state = Convert.ToInt32(parts[3]);
                                Direct direct = Direct.N;
                                if (parts[4] == "R") direct = Direct.R;
                                else if (parts[4] == "L") direct = Direct.L;

                                turingMachine.AddCommand(current_symbol, current_state, new Command(new_symbol, new_state, direct));
                            }
                        }
                    }

                }
            }
            Console.WriteLine(turingMachine.Execute());
            Console.ReadLine();
        }
    }

}
