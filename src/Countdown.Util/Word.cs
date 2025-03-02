namespace Countdown.Util
{
    public class Word
    {
        /// <summary>
        /// Sort a word alphabetically.
        /// </summary>
        /// <param name="word"></param>
        /// <returns></returns>
        public static string Sort(string word)
        {
            char[] letters = word.ToCharArray();
            Array.Sort(letters);
            return new string(letters);
        }

        /// <summary>
        /// Generate all permutations of a word with a given number of letters removed.
        /// </summary>
        /// <param name="word"></param>
        /// <param name="remove"></param>
        /// <returns></returns>
        public static IEnumerable<string> Permutations(string word, int remove)
        {
            if (remove == 0)
            {
                yield return word;
            }
            else
            {
                for (int i = 0; i < word.Length; i++)
                {
                    string next = word.Remove(i, 1);
                    foreach (string permutation in Permutations(next, remove - 1))
                    {
                        yield return permutation;
                    }
                }
            }
        }
    }
}
