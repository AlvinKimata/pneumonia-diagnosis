using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using school_project.Models;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;


namespace school_project.Controllers;

public class AnalysisController: Controller
{
    private readonly AppDbContext _context;
    public AnalysisController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    
    public async Task<IActionResult> Index()
    {
        return View(await _context.BatchImageDiagnosis.ToListAsync());
    }

    [HttpGet]
    public async Task<IActionResult> DataAnalysis(int? id)
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



}