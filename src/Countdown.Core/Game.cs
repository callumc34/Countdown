namespace Countdown.Core
{
    public class Game
    {
        /// <summary>
        /// Minimum number of vowels and consonants required for a valid word.
        /// </summary>
        private const int MIN_VOWELS = 3;

        /// <summary>
        /// Minimum number of consonants required for a valid word.
        /// </summary>
        private const int MIN_CONSONANTS = 4;

        /// <summary>
        /// File containing a list of valid words.
        /// </summary>
        private string WORD_FILE = "data/words.txt";

        /// <summary>
        /// Current score.
        /// </summary>
        private int score;

        /// <summary>
        /// Current list of letters.
        /// </summary>
        private List<char> letters;

        /// <summary>
        /// Solver for finding optimal answers and validating words.
        /// </summary>
        private Solver solver;

        /// <summary>
        /// Random number generator for selecting letters.
        /// </summary>
        private static readonly Random random = new Random();

        /// <summary>
        /// Vowel frequencies from https://en.wikipedia.org/wiki/Letter_frequency
        /// </summary>
        private static readonly (char letter, int weight)[] vowelWeights =
        [
            ('a', 8), ('e', 13), ('i', 7), ('o', 8), ('u', 3)
        ];

        /// <summary>
        /// Consonant frequencies from https://en.wikipedia.org/wiki/Letter_frequency
        /// </summary>
        private static readonly (char letter, int weight)[] consonantWeights =
        [
            ('b', 2), ('c', 3), ('d', 4), ('f', 2), ('g', 2),
            ('h', 6), ('j', 1), ('k', 1), ('l', 4), ('m', 2),
            ('n', 7), ('p', 2), ('q', 1), ('r', 6), ('s', 6),
            ('t', 9), ('v', 1), ('w', 2), ('x', 1), ('y', 2), ('z', 1)
        ];

        /// <summary>
        /// Create a new game with a score of 0 and an empty list of letters.
        /// </summary>
        public Game()
        {
            score = 0;
            letters = new List<char>();
            solver = new Solver(WORD_FILE);
        }

        /// <summary>
        /// List of available letters.
        /// </summary>
        public List<char> Letters
        {
            get { return letters; }
        }

        /// <summary>
        /// Current score.
        /// </summary>
        public int Score
        {
            get { return score; }
        }

        /// <summary>
        /// Start a new round by clearing the list of letters.
        /// </summary>
        public void NextRound()
        {
            letters.Clear();
        }

        /// <summary>
        /// Get the score for a word, adding it to the total if valid.
        /// </summary>
        /// <param name="word"></param>
        /// <returns></returns>
        /// <exception cref="IllegalAnswerException"></exception>
        public int Answer(string word)
        {
            if (word.Length > letters.Count)
            {
                throw new IllegalAnswerException("Cannot use more letters than are available.");
            }

            // Validate the word uses the letters
            var available = new Dictionary<char, int>();
            foreach (char c in letters)
            {
                char lower = char.ToLower(c);
                if (available.ContainsKey(lower))
                {
                    available[lower]++;
                }
                else
                {
                    available[lower] = 1;
                }
            }
            foreach (char c in word.ToLower())
            {
                if (!available.ContainsKey(c) || available[c] == 0)
                {
                    throw new IllegalAnswerException("Submitted word uses unavailable letters.");
                }
                available[c]--;
            }

            if (!solver.IsValidWord(word))
            {
                throw new IllegalAnswerException("Word is not in the dictionary.");
            }

            int value = word.Length;

            // Double points for max length words
            if (word.Length == Solver.MAX_LENGTH)
            {
                value += Solver.MAX_LENGTH;
            }

            score += value;
            return value;
        }

        /// <summary>
        /// Finds the best answer for a set of letters.
        /// </summary>
        /// <returns></returns>
        public string BestAnswer()
        {
            string key = new string(letters.ToArray());
            return solver.NearestSolution(key)[0];
        }

        /// <summary>
        /// Add a consonant to the list of available letters.
        /// </summary>
        public void AddConsonant()
        {
            char letter = GetRandomLetter(consonantWeights);
            AddLetter(letter);
        }

        /// <summary>
        /// Add a vowel to the list of available letters.
        /// </summary>
        public void AddVowel()
        {
            char letter = GetRandomLetter(vowelWeights);
            AddLetter(letter);
        }

        /// <summary>
        /// Add a random letter to the list of available letters.
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        private bool IsVowel(char c)
        {
            return "aeiou".IndexOf(c) >= 0;
        }

        /// <summary>
        /// Add a letter to the list of available letters.
        /// </summary>
        /// <param name="letter"></param>
        /// <exception cref="LetterLimitException"></exception>
        /// <exception cref="InsufficientSlotsException"></exception>
        private void AddLetter(char letter)
        {
            if (letters.Count >= Solver.MAX_LENGTH)
            {
                throw new LetterLimitException(Solver.MAX_LENGTH);
            }

            int vowelCount = letters.Count(ch => IsVowel(ch));
            int consonantCount = letters.Count - vowelCount;
            int remainingSlots = Solver.MAX_LENGTH - letters.Count;

            // Assert the letters follow the right format
            if (IsVowel(letter))
            {
                int neededConsonants = Math.Max(0, MIN_CONSONANTS - consonantCount);
                if (remainingSlots - 1 < neededConsonants)
                {
                    throw new InsufficientSlotsException("Cannot add vowel as it would leave insufficient slots for the required consonants.");
                }
            }
            else
            {
                int neededVowels = Math.Max(0, MIN_VOWELS - vowelCount);
                if (remainingSlots - 1 < neededVowels)
                {
                    throw new InsufficientSlotsException("Cannot add consonant as it would leave insufficient slots for the required vowels.");
                }
            }

            letters.Add(letter);
        }

        /// <summary>
        /// Get a random letter based on the given weights, used to add a new letter using consonant or vowel weights.
        /// </summary>
        /// <param name="letterWeights"></param>
        /// <returns></returns>
        private char GetRandomLetter((char letter, int weight)[] letterWeights)
        {
            int totalWeight = letterWeights.Sum(lw => lw.weight);
            int randomValue = random.Next(totalWeight);
            foreach (var lw in letterWeights)
            {
                if (randomValue < lw.weight)
                {
                    return lw.letter;
                }
                randomValue -= lw.weight;
            }
            return letterWeights.Last().letter;
        }
    }
}
