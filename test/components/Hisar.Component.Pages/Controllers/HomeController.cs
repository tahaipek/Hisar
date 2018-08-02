using Hisar.Component.Pages.DatabaseModel;
using Hisar.Component.Pages.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using NetCoreStack.Contracts;
using NetCoreStack.Hisar.Extensions;
using NetCoreStack.Hisar.Server;
using NetCoreStack.Mvc;
using NetCoreStack.Mvc.Extensions;
using System;
using System.Linq;

namespace Hisar.Component.Pages.Controllers
{
    public class HomeController : HisarControllerServerBase
    {
        private readonly IUrlHelperFactory _urlHelper;

        public HomeController(IUrlHelperFactory urlHelper)
        {
            _urlHelper = urlHelper;
        }

        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Create()
        {
            return View("CreateOrEdit", new PageCreateOrEditViewModel());
        }

        public IActionResult Edit(string id)
        {
            var repo = UnitOfWork.Repository<Page>();
            var page = repo.FirstOrDefault(k => k.Id == id);
            if (page != null)
            {
                return View("CreateOrEdit", new PageCreateOrEditViewModel
                {
                    Id = page.Id,
                    Title = page.Title,
                    Content = page.Content,
                    State = page.State,
                    Created = page.Created,
                    Modified = page.Modified,
                    Tags = page.Tags,

                });
            }

            return NotFound();
        }

        [HttpPost]
        public IActionResult CreateOrEdit([FromBody]PageCreateOrEditViewModel viewModel)
        {
            if (!ModelState.IsValid)
                return Json(new WebResult(resultState: ResultState.Error));

            var repo = UnitOfWork.Repository<Page>();
            if (viewModel.IsNew)
            {
                var newPage = new Page
                {
                    Created = DateTime.Now,
                    ObjectState = ObjectState.Added,
                    State = viewModel.State,
                    Title = viewModel.Title,
                    Content = viewModel.Content,
                    Tags = viewModel.Tags,
                };
                repo.SaveAllChanges(newPage);

            }
            else
            {
                var data = repo.First(k => k.Id == viewModel.Id);
                data.Modified = DateTime.Now;
                data.ObjectState = ObjectState.Modified;
                data.State = viewModel.State;
                data.Title = viewModel.Title;
                data.Content = viewModel.Content;
                data.Tags = viewModel.Tags;

                repo.SaveAllChanges(data);
            }

            return Json(new WebResult(redirectUrl: Url.Action(nameof(Index), nameof(HomeController).ControllerName())));

        }


        public IActionResult GetPages(CollectionRequest request)
        {
            //var updateUrl = _urlHelper.GetUrlHelper(ControllerContext).ComponentContent(Url.Action(nameof(Edit), nameof(HomeController).ControllerName()));
            var updateUrl = Url.Action(nameof(Edit), nameof(HomeController).ControllerName());
            var repo = UnitOfWork.Repository<Page>();
            var list = repo.Select(k => new GetPagesListItem
            {
                Id = k.Id,
                Title = k.Title,
                State = k.State,
                UpdateUrl = updateUrl,
                Created = k.Created,
                Modified = k.Modified,

            });

            return Json(list.ToCollectionResult(request, topOrderPropertyName: nameof(Page.Created), sortDirection: ListSortDirection.Descending));
        }
    }
}
