using System.ComponentModel;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Data;
using System;

namespace Platform.Source
{
    //
    [DisplayName("文本数据源")]
    public class TextSource : FileSource
    {
        //
        [DisplayName("初始化")]
        protected override void Init()
        {
            if(!string.IsNullOrEmpty(FilePath))
                _text = File.ReadAllText(FilePath, string.IsNullOrEmpty(Enctype) ? Encoding.UTF8 : Encoding.GetEncoding(Enctype));
        }
        //
        [DisplayName("通过正则表达式加载表")]
        public override DataTable Load(string matchExpression, params string[] ignore)
        {
            if(_text == null)
                Init();
            DataTable table = new DataTable();
            //分组作为列
            Regex regex = new Regex(matchExpression);
            foreach(string group in regex.GetGroupNames())
            {
                if(group == "0")
                    table.Columns.Add("column" + (table.Columns.Count + 1));
                else
                    table.Columns.Add(group);
            }
            //提取数据
            MatchCollection matches = regex.Matches(_text);
            foreach(Match match in matches)
            {
                DataRow datarow = table.NewRow();
                GroupCollection groups = match.Groups;
                for(int c = 0; c < groups.Count; c++)
                {
                    datarow[c] = groups[c].Value;
                }
                table.Rows.Add(datarow);
            }
            return table;
        }
        //
        [DisplayName("保存表")]
        public override int Save(DataTable table)
        {
            throw new Exception(SourceName + " " + GetType().Name + " 不支持保存数据");
        }



        //
        [DisplayName("文本编码")]
        public string Enctype { get; set; }


        //缓存文本
        protected internal string _text;
    } 
}