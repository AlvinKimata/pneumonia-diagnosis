using Microsoft.AspNetCore.Mvc;
using school_project.Models;
using System.Threading.Tasks;
using System;
using System.Net.Http;
using school_project.ViewModels;
using Microsoft.AspNetCore.Hosting;


namespace school_project.Controllers;

public class DiagnosisController: Controller
{
    private readonly Microsoft.AspNetCore.Hosting.IHostingEnvironment hostingEnvironment;
    private readonly IProjectRepository projectRepository;

    public DiagnosisController(Microsoft.AspNetCore.Hosting.IHostingEnvironment hostingEnvironment,
                              IProjectRepository projectRepository)
        {
            this.hostingEnvironment = hostingEnvironment;
            this.projectRepository = projectRepository;
        }

    private static async Task<byte[]> ReadStreamAsync(FileStream stream)
    {
        using (var memoryStream = new MemoryStream())
        {
            await stream.CopyToAsync(memoryStream);
            return memoryStream.ToArray();
        }
    }
    
    
    public async Task<IActionResult> ConvertImageToBytes(SingleImageDiagnosisViewModel model)
    {
        var apiUrl = "http://localhost:12345/classification";
        HttpClient httpClient = new HttpClient();
        if (ModelState.IsValid)
        {
            foreach(var formFile in model.Photos)
            {
                using (FileStream imageStream = System.IO.File.OpenRead(formFile.FileName))
                {
                    // Create a ByteArrayContent with the image data
                    ByteArrayContent content = new ByteArrayContent(await ReadStreamAsync(imageStream));

                    // Set the Content-Type header
                    content.Headers.Add("Content-Type", "image/jpeg");

                    //Send the POST request.
                    HttpResponseMessage response = await httpClient.PostAsync(apiUrl, content);

                    // Check the response status
                    if (response.IsSuccessStatusCode)
                    {
                        model.ImageResult = await response.Content.ReadAsStringAsync();
                        // string responseContent = await response.Content.ReadAsStringAsync();
                        // return Json($"Response: {responseContent}");
                        // return Json(new { Response = responseContent }); 
                        return View(model);

                    }
                    else
                    {
                        return Json($"Request failed with status code {response.StatusCode}");
                    }
                }
            }
        }

      return View(model);
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
    public IActionResult CreateSingleImageProject(SingleImageDiagnosisViewModel model)
    {
        if (ModelState.IsValid)
        {
            string uniqueFileName = ProcessUploadedFile(model);
            SingleImageDiagnosis newSingleImageDiagnosis = new SingleImageDiagnosis
            {
                Photos = uniqueFileName,
                ImageResult = model.ImageResult
            };

            projectRepository.Add(newSingleImageDiagnosis);
            return RedirectToAction("Index");

        }

        return View();
    }

    public ViewResult BatchImageDiagnosis()
    {
        return View();
    }
    

    public ViewResult SingleImageDiagnosis()
    {
        return View();
    }

}