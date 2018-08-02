using NetCoreStack.Contracts;
using System;
using System.ComponentModel.DataAnnotations;

namespace Hisar.Component.Pages.Models
{
  
    public class PageCreateOrEditViewModel : CollectionModelBson
    {
        [Required]
        public string Title { get; set; }


        public string[] Tags { get; set; }

        public PageState State { get; set; }

        public string UpdateUrl { get; set; }

        public DateTime Created { get; set; }
        public DateTime? Modified { get; set; }

        public string Content { get; set; }
    }
}
