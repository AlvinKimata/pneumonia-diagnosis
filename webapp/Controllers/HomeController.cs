using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using school_project.Models;
using System.Net.Http;
using System.IO;
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
    public IActionResult CreateBatch(){
        return View();
    }

    [HttpGet]
    // GET: ProfessionalBodies
    public async Task<IActionResult> List()
    {
        return View(await _context.SingleImageDiagnosis.ToListAsync());
    }


    [HttpGet]
    // GET: ProfessionalBodies
    public async Task<IActionResult> ListBatch()
    {
        return View(await _context.BatchImageDiagnosis.ToListAsync());
    }

    private static async Task<byte[]> ReadStreamAsync(FileStream stream)
    {
        using (var memoryStream = new MemoryStream())
        {
            await stream.CopyToAsync(memoryStream);
            return memoryStream.ToArray();
        }
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
            string responseContent = null;

            //Make API call.
            string apiUrl = "http://localhost:12345/classification";
            HttpClient httpClient = new HttpClient();

            string uploadsFolder = Path.Combine(hostingEnvironment.WebRootPath, "images");
            string filePath = Path.Combine(uploadsFolder, uniqueFileName);
            FileStream imageStream = System.IO.File.OpenRead(filePath);

            // Create a ByteArrayContent with the image data
            ByteArrayContent content = new ByteArrayContent(await ReadStreamAsync(imageStream));

            // Set the Content-Type header
            content.Headers.Add("Content-Type", "image/jpeg");

            //Send the POST request.
            HttpResponseMessage response = await httpClient.PostAsync(apiUrl, content);

            // Check the response status
            if (response.IsSuccessStatusCode)
            {
                responseContent = await response.Content.ReadAsStringAsync();
            }
            else
            {
                responseContent = "No prediction";
            }

            SingleImageDiagnosis newSingleImageDiagnosis = new SingleImageDiagnosis
            {
                Name = model.Name,
                Photos = uniqueFileName,
                ImageResult = responseContent
            };
            
            _context.Add(newSingleImageDiagnosis);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        return View();
    }

    [HttpPost]
    public async Task<IActionResult> CreateBatch(BatchImageDiagnosisViewModel model)
    {
        if (ModelState.IsValid)
        {
            string filePath = null;
            string apiUrl = "http://localhost:12345/classification";
            HttpClient httpClient = new HttpClient();
            List<string> responseContent = new List<string>();
            List<string> uniqueFileNames = new List<string>();

            //Loop through the images and copy them to a directory.
            foreach(IFormFile image in model.Photos)
            {            

                string uploadsFolder = Path.Combine(hostingEnvironment.WebRootPath, model.Name);
                string uniqueFileName = Guid.NewGuid().ToString() + "_" + image.FileName;
                uniqueFileNames.Add(uniqueFileName);
                filePath = Path.Combine(uploadsFolder, uniqueFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    image.CopyTo(fileStream);
                }   

                //Perform api call to the image.
                FileStream imageStream = System.IO.File.OpenRead(filePath);

                // Create a ByteArrayContent with the image data
                ByteArrayContent content = new ByteArrayContent(await ReadStreamAsync(imageStream));

                // Set the Content-Type header
                content.Headers.Add("Content-Type", "image/jpeg");

                //Send the POST request.
                HttpResponseMessage response = await httpClient.PostAsync(apiUrl, content);

                // Check the response status
                if (response.IsSuccessStatusCode)
                {
                    var imageresult = await response.Content.ReadAsStringAsync();
                    responseContent.Append(imageresult);
                }
                else
                {
                    responseContent.Append("No prediction");
                }

            }


            BatchImageDiagnosis newBatchImageDiagnosis = new BatchImageDiagnosis
            {
                Name = model.Name,
                Photos = uniqueFileNames,
                ImagesResults = responseContent
            };
            
            _context.Add(newBatchImageDiagnosis);
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
