
using System.ComponentModel;
using System.Text;

namespace Platform.Source
{
    //
    [DisplayName("抽象文件数据源")]
    public abstract class FileSource : Source
    {
        //
        [DisplayName("文件路径")]
        public string FilePath { get; set; }
        
    }
}