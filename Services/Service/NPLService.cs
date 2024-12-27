using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace e_commerce.Services.Service
{
    public class NPLService
    {
        static string NormalizeText(string input)
        {
            return input.Replace("@", "a")
                        .Replace("$", "s")
                        .Replace("1", "i")
                        .Replace("!", "i")
                        .Replace("0", "o")
                        .Replace("5", "s")
                        .Replace("3", "e")
                        .Replace("4", "a")
                        .Replace("v", "u");
        }
        private static List<string> FindProfaneWords(string text, List<string> swearWords)
        {
            var wordsInText = text.Split(new[] { ' ', '.', ',', ';', '!', '?' }, StringSplitOptions.RemoveEmptyEntries);
            List<string> detectedProfanity = new List<string>();

            foreach (var word in wordsInText)
            {
                if (swearWords.Contains(word))
                {
                    detectedProfanity.Add(word);
                }
            }

            return detectedProfanity;
        }
        public static bool IsProfane(string reviewText)
        {
            var assembly = Assembly.GetExecutingAssembly();
            var resourceName = "e_commerce.Resources.swearwords.txt";
            using var stream = assembly.GetManifestResourceStream(resourceName);
            using var reader = new StreamReader(stream);


            List<string> swearWords = reader.ReadToEnd().Split('\n').Select(s => s.Trim()).ToList();

            // Check for profanity in the review
            var profaneWords = FindProfaneWords(reviewText, swearWords);

            if (profaneWords.Any())
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public static async Task<bool> IsToxic(string reviewBody)
        {
            var client = new HttpClient();
            client.DefaultRequestHeaders.Add("Authorization", "Bearer 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");

            var input = new
            {
                inputs = NormalizeText(reviewBody)
            };

            var json = JsonConvert.SerializeObject(input);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PostAsync("https://api-inference.huggingface.co/models/s-nlp/roberta_toxicity_classifier", content);
            var results = await response.Content.ReadAsStringAsync();

            var classifications = JsonConvert.DeserializeObject<List<List<ClassificationResult>>>(results);

            double toxicScore = 0;

            foreach (var result in classifications[0])
            {
                if (result.Label == "toxic")
                {
                    toxicScore = result.Score;
                    break;
                }
            }

            return toxicScore >= 0.85;
        }
    }

    internal class ClassificationResult
    {
        [JsonPropertyName("label")]
        public string Label;

        [JsonPropertyName("score")]
        public double Score;
    }
}
