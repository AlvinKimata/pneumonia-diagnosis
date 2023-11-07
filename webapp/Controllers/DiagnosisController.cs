using Microsoft.AspNetCore.Mvc;
using school_project.Models;
using System.Threading.Tasks;
using System;
using System.Net.Http;
using school_project.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;



namespace school_project.Controllers;

public class DiagnosisController : Controller
{
    private readonly Microsoft.AspNetCore.Hosting.IHostingEnvironment hostingEnvironment;
    private readonly AppDbContext _context;

    public DiagnosisController(Microsoft.AspNetCore.Hosting.IHostingEnvironment hostingEnvironment,
                            AppDbContext context)
    {
        this.hostingEnvironment = hostingEnvironment;
        _context = context;
    }

    private static async Task<byte[]> ReadStreamAsync(FileStream stream)
    {
        using (var memoryStream = new MemoryStream())
        {
            await stream.CopyToAsync(memoryStream);
            return memoryStream.ToArray();
        }
    }
    public async Task<IActionResult> BatchDetails(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var batchImageDiagnosisProject = await _context.BatchImageDiagnosis
            .Include(b => b.Photos)
            .Include(b => b.ImagesResults)
            .FirstOrDefaultAsync(m => m.Id == id);
        if (batchImageDiagnosisProject == null)
        {
            return NotFound();
        }

        return View(batchImageDiagnosisProject);
    }
    [HttpGet]
    public async Task<IActionResult> List()
    {
        return View(await _context.SingleImageDiagnosis.ToListAsync());
    }
    [HttpGet]
    public async Task<IActionResult> ListBatch()
    {
        return View(await _context.BatchImageDiagnosis.ToListAsync());
    }

    [HttpPost]
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }
        var batchImageDiagnosisProject = await _context.BatchImageDiagnosis
        .FirstOrDefaultAsync(m => m.Id == id);

        _context.Remove(batchImageDiagnosisProject);
        await _context.SaveChangesAsync();
        return RedirectToAction("ListBatch");

    }

    

    //Get results from a batch instance.
    public List<float> GetResultsFromBatchExample(int id)
    {
        BatchImageDiagnosis imageresults = _context.BatchImageDiagnosis
        .Include(b => b.ImagesResults)
        .FirstOrDefault(b => b.Id == id);

        // Create a list to store the parsed integers
        List<float> imagesStatus = new List<float>();

        // Parse each element in the string array and add it to the list
        for(int i = 0; i < imageresults.ImagesResults.Count; i++)
        {
            var floatItem = float.Parse(imageresults.ImagesResults[i].imageresult);
            imagesStatus.Add(floatItem);
        }
        return imagesStatus;
    }
}