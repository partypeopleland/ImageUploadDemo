using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ImageUploadDemo.Adapter;
using ImageUploadDemo.Helper;
using ImageUploadDemo.Models;

namespace ImageUploadDemo.Controllers
{
    public class HomeController : Controller
    {
        private MariaAdapter _adapter;

        protected MariaAdapter Adapter
        {
            get => _adapter ?? new MariaAdapter();
            set => _adapter = value;
        }

        public ActionResult Index()
        {
            return View(Adapter.GetAll());
        }

        /// <summary>
        /// 圖片上傳
        /// </summary>
        /// <returns></returns>
        public ActionResult Upload()
        {
            return View();
        }

        /// <summary>
        /// 表單資料+圖片上傳
        /// </summary>
        /// <returns></returns>
        public ActionResult Form()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Upload(List<HttpPostedFileBase> files)
        {
            Dictionary<string,byte[]> dict = PrepareZipData(files);

            // 壓縮
            byte[] data = ZipHelper.ZipData(dict);

            // 寫DB
            Adapter.Create(new ZipEntity()
            {
                Data = data,
                Before = GetOriginLength(files),
                After = data.Length
            });

            return RedirectToAction("Index");
        }

        private double GetOriginLength(List<HttpPostedFileBase> files) 
            => files.Sum(file => file.ContentLength);

        private Dictionary<string, byte[]> PrepareZipData(List<HttpPostedFileBase> files) 
            => files.ToDictionary(file => file.FileName, ConvertToBinary);

        private byte[] ConvertToBinary(HttpPostedFileBase file)
        {
            MemoryStream target = new MemoryStream();
            file.InputStream.CopyTo(target);
            return target.ToArray();
        }

        public ActionResult Detail(int id)
        {
            byte[] data = Adapter.Find(id)?.Data;
            if (data == null) return RedirectToAction("Index");
            
            Dictionary<string, byte[]> res = ZipHelper.UnzipData(data);
            return View(res);
        }
    }
}