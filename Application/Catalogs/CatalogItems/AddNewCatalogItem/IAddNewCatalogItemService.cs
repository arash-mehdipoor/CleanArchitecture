using Application.Dtos;
using Application.Interfaces.Context;
using AutoMapper;
using Domain.Catalogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Catalogs.CatalogItems.AddNewCatalogItem
{
    public interface IAddNewCatalogItemService
    {
        BaseDto<int> Execute(AddNewCatalogItemDto request);
    }

    public class AddNewCatalogItemService : IAddNewCatalogItemService
    {
        private readonly IDatabaseContext _context;
        private readonly IMapper _mapper;

        public AddNewCatalogItemService(IDatabaseContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public BaseDto<int> Execute(AddNewCatalogItemDto request)
        {
            var catalogItem = _mapper.Map<CatalogItem>(request);
            _context.CatalogItems.Add(catalogItem);
            _context.SaveChanges();
            return new BaseDto<int>(true, new List<string> { "با موفقیت ثبت شد" }, catalogItem.Id);
        }
    }
}
