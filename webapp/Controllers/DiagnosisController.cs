using Microsoft.AspNetCore.Mvc;
using school_project.Models;
using System.Threading.Tasks;
using System;
using System.Net.Http;
using school_project.ViewModels;
using Microsoft.AspNetCore.Hosting;


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

}