using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace TaskBoard.API
{
    public static class ExtensionMethods
    {
        public static async Task<string> GetBodyAsync(this HttpRequest request)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            using var sr = new StreamReader(request.Body);
            return await sr.ReadToEndAsync();
        }
    }
}