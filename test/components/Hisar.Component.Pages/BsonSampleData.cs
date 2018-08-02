using Hisar.Component.Pages;
using Hisar.Component.Pages.DatabaseModel;
using System;

namespace Hisar.Component.Guideline.Models
{
    public static class BsonSampleData
    {
        public static Page[] GetPages()
        {
            var pageBson = new Page[] {
                new Page { Created = DateTime.Now, Title = "Worth A Thousand Words", State = PageState.Preview, Tags = new string[]{ "Demo","Page"}, Content = @"<div>Hello world!</div>" }
            };

            return pageBson;
        }

    }
}
