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
    [Trait("Name", nameof(UserOperationsTests))]
    public class UserOperationsTests
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IUserRepository _userRepository;
        public UserOperationsTests(UserManager<AppUser> userManager, IUserRepository userRepository)
        {
            _userManager = userManager;
            _userRepository = userRepository;
        }

        [Fact]
        public async Task CreateUserCustomerTest()
        {
            AppUser appUser = new AppUser
            {
                Id = Guid.Parse("62c01466-b243-4b5a-9f81-fc4d39507fae"),
                FullName = "Mirza Ghulam Rasyid",
                Email = "ghulamcyber@hotmail.com",
                EmailConfirmed = true,
                UserName = "ghulamcyber@hotmail.com"
            };
            appUser.Addresses.Add(new AppUserAddress
            {
                Id = Guid.Parse("e78ef4e9-8727-4c01-950f-1f1589ee71c9"),
                AddressLine = "AH. Nasution Street 215",
                City = "Pekalongan",
                Region = "East-Lampung",
                Country = "Indonesia",
                PostalCode = "34191"
            });
            string pwd = "@Future30";
            IdentityResult result = await _userManager.CreateAsync(appUser, pwd);
            Assert.True(result.Succeeded);
            result = await _userManager.AddClaimsAsync(appUser, new List<Claim>
            {
                new Claim(JwtClaimTypes.Name, appUser.FullName),
                new Claim(JwtClaimTypes.Email, appUser.Email),
                new Claim(JwtClaimTypes.Role, GlobalConstants.CustomerUserRoleName)
            });
            Assert.True(result.Succeeded);
        }


        [Theory]
        [InlineData("62c01466-b243-4b5a-9f81-fc4d39507fae")]
        public async Task FindUserByIdTest(string id)
        {
            AppUser appUser = await _userManager.FindByIdAsync(id);
            Assert.NotNull(appUser);
            Assert.Equal("ghulamcyber@hotmail.com", appUser.Email, ignoreCase: true);
        }

        [Theory]
        [InlineData("62c01466-b243-4b5a-9f81-fc4d39507fae")]
        public async Task FindUserByIdUserRepoTest(string id)
        {
            Guid userId = Guid.Parse(id);
            AppUser appUser = await _userRepository.GetUserById(userId);
            Assert.NotNull(appUser);
            Assert.Equal("ghulamcyber@hotmail.com", appUser.Email, ignoreCase: true);
        }


        [Theory]
        [InlineData("ghulamcyber@hotmail.com")]
        public async Task FindUserByEmailTest(string email)
        {
            AppUser appUser = await _userManager.FindByEmailAsync(email);
            Assert.NotNull(appUser);
        }


        [Theory]
        [InlineData("62c01466-b243-4b5a-9f81-fc4d39507fae")]

        public async Task ChangeNameTest(string id)
        {
            Guid userId = Guid.Parse(id);
           
            //temporarily changed to "Mirza G. Rasyid"
            string oldName = "Mirza Ghulam Rasyid";
            string newName = "Mirza G. Rasyid";
            await _userRepository.ChangeName(userId, newName);
            AppUser appUser = await _userManager.FindByIdAsync(id);
            Assert.Equal(newName, appUser.FullName);

            //revert it back
            await _userRepository.ChangeName(userId, oldName);
            appUser = await _userManager.FindByIdAsync(id);
            Assert.Equal(oldName, appUser.FullName);
        }



    }
}
