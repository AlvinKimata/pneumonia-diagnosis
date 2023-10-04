using System;
using System.Net.Http;
using System.IO;
using System.Threading.Tasks;

class Program
{
    static async Task Main(string[] args)
    {
        // Replace with the actual URL
        string apiUrl = "http://localhost:12345/classification";
        string imagePath = "inputs/normal.jpeg"; // Replace with the actual path to your image file

        try
        {
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
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
        }
    }

    private static async Task<byte[]> ReadStreamAsync(FileStream stream)
    {
        using (var memoryStream = new MemoryStream())
        {
            await stream.CopyToAsync(memoryStream);
            return memoryStream.ToArray();
        }
    }
}
