using System.Data;
using System.Data.Common;
using System.ComponentModel;
using MySql.Data.MySqlClient;

namespace Platform.Source
{
    //
    [DisplayName("MySQL数据库")]
    public class MySQLSource : DatabaseSource
    {
        //
        [DisplayName("初始化")]
        protected override void Init()
        {
            _factory = MySqlClientFactory.Instance;
            MySqlConnectionStringBuilder builder = new MySqlConnectionStringBuilder();
            builder.Server = Server;
            builder.UserID = User;
            builder.Password = Password;
            builder.Database = Database;
            _connectionString = builder.ConnectionString;
        }

        // //数据库列表
        // public  void Databases()
        // {
        //     string sql = @"
        //     select schema_name 数据库名 
        //     from information_schema.schemata";
        //     ViewBag.DataTable = Excute(sql);
        // }
        // //表列表
        // public  void Tables([Required]string db)
        // {
        //     ViewBag.db = db;
        //     string sql = @"
        //     select table_schema 数据库名, table_name 表名, table_comment 表描述 
        //     from information_schema.tables 
        //     where table_schema = '" + db + "'";
        //     ViewBag.DataTable = Excute(sql);
        // }
        // //列列表
        // public override void Columns([Required]string db, [Required]string table)
        // {
        //     ViewBag.db = db;
        //     ViewBag.table = table;
        //     string sql = @"
        //     select t1.table_schema 数据库名, t1.table_name 表名, t1.column_name 列名, t1.column_comment 列描述, 
        //         t1.column_type 列类型, 
        //         case when t1.column_key = 'PRI' then true end 主键, 
        //         case when t1.is_nullable = 'NO' then true end 禁空, 
        //         case when t1.extra = 'auto_increment' then true end 自增, 
        //         case when t1.column_key = 'UNI' then true end 唯一,
        //         case when t1.extra = 'auto_increment' then t1.extra else t1.column_default end 默认值, 
        //         t1.extra 附加参数,
        //         t2.referenced_table_schema 外键数据库名, t2.referenced_table_name 外键表名, t2.referenced_column_name 外键列名
        //     from information_schema.columns as t1
        //     left join information_schema.KEY_COLUMN_USAGE as t2 on t2.table_schema = t1.table_schema and t2.table_name = t1.table_name 
        //         and t2.column_name = t1.column_name and t2.referenced_table_name is not null
        //     where t1.table_schema = '" + db + "' and t1.table_name = '" + table + "' order by t1.ordinal_position";
        //     ViewBag.DataTable = Excute(sql);
        // }
        // //索引列表
        // public  void Indexes([Required]string db, [Required]string table)
        // {
        //     ViewBag.db = db;
        //     ViewBag.table = table;
        //     string sql = @"
        //     select table_schema 数据库名, table_name 表名, index_name 索引名, 
        //         case when index_name = 'PRIMARY' then true end 主键, 
        //         case when non_unique = 0 then true end 唯一, 
        //         group_concat(column_name) 索引列 
        //     from information_schema.statistics 
        //     where table_schema = '" + db + "' and table_name = '" + table + @"'
        //     group by table_schema, table_name, index_name, non_unique ";
        //     ViewBag.DataTable = Excute(sql);
        // }

    }
}