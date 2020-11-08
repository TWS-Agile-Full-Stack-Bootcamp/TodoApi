using System;
namespace TodoApi.Services
{
    public class ShowService
    {
        private readonly TextService textService;

        public ShowService(TextService textService)
        {
            this.textService = textService;
        }

        public virtual string GetShowLabel()
        {
            return textService.GetText();
        }
    }
}
