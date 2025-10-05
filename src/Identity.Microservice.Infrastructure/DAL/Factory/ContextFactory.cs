using System;
using Infrastructure.Configuration;
using Infrastructure.DAL;
using Infrastructure.DAL.Factory;
using Microsoft.AspNetCore.Http;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using UserModule.Infrastructure.DAL.EfContext;

namespace UserModule.Infrastructure.DAL.Factory
{
    /// <summary>
    /// context factory for ef
    /// </summary>
    public class ContextFactory : IContextFactory
    {
        private readonly IOptions<ConnectionSettings> _connectionOptions;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ContextFactory(IOptions<ConnectionSettings> connectionOptions, IHttpContextAccessor httpContextAccessor)
        {
            _connectionOptions = connectionOptions;
            _httpContextAccessor = httpContextAccessor;
        }

        public IDataContext DbContext => new DataContext(GetDataContext().Options, _httpContextAccessor);

        private DbContextOptionsBuilder<DataContext> GetDataContext()
        {
            ValidateDefaultConnection();

            var sqlConnectionBuilder = new SqlConnectionStringBuilder(_connectionOptions.Value.Db);

            var contextOptionsBuilder = new DbContextOptionsBuilder<DataContext>();

            contextOptionsBuilder.UseSqlServer(sqlConnectionBuilder.ConnectionString);

            return contextOptionsBuilder;
        }

        private void ValidateDefaultConnection()
        {
            if (string.IsNullOrEmpty(_connectionOptions.Value.Db))
            {
                throw new ArgumentNullException(nameof(_connectionOptions.Value.Db));
            }
        }
    }
}