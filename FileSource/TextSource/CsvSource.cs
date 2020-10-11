using System.ComponentModel;
using System.Data;
using System.Text.RegularExpressions;

namespace Platform.Source
{
    //
    [DisplayName("Csv数据源")]
    public class CsvSource : TextSource
    {
        //
        [DisplayName("初始化")]
        protected override void Init()
        {
            base.Init();
            _table = new DataTable();
            string[] rows = Regex.Split(_text, RowSeparator);
            foreach(string row in rows)
            {
                if(_table.Columns.Count == 0) 
                {
                    string[] cells = Regex.Split(row.TrimEnd(), ColumnSeparator);
                    foreach(string cell in cells)
                    {
                        _table.Columns.Add(cell);
                    }
                }
                else
                {
                    DataRow datarow = _table.NewRow();
                    string[] cells = Regex.Split(row.TrimEnd(), ColumnSeparator);
                    for(int c = 0; c < cells.Length; c++)
                    {
                        datarow[c] = cells[c];
                    }
                    _table.Rows.Add(datarow);
                }
            }
        }

        //
        [DisplayName("加载表")]
        public override DataTable Load(string ignore, params string[] columnNames)
        {
            if(_table == null)
                Init();
            return columnNames == null ? _table.Copy() : _table.DefaultView.ToTable(false, columnNames);
        }
        //
        [DisplayName("储存表")]
        public override int Save(DataTable table)
        {
            return 0;
        }


        //
        [DisplayName("行分隔符")]
        public string RowSeparator { get; set; } = "\n";
        //
        [DisplayName("列分隔符")]
        public string ColumnSeparator { get; set; } = ",";


        //缓存对象
        private DataTable _table;
    }
}