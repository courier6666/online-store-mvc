using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Storage;
using Store.Persistence.Main.DatabaseContexts;
using Store.Domain.Entities.Interfaces;
using Store.Domain.Repositories;
using Store.Persistence.Main.Repositories;
using Microsoft.AspNetCore.Identity;
using Store.Persistence.Main.Identity;

namespace Store.Persistence.Main.UnitOfWorks
{
    public class EfCoreUnitOfWork : IUnitOfWork
    {
        private ApplicationDbContext _context;
        private IDbContextTransaction? _transaction;
        private ICashDepositRepository? _cashDepositRepository;
        private IOrderEntryRepository? _orderEntryRepository;
        private IOrderRepository? _orderRepository;
        private IProductRepository? _productRepository;
        private IUserRepository? _userRepository;
        private IPaymentDetailsRepository? _paymentDetailsRepository;
        private IAddressRepository? _addressRepository;
        private RoleManager<IdentityRole<Guid>> _roleManager;
        private UserManager<AppUser> _userManager;
        public EfCoreUnitOfWork(ApplicationDbContext context, RoleManager<IdentityRole<Guid>> roleManager, UserManager<AppUser> userManager)
        {
            _context = context;
            _roleManager = roleManager;
            _userManager = userManager;
        }
        public ICashDepositRepository CashDepositRepository {
            get
            {
                _cashDepositRepository ??= new EfCashDepositRepository(_context);
                return _cashDepositRepository;
            }
        }

        public IOrderEntryRepository OrderEntryRepository {
            get
            {
                _orderEntryRepository ??= new EfOrderEntryRepository(_context);
                return _orderEntryRepository;
            }
        }

        public IOrderRepository OrderRepository {
            get
            {
                _orderRepository ??= new EfOrderRepository(_context);
                return _orderRepository;
            }
        }

        public IProductRepository ProductRepository {
            get
            {
                _productRepository ??= new EfProductRepository(_context);
                return _productRepository;
            }
        }

        public IUserRepository UserRepository {
            get
            {
                _userRepository ??= new EfUserRepository(_context, _roleManager, _userManager);
                return _userRepository;
            }
        }

        public IPaymentDetailsRepository PaymentDetailsRepository
        {
            get
            {
                _paymentDetailsRepository ??= new EfPaymentDetailsRepository(_context);
                return _paymentDetailsRepository;
            }
        }

        public IAddressRepository AddressRepository
        {
            get
            {
                _addressRepository ??= new EfAddressRepository(_context);
                return _addressRepository;
            }
        }

        public void BeginTransaction()
        {
            _transaction = _context.Database.BeginTransaction();
        }

        public async Task CommitAsync()
        {
            await _context.SaveChangesAsync();
            await _transaction.CommitAsync();
        }

        public void Dispose()
        {
            _cashDepositRepository?.Dispose();
            _cashDepositRepository = null;

            _orderEntryRepository?.Dispose();
            _orderEntryRepository = null;

            _orderRepository?.Dispose();
            _orderRepository = null;

            _productRepository?.Dispose();
            _productRepository = null;

            _userRepository?.Dispose();
            _userRepository = null;

            _context.Dispose();
            _context = null;
        }

        public void Rollback()
        {
            _transaction.Rollback();
        }
    }
}
