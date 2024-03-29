﻿using FarmFresh.Backend.DataTransferObjects;
using FarmFresh.Backend.Shared;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
namespace FarmFresh.Backend.Services.Interfaces
{
    public interface ICustomerServices
    {
        Task<ProductDto> GetProduct(Guid productId);
        Task<BaseListOutput<ProductDto>> GetProducts(ProductListInput input);
        Task<IEnumerable<string>> GetProductCategories(BaseListInput input);
        Task<BaseListOutput<OrderHistoryDto>> GetOrderHistories(Guid userId, BaseListInput input);
        Task MakeAnOrder(OrderHistoryDto dto);
    }
}
