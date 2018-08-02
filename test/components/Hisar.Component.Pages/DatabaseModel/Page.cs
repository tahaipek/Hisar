using NetCoreStack.Contracts;
using NetCoreStack.Data.Contracts;
using System;
using System.ComponentModel.DataAnnotations;

namespace Hisar.Component.Pages.DatabaseModel
{
    [CollectionName("Pages")]
    public class Page : EntityIdentityBson
    {
        public Page()
        {
            Created = DateTime.Now;
        }

        [Required]
        public string Title { get; set; }

        [Required]
        public string Content { get; set; }

        public string[] Tags { get; set; }

        [Required]
        public PageState State { get; set; }

        public DateTime Created { get; set; }
        public DateTime? Modified { get; set; }
    }
}