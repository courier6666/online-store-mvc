﻿using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Store.Domain.Entities.Interfaces;
using Store.Domain.Repositories;
using Store.Persistence.Main.DatabaseContexts;
using Store.Persistence.Main.Identity;

namespace Store.Persistence.Main.Repositories
{
    public class EfUserRepository : IUserRepository
    {
        private ApplicationDbContext _context;
        private RoleManager<IdentityRole<Guid>> _roleManager;
        private UserManager<AppUser> _userManager;

        public EfUserRepository(ApplicationDbContext context, RoleManager<IdentityRole<Guid>> roleManager, UserManager<AppUser> userManager)
        {
            _context = context;
            _roleManager = roleManager;
            _userManager = userManager;
        }
        public void Add(IUser entity)
        {
            if (entity is AppUser user)
                _context.Users.Add(user);
        }

        public void Delete(IUser entity)
        {
            if (entity is AppUser user)
                _context.Users.Remove(user);
        }

        public void DeleteRange(IEnumerable<IUser> entities)
        {
            _context.RemoveRange(entities);
        }

        public void Dispose()
        {
            _context = null;
        }

        public async Task<IEnumerable<IUser>> GetAllAsync()
        {
            return await _context.Users.ToListAsync();
        }


        public async Task<IUser> GetByIdAsync(Guid id)
        {
            var user = await _context.Users.Include(u => u.Address).FirstOrDefaultAsync(u => u.Id.Equals(id));
            user.Roles = await _userManager.GetRolesAsync(user);
            return user;
        }

        public void Update(IUser entity)
        {
            if (entity is AppUser user)
                _context.Users.Update(user);
        }
    }
}
