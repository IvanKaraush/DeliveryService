using Domain.Models.VievModels;
using Domain.Models.ApplicationModels;
using Domain.Stores;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using Domain.Models.Entities.SQLEntities;
using Microsoft.Extensions.Options;

namespace Persistence.Repository
{
    public class UserRepository : IUserStore
    {
        public UserRepository(SQLContext context, IDistributedCache cache, IOptions<ReposOptions> repositoryOptions) 
        {
            Context = context;
            Cache = cache;
            CacheOptions = new DistributedCacheEntryOptions() { SlidingExpiration = TimeSpan.FromMinutes(repositoryOptions.Value.CacheExpirationMins) };
        }
        private readonly SQLContext Context;
        private readonly IDistributedCache Cache;
        private readonly DistributedCacheEntryOptions CacheOptions;

        public async Task AddUser(User user)
        {
            await Context.Users.AddAsync(user);
            await Context.SaveChangesAsync();
            await Cache.SetStringAsync(user.Id.ToString(), JsonConvert.SerializeObject(user), CacheOptions);
        }

        public async Task RemoveUser(Guid id)
        {
            User? user;
            string? userString = await Cache.GetStringAsync(id.ToString());
            if (userString != null)
            {
                user = JsonConvert.DeserializeObject<User>(userString);
                await Cache.RemoveAsync(id.ToString());
            }
            else
                user = await Context.Users.FirstOrDefaultAsync(u => u.Id == id);
            if (user == null)
                throw new DoesNotExistException(typeof(User));
            Context.Users.Remove(user);
            await Context.SaveChangesAsync();
        }

        public async Task<bool> EditUserTelegram(Guid id, string newTelegramId)
        {
            User? user;
            string? userString = await Cache.GetStringAsync(id.ToString());
            if (userString != null)
            {
                user = JsonConvert.DeserializeObject<User>(userString);
                await Cache.RemoveAsync(id.ToString());
            }
            else
                user = await Context.Users.FirstOrDefaultAsync(u => u.Id == id);
            if (user == null)
                throw new DoesNotExistException(typeof(User));
            bool hadAlreadyTelegramId = false;
            if (user.TelegramId != null)
                hadAlreadyTelegramId = true;
            user.TelegramId = newTelegramId;
            Context.Users.Update(user);
            await Context.SaveChangesAsync();
            await Cache.SetStringAsync(user.Id.ToString(), JsonConvert.SerializeObject(user), CacheOptions);
            return hadAlreadyTelegramId;
        }
        public async Task EditUserAuth(Guid id, AuthModel newAuth)
        {
            User? user;
            string? userString = await Cache.GetStringAsync(id.ToString());
            if (userString != null)
            {
                user = JsonConvert.DeserializeObject<User>(userString);
                await Cache.RemoveAsync(id.ToString());
            }
            else
                user = await Context.Users.FirstOrDefaultAsync(u => u.Id == id);
            if (user == null)
                throw new DoesNotExistException(typeof(User));
            user.Login = newAuth.Login;
            user.Password = newAuth.Password;
            Context.Users.Update(user);
            await Context.SaveChangesAsync();
            await Cache.SetStringAsync(user.Id.ToString(), JsonConvert.SerializeObject(user), CacheOptions);
        }
        public async Task AssignAsAdmin(Guid id)
        {
            User? user;
            string? userString = await Cache.GetStringAsync(id.ToString());
            if (userString != null)
            {
                user = JsonConvert.DeserializeObject<User>(userString);
                await Cache.RemoveAsync(id.ToString());
            }
            else
                user = await Context.Users.FirstOrDefaultAsync(u => u.Id == id);
            if (user == null)
                throw new DoesNotExistException(typeof(User));
            user.IsAdmin = true;
            Context.Users.Update(user);
            await Context.SaveChangesAsync();
            await Cache.SetStringAsync(user.Id.ToString(), JsonConvert.SerializeObject(user), CacheOptions);
        }
        public async Task UnassignAsAdmin(Guid id)
        {
            User? user;
            string? userString = await Cache.GetStringAsync(id.ToString());
            if (userString != null)
            {
                user = JsonConvert.DeserializeObject<User>(userString);
                await Cache.RemoveAsync(id.ToString());
            }
            else
                user = await Context.Users.FirstOrDefaultAsync(u => u.Id == id);
            if (user == null)
                throw new DoesNotExistException(typeof(User));
            user.IsAdmin = false;
            Context.Users.Update(user);
            await Context.SaveChangesAsync();
            await Cache.SetStringAsync(user.Id.ToString(), JsonConvert.SerializeObject(user), CacheOptions);
        }

        public async Task<User> GetUserByAuth(AuthModel authModel)
        {
            User? user = await Context.Users.FirstOrDefaultAsync(u => u.Login == authModel.Login && u.Password == authModel.Password);
            if (user == null)
                throw new DoesNotExistException(typeof(User));
            string key = user.Id.ToString();
            if (await Cache.GetStringAsync(key) != null)
                await Cache.RefreshAsync(key);
            else
                await Cache.SetStringAsync(user.Id.ToString(), JsonConvert.SerializeObject(user), CacheOptions);
            return user;
        }

        public async Task DebitBonuses(Guid id, decimal amount)
        {
            User? user;
            string? userString = await Cache.GetStringAsync(id.ToString());
            if (userString != null)
            {
                user = JsonConvert.DeserializeObject<User>(userString);
                await Cache.RemoveAsync(id.ToString());
            }
            else
                user = await Context.Users.FirstOrDefaultAsync(u => u.Id == id);
            if (user == null)
                throw new DoesNotExistException(typeof(User));
            user.Bonuses -= amount;
            Context.Users.Update(user);
            await Context.SaveChangesAsync();
            await Cache.SetStringAsync(user.Id.ToString(), JsonConvert.SerializeObject(user), CacheOptions);
        }

        public async Task<User> GetUserById(Guid id)
        {
            User? user;
            string? userString = await Cache.GetStringAsync(id.ToString());
            if (userString != null)
            {
                user = JsonConvert.DeserializeObject<User>(userString);
            }
            else
                user = await Context.Users.FirstOrDefaultAsync(u => u.Id == id);
            if (user == null)
                throw new DoesNotExistException(typeof(User));
            return user;
        }

        public async Task AddUserBirthDate(Guid id, DateOnly birthDate)
        {
            User? user;
            string? userString = await Cache.GetStringAsync(id.ToString());
            if (userString != null)
            {
                user = JsonConvert.DeserializeObject<User>(userString);
            }
            else
                user = await Context.Users.FirstOrDefaultAsync(u => u.Id == id);
            if (user == null)
                throw new DoesNotExistException(typeof(User));
            if (user.BirthDate != null)
                throw new WasAlreadySetException("Birthdate");
            user.BirthDate = birthDate;
            Context.Users.Update(user);
            await Context.SaveChangesAsync();
            await Cache.SetStringAsync(user.Id.ToString(), JsonConvert.SerializeObject(user), CacheOptions);
        }

        public async Task<List<string>> GetBirthdayPeopleTelegram()
        {
            return await Context.Users.Where(u => u.TelegramId != null && u.BirthDate.HasValue && u.BirthDate.Value == DateOnly.FromDateTime(DateTime.Now)).Select(u => u.TelegramId).ToListAsync();
        }
    }   
}
