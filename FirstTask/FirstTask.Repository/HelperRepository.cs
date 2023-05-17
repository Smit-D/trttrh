using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FirstTask.Repository
{
    public class HelperRepository
    {
        public static string ConvertImageToBase64(IFormFile file)
        {
            using var stream = file.OpenReadStream();
            var bytes = new byte[file.Length];
            stream.Read(bytes, 0, (int)file.Length);
            var base64string = Convert.ToBase64String(bytes);
            return "data:" + file.ContentType + ";base64," + base64string;
        }
    }
}
