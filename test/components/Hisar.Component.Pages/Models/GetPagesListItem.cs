using NetCoreStack.Contracts;
using System;

namespace Hisar.Component.Pages.Models
{
    public class GetPagesListItem : CollectionModelBson
    {
        [PropertyDescriptor(EnableFilter = true, IsSelectable = true)]
        public string Title { get; set; }

        [PropertyDescriptor(EnableFilter = false, IsSelectable = true)]
        public string Content { get; set; }

        [PropertyDescriptor(EnableFilter = false, IsSelectable = true)]
        public string[] Tags { get; set; }

        [PropertyDescriptor(EnableFilter = true, IsSelectable = true)]
        public PageState State { get; set; }

        [PropertyDescriptor(EnableFilter = false)]
        public string UpdateUrl { get; set; }


        public DateTime Created { get; set; }
        public DateTime? Modified { get; set; }
    }
}
