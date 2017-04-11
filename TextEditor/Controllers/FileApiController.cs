using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace TextEditor.Controllers
{
    [RoutePrefix("api/file")]
    public class FileApiController : ApiController
    {
        [Route("upload")]
        public async Task<HttpResponseMessage> Upload()
        {
            if (!Request.Content.IsMimeMultipartContent())
            {
                return new HttpResponseMessage(HttpStatusCode.UnsupportedMediaType);
            }

            var provider = new MultipartMemoryStreamProvider();
            await Request.Content.ReadAsMultipartAsync(provider);
            foreach (var file in provider.Contents)
            {
                var dataStream = await file.ReadAsByteArrayAsync();
                Stream stream = new MemoryStream(dataStream);
                var filename = file + ".docx";
                WriteFile(filename, dataStream);
            }

            return new HttpResponseMessage(HttpStatusCode.OK);
        }

        public static void WriteFile(string fileName, byte[] bytes)
        {
            string path = "~/Upload/";

            if (File.Exists(HttpContext.Current.Server.MapPath(path+fileName)))
                File.Delete(HttpContext.Current.Server.MapPath(path+fileName));
            try
            {
                using (FileStream fs = new FileStream(HttpContext.Current.Server.MapPath(path+fileName), FileMode.CreateNew, FileAccess.Write))
                {
                    fs.Write(bytes, 0, bytes.Length);
                    fs.Close();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            
        }
    } 
}
