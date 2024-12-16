using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VietWay.Repository.DataAccessObject
{
    public class DatabaseConfig(string connectionString)
    {
        private readonly string _connectionString = connectionString;
        public string ConnectionString { get => _connectionString; }
    }
}
