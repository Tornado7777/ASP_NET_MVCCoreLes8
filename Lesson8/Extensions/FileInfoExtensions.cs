using Microsoft.EntityFrameworkCore.Diagnostics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lesson8.Extensions
{
    public static class FileInfoExtensions
    {
        /// <summary>
        /// Метод-расширения для класса FileInfo
        /// Позволяет выполнять запуск файла
        /// </summary>
        /// <param name="file">запускаемый файл</param>
        /// <returns></returns>
        public static Process? Execute(this FileInfo file)
        {
            var processStartInfo =new ProcessStartInfo(file.FullName)
            {
                UseShellExecute = true,
            };

            return Process.Start(processStartInfo);
        }
    }
}
