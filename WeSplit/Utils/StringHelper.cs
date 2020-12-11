using System.Text.RegularExpressions;
using System.Linq;

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
    }
}
