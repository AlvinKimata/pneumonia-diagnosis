using Microsoft.AspNetCore.Mvc;
using school_project.Models;
using System.Threading.Tasks;
using System;
using System.Net.Http;
using System.IO;
using Microsoft.Extensions.Configuration;


namespace school_project.Controllers;

public class DiagnosisController: Controller
{
    private IConfiguration _config;

    public Startup(IConfiguration config)
        {
            _config = config;
        }

    private static async Task<byte[]> ReadStreamAsync(FileStream stream)
    {
        using (var memoryStream = new MemoryStream())
        {
            await stream.CopyToAsync(memoryStream);
            return memoryStream.ToArray();
        }
    }
    
   
    public async Task<IActionResult> ImageInference(string imagePath)
    {
        var classificationEndpoint = _config.GetConnectionString("classificationEndpoint");

        try{
             using (HttpClient httpClient = new HttpClient())
             using (FileStream imageStream = File.OpenRead(imagePath))
             {
                 // Create a ByteArrayContent with the image data
                ByteArrayContent content = new ByteArrayContent(await ReadStreamAsync(imageStream));

                // Set the Content-Type header
                content.Headers.Add("Content-Type", "image/jpeg");

                // Send the POST request
                HttpResponseMessage response = await httpClient.PostAsync(classificationEndpoint, content);

                // Check the response status
                if (response.IsSuccessStatusCode)
                {
                    string responseContent = await response.Content.ReadAsStringAsync();
                    return Json($"Response: {responseContent}");
                }
                else
                {
                    return Json($"Request failed with status code {response.StatusCode}");
                }
             }
        }
        catch(Exception ex){
            Console.WriteLine($"An error occurred: {ex.Message}");
        }
    }

   [HttpPost]
   [AllowAnonymous]
    public async Task<IActionResult> ImageInference(SingleImageDiagnosisViewModel model)
    {
        if (ModelState.IsValid)
        {
            var imageInstance = new SingleImageDiagnosis { 
                id = model.id,
                PhotoPath = model.PhotoPath
            };
            var modelResult = await ImageInference(imageInstance.PhotoPath)

            if (modelResult.Succeeded)
            {
                return modelResult
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }
        }
        return View(model);
    }

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

    public ViewResult Batch_Diagnosis()
    {
        return View();
    }

    public ViewResult Single_Diagnosis()
    {
        return View();
    }

}