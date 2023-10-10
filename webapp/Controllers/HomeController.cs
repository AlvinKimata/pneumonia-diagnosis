using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using school_project.Models;
using school_project.ViewModels;
using System.ComponentModel.DataAnnotations;
namespace school_project.Controllers;

public class HomeController : Controller
{
    private readonly Microsoft.AspNetCore.Hosting.IHostingEnvironment hostingEnvironment;
    private readonly IProjectRepository projectRepository;

    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger,
                        Microsoft.AspNetCore.Hosting.IHostingEnvironment hostingEnvironment,
                        IProjectRepository projectRepository)
    {
        _logger = logger;
        this.hostingEnvironment = hostingEnvironment;
        _projectRepository = projectRepository;
    }

    public IActionResult Index()
    {
        return View();
    }

    public ViewResult ListSingleImageProject()
    {
        var model = _projectRepository.GetProject();
        return View(model);
    }


    [HttpPost]
    public IActionResult CreateSingleImageProject(SingleImageDiagnosisViewModel model)
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

            projectRepository.Add(newSingleImageDiagnosis);
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
