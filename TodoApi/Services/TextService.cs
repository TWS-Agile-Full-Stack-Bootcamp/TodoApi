using System;
namespace TodoApi.Services
{
    public class TextService
    {
        private const string ORIGINAL_OUTPUT = "Original Text Service Text";

        public TextService()
        {
        }

        public string GetText()
        {
            return ORIGINAL_OUTPUT;
        }
    }
}
