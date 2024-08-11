using Domain.Models.Entities;
using Domain.Models.VievModels;
using Domain.Models.ApplicationModels;
using Domain.Stores;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;

namespace Persistence.Repository
{
    public class UserRepository : IUserStore
    {
        public UserRepository(SQLContext context, IDistributedCache cache) 
        {
            Context = context;
        }
        private readonly SQLContext Context;
        private readonly IDistributedCache Cache;
        private readonly DistributedCacheEntryOptions CacheOptions = new DistributedCacheEntryOptions() { SlidingExpiration = TimeSpan.FromMinutes(5) };

        public async void AddUser(User user)
        {
            await Context.Users.AddAsync(user);
            await Context.SaveChangesAsync();
            await Cache.SetStringAsync(user.Id.ToString(), JsonConvert.SerializeObject(user), CacheOptions);
        }

        public async void RemoveUser(Guid id)
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

        public async void EditUserTelegram(Guid id, string newTelegramId)
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
            user.TelegramId = newTelegramId;
            Context.Users.Update(user);
            await Context.SaveChangesAsync();
            await Cache.SetStringAsync(user.Id.ToString(), JsonConvert.SerializeObject(user), CacheOptions);
        }
        public async void EditUserAuth(Guid id, AuthModel newAuth)
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
        public async void AssignAsAdmin(Guid id)
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
        public async void UnassignAsAdmin(Guid id)
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

        public async void DebitBonuses(Guid id, decimal amount)
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
    }   
}
