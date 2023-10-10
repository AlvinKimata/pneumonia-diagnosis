﻿using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using school_project.Models;
using school_project.ViewModels;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
namespace school_project.Controllers;

public class HomeController : Controller
{
    private readonly Microsoft.AspNetCore.Hosting.IHostingEnvironment hostingEnvironment;

    private readonly ILogger<HomeController> _logger;
    private readonly AppDbContext _context;

    public HomeController(ILogger<HomeController> logger,
                        Microsoft.AspNetCore.Hosting.IHostingEnvironment hostingEnvironment,
                        AppDbContext context)
    {
        _logger = logger;
        this.hostingEnvironment = hostingEnvironment;
        _context = context;
    }

    public IActionResult Index()
    {
        return View();
    }

    [HttpGet]
    public IActionResult Create(){
        return View();
    }

    [HttpGet]
    // GET: ProfessionalBodies
    public async Task<IActionResult> List()
    {
        return View(await _context.SingleImageDiagnosis.ToListAsync());
    }

    [HttpGet]
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var singleImageDiagnisisInstance = await _context.SingleImageDiagnosis
                .FirstOrDefaultAsync(m => m.Id == id);
            if (singleImageDiagnisisInstance == null)
            {
                return NotFound();
            }

            return View(singleImageDiagnisisInstance);
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
    public async Task<IActionResult> Create(SingleImageDiagnosisViewModel model)
    {
        if (ModelState.IsValid)
        {
            string uniqueFileName = ProcessUploadedFile(model);
            SingleImageDiagnosis newSingleImageDiagnosis = new SingleImageDiagnosis
            {
                Name = model.Name,
                Photos = uniqueFileName,
                // ImageResult = model.ImageResult
            };
            _context.Add(newSingleImageDiagnosis);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        return View();
    }

        // [HttpGet]
        // public ViewResult Edit(int id)
        // {
        //     // Project project = _employeeRepository.GetEmployee(id);
        //     EmployeeEditViewModel employeeEditViewModel = new EmployeeEditViewModel()
        //     {
        //         Id = employee.Id,
        //         Name = employee.Name,
        //         Email = employee.Email,
        //         Deprtment = employee.Deprtment,
        //         ExistingPhotoPath = employee.PhotoPath
        //     };

        //     return View(employeeEditViewModel);
        // }

        // [HttpPost]
        // public IActionResult Edit(EmployeeEditViewModel model)
        // {
        //     if (ModelState.IsValid)
        //     {
        //         Employee employee = _employeeRepository.GetEmployee(model.Id);
        //         //Update properties.
        //         employee.Name = model.Name;
        //         employee.Email = model.Email;
        //         employee.Deprtment = model.Deprtment;

        //         if (model.Photos != null)
        //         {
        //             if (model.ExistingPhotoPath != null)
        //             {
        //                 string filePath = Path.Combine(hostingEnvironment.WebRootPath, "images", model.ExistingPhotoPath);
        //                 System.IO.File.Delete(filePath);
        //             };

        //             employee.PhotoPath = ProcessUploadedFile(model);
        //         }

        //         _employeeRepository.Update(employee);
        //         return RedirectToAction("Index");

        //     }

        //     return View();
        // }


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
