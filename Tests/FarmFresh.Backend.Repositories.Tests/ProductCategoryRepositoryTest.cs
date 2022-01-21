using FarmFresh.Backend.Entities;
using FarmFresh.Backend.Repositories.Interfaces;
using FarmFresh.Backend.Shared;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace FarmFresh.Backend.Repositories.Tests
{
    [Trait("Repo","ProductCategory")]
    public class ProductCategoryRepositoryTest
    {
        private readonly IProductCategoryRepository _repository;

        public ProductCategoryRepositoryTest(IProductCategoryRepository repository)
        {
            _repository = repository;
        }

        [Fact]
        public async Task BulkInsertAndGetAllTest()
        {
            List<AppProductCategory> entities = new List<AppProductCategory>
            {
                new AppProductCategory
                {
                    Id = Guid.Parse("72fb7f50-7020-431b-9e43-1712796c83ce"),
                    Name = "Fruit & Veg"
                },
                new AppProductCategory
                {
                    Id = Guid.Parse("6eef1f09-1659-4151-a407-a1909645b84c"),
                    Name = "Meat & Seafood"
                },
                new AppProductCategory
                {
                    Id = Guid.Parse("fb3ae426-935c-4213-99a6-40039e1642eb"),
                    Name = "Dairy and Chilled"
                },
                new AppProductCategory
                {
                    Id = Guid.Parse("ac748e35-b497-4e4c-9b90-f014e3b4d773"),
                    Name = "Bakery"
                },
                new AppProductCategory
                {
                    Id = Guid.Parse("2cbec91c-a08a-4be8-ac7f-89ae4d46fcab"),
                    Name = "Beverages"
                }
            };
            await _repository.BulkInsert(entities);
            var result = await _repository.GetAll(new BaseRequest());
            Assert.Equal(5, result.TotalRows);

        }

        [Theory]
        [InlineData("ac748e35-b497-4e4c-9b90-f014e3b4d773")]
        [InlineData("2cbec91c-a08a-4be8-ac7f-89ae4d46fcab")]

        public async Task GetByIdTest(string id)
        {
            Guid parsedId = Guid.Parse(id);
            var entity = await _repository.GetById(parsedId);
            Assert.NotNull(entity);
        }

        [Fact]
        public async Task UpdateTest()
        {
            var entity = await _repository.GetById(Guid.Parse("fb3ae426-935c-4213-99a6-40039e1642eb"));
            Assert.NotNull(entity);
            entity.Name = "Dairy & Chilled";
            entity.ModifiedDate = DateTime.Now;
            await _repository.Update(entity);
            entity = await _repository.GetById(Guid.Parse("fb3ae426-935c-4213-99a6-40039e1642eb"));
            Assert.Equal("Dairy & Chilled", entity.Name);
        }



        [Theory]
        [InlineData("ac748e35-b497-4e4c-9b90-f01473b4d773")]
        public async Task GetByIdNotFoundTest(string id)
        {
            Guid parsedId = Guid.Parse(id);
            var exception = await Assert.ThrowsAsync<Exception>(async () =>
            {
                var entity = await _repository.GetById(parsedId);
                Assert.NotNull(entity);
            });
            Assert.NotNull(exception);
        }
    }
}
