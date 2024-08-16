using Domain.Models.ApplicationModels;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure
{
    public class DirectoryService : BackgroundService
    {
        public DirectoryService(IOptions<ServsOptions> options) 
        {
            GoodsImagesPath = options.Value.GoodsImagesPath;
        }
        private readonly string GoodsImagesPath;
        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            string[] paths = GoodsImagesPath.Split("\\");
            for (int i = 1; i < paths.Length; i++)
            {
                paths[i] = paths[i - 1] + "\\" + paths[i];
            }
            foreach (string path in paths)
            {
                DirectoryInfo drinfo = new DirectoryInfo(Directory.GetCurrentDirectory() + "\\" + path);
                if (!drinfo.Exists)
                {
                    drinfo.Create();
                }
            }
            return Task.CompletedTask;
        }
    }
}
