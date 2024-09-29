using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;
using Shouldly;
using Store.Core.Application.DataTransferObjects;
using Store.Core.Application.Responses;
using Store.Core.Infrastructure.Mappers;
using Store.Onion.Application.DataTransferObjects;
using Store.Onion.Domain.Entities;
using Store.Onion.Domain.Entities.Model;

namespace InfrastructureUnitsTests
{
    public class AutoMapperAdapterAndDtoModelEntitiesTests
    {
        private AutoMapperAdapter _mapper;
        [SetUp]
        public void Setup()
        {
            _mapper = CustomMapperFactory.Create();
        }

        [Test]
        public void MapProduct_To_ProductDto_Properties_AreEqual()
        {
            Guid productId = Guid.NewGuid();
            //arrange
            Product product = new Product()
            {
                Id = productId,
                Name = "Product1",
                Category = "Cat1",
                Description = "Description",
                Price = 120.00m
            };

            ProductDto expected = new ProductDto()
            {
                Id = productId,
                Name = "Product1",
                Category = "Cat1",
                Description = "Description",
                Price = 120.00m
            };

            //act
            ProductDto productDto = _mapper.Map<Product, ProductDto>(product);

            //assert
            productDto.Id.ShouldBe(expected.Id);
            productDto.Name.ShouldBe(expected.Name);
            productDto.Category.ShouldBe(expected.Category);
            productDto.Description.ShouldBe(expected.Description);
            productDto.Price.ShouldBe(expected.Price);
        }
        [Test]
        public void MapAddressDto_To_Address_Properties_AreEqual()
        {
            //arrange
            AddressDto addressDto = new AddressDto()
            {
                Country = "Ukraine",
                State = "Volyn",
                City = "Some city",
                Street = "Some street"
            };

            Address expected = new Address()
            {
                Country = "Ukraine",
                State = "Volyn",
                City = "Some city",
                Street = "Some street"
            };

            //act
            Address address = _mapper.Map<AddressDto, Address>(addressDto);

            //assert
            address.Country.ShouldBe(expected.Country);
            address.State.ShouldBe(expected.State);
            address.City.ShouldBe(expected.City);
            address.Street.ShouldBe(expected.Street);
        }

        [Test]
        public void MapUserRegistrationDto_To_User_Properties_AreEqual()
        {
            //arrange
            UserRegistrationDto userRegistrationDto = new UserRegistrationDto()
            {
                FirstName = "FirstName",
                LastName = "lastName",
                Email = "email@gmail.com",
                PhoneNumber = "+333333",
                Birthday = "2001-01-01",
                Login = "Login1",
                PasswordHash = "Password",
                Address = new AddressDto()
                {
                    Country = "Ukraine",
                    State = "Volyn",
                    City = "Some city",
                    Street = "Some street"
                }
            };

            User expected = new User()
            {
                FirstName = "FirstName",
                LastName = "lastName",
                Email = "email@gmail.com",
                PhoneNumber = "+333333",
                Birthday = new DateOnly(2001, 1, 1),
                Login = "Login1",
                PasswordHash = "Password",
                Address = new Address()
                {
                    Country = "Ukraine",
                    State = "Volyn",
                    City = "Some city",
                    Street = "Some street"
                }
            };
            //act
            User user = _mapper.Map<UserRegistrationDto, User>(userRegistrationDto);

            //assert
            user.FirstName.ShouldBe(expected.FirstName);
            user.LastName.ShouldBe(expected.LastName);
            user.Email.ShouldBe(expected.Email);
            user.PhoneNumber.ShouldBe(expected.PhoneNumber);
            user.Birthday.ShouldBe(expected.Birthday);
            user.Login.ShouldBe(expected.Login);
            user.PasswordHash.ShouldBe(expected.PasswordHash);

            user.Address.Country.ShouldBe(expected.Address.Country);
            user.Address.State.ShouldBe(expected.Address.State);
            user.Address.City.ShouldBe(expected.Address.City);
            user.Address.Street.ShouldBe(expected.Address.Street);
        }
        [Test]
        public void MapCashDeposit_To_CashDepositDto_PropertiesAreEqual()
        {
            //arrange
            CashDeposit cashDeposit = new UserCashDeposit()
            {
                CurrentMoneyBalanceSetter = 500.00m,
                Id = Guid.NewGuid()
            };
            //act
            CashDepositDto mappedDepositDto = _mapper.Map<CashDeposit, CashDepositDto>(cashDeposit);

            //assert
            mappedDepositDto.CurrentMoneyBalance.ShouldBe(cashDeposit.CurrentMoneyBalance);
            mappedDepositDto.Id.ShouldBe(cashDeposit.Id);
        }
    }
}
