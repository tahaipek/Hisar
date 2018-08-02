using Hisar.Component.Guideline.Models;
using Hisar.Component.Pages.DatabaseModel;
using Microsoft.Extensions.DependencyInjection;
using NetCoreStack.Data.Interfaces;
using NetCoreStack.Mvc.Helpers;
using System;

namespace Hisar.Component.Pages
{
    public static class BsonDataInitializer
    {
        public static void InitializePagesMongoDb(IServiceProvider serviceProvider)
        {
            if (NetworkHelper.ConnectionCheck(27017))
            {
                using (var db = serviceProvider.GetService<IMongoDbDataContext>())
                {
                    //db.MongoDatabase.DropCollection(db.CollectionNameSelector.GetCollectionName<Page>());
                    //var pages = BsonSampleData.GetPages();
                    //db.Collection<Page>().InsertMany(pages);
                }
            }
        }
    }
}