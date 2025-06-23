using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace GorillaBrainFuck.Types
{
    public class Interpreter
    {
        private byte[] memory;
        private int pointer;
        private TextReader input;
        private TextWriter output;
        private const int MaxIterations = 1000000;

        public Interpreter(TextReader input = null, TextWriter output = null)
        {
            memory = new byte[30000];
            pointer = 0;
            this.input = input ?? Console.In;
            this.output = output ?? Console.Out;
        }

        public async Task<string> Execute(string code)
        {
            StringBuilder result = new StringBuilder();
            Dictionary<int, int> bracketPairs = PrecomputeBracketPairs(code);
            int iterationCount = 0;

            for (int i = 0; i < code.Length; i++)
            {
                if ("<>+-.,[]".IndexOf(code[i]) != -1)
                {
                    if (++iterationCount > MaxIterations)
                        throw new InvalidOperationException("stop looping you not kind person");
                }
                else
                {
                    continue;
                }

                try
                {
                    switch (code[i])
                    {
                        case '>': pointer = (pointer + 1) % memory.Length; break;
                        case '<': pointer = (pointer - 1 + memory.Length) % memory.Length; break;
                        case '+': memory[pointer]++; break;
                        case '-': memory[pointer]--; break;
                        case '.':
                            char outputChar = (char)memory[pointer];
                            await output.WriteAsync(outputChar);
                            result.Append(outputChar);
                            break;
                        case ',':
                            char[] buffer = new char[1];
                            int read = await input.ReadAsync(buffer, 0, 1);
                            if (read > 0)
                                memory[pointer] = (byte)buffer[0];
                            else
                                memory[pointer] = 0;
                            break;
                        case '[':
                            if (memory[pointer] == 0 && bracketPairs.TryGetValue(i, out int jumpTo))
                            {
                                i = jumpTo;
                                iterationCount--;
                            }
                            break;
                        case ']':
                            if (memory[pointer] != 0 && bracketPairs.TryGetValue(i, out int jumpBack))
                            {
                                i = jumpBack;
                                iterationCount--;
                            }
                            break;
                    }
                }
                catch (Exception ex)
                {
                    throw new InvalidOperationException($"error '{code[i]}' at position {i}: {ex.Message}");
                }
            }

            return result.ToString();
        }

        private Dictionary<int, int> PrecomputeBracketPairs(string code)
        {
            Dictionary<int, int> pairs = new Dictionary<int, int>();
            Stack<int> openBrackets = new Stack<int>();

            for (int i = 0; i < code.Length; i++)
            {
                if (code[i] == '[')
                {
                    openBrackets.Push(i);
                }
                else if (code[i] == ']')
                {
                    if (openBrackets.Count == 0)
                        throw new InvalidOperationException("missing opening bracket");

                    int openPos = openBrackets.Pop();
                    pairs[openPos] = i;
                    pairs[i] = openPos;
                }
            }

            if (openBrackets.Count > 0)
                throw new InvalidOperationException("missing closing bracket");

            return pairs;
        }

        public void Reset()
        {
            Array.Clear(memory, 0, memory.Length);
            pointer = 0;
        }

        public async Task<string> ExecuteFile(string filePath)
        {
            string code = await File.ReadAllTextAsync(filePath);
            return await Execute(code);
        }
    }
}
