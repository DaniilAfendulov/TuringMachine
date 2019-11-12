using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static turing_machine.Program;

namespace turing_machine
{
    class MultytapeTuringMachine
    {
        int _tapesAmount;
        TuringMachine[] _turingMachines;

        public MultytapeTuringMachine(int tapesAmount, List<TuringMachine> turingMachines)
        {
            _tapesAmount = tapesAmount;
            _turingMachines = turingMachines.ToArray();
        }
        public void AddCommand(int tapeNumber, char s, int q, Command command)
        {
            _turingMachines[tapeNumber].AddCommand(s, q, command);
        }

        public string Execute()
        {
            
            return _
        }

        publiс void ExecuteCommand()
        {

        }

        static public void StartFromFile(string filePath)
        {
            MultytapeTuringMachine multytapeTuringMachine = null;
            int tapesAmount = 0;
            List<TuringMachine> turingMachines = new List<TuringMachine>();
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
                            if (line.Contains("Numbers ="))
                            {
                                int index = line.IndexOf('=');
                                tapesAmount = int.Parse(line.Substring(++index));
                            }
                            else if (line.Contains("word  ="))
                            {
                                int index = line.IndexOf('=');
                                turingMachines.Add(new TuringMachine(line.Substring(++index)));
                            }
                            else if (multytapeTuringMachine == null)
                            {
                                multytapeTuringMachine = new MultytapeTuringMachine(tapesAmount, turingMachines);
                            }
                            if (multytapeTuringMachine != null)
                            {
                                string[] parts = line.Split(new char[] { ',', ':' });

                                int current_state = Convert.ToInt32(parts[1]);
                                char[] current_symbols = parts[0].ToCharArray();

                                char[] new_symbols = parts[2].ToCharArray();
                                int new_state = Convert.ToInt32(parts[3]);
                                List<Direct> directs = new List<Direct>();
                                foreach (var direct in parts[4].ToCharArray())
                                {
                                    if (direct == 'N') directs.Add(Direct.N);
                                    else if (direct == 'R') directs.Add(Direct.R);
                                    else if (direct == 'L') directs.Add(Direct.L);
                                }

                                for (int i = 0; i < tapesAmount; i++)
                                {
                                    multytapeTuringMachine.AddCommand(i, new_symbols[i], new_state,
                                        new Command(new_symbols[i], new_state, directs[i]));
                                }                                
                            }
                        }
                    }
                }
            }
            Console.WriteLine(multytapeTuringMachine.Execute());
            Console.ReadLine();
        }
    }
}
