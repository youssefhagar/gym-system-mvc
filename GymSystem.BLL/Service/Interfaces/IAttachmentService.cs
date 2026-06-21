using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymSystem.BLL.Service.Interfaces
{
    public interface IAttachmentService
    {
        Task<string>? UploadAsync(Stream stream, string filename, string foldername, CancellationToken ct);
        bool Delete(string filename, string foldername);
       // (string filename, string foldername)? GetAttachment(string filename, string foldername);
    }
}
