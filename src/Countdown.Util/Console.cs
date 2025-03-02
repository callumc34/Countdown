namespace Countdown.Util
{
    public class Console
    {
        /// <summary>
        /// Read a line from the console with a maximum time limit.
        /// </summary>
        /// <param name="seconds"></param>
        /// <returns></returns>
        public static async Task<string> ReadLineWithTimeout(int seconds)
        {
            using var cts = new CancellationTokenSource();
            var inputTask = Task.Run(() => System.Console.ReadLine(), cts.Token);
            if (await Task.WhenAny(inputTask, Task.Delay(seconds * 1000)) == inputTask)
            {
                cts.Cancel();
                return inputTask.Result;
            }
            return null;
        }
    }
}
