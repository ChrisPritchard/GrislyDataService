
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using Microsoft.AspNetCore.Mvc;

namespace GrislyDataService.Controllers
{
    [Route("api/Entries")]
    public class EntryController : Controller
    {
        const string zipFileName = "./AppData/entries.zip";

        [HttpGet("")]
        public IActionResult Get([FromQuery]string entryName)
        {
            var entryData = GetEntriesFromZip(entryName).Single();
            return entryData != null 
                ? Content(entryData, "application/json") 
                : (IActionResult)NotFound();
        }

        private IEnumerable<string> GetEntriesFromZip(params string[] entryNames)
        {
            using(var zipFile = new FileStream(zipFileName, FileMode.Open))
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