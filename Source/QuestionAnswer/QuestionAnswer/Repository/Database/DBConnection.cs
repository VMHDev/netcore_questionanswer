using log4net;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace QuestionAnswer.Repository.Database
{
    public class DBConnection : IDisposable
    {
        private static readonly ILog _logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private SqlConnection _sqlConnection;
        public string DBConnectionString { get; }
        public IConfiguration Configuration { get; }
        public DBConnection(IConfiguration configuration) : this("DBConnection")
        {
            Configuration = configuration;
        }

        /// <summary>
        /// Connect database
        /// </summary>
        /// <param name="connectionStringName"></param>
        public DBConnection(string connectionStringName)
        {
            try
            {
                string environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
                if (environment != null)
                {
                    environment = "." + environment;
                }

                var configuration = new ConfigurationBuilder()
                       .SetBasePath(Directory.GetCurrentDirectory())
                       .AddJsonFile($"appsettings{environment}.json")
                       .Build();

                var connectionString = configuration.GetSection(connectionStringName).Value;
                DBConnectionString = connectionString;
                _sqlConnection = new SqlConnection(DBConnectionString);
            }
            catch (Exception ex)
            {
                _sqlConnection = null;
                _logger.Error(ex);
            }
        }

        /// <summary>
        /// Dispose connection
        /// </summary>
        public void Dispose()
        {
            _sqlConnection = null;
        }
    }
}
