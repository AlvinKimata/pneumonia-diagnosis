using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using school_project.Models;
using school_project.ViewModels;
using System.ComponentModel.DataAnnotations;
namespace school_project.Controllers;

public class HomeController : Controller
{
    private readonly Microsoft.AspNetCore.Hosting.IHostingEnvironment hostingEnvironment;

    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger,
                        Microsoft.AspNetCore.Hosting.IHostingEnvironment hostingEnvironment)
    {
        _logger = logger;
        this.hostingEnvironment = hostingEnvironment;
    }

    public IActionResult Index()
    {
        return View();
    }

    [HttpGet]
    public IActionResult Create(){
        return View();
    }


    [HttpPost]
    private string ProcessUploadedFile(SingleImageDiagnosisViewModel model)
    {
        string uniqueFileName = null;
      
        if (model.Photos != null && model.Photos.Count > 0)
        {
            foreach (IFormFile photo in model.Photos)
            {
                string uploadsFolder = Path.Combine(hostingEnvironment.WebRootPath, "images");
                uniqueFileName = Guid.NewGuid().ToString() + "_" + photo.FileName;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    photo.CopyTo(fileStream);
                }   
            }
        }
        return uniqueFileName;
    }

   
    [HttpPost]
    public IActionResult Create(SingleImageDiagnosisViewModel model)
    {
        if (ModelState.IsValid)
        {
            string uniqueFileName = ProcessUploadedFile(model);
            SingleImageDiagnosis newSingleImageDiagnosis = new SingleImageDiagnosis
            {
                Name = model.Name,
                Photos = uniqueFileName,
                ImageResult = model.ImageResult
            };

            // projectRepository.Add(newSingleImageDiagnosis);
            return RedirectToAction("Index");

        }

        return View();
    }
    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }


}
