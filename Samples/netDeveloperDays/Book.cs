using DeepL;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TCDev.APIGenerator.Attributes;
using TCDev.APIGenerator.Data;
using TCDev.APIGenerator.Events;
using TCDev.APIGenerator.Hooks;
using TCDev.APIGenerator.Interfaces;

namespace netDeveloperDays
{

    public class Author : IObjectBase<Guid>
    {

        public Guid Id { get; set; }
        [EmailAddress]
        public string Name { get; set; }
        public virtual List<Book>? Books { get; set; }

    }

    [Api("/books")]
    [Event(AMQPEvents.All,"books", "NetDD")]
    [Cachable("sdafasdf_{0}")]
    public class Book : IObjectBase<Guid>, IBeforeCreate<Book>
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public DateTime CreatedDate { get; set; }
        public string Description { get; set; }
        public string DescriptionPL { get; set; }

        public Guid AuthorId { get; set; }
        public virtual Author? Author { get; set; }

        public Task<Book> BeforeCreate(Book newItem, 
            IApplicationDataService<GenericDbContext, AuthDbContext> data)
        {
            newItem.Title = $"ThisIsMyFancyTitleUpdate -> {newItem.Title}";

            DeepLService? translator = data.Context.RequestServices.GetService(typeof(DeepLService)) as DeepLService;
            if (translator != null)
            {
                var translation = translator.TranslateString(new string[] { newItem.Description }, LanguageCode.English, LanguageCode.Polish, null).Result;
                newItem.DescriptionPL = translation;
            }

            return Task.FromResult(newItem);
        }

    }
}
