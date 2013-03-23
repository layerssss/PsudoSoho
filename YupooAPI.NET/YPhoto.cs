using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YupooAPI.NET
{
    public class YPhoto:Abstract.YAPI
    {

        public YUploadPhotoResponse UploadPhoto(
            string token,
            string filePath,
            bool hideFilename,
            string fileName,
            string fileContentType,
            string albumId,
            string title,
            string description,
            params string[] tags)
        {
            var req = new yPostFileRequest(filePath, "photo", fileContentType,hideFilename,fileName);
            this.MakeRequest(req);
            req.Params["auth_token"] = token;
            req.Params["album_id"] = albumId;
            req.Params["title"] = title;
            req.Params["description"] = description;
            //req.Params["tags"] = "";
            //foreach (var tag in tags)
            //{
            //    req.Params["tags"] += tag + ' ';
            //}
            //req.Params["tags"] = req.Params["tags"].TrimEnd();
            var res = new YUploadPhotoResponse();
            req.MakeResponse(res);
            return res;
        }
        public YUploadPhotoResponse UploadPhoto(
            string token,
            string filePath,
            string fileContentType,
            string albumId,
            string title,
            string description,
            params string[] tags)
        {
            return UploadPhoto(token, filePath, false,"", fileContentType, albumId, title, description, tags);
        }
        public class YUploadPhotoResponse : Abstract.YResponse
        {
            public string Id
            {
                get{
                    return base.Dics["photo"]["id"];
                }
            }
            public string Owner
            {
                get
                {
                    return base.Dics["photo"]["owner"];
                }
            }
            public string Dir
            {
                get
                {
                    return base.Dics["photo"]["dir"];
                }
            }
            public string AbsoluteUrl
            {
                get
                {
                    return '/' + base.Dics["photo"]["bucket"] + '/' + base.Dics["photo"]["key"] + '/' + base.Dics["photo"]["secret"] + ".jpg";
                }
            }
            public string AbsoluteUrlSmall
            {
                get
                {
                    return '/' + base.Dics["photo"]["bucket"] + '/' + base.Dics["photo"]["key"] + "/small/";
                }
            }
            public string AbsoluteUrlMedium
            {
                get
                {
                    return '/' + base.Dics["photo"]["bucket"] + '/' + base.Dics["photo"]["key"] + "/medium/";
                }
            }
            public string AbsoluteUrlThumb
            {
                get
                {
                    return '/' + base.Dics["photo"]["bucket"] + '/' + base.Dics["photo"]["key"] + "/thumb/";
                }
            }
            public string Title
            {
                get
                {
                    return base.Dics["photo"]["title"];
                }
            }
        }
        public void Delete(string token, string id)
        {

            var req = new yGetRequest();
            this.MakeRequest(req);
            req.Method = "yupoo.photos.delete";
            req.Params["auth_token"] = token;
            req.Params["photo_id"] = id;
            var res = new yBlankResponse();
            req.MakeResponse(res);
        }
        public YPhoto(string sharedKey, string apiKey, string endPointRootUrl)
            : base( sharedKey,  apiKey,  endPointRootUrl)
        {
        }
    }
}
