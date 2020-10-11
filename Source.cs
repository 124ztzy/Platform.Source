using System.Data;
using System.ComponentModel;

namespace Platform.Source
{
    //
    [DisplayName("抽象数据源")]
    public abstract class Source
    {
        //
        [DisplayName("初始化")]
        protected abstract void Init();
        //
        [DisplayName("加载表")]
        public abstract DataTable Load(string tableExtract, params string[] columnExtracts);
        //
        [DisplayName("保存表")]
        public abstract int Save(DataTable table);


        //
        [DisplayName("显示")]
        public override string ToString()
        {
            return SourceName;
        }


        //
        [DisplayName("数据源名")]
        public string SourceName { get; set; }

    }
}