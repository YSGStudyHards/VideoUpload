/**
 * Authority：追追时光者
 * CreateTime：2020.08.01
 * Description：文件，图片，视频，音频统一上传接口
 */
using System;
using System.IO;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace VideoUpload.Controllers
{
    /// <summary>
    /// 文件，图片，视频，音频统一上传服务
    /// </summary>
    public class FileUploadController : Controller
    {
        /// <summary>
        /// 对验证和处理 HTML 窗体中的输入数据所需的信息进行封装，如FromData拼接而成的文件[图片,视频，文档等文件上传]
        /// </summary>
        /// <param name="context">FemContext对验证和处理html窗体中输入的数据进行封装</param>
        /// <returns></returns>
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult FileLoad(FormContext context)//FemContext对验证和处理html窗体中输入的数据进行封装
        {

            HttpPostedFileBase httpPostedFileBase = Request.Files[0];//获取文件流
            if (httpPostedFileBase != null)
            {
                try
                {
                    ControllerContext.HttpContext.Request.ContentEncoding = Encoding.GetEncoding("UTF-8");
                    ControllerContext.HttpContext.Response.Charset = "UTF-8";

                    string fileName = Path.GetFileName(httpPostedFileBase.FileName);//原始文件名称
                    string fileExtension = Path.GetExtension(fileName);//文件扩展名

                    byte[] fileData = ReadFileBytes(httpPostedFileBase);//文件流转化为二进制字节

                    string result = SaveFile(fileExtension, fileData);//文件保存
                    return string.IsNullOrEmpty(result) ? Json(new { code = 0, path = "", msg = "网络异常，文件上传失败~" }) : Json(new { code = 1, path = result, msg = "文件上传成功" });
                }
                catch (Exception ex)
                {
                    return Json(new { code = 0, msg = ex.Message, path = "" });
                }
            }
            else
            {
                return Json(new { code = 0, path = "", msg = "网络异常，文件上传失败~" });
            }
        }


        /// <summary>
        /// 将文件流转化为二进制字节
        /// </summary>
        /// <param name="fileData">图片文件流</param>
        /// <returns></returns>
        private byte[] ReadFileBytes(HttpPostedFileBase fileData)
        {
            byte[] data;
            using (var inputStream = fileData.InputStream)
            {
                if (!(inputStream is MemoryStream memoryStream))
                {
                    memoryStream = new MemoryStream();
                    inputStream.CopyTo(memoryStream);
                }
                data = memoryStream.ToArray();
            }
            return data;
        }

        /// <summary>
        /// 保存文件
        /// </summary>
        /// <param name="fileExtension">文件扩展名</param>
        /// <param name="fileData">图片二进制文件信息</param>
        /// <returns></returns>
        private string SaveFile(string fileExtension, byte[] fileData)
        {
            string result;
            string saveName = Guid.NewGuid().ToString() + fileExtension; //保存文件名称
            string basePath = "UploadFile";
            string saveDir = DateTime.Now.ToString("yyyy-MM-dd");

            // 文件上传后的保存路径
            string serverDir = Path.Combine(Server.MapPath("~/"), basePath, saveDir);

            string fileNme = Path.Combine(serverDir, saveName);//保存文件完整路径
            try
            {
                var savePath = Path.Combine(saveDir, saveName);

                //项目中是否存在文件夹，不存在创建
                if (!Directory.Exists(serverDir))
                {
                    Directory.CreateDirectory(serverDir);
                }

                System.IO.File.WriteAllBytes(fileNme, fileData);//WriteAllBytes创建一个新的文件，按照对应的文件流写入，假如已存在则覆盖

                //返回前端项目文件地址
                result = "/" + basePath + "/" + saveDir + "/" + saveName;
            }
            catch (Exception ex)
            {
                result = "发生错误" + ex.Message;
            }
            return result;
        }

    }
}