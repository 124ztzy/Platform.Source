using System.Data;
using System.Data.Common;
using System.ComponentModel;

namespace Platform.Source
{
    //
    [DisplayName("抽象数据库")]
    public abstract class DatabaseSource : AbstractSource
    {
        //
        [DisplayName("加载表")]
        public override int Load(DataTable table, string sql, params string[] none)
        {
            if(_factory == null || _connectionString == null)
                Init();
            using(DbConnection connection = _factory.CreateConnection())
            {
                connection.ConnectionString = _connectionString;
                connection.Open();
                DbCommand command = _factory.CreateCommand();
                command.Connection = connection;
                command.CommandText = sql;
                table.ExtendedProperties.Add("sql", sql);
                DbDataAdapter adapter = _factory.CreateDataAdapter();
                adapter.MissingSchemaAction = MissingSchemaAction.AddWithKey;
                adapter.SelectCommand = command;
                int count = adapter.Fill(table);
                connection.Close();
                return count;
            }
        }
        //
        [DisplayName("保存表")]
        public override int Save(DataTable table, string sql)
        {
            if(_factory == null || _connectionString == null)
                Init();
            using(DbConnection connection = _factory.CreateConnection())
            {
                connection.ConnectionString = _connectionString;
                connection.Open();
                DbCommand command = _factory.CreateCommand();
                command.Connection = connection;
                command.CommandText = sql;
                DbDataAdapter adapter = _factory.CreateDataAdapter();
                adapter.SelectCommand = command;
                DbCommandBuilder builder = _factory.CreateCommandBuilder();
                builder.DataAdapter = adapter;
                int count = adapter.Update(table);
                table.AcceptChanges();
                connection.Close();
                return count;
            }  
        }


        //
        [DisplayName("服务器地址")]
        public string Server { get; set; }
        //
        [DisplayName("数据库名")]
        public string Database { get; set; }
        //
        [DisplayName("用户名")]
        public string User { get; set; }
        //
        [DisplayName("密码")]
        [PasswordPropertyText(true)]
        public string Password { get; set; }


        //数据库底层缓存
        protected DbProviderFactory _factory;
        protected string _connectionString;
    }
}