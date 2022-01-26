using Ganss.XSS;
using System.Linq;
using System.Text;

namespace OnlineStore.Api.Infrastructure.Extensions
{
    public static class StringExtensions
    {
        public static int GetNumbers(this string input)
        {
            return input != null && int.TryParse(input.Where(c => char.IsDigit(c)).ToArray(), out var number) ? number : 0;
        }

        public static string RemoveDiacritics(this string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return value;
            }

            var stFormD = value.Normalize(NormalizationForm.FormD);
            var len = stFormD.Length;
            var sb = new StringBuilder();

            for (var i = 0; i < len; i++)
            {
                var uc = System.Globalization.CharUnicodeInfo.GetUnicodeCategory(stFormD[i]);
                if (uc != System.Globalization.UnicodeCategory.NonSpacingMark)
                {
                    sb.Append(stFormD[i]);
                }
            }

            return sb.ToString().Normalize(NormalizationForm.FormC);
        }

        public static string SafeUrl(this string input)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                return input;
            }

            input = input.Replace(' ', '-');
            input.RemoveDiacritics();
            var sb = new StringBuilder();
            foreach (var c in input)
            {
                if ((c >= '0' && c <= '9') || (c >= 'A' && c <= 'Z') || (c >= 'a' && c <= 'z') || c == '_' || c == '-' || c == '/' || c == '.')
                {
                    sb.Append(c);
                }
            }
            return sb.ToString();
        }

        public static string SanitizeHtml(this string input)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                return input;
            }

            var sanitizer = new HtmlSanitizer();
            input = sanitizer.Sanitize(input);

            return input;
        }
    }
}
