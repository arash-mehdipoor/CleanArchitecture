using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Catalogs.CatalogItems.AddNewCatalogItem;
using Application.Catalogs.CatalogItems.CatalogItemService;
using Application.Dtos;
using Infrastructure.ExternalApi.ImageServer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Admin.EndPoint.Pages.CatalogItem
{
    public class CreateModel : PageModel
    {
        private readonly IAddNewCatalogItemService _addNewCatalogItemService;
        private readonly ICatalogItemService _catalogItemService;
        private readonly IImageUploadService _imageUploadService;

        public CreateModel(IAddNewCatalogItemService addNewCatalogItemService,
            ICatalogItemService catalogItemService,
            IImageUploadService imageUploadService
            )
        {
            _addNewCatalogItemService = addNewCatalogItemService;
            _catalogItemService = catalogItemService;
            _imageUploadService = imageUploadService;
        }
        [BindProperty]
        public AddNewCatalogItemDto Data { get; set; }
        public SelectList Categories { get; set; }
        public SelectList Brands { get; set; }
        public List<IFormFile> Files { get; set; }
        public void OnGet()
        {
            Categories = new SelectList(_catalogItemService.GetCatalogType(), "Id", "Type");
            Brands = new SelectList(_catalogItemService.GetBrand(), "Id", "Brand");
        }

        public JsonResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(e => e.Errors);
                return new JsonResult(new BaseDto<int>(false, errors.Select(p => p.ErrorMessage).ToList(), 0));
            }

            for (int i = 0; i < Request.Form.Files.Count; i++)
            {
                var file = Request.Form.Files[i];
                Files.Add(file);
            }

            List<AddNewCatalogItemImage_Dto> images = new List<AddNewCatalogItemImage_Dto>();
            if (Files.Count > 0)
            {
                var imageAddress = _imageUploadService.Upload(Files);
                foreach (var item in imageAddress)
                {
                    images.Add(new AddNewCatalogItemImage_Dto() { Src = item });
                }
            }
            Data.Images = images;
            var resultService = _addNewCatalogItemService.Execute(Data);
            return new JsonResult(resultService);
        }
    }
}
