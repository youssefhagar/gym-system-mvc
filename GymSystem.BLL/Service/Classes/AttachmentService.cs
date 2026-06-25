using GymSystem.BLL.Service.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymSystem.BLL.Service.Classes
{
    public class AttachmentService : IAttachmentService
    {

        private readonly long MaxFileSizeInMB = 5*1024*1024; // 5 MB in bytes 
        private readonly string[] allowedExtention = { ".jpg", ".jpeg", ".png" };  // 5 MB in bytes 
        private readonly ILogger<AttachmentService> logger;
        private readonly IWebHostEnvironment env;

        public AttachmentService(ILogger<AttachmentService> logger, IWebHostEnvironment env)
        {
            this.logger = logger;
            this.env = env;
        }

        public bool Delete(string filename, string foldername)
        {

            if (string.IsNullOrEmpty(filename) || string.IsNullOrEmpty(foldername)) return false;

            try
            {
                var fullpath = Path.Combine(env.ContentRootPath, foldername, filename);
                if (File.Exists(fullpath))
                {
                    File.Delete(fullpath);
                    return true;
                }
                return false;
            }
            catch (Exception)
            {
                logger.LogError("Failed to delete file {filename} in folder {foldername}.", filename, foldername);
                return false; 
            }

        }

        public (Stream stream, string contenttype)? GetFile(string filename, string foldername)
        {
            string filePath = Path.Combine(env.WebRootPath, foldername, filename);

            if (!File.Exists(filePath))
            {
                return null;
            }

            var contentType = GetContentType(filename);
            var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);

            return (fileStream, contentType);

        }
        private string GetContentType(string fileName)
        {
            var extension = Path.GetExtension(fileName).ToLower();

            return extension switch
            {
                ".jpg" => "image/jpeg",
                ".jpeg" => "image/jpeg",
                ".png" => "image/png",
                _ => "application/octet-stream",
            };
        }

        public async Task<string>? UploadAsync(Stream stream, string filename, string foldername, CancellationToken ct)
        {
            if (stream is null || !stream.CanRead) return null!;
            if (stream.Length == 0) return null!;
            if(stream.Length > MaxFileSizeInMB)
            {
                logger.LogWarning($"File size {stream.Length} bytes is greater than the allowed size of {MaxFileSizeInMB} bytes.");
                return null!;
            }
            var fileExtention = Path.GetExtension(filename);
            if(String.IsNullOrEmpty(fileExtention) || !allowedExtention.Contains(fileExtention))
            {
                logger.LogWarning($"File extention {fileExtention} is not allowed.");
                return null!;
            }

            var uploadFolderPath = Path.Combine(env.WebRootPath, foldername);
            Directory.CreateDirectory(uploadFolderPath);
            var storedfilename = $"{Guid.NewGuid()}{fileExtention}";
            var filePath = Path.Combine(uploadFolderPath, storedfilename);
            try
            {
                await using var fileStream = new FileStream(filePath, FileMode.Create,FileAccess.Write,FileShare.None);
                await stream.CopyToAsync(fileStream, ct);
                return storedfilename;
            }
            catch (Exception)
            {
                logger.LogWarning($"Failed to upload file {filename} to {filePath}.");
                return null!;
            }

        }
    }
}
