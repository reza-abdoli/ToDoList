using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Data.Context;
using Data.Dto;
using Data.Entity;
using Microsoft.EntityFrameworkCore;

namespace Data.Repository;

public class Repository<IEntity> : IRepository<IEntity> where IEntity : BaseEntity
{
    private readonly AppDbContext _appDbContext;
    private readonly DbSet<IEntity> _dbSet;

    public Repository(AppDbContext appDbContext)
    {
        _appDbContext = appDbContext;
        _dbSet = _appDbContext.Set<IEntity>();
        // _dbSetUser = _appDbContext.Set<User>();
        // _dbSetToDoList = _appDbContext.Set<ToDoList>();
        // what if we had 50 tables ?? solution => generic types!
    }

    public void AddEntity(IEntity entity)
    {
        _dbSet.Add(entity);
        #region diffrence between Add/AddAsync
                    /*
https://claude.ai/chat/6f2bbc3a-aa9d-47fe-87ca-5472886b438c

برای Add هم همینه ؟ فقط وضعیت ابجکت رو توی چنج ترکر به added تغییر میده و در صورت صدا زدن SaveChangesAsync که عملیات ها واقعا در دیتابیس انجام میشن ؟ پس چرا یه AddAsync هم داریم ؟4:44 PMClaude responded: دقیقاً!دقیقاً! و سوال خیلی خوبیه درباره AddAsync
جواب کوتاه: AddAsync تقریباً هیچ‌وقت لازم نیست!
csharp// این دو عملاً یکی هستن در ۹۹٪ مواقع
_appDbContext.Users.Add(user);
await _appDbContext.Users.AddAsync(user);

پس چرا AddAsync وجود داره؟
تنها دلیلش اینه که بعضی دیتابیس‌ها از HiLo یا Sequence برای تولید ID استفاده میکنن:
HiLo/Sequence → قبل از insert، باید یه بار به دیتابیس بره و ID بگیره
                 این عملیات async هست → پس AddAsync معنی پیدا میکنه
ولی با Identity معمولی (که اکثراً استفاده میشه):
csharp// ID توسط دیتابیس بعد از insert تولید میشه
// نیازی به رفتن به دیتابیس قبل از SaveChanges نیست
// پس Add و AddAsync کاملاً یکسانن

توصیه تیم EF Core خودشون:

از Add استفاده کن مگه اینکه مطمئنی از value generator ای استفاده میکنی که async I/O داره


        */
        #endregion
    }

    public void DeleteEntity(IEntity entity)
    {
        _dbSet.Remove(entity);
    }

    public async ValueTask DisposeAsync()
    {
        if (_appDbContext != null)
        {
            await _appDbContext.DisposeAsync();
        }
    }

    public async Task<IEntity?> GetEntityById(int id)
    {
        return await _dbSet.SingleOrDefaultAsync(entity => entity.Id == id);
    }

    public IQueryable<IEntity> GetQueryable()
    {
        return _dbSet.AsQueryable();
    }

    public async Task SaveChanges()
    {
        await _appDbContext.SaveChangesAsync();
    }

    public void UpdateEntity(IEntity entity)
    {
        _dbSet.Update(entity);
    }
}

#region before generic type
    // public async Task AddUser(User user)
    // {
    //     await _dbSetUser.AddAsync(user);
    // }

    // public async ValueTask DisposeAsync()
    // {
    //     if (_appDbContext != null)
    //     {
    //         await _appDbContext.DisposeAsync();
    //     }
    // }

    // public IQueryable<User> GetQueryableUsers()
    // {
    //     return _dbSetUser.AsQueryable();
    // }
    // public IQueryable<ToDoList> GetQueryableToDoLists()
    // {
    //     return _dbSetToDoList.AsQueryable();
    // }

    // // public async Task<User?> GetUser(string name)
    // // {
    // //     return await _dbSetUser.FirstOrDefaultAsync(u => u.Name == name);
    // // }

    // public async Task SaveChanges()
    // {
    //     await _appDbContext.SaveChangesAsync();
    // }

    // public async Task AddToDoList(ToDoList toDoList)
    // {
    //     await _dbSetToDoList.AddAsync(toDoList);
    // }

    // public void DeleteToDoList(ToDoList toDoList)
    // {
    //     _dbSetToDoList.Update(toDoList);
    // }

    // public void UpdateToDoList(ToDoList toDoList)
    // {
    //     _dbSetToDoList.Update(toDoList);
    // }
    
#endregion