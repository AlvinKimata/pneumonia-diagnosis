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

    public DiagnosisController(Microsoft.AspNetCore.Hosting.IHostingEnvironment hostingEnvironment)
        {
            this.hostingEnvironment = hostingEnvironment;
        }

    private static async Task<byte[]> ReadStreamAsync(FileStream stream)
    {
        using (var memoryStream = new MemoryStream())
        {
            await stream.CopyToAsync(memoryStream);
            return memoryStream.ToArray();
        }
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