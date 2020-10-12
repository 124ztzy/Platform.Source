using System;
using System.Data;
using System.ComponentModel;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Platform.Source
{
    //
    [DisplayName("Json数据源")]
    public class JsonSource : TextSource
    {
        //
        [DisplayName("初始化")]
        protected override void Init()
        {
            base.Init();
            _json = JToken.Parse(_text);
        }
        //
        [DisplayName("通过JsonPath加载表")]
        public override DataTable Load(string rowExtract, params string[] columnExtracts)
        {
            if(_json == null)
                Init();
            DataTable table = new DataTable();
            IEnumerable<JToken> list = _json.SelectTokens(rowExtract);
            //无列提取时，用行提取第一个元素，创建表结构
            if(columnExtracts == null || columnExtracts.Length == 0)
            {
                foreach(JToken item in list)
                {
                    if(table.Columns.Count == 0)
                    {
                        foreach(JProperty cell in item)
                        {
                            table.Columns.Add(cell.Name);
                        }
                    }
                    DataRow row = table.NewRow();
                    foreach(JProperty cell in item)
                    {
                        row[cell.Name] = cell.Value;
                    }
                    table.Rows.Add(row);
                }
            } 
            //列提取做表结构
            else
            {
                foreach(string extract in columnExtracts)
                {
                    table.Columns.Add(extract);
                }
                foreach(JToken item in list)
                {
                    DataRow row = table.NewRow();
                    foreach(string extract in columnExtracts)
                    {
                        JToken cell = item.SelectToken(extract);
                        row[extract] = cell?.ToString();
                    }
                    table.Rows.Add(row);
                } 
            }
            return table;
        }
        //
        [DisplayName("保存表")]
        public override int Save(DataTable table)
        {
            string text = JsonConvert.SerializeObject(table);
            File.WriteAllText(FilePath, text, string.IsNullOrEmpty(Enctype) ? Encoding.UTF8 : Encoding.GetEncoding(Enctype));
            return table.Rows.Count;
        }


        //缓存json对象
        protected JToken _json;
    }
}