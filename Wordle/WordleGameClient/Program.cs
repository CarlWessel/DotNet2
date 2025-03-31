using Grpc.Core;
using Grpc.Net.Client;
using Wordle.Protos;

namespace WordleGameClient
{
    internal class Program
    {
        static void Main(string[] args)
        {
            try
            {
                var channel = GrpcChannel.ForAddress("https://localhost:7075");
                var client = new DailyWord.DailyWordClient(channel);


                var request = new GetWordRequest();
                var response = client.GetWord(request);

                string correctWord = response.Word;
                bool isCorrect = false;
                int attempts = 0;
                const int maxAttempts = 6;

                Console.WriteLine(response.Word);

                while (attempts < maxAttempts && !isCorrect)
                {
                    Console.Write($"Guess {attempts + 1}/{maxAttempts}: Enter your guess: ");
                    var guess = Console.ReadLine()?.ToLower();

                    if (string.IsNullOrEmpty(guess) || guess.Length != 5)
                    {
                        Console.WriteLine("Please enter a 5-letter word.");
                        continue;
                    }

                    attempts++;

                    var validateRequest = new ValidateWordRequest {Word = guess };
                    var validateResponse = client.ValidateWord(validateRequest);

                    if (validateResponse.IsValid == true)
                    {
                        isCorrect = true;
                        Console.WriteLine("Congratulations! You guessed the word correctly!");
                    }
                    else
                    {
                        Console.WriteLine("That's not the correct word. Try again.");
                    }
                }
                Console.WriteLine(response.Word);
            }
            catch (RpcException e)
            {
                Console.WriteLine(e.Status.Detail);
            }
        }

    }
}
