using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Core.Helper
{
    public class UploadHelper
    {
        public static string Save(HttpPostedFileBase file,string mark)
        {
            var root = @"Upload/" + mark;
            string phicyPath = Path.Combine(HttpRuntime.AppDomainAppPath, root);
            Directory.CreateDirectory(phicyPath);
            var fileName = Guid.NewGuid().ToString("N") + file.FileName.Substring(file.FileName.LastIndexOf('.'));
            string path= Path.Combine(phicyPath, fileName);
            file.SaveAs(path);
            return string.Format("/{0}/{1}",root,fileName);
        }
    }
}
