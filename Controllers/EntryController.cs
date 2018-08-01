
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace GrislyDataService.Controllers
{
    [Route("api/Entries")]
    public class EntryController : Controller
    {
        private readonly GrislyConfig config;

        public EntryController(IOptions<GrislyConfig> configOptions) => config = configOptions.Value;

        [HttpGet("")]
        public IActionResult GetAll()
        {
            return Content("[1,2,3,4,5]", "application/json");
        }

        [HttpGet("{entryName}")]
        public IActionResult Get([FromRoute]string entryName)
        {
            var entryData = GetEntriesFromZip(entryName).Single();
            return entryData != null 
                ? Content(entryData, "application/json") 
                : (IActionResult)NotFound();
        }

        private IEnumerable<string> GetEntriesFromZip(params string[] entryNames)
        {
            using(var zipFile = new FileStream(config.DataPath, FileMode.Open))
            using(var archive = new ZipArchive(zipFile, ZipArchiveMode.Read))
            {   
                foreach(var entryName in entryNames)
                {
                    var entry = archive.GetEntry(entryName + ".json");
                    if(entry == null)
                        yield return null;
                    else
                        using(var reader = new StreamReader(entry.Open()))
                            yield return reader.ReadToEnd();
                }
            }
        }
    }
}