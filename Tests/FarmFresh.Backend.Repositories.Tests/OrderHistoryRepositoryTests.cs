using FarmFresh.Backend.Entities;
using FarmFresh.Backend.Repositories.Interfaces;
using FarmFresh.Backend.Shared;
using IdentityModel;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Xunit;

namespace FarmFresh.Backend.Crud.Tests
{
    [Trait("Name", nameof(OrderHistoryRepositoryTests))]
    public class OrderHistoryRepositoryTests
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IOrderHistoryRepository _orderHistoryRepository;
        private readonly IProductRepository _productRepository;
        public OrderHistoryRepositoryTests(
                UserManager<AppUser> userManager, 
                IOrderHistoryRepository orderHistoryRepository,
                IProductRepository productRepository
            )
        {
            _userManager = userManager;
            _orderHistoryRepository = orderHistoryRepository;
            _productRepository = productRepository;
        }

        [Fact]
        public async Task CreateNewUsersAndMakeOrdersTest()
        {
            AppUser user1 = new AppUser
            {
                FullName = "Beggi Mammad",
                Email = "beggimammad@hotmail.com",
                EmailConfirmed = true,
                UserName = "beggimammad@hotmail.com"
            };
            user1.Addresses.Add(new AppUserAddress
            {
                AddressLine = "AH. Nasution Street 218",
                City = "Pekalongan",
                Region = "East-Lampung",
                Country = "Indonesia",
                PostalCode = "34191"
            });
            string user1Pwd = "@Future30";
            IdentityResult result = await _userManager.CreateAsync(user1, user1Pwd);
            Assert.True(result.Succeeded);
            result = await _userManager.AddClaimsAsync(user1, new List<Claim>
            {
                new Claim(JwtClaimTypes.Name, user1.FullName),
                new Claim(JwtClaimTypes.Email, user1.Email),
                new Claim(JwtClaimTypes.Role, GlobalConstants.CustomerUserRoleName)
            });
            Assert.True(result.Succeeded);


            AppUser user2 = new AppUser
            {
                FullName = "Ahmad Fikri",
                Email = "fikriahmad@hotmail.com",
                EmailConfirmed = true,
                UserName = "fikriahmad@hotmail.com"
            };
            user2.Addresses.Add(new AppUserAddress
            {
                AddressLine = "AH. Nasution Street 211",
                City = "Pekalongan",
                Region = "East-Lampung",
                Country = "Indonesia",
                PostalCode = "34191"
            });
            string user2Pwd = "@Future30";
            result = await _userManager.CreateAsync(user2, user2Pwd);
            Assert.True(result.Succeeded);
            result = await _userManager.AddClaimsAsync(user2, new List<Claim>
            {
                new Claim(JwtClaimTypes.Name, user2.FullName),
                new Claim(JwtClaimTypes.Email, user2.Email),
                new Claim(JwtClaimTypes.Role, GlobalConstants.CustomerUserRoleName)
            });
            Assert.True(result.Succeeded);



            //Make order for user1
            Guid freshCrabProductId = Guid.Parse("36C53A26-43AB-41AF-AD13-9C7C28ABB1BD");
            var freshCrabProduct = await _productRepository.GetById(freshCrabProductId);
            AppOrderHistory orderForUser1 = new AppOrderHistory
            {
                Item = freshCrabProduct.Name,
                Price = freshCrabProduct.Price,
                Total = 3,
                StoreId = freshCrabProduct.StoreId,
                UserId = user1.Id
            };
            await _orderHistoryRepository.Insert(orderForUser1);
            await _productRepository.UpdateProductAvailableAmount(freshCrabProduct.StoreId, freshCrabProductId, freshCrabProduct.AvailableAmount - 3);

            var user1Orders = await _orderHistoryRepository.GetAllByUser(user1.Id, new BaseListInput());
            Assert.Single(user1Orders.Rows);

            //Make order for user2
            Guid cocaColaProductId = Guid.Parse("A6F7CF0F-34B2-4B1F-9DE5-5C9A92351CAD");
            var cocaColaProduct = await _productRepository.GetById(freshCrabProductId);
            AppOrderHistory orderForUser2 = new AppOrderHistory
            {
                Item = cocaColaProduct.Name,
                Price = cocaColaProduct.Price,
                Total = 10,
                StoreId = cocaColaProduct.StoreId,
                UserId = user2.Id
            };
            await _orderHistoryRepository.Insert(orderForUser2);
            await _productRepository.UpdateProductAvailableAmount(cocaColaProduct.StoreId, cocaColaProductId, cocaColaProduct.AvailableAmount - 10);


            var user2Orders = await _orderHistoryRepository.GetAllByUser(user2.Id, new BaseListInput());
            Assert.Single(user2Orders.Rows);

        }


        [Theory]
        [InlineData("2B616B06-88E7-4CD3-8C4E-2968624C4A71")]
        public async Task GetAllOrderHistoriesByStoreTest(string storeId)
        {
            var orderHistories = await _orderHistoryRepository.GetAllByStore(Guid.Parse(storeId), new BaseListInput());
            Assert.Equal(expected: 2, orderHistories.Rows.Count());
        }
    }
}
