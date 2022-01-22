using FarmFresh.Backend.Entities;
using FarmFresh.Backend.Repositories.Interfaces;
using FarmFresh.Backend.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace FarmFresh.Backend.Crud.Tests
{

    [Trait("Name", nameof(ProductRepositoryTest))]
    public class ProductRepositoryTest
    {
        private readonly IProductCategoryRepository _productCategoryRepository;
        private readonly IProductRepository _productRepository;
        private readonly IStoreRepository _storeRepository;
        public ProductRepositoryTest(
            IProductCategoryRepository productCategoryRepository, 
            IProductRepository productRepository,
            IStoreRepository storeRepository)
        {
            _productCategoryRepository = productCategoryRepository;
            _productRepository = productRepository;
            _storeRepository = storeRepository;
        }

        [Theory]
        [InlineData("2B616B06-88E7-4CD3-8C4E-2968624C4A71", "72FB7F50-7020-431B-9E43-1712796C83CE")]
        public async Task InsertTest(string storeIdStr, string productCategoryIdStr)
        {
            var raraStore = await _storeRepository
                .GetById(Guid.Parse(storeIdStr));
            Assert.NotNull(raraStore);

            var fruitAndVegCategory = await _productCategoryRepository
                .GetById(Guid.Parse(productCategoryIdStr));
            Assert.NotNull(fruitAndVegCategory);


            AppProduct appProduct = new AppProduct
            {
                Id = Guid.Parse("55026990-6d71-4414-99c3-c301aeec257e"),
                Name = "Tomatos",
                Description = "Fresh Tomatos from Lampung",
                AvailableAmount = 1000,
                Price = 0.5M,
                ImageUrl = @"https://cdn0-production-images-kly.akamaized.net/iZQHpRquiMoCtSBIenbAIOuDofA=/640x360/smart/filters:quality(75):strip_icc():format(jpeg)/kly-media-production/medias/1018928/original/082399700_1444710907-tomat-rsmadkotakediri.jpg",
                CategoryId = fruitAndVegCategory.Id,
                StoreId = raraStore.Id
            };

            await _productRepository.Insert(appProduct);
            appProduct = await _productRepository.GetById(raraStore.Id, appProduct.Id);
            Assert.NotNull(appProduct);

        }

        [Theory]
        [InlineData("2B616B06-88E7-4CD3-8C4E-2968624C4A71", "55026990-6D71-4414-99C3-C301AEEC257E")]
        public async Task GetAndUpdateTest(string storeId, string productId)
        {
            var entity = await _productRepository.GetById(Guid.Parse(storeId), Guid.Parse(productId));
            Assert.NotNull(entity);
            entity.AvailableAmount += 1000;
            entity.Price += 0.5M;
            await _productRepository.Update(entity);
            entity = await _productRepository.GetById(Guid.Parse(storeId), Guid.Parse(productId));
            Assert.Equal(2000, entity.AvailableAmount);
            Assert.Equal(1.0M, entity.Price);
        }

        [Theory]
        [InlineData("2B616B06-88E7-4CD3-8C4E-2968624C4A71", "55026990-6D71-4414-99C3-C301AEEC257E")]
        public async Task GetAndUpdateNegativeTest(string storeId, string productId)
        {
            var entity = await _productRepository.GetById(Guid.Parse(storeId), Guid.Parse(productId));
            Assert.NotNull(entity);
            entity.AvailableAmount = -1;
           await Assert.ThrowsAsync<Exception>(async () =>
            {

                await _productRepository.Update(entity);
                entity = await _productRepository.GetById(Guid.Parse(storeId), Guid.Parse(productId));
                Assert.Equal(2000, entity.AvailableAmount);
                Assert.Equal(1.0M, entity.Price);
            });
        }

        [Fact]
        public async Task BulkInsertTest()
        {
            Guid raraStoreId = Guid.Parse("2B616B06-88E7-4CD3-8C4E-2968624C4A71");
            Guid fruitAndVegCategoryId = Guid.Parse("72FB7F50-7020-431B-9E43-1712796C83CE");
            Guid dairyAndChilledCategoryId = Guid.Parse("FB3AE426-935C-4213-99A6-40039E1642EB");
            Guid beveragesCategoryId = Guid.Parse("2CBEC91C-A08A-4BE8-AC7F-89AE4D46FCAB");
            Guid mealAndSeafoodCategoryId = Guid.Parse("6EEF1F09-1659-4151-A407-A1909645B84C");
            Guid bakeryCategoryId = Guid.Parse("AC748E35-B497-4E4C-9B90-F014E3B4D773");
            List<AppProduct> products = new List<AppProduct>();

            //Fruit and Veg Category
            products.AddRange(new List<AppProduct>
            {
                new AppProduct
                {
                    Name = "Fresh Cabbages",
                    Description = "Fresh Cabbages from Palembang",
                    Price = 0.75M,
                    AvailableAmount = 500,
                    ImageUrl = "https://cdns.klimg.com/merdeka.com/i/w/news/2020/03/29/1161318/540x270/8-manfaat-luar-biasa-yang-terkandung-dalam-lembaran-kubis.jpg",
                    CategoryId = fruitAndVegCategoryId,
                    StoreId = raraStoreId
                },
                new AppProduct
                {
                    Name = "Grapefruits",
                    Description = "Grapefruit from Kalimantan",
                    Price = 1.0M,
                    AvailableAmount = 120,
                    ImageUrl = "https://cf.shopee.co.id/file/bf7016b8a6f00a80b2da8241cd4ae4b3",
                    CategoryId = fruitAndVegCategoryId,
                    StoreId = raraStoreId
                }
            });


            //Dairy and Chilled
            products.AddRange(new List<AppProduct>
            {
                new AppProduct
                {
                    Name = "Lampung Cow's Milk",
                    Description = "Fresh cow's milk",
                    Price = 2.5M,
                    AvailableAmount = 10,
                    ImageUrl = "https://image-cdn.medkomtek.com/t2V48R-CBP_i9BW6hMbhoiraEZU=/640x640/smart/klikdokter-media-buckets/medias/1778836/original/088090200_1511410761-Susu-Soya-dan-Susu-Sapi-Samakah-Kualitasnya.jpg",
                    CategoryId = dairyAndChilledCategoryId,
                    StoreId = raraStoreId
                },
                new AppProduct
                {
                    Name = "Soy Milk",
                    Description = "Healthy soy milk",
                    Price = 1.5M,
                    AvailableAmount = 43,
                    ImageUrl = "https://media.suara.com/pictures/970x544/2019/02/25/51937-susu.jpg",
                    CategoryId = dairyAndChilledCategoryId,
                    StoreId = raraStoreId
                },
            });

            //Beverages
            products.AddRange(new List<AppProduct>
            {
                new AppProduct
                {
                    Name = "Coca Cola",
                    Price = 0.2M,
                    AvailableAmount = 134,
                    ImageUrl = "https://assets.pikiran-rakyat.com/crop/26x151:757x746/750x500/photo/2021/06/17/482138917.jpg",
                    CategoryId = beveragesCategoryId,
                    StoreId = raraStoreId
                },
                 new AppProduct
                {
                    Name = "Small Sprite",
                    Description = "Canned bottle sprite",
                    Price = 0.1M,
                    AvailableAmount = 44,
                    ImageUrl = "https://cf.shopee.co.id/file/895eecc8f78997bdd5a07d1933d4376c",
                    CategoryId = beveragesCategoryId,
                    StoreId = raraStoreId
                },
            });

            //Meal and Seafood
            products.AddRange(new List<AppProduct>
            {
                new AppProduct
                {
                    Name = "Fresh crabs",
                    Price = 0.4M,
                    Description = "Fresh crabs from fisherman",
                    AvailableAmount = 502,
                    ImageUrl = "https://asiatoday.id/wp-content/uploads/2020/02/Ekspor-Kepiting-dari-Papua.-ist.jpg",
                    CategoryId = mealAndSeafoodCategoryId,
                    StoreId = raraStoreId
                },
                 new AppProduct
                {
                    Name = "Fresh Squids",
                    Price = 0.1M,
                    AvailableAmount = 80,
                    ImageUrl = "https://images.tokopedia.net/img/cache/500-square/VqbcmM/2021/6/15/ce105c62-7d3c-47b2-a462-7b9838f2088d.jpg",
                    CategoryId = mealAndSeafoodCategoryId,
                    StoreId = raraStoreId
                },
            });


            //Bakery
            products.AddRange(new List<AppProduct>
            {
                new AppProduct
                {
                    Name = "Sari Roti Bread",
                    Description = "Fresh sari roti from the oven",
                    Price = 0.1M,
                    AvailableAmount = 32,
                    ImageUrl = "https://s3.bukalapak.com/img/8318823808/large/sari_roti_sobek_cklt_ns_pcs_scaled.jpg",
                    CategoryId = bakeryCategoryId,
                    StoreId = raraStoreId
                }
            });


            int expectedTotal = products.Count;
            int actualTotal = await _productRepository.BulkInsert(raraStoreId, products);
            
            Assert.Equal(expectedTotal, actualTotal);

        }


        [Theory]
        [InlineData("fresh")]
        public async Task SearchProductByNewCategory(string query)
        {
            //default pagination
            var result = await _productRepository.GetAll(new ProductListInput
            {
                Query = query,
                Category = GlobalConstants.CategoryNewKeyword
            });
            int expectedTotalRows = 3;
            Assert.Equal(expectedTotalRows, result.Rows.Count());

            //with simple pagination
            result = await _productRepository.GetAll(new ProductListInput
            {
                Query = query,
                FetchSize = 2,
                SkipCount = 1,
                Category = GlobalConstants.CategoryNewKeyword
            });
            expectedTotalRows = 2;
            Assert.Equal(expectedTotalRows, result.Rows.Count());
        }

        [Theory]
        [InlineData("Fruit & Veg")]
        public async Task SearchProductByDatabaseDataCategory(string category)
        {
            //default pagination
            var result = await _productRepository.GetAll(new ProductListInput
            {
                Category = category
            });
            int expectedTotalRows = 3;
            Assert.Equal(expectedTotalRows, result.Rows.Count());
        }

        [Theory]
        [InlineData("A6F7CF0F-34B2-4B1F-9DE5-5C9A92351CAD")]
        public async Task Update(string productId)
        {
            var entity = await _productRepository.GetById(Guid.Parse(productId));
            string desc = "Cold coca cola for your lunch";
            entity.Description = desc;
            await _productRepository.Update(entity);

            entity = await _productRepository.GetById(Guid.Parse(productId));
            Assert.Equal(desc, entity.Description);
        }

    }
}
