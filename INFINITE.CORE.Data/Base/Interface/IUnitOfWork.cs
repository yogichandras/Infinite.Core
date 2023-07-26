﻿using INFINITE.CORE.Shared.Attributes;
using Microsoft.EntityFrameworkCore;

namespace INFINITE.CORE.Data.Base.Interface
{
    public interface IUnitOfWork<TDbContext> : IDisposable where TDbContext : DbContext
    {
        IQueryable<TEntity> Entity<TEntity>() where TEntity : class, IEntity;
        void Add<TEntity>(TEntity entity) where TEntity : class, IEntity;
        void Add<TEntity>(IEnumerable<TEntity> items) where TEntity : class, IEntity;
        Task<(bool Success, string Message, Exception? ex, List<ChangeLog>? log)> AddSave<TEntity>(TEntity entity) where TEntity : class, IEntity;

        Task<(bool Success, string Message, Exception? ex, List<ChangeLog>? log)> AddSave<TEntity>(IEnumerable<TEntity> items) where TEntity : class, IEntity;

        void Update<TEntity>(TEntity entity) where TEntity : class, IEntity;
        void Update<TEntity>(IEnumerable<TEntity> items) where TEntity : class, IEntity;
        Task<(bool Success, string Message, Exception? ex, List<ChangeLog>? log)> UpdateSave<TEntity>(TEntity entity) where TEntity : class, IEntity;
        Task<(bool Success, string Message, Exception? ex, List<ChangeLog>? log)> UpdateSave<TEntity>(IEnumerable<TEntity> items) where TEntity : class, IEntity;
        void Delete<TEntity>(TEntity entity) where TEntity : class, IEntity;
        void Delete<TEntity>(IEnumerable<TEntity> items) where TEntity : class, IEntity;
        void SoftDelete<TEntity>(TEntity entity) where TEntity : class, ISoftEntity;
        void SoftDelete<TEntity>(IEnumerable<TEntity> items) where TEntity : class, ISoftEntity;
        Task<(bool Success, string Message, Exception? ex, List<ChangeLog>? log)> DeleteSave<TEntity>(TEntity entity) where TEntity : class, IEntity;
        Task<(bool Success, string Message, Exception? ex, List<ChangeLog>? log)> DeleteSave<TEntity>(IEnumerable<TEntity> items) where TEntity : class, IEntity;
        Task<(bool Success, string Message, Exception? ex, List<ChangeLog>? log)> SoftDeleteSave<TEntity>(TEntity entity) where TEntity : class, ISoftEntity;
        Task<(bool Success, string Message, Exception? ex, List<ChangeLog>? log)> SoftDeleteSave<TEntity>(IEnumerable<TEntity> items) where TEntity : class, ISoftEntity;
        Task<List<TEntity>> List<TEntity>(IQueryable<TEntity> query) where TEntity : IEntity;
        Task<TEntity> Single<TEntity>(IQueryable<TEntity> query) where TEntity : IEntity;
        Task<int> Count<TEntity>(IQueryable<TEntity> query) where TEntity : IEntity;
        Task<bool> Any<TEntity>(IQueryable<TEntity> query) where TEntity : IEntity;
        void ExecuteQuery(string query);
        Task<(bool Success, string Message, Exception? ex, List<ChangeLog>? log)> ExecuteQuerySave(string query);
        Task<(bool Success, string Message, T Result, Exception? ex)> SingleQuery<T>(string query) where T : class;
        Task<(bool Success, string Message, List<T> Result, Exception? ex)> ListQuery<T>(string query) where T : class;
        Task<(bool Success, string Message, List<Dictionary<string, string>> Result, Exception ex)> DynamicQuery(string query);
        Task<(bool Success, string Message, Exception? ex, List<ChangeLog>? log)> Commit();
        Task<(bool Success, string Message, Exception? ex, List<ChangeLog>? log)> Commit<T>(Func<T> f);

    }
}
