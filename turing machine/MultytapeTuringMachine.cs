using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static turing_machine.Program;

namespace turing_machine
{
    public class MCommand
    {
        public string symbols;
        public int q;
        public List<Direct> direct;

        public MCommand(string s, int q, List<Direct> d)
        {
            symbols = s;
            this.q = q;
            direct = d;
        }

    }

    class MultytapeTuringMachine
    {
        int _q = 1;
        int _tapesAmount;
        List<List<char>> _words;
        int[] _indexes;
        Dictionary<int, Dictionary<string, MCommand>> _tacts = new Dictionary<int, Dictionary<string, MCommand>>();

        public MultytapeTuringMachine(int tapesAmount, List<List<char>> words)
        {
            _tapesAmount = tapesAmount;
            _words = words;
            _indexes = new int[tapesAmount];
            for (int i = 0; i < _indexes.Length; i++)
            {
                _indexes[i] = 0;
            }
            while (_words.Count < _tapesAmount)
            {
                _words.Add(new List<char>(new char[] { ' ' }));
            }


        }
        public bool AddCommand(string s, int q, MCommand command)
        {
            
            if (!_tacts.ContainsKey(q)) _tacts.Add(q, new Dictionary<string, MCommand>());
            if (!_tacts[q].ContainsKey(s))
            {
                _tacts[q].Add(s, command);
                return true;
            }
            return false;
        }

        void ExecuteCommand(MCommand command)
        {
            for (int i = 0; i < _words.Count; i++)
            {
                _words[i][_indexes[i]] = command.symbols.ToCharArray()[i];
                if (_words[i].Count == 1)
                {
                    _words[i].Insert(0, ' ');
                    _indexes[i]++;
                    _words[i].Add(' ');
                }
                switch (command.direct[i])
                {
                    case Direct.R:
                        _indexes[i]--;
                        if (_indexes[i] == 0)
                        {
                            _words[i].Insert(0, ' ');
                            _indexes[i]++;
                        }                       
                        break;
                    case Direct.L:
                        _indexes[i]++;
                        if (_indexes[i] == _words[i].Count) _words[i].Add(' ');
                        break;
                    case Direct.N:
                        break;
                    default:
                        throw new Exception("invalid direct");
                }
            }
            _q = command.q;
            if (_q == 0) return;
            List<char> word = new List<char>();
            for (int i = 0; i < _words.Count; i++)
            {
                word.Add(_words[i][_indexes[i]]);
            }

            ExecuteCommand(_tacts[_q][new string(word.ToArray())]);
        }

        static public void StartFromFile(string filePath)
        {
            MultytapeTuringMachine multytapeTuringMachine = null;
            int tapesAmount = 0;
            List<List<char>> words = new List<List<char>>();
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
                            if (line.ToLower().Contains("numbers ="))
                            {
                                int index = line.IndexOf('=');
                                tapesAmount = int.Parse(line.Substring(++index));
                            }
                            else if (line.Contains("word="))
                            {
                                int index = line.IndexOf('=');
                                words.Add(new List<char>(line.Substring(++index).ToCharArray()));
                            }
                            else if (multytapeTuringMachine == null)
                            {
                                multytapeTuringMachine = new MultytapeTuringMachine(tapesAmount, words);
                            }
                            if (multytapeTuringMachine != null)
                            {
                                string[] parts = line.Split(new char[] { ',', '-' });

                                int current_state = Convert.ToInt32(parts[0]);
                                string current_symbols = parts[1];

                                string new_symbols = parts[3];
                                int new_state = Convert.ToInt32(parts[2]);
                                List<Direct> directs = new List<Direct>();
                                foreach (var direct in parts[4].ToCharArray())
                                {
                                    if (direct == 'N') directs.Add(Direct.N);
                                    else if (direct == 'R') directs.Add(Direct.R);
                                    else if (direct == 'L') directs.Add(Direct.L);
                                }

                                multytapeTuringMachine.AddCommand(current_symbols, current_state,
                                        new MCommand(new_symbols, new_state, directs));

                            }
                        }
                    }
                }
            }
            Console.WriteLine(multytapeTuringMachine.Execute());
            Console.ReadLine();
        }

        private string Execute()
        {
            List<char> word = new List<char>();
            for (int i = 0; i < _words.Count; i++)
            {
                word.Add(_words[i][_indexes[i]]);
            }
            ExecuteCommand(_tacts[_q][new string(word.ToArray())]);

            List<string> words = new List<string>();
            for (int i = 0; i < _words.Count; i++)
            {
                words.Add(new string(_words[i].ToArray()));
            }

            return string.Join(Environment.NewLine, words.ToArray());
        }
    }
}
