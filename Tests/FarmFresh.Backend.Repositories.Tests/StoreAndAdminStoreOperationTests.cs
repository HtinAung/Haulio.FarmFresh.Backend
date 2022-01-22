using FarmFresh.Backend.Entities;
using FarmFresh.Backend.Repositories.Interfaces;
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
    [Trait("Name", nameof(StoreAndAdminStoreOperationTests))]
    public class StoreAndAdminStoreOperationTests
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IStoreRepository _storeRepository;
        public StoreAndAdminStoreOperationTests(
                UserManager<AppUser> userManager,
                IStoreRepository storeRepository
            )
        {
            _userManager = userManager;
            _storeRepository = storeRepository;
        }

        //Step 1: Create Admin Store and Store
        [Fact]
        public async Task CreateAdminStoreAndStore()
        {
            AppUser appUser = new AppUser
            {
                Id = Guid.Parse("a4c15527-4e5f-45ff-a3ef-1cbf1e46e180"),
                FullName = "Rara Anjani",
                Email = "raraanjani@gmail.com",
                EmailConfirmed = true,
                UserName = "raraanjani@gmail.com",
                Addresses = new List<AppUserAddress>
                {
                    new AppUserAddress
                    {
                        AddressLine = "Abc123 Street 216",
                        City = "Metro",
                        Region = "Metro",
                        Country = "Indonesia",
                        PostalCode = "34111"
                    }
                }
            };

            var identityResult = await _userManager.CreateAsync(appUser, "@Future30");
            Assert.True(identityResult.Succeeded);
            
            AppStore appStore = new AppStore
            {
                Id = Guid.Parse("2b616b06-88e7-4cd3-8c4e-2968624c4a71"),
                Name = "Rara Store",
                UserId = appUser.Id
            };
            
            Guid storeId = await _storeRepository.Insert(appStore);
            Assert.Equal(appStore.Id, storeId);
            await _storeRepository.TieAdminUserWithStore(storeId, appUser.Id);

            appUser = await _userManager.FindByIdAsync(appUser.Id.ToString());
            appStore = await _storeRepository.GetById(storeId);

            Assert.Equal(appUser.StoreId, appStore.Id);
            Assert.Equal(appUser.Id, appStore.UserId);
        }

        [Theory]
        [InlineData("2B616B06-88E7-4CD3-8C4E-2968624C4A71")]
        public async Task GetStoreById(string id)
        {
            Guid storeId = Guid.Parse(id);
            var store = await _storeRepository.GetById(storeId);
            Assert.NotNull(store);

        }

    }
}
