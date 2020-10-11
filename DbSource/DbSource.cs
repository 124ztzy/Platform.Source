using System.Data;
using System.Data.Common;
using System.ComponentModel;

namespace Platform.Source
{
    //
    [DisplayName("抽象数据库")]
    public abstract class DbSource : Source
    {
        //
        [DisplayName("通过sql加载表")]
        public override DataTable Load(string sql, params string[] ignore)
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
                DataTable table = new DataTable();
                table.ExtendedProperties.Add("sql", sql);
                DbDataAdapter adapter = _factory.CreateDataAdapter();
                adapter.MissingSchemaAction = MissingSchemaAction.AddWithKey;
                adapter.SelectCommand = command;
                int count = adapter.Fill(table);
                connection.Close();
                return table;
            }
        }
        //
        [DisplayName("保存表")]
        public override int Save(DataTable table)
        {
            string sql = table.ExtendedProperties.Contains("sql") ? 
                table.ExtendedProperties["sql"].ToString() :
                "select * from " + table.TableName + " where 1=2 ";
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