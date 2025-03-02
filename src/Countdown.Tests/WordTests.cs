namespace Countdown.Tests
{
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Countdown.Util;

    [TestClass]
    public class WordTests
    {
        [TestMethod]
        public void Sort_SortsLettersCorrectly()
        {
            Assert.AreEqual("abc", Word.Sort("cba"));
        }

        [TestMethod]
        public void Sort_EmptyString_ReturnsEmpty()
        {
            Assert.AreEqual("", Word.Sort(""));
        }

        [TestMethod]
        public void Sort_SortsComplexWord()
        {
            Assert.AreEqual("aaabnn", Word.Sort("banana"));
        }

        [TestMethod]
        public void Permutations_Remove0_ReturnsOriginalWord()
        {
            var result = Word.Permutations("hello", 0).ToList();
            Assert.AreEqual(1, result.Count);
            Assert.AreEqual("hello", result[0]);
        }

        [TestMethod]
        public void Permutations_Remove1_FromABC_ReturnsExpected()
        {
            var expected = new List<string> { "bc", "ac", "ab" };
            var result = Word.Permutations("abc", 1).ToList();
            CollectionAssert.AreEqual(expected, result);
        }

        [TestMethod]
        public void Permutations_Remove2_FromABC_ReturnsExpected()
        {
            var expected = new List<string> { "c", "b", "c", "a", "b", "a" };
            var result = Word.Permutations("abc", 2).ToList();
            CollectionAssert.AreEqual(expected, result);
        }

        [TestMethod]
        public void Permutations_WithDuplicates_Remove1_ReturnsExpected()
        {
            var expected = new List<string> { "ab", "ab", "aa" };
            var result = Word.Permutations("aab", 1).ToList();
            CollectionAssert.AreEqual(expected, result);
        }
    }
}
