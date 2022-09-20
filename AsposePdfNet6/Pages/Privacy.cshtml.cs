using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text;
using Aspose.Pdf;

namespace AsposePdfNet6.Pages
{
    public class PrivacyModel : PageModel
    {
        private readonly ILogger<PrivacyModel> _logger;
        private readonly IConfiguration _config;

        public PrivacyModel(ILogger<PrivacyModel> logger, IConfiguration config)
        {
            _logger = logger;
            _config = config;
        }

        public ActionResult OnGet()
        {
            var license = _config["AsposeLicense"];

            using MemoryStream licenseStream = license.AsMemoryStream();

            new License().SetLicense(licenseStream);

            Document pdf = new ("testfile.pdf");

            pdf = pdf.RemoveBlankPages();

            // IsBlank() method thorws error
            if (pdf.Pages[1].IsBlank(0.01d)) 
            {
                _logger.LogInformation("first page is blank");
            }

            using MemoryStream ms = new ();

            pdf.Save(ms);

            return File(ms.ToArray(), "application/octet-stream", "testfile.pdf");
        }

    }

    public static class Utils 
    {
        public static MemoryStream AsMemoryStream(this string content)
            => new (Encoding.ASCII.GetBytes(content));

        public static Document RemoveBlankPages(this Document document)
        {
            var blankPageNumbers = document.Pages
                .Select((page, index) => new { 
                    isBlank = page.IsBlank(0.01d), 
                    pageNumber = index + 1 // page number is one based indexed
                })
                .Where(p => p.isBlank)
                .Select(p => p.pageNumber);

            document.Pages.Delete(blankPageNumbers.ToArray());

            return document;
        }
    } 
}