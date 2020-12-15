using System.Linq;
using System.IO;
using System;

namespace WeSplit.Utils
{
    class StringHelper
    {
        private static readonly string[] VietnameseSigns = new string[]

        {

            "aAeEoOuUiIdDyY",

            "áàạảãâấầậẩẫăắằặẳẵ",

            "ÁÀẠẢÃÂẤẦẬẨẪĂẮẰẶẲẴ",

            "éèẹẻẽêếềệểễ",

            "ÉÈẸẺẼÊẾỀỆỂỄ",

            "óòọỏõôốồộổỗơớờợởỡ",

            "ÓÒỌỎÕÔỐỒỘỔỖƠỚỜỢỞỠ",

            "úùụủũưứừựửữ",

            "ÚÙỤỦŨƯỨỪỰỬỮ",

            "íìịỉĩ",

            "ÍÌỊỈĨ",

            "đ",

            "Đ",

            "ýỳỵỷỹ",

            "ÝỲỴỶỸ"

        };
        public static string ConvertToNoSpaceAndUnsigned(string input)
        {
            // Bỏ đi khoảng trắng
            input = string.Concat(input.Where(c => !char.IsWhiteSpace(c)));

            //Tiến hành thay thế , lọc bỏ dấu cho chuỗi tiếng việt
            for (int i = 1; i < VietnameseSigns.Length; i++)
            {
                for (int j = 0; j < VietnameseSigns[i].Length; j++)
                    input = input.Replace(VietnameseSigns[i][j], VietnameseSigns[0][i - 1]);
            }
            input = input.ToLower();
            return input;
        }
        public static string CopyFile(string pathCopy, int tripID, bool type)
        {              
            string newThumbnailFileName = Path.GetFileName(pathCopy);

            string newThumbnail;
            if (type)
            {
                newThumbnail = $"Assets\\trips\\{tripID}\\{newThumbnailFileName}";
            }
            else newThumbnail = $"Assets\\trips\\{tripID}\\list\\{newThumbnailFileName}";

            // Create dir if it is new trip
            Directory.CreateDirectory($"Assets\\trips\\{tripID}\\list");
            // Copy images to dir
            string currentFolder = AppDomain.CurrentDomain.BaseDirectory;
            string newThumbnailDestination = $"{currentFolder}{newThumbnail}";
            File.Copy(pathCopy, newThumbnailDestination, true);
            return newThumbnail;
        }
    }
}
