using Microsoft.AspNetCore.Mvc;
using school_project.Models;
using System.Threading.Tasks;
using System;
using System.Net.Http;
using System.IO;

namespace school_project.Controllers;

public class DiagnosisController: Controller
{
    public string apiUrl = "http://localhost:12345/classification";
    private static async Task<byte[]> ReadStreamAsync(FileStream stream)
    {
        using (var memoryStream = new MemoryStream())
        {
            await stream.CopyToAsync(memoryStream);
            return memoryStream.ToArray();
        }
    }
    
    public ViewResult Single_Diagnosis()
    {
        return View();
    }

    public async Task<IActionResult> ImageInference(string imagePath)
    {
        try{
             using (HttpClient httpClient = new HttpClient())
             using (FileStream imageStream = File.OpenRead(imagePath))
             {
                 // Create a ByteArrayContent with the image data
                ByteArrayContent content = new ByteArrayContent(await ReadStreamAsync(imageStream));

                // Set the Content-Type header
                content.Headers.Add("Content-Type", "image/jpeg");

                // Send the POST request
                HttpResponseMessage response = await httpClient.PostAsync(apiUrl, content);

                // Check the response status
                if (response.IsSuccessStatusCode)
                {
                    string responseContent = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"Response: {responseContent}");
                }
                else
                {
                    Console.WriteLine($"Request failed with status code {response.StatusCode}");
                }
             }
        }
        catch(Exception ex){
            Console.WriteLine($"An error occurred: {ex.Message}");
        }
    }

     public ViewResult Batch_Diagnosis()
    {
        return View();
    }

}