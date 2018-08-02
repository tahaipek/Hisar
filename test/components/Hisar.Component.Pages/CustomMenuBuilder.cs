using Hisar.Component.Pages;
using Microsoft.AspNetCore.Mvc;
using NetCoreStack.Hisar;
using System.Collections.Generic;

namespace Hisar.Component.Pages
{
    public class CustomMenuBuilder : DefaultMenuItemsBuilder<Startup>
    {
        public override IEnumerable<IMenuItem> Build(IUrlHelper urlHelper)
        {
            var items = new List<IMenuItem>
            {
                new DefaultMenuItem
                {
                    Icon = FontAwesomeIcon.File,
                    Order = 1,
                    Path = ResolvePath(urlHelper, "~/"),
                    ShowInMenu = true,
                    Text = "Pages"
                }
            };

            return items;
        }
    }
}
