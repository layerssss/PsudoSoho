using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Net;
using System.IO;
namespace YupooAPI.NET
{
    class yPostFileRequest:Abstract.YRequest
    {
        public static Stream UploadFileEx(
            string uploadfile,
            bool hideFilename,
            string filename,
            string url,
            string fileFormName,
            string contenttype,
            string postData)
        {
            if ((fileFormName == null) ||
             (fileFormName.Length == 0))
            {
                fileFormName = "file";
            }

            if ((contenttype == null) ||
             (contenttype.Length == 0))
            {
                contenttype = "application/octet-stream";
            }
            long length = 0;

            string boundary = "----------" + DateTime.Now.Ticks.ToString("x");
            HttpWebRequest webrequest = (HttpWebRequest)WebRequest.Create(url);
            webrequest.Timeout=Abstract.YAPI.Timeout;
            webrequest.ContentType = "multipart/form-data; boundary=" + boundary;
            webrequest.Method = "POST";

            // Build up the post message header
            StringBuilder sb = new StringBuilder();

            foreach (var par in postData.Split(new[] { "&" }, StringSplitOptions.RemoveEmptyEntries))
            {
                var name = par.Split('=')[0];
                var value = System.Web.HttpUtility.UrlDecode(par.Split('=')[1]);
                sb.Append("--");
                sb.Append(boundary);
                sb.Append("\r\n");
                sb.Append("Content-Disposition: form-data; name=\"");
                sb.Append(name);
                sb.Append("\"\r\n");
                sb.Append("\r\n");
                sb.Append(value);
                sb.Append("\r\n");
            }

            sb.Append("--");
            sb.Append(boundary);
            sb.Append("\r\n");
            sb.Append("Content-Disposition: form-data; name=\"");
            sb.Append(fileFormName);
            sb.Append("\"; filename=\"");
            sb.Append(hideFilename ? filename : Path.GetFileName(uploadfile));
            sb.Append("\"");
            sb.Append("\r\n");
            sb.Append("Content-Type: ");
            sb.Append(contenttype);
            sb.Append("\r\n");
            sb.Append("\r\n");

            string postHeader = sb.ToString();
            byte[] postHeaderBytes = Encoding.ASCII.GetBytes(postHeader);

            // Build the trailing boundary string as a byte array
            // ensuring the boundary appears on a line by itself
            byte[] boundaryBytes = Encoding.ASCII.GetBytes("\r\n--" + boundary + "--\r\n");

            FileStream fileStream = new FileStream(uploadfile, FileMode.Open, FileAccess.Read);
            fileStream.Seek(0, SeekOrigin.Begin);
                length += postHeaderBytes.Length + (new FileInfo(uploadfile)).Length + boundaryBytes.Length;



                webrequest.ContentLength = length;

                Stream requestStream = webrequest.GetRequestStream();

                // Write out our post header
                requestStream.Write(postHeaderBytes, 0, postHeaderBytes.Length);

                // Write out the file contents
                byte[] buffer = new Byte[4096];
                int bytesRead = 0;
            try{
                while ((bytesRead = fileStream.Read(buffer, 0, buffer.Length)) > 0)
                {
                    requestStream.Write(buffer, 0, bytesRead);
                }
                fileStream.Close();
            }
            catch(Exception ex)
            {
                throw (new Exception("ErrorWriting at " + fileStream.Position + "/" + length + ":" + ex.ToString() + "\r\nHeader:\r\n" + sb.ToString()));
            }

            // Write out the trailing boundary
            requestStream.Write(boundaryBytes, 0, boundaryBytes.Length);
            requestStream.Flush();
            WebResponse responce = webrequest.GetResponse();
            Stream s = responce.GetResponseStream();
            return s;
        }
        yPostFileRequest()
        {
        }
        public yPostFileRequest(string filePath, string fileFormName, string fileContentType,bool hideFilename,string filename)
        {
            this.Params["filePath"] = filePath;
            this.Params["fileFormName"] = fileFormName;
            this.Params["fileContentType"] = fileContentType;
            this.Params["hideFilename"] = hideFilename.ToString();
            this.Params["filename"] = filename;
        }
        public override void MakeResponse(Abstract.YResponse response)
        {
            var url = this.urlBase + "api/upload/";
            //try
            //{
                var filePath = this.Params["filePath"]; 
                this.Params.Remove("filePath");

                var fileFormName = this.Params["fileFormName"];
                this.Params.Remove("fileFormName");

                var fileContentType = this.Params["fileContentType"];
                this.Params.Remove("fileContentType");

                var hideFilename = Convert.ToBoolean(this.Params["hideFilename"]);
                this.Params.Remove("hideFilename");

                var fileName = this.Params["filename"];
                this.Params.Remove("filename");

                var res = UploadFileEx(filePath,hideFilename,fileName, url , fileFormName, fileContentType,this.makeQuery());
                response.ReadREST(res);
                try
                {
                    res.Close();
                }
                catch { }
            //}
            //catch (Exception ex)
            //{
            //    throw (new Exception(string.Format("URL:{0} EX:{1}", url, ex.Message)));
            //}
        }
    }
}
