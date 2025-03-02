namespace Countdown.Core
{
    using Countdown.Util;

    public class Solver
    {
        /// <summary>
        /// Max supported length.
        /// </summary>
        public const int MAX_LENGTH = 9;

        /// <summary>
        /// Dictionary of solutions for a sorted string key.
        /// </summary>
        private Dictionary<string, List<string>> solutions;

        /// <summary>
        /// Construct a new solver from a text file of words.
        /// </summary>
        /// <param name="file"></param>
        /// <exception cref="FileNotFoundException"></exception>
        public Solver(string file)
        {
            if (!File.Exists(file))
            {
                throw new FileNotFoundException($"File not found: {file}");
            }

            solutions = new Dictionary<string, List<string>>();

            // Sort each word alphabetically to group together anagrams
            foreach (string line in File.ReadLines(file))
            {
                string word = line.Trim();
                if (string.IsNullOrEmpty(word))
                {
                    continue;
                }

                string key = Word.Sort(word);

                if (!solutions.ContainsKey(key))
                {
                    solutions[key] = [];
                }
                solutions[key].Add(word);
            }
        }

        /// <summary>
        /// Check if a word is known.
        /// </summary>
        /// <param name="word"></param>
        /// <returns></returns>
        public bool IsValidWord(string word)
        {
            string sorted = Word.Sort(word);
            return solutions.ContainsKey(sorted) && solutions[sorted].Contains(word);
        }

        /// <summary>
        /// Get a list of solutions for a word.
        /// </summary>
        /// <param name="word"></param>
        /// <returns></returns>
        public List<string> Solution(string word)
        {
            if (word.Length > MAX_LENGTH)
            {
                return [];
            }

            string key = Word.Sort(word);
            if (solutions.ContainsKey(key))
            {
                return solutions[key];
            }
            else
            {
                return [];
            }
        }

        /// <summary>
        /// Find the closest solution for a word.
        /// </summary>
        /// <param name="word"></param>
        /// <returns></returns>
        public List<string> NearestSolution(string word)
        {
            List<string> nearest = [];
            for (int i = 0; i < MAX_LENGTH; i++)
            {
                foreach (string permutation in Word.Permutations(word, i))
                {
                    nearest = Solution(permutation);
                    if (nearest.Count > 0)
                    {
                        return nearest;
                    }
                }
            }

            return nearest;
        }
    }
}
