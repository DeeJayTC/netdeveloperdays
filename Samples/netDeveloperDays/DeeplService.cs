using DeepL;

namespace netDeveloperDays
{
    public class DeepLService
    {
        private Translator translator { get; set; }
        public DeepLService(IConfiguration config)
        {
            translator = new Translator(config.GetValue<string>("DeepLApiKey"));
        }


        public async Task<string> TranslateString(string[] input, string sourceLanguage, string targetLanguage, TextTranslateOptions options, CancellationToken cancellationToken = new CancellationToken())
        {
            var result = await translator.TranslateTextAsync(input, sourceLanguage, targetLanguage, options, cancellationToken);
            return result[0].Text;
        }

    }
}