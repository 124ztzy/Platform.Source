using System.Net;
using System.ComponentModel;
using System.Text;
using System.Data;
using System;

namespace Platform.Source
{
    //
    [DisplayName("网络数据源")]
    public class WebSource : TextSource
    {
        //
        [DisplayName("初始化")]
        protected override void Init()
        {
            //下载数据
            WebClient client = new WebClient();
            Encoding encoding = string.IsNullOrEmpty(Enctype) ? Encoding.UTF8 : Encoding.GetEncoding(Enctype);
            client.Headers.Add(HttpRequestHeader.Referer, Referer);
            client.Headers.Add(HttpRequestHeader.Cookie, Cookie);
            byte[] bytes = null;
            if(string.IsNullOrEmpty(Post))
                bytes = client.DownloadData(Url);
            else
                bytes = client.UploadData(Url, encoding.GetBytes(Post));
            //创建子数据源
            switch(SourceType)
            {
                case SourceType.Excel:
                    break;
                case SourceType.Text:
                    TextSource textSource = new TextSource();
                    textSource._text = encoding.GetString(bytes);
                    _fileSource = textSource;
                    break;
                case SourceType.Csv:
                    CsvSource csvSource = new CsvSource();
                    csvSource._text = encoding.GetString(bytes);
                    _fileSource = csvSource;
                    break;
                case SourceType.Xml:
                    break;
                case SourceType.Html:
                    break;
                case SourceType.Json:
                    JsonSource jsonSource = new JsonSource();
                    jsonSource._text = encoding.GetString(bytes);
                    _fileSource = jsonSource;
                    break;
            }
        }
        //
        [DisplayName("提取")]
        public override DataTable Load(string tableExtract, params string[] columnExtracts)
        {
            if(_fileSource == null)
                Init();
            return _fileSource.Load(tableExtract, columnExtracts);
        }
        

        //
        [DisplayName("引用地址")]
        public string Referer { get; set; }
        //
        [DisplayName("请求地址")]
        public string Url { get; set; }
        //
        [DisplayName("提交参数")]
        public string Post { get; set; }
        //
        [DisplayName("浏览器Cookie")]
        public string Cookie { get; set; }
        //
        [DisplayName("源类型")]
        public SourceType SourceType { get; set; }


        //缓存文件源对象
        private FileSource _fileSource;
    }


    //源类型
    public enum SourceType
    {
        Excel,
        Text,
        Csv,
        Xml,
        Html,
        Json,
    }
}