using Wordle.Protos;
using System.Text.Json;
using Grpc.Core;
using System;

namespace Wordle.Services
{
    public class WordServer : DailyWord.DailyWordBase
    {
        private readonly List<string> words;
        private string dailyWord;

        public WordServer()
        {
            //string jsonPath = "wordle.json";
            string jsonPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "wordle.json");
            words = JsonSerializer.Deserialize<List<string>>(File.ReadAllText(jsonPath));

            int dailyVal = int.Parse(DateTime.UtcNow.ToString("yyyyMMdd"));

            Random random = new Random(dailyVal);

            dailyWord = words[random.Next(words.Count)];
        }

        public override Task<GetWordResponse> GetWord(GetWordRequest request, ServerCallContext context)
        {
            return Task.FromResult(GetDailyWord());
        }

        public override Task<ValidateWordResponse> ValidateWord(ValidateWordRequest request, ServerCallContext context)
        {
            return Task.FromResult(validateWordResponse(request.Word));
        }

        //Helper methods
        private GetWordResponse GetDailyWord()
        {
            return new GetWordResponse { Word = dailyWord };
        }

        private ValidateWordResponse validateWordResponse(string word)
        {
            return new ValidateWordResponse { IsValid = string.Equals(word, dailyWord) };
        }
    }
}
