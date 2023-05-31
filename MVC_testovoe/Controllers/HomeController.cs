using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MVC_testovoe.Data;
using MVC_testovoe.Models;
using MVC_testovoe.Models.VM;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace MVC_testovoe.Controllers
{
    public class HomeController : Controller
    {
        private ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }


        //Функция, которая экспортирует данные в формат Excel
        public async Task<ActionResult> ExportToExcel()
        {
            var izdels = await _context.Izdels.ToListAsync();// Получаем все изделия из базы данных асинхронно
            var links = await _context.Links.ToListAsync();// Получаем все связи между изделиями из базы данных асинхронно

            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Sheet 1");

                worksheet.Cells[1, 1].Value = "Изделие";
                worksheet.Cells[1, 2].Value = "Кол-во";
                worksheet.Cells[1, 3].Value = "Стоимость";
                worksheet.Cells[1, 4].Value = "Цена";
                //worksheet.Cells[1, 5].Value = "Уровень";

                int row = 2;
                foreach (var izdel in izdels)// Проверяем, является ли изделие корневым 
                {
                    var isRoot = links.All(x => x.IzdelId != izdel.Id);
                    if (isRoot)
                    {
                        RecursiveAdd(izdel, links, worksheet, ref row, 1, 1);// Если изделие корневое, рекурсивно добавляем его и все подизделия в Excel
                    }
                }

                worksheet.Column(1).AutoFit();
                var stream = new MemoryStream();
                package.SaveAs(stream);

                string excelName = $"Отчет-{DateTime.Now.ToString("yyyyMMddHHmmssfff")}.xlsx";

                return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", excelName);
            }
        }

        // Рекурсивная функция для добавления изделия и всех его подизделий в Excel
        private decimal RecursiveAdd(Izdel izdel, List<Link> links, ExcelWorksheet worksheet, ref int row, int level, int quantity)
        {

            decimal cost = izdel.Price * quantity;// Вычисляем стоимость изделия как произведение его цены на количество

            worksheet.Cells[row, 1].Value = new String(' ', (level - 1) * 5) + izdel.Name;
            worksheet.Cells[row, 2].Value = quantity;
            worksheet.Cells[row, 3].Value = cost; //временная цена
            worksheet.Cells[row, 4].Value = izdel.Price;
            //worksheet.Cells[row, 5].Value = level;
            int currentRow = row; 
            row++;

            var relatedLinks = links.Where(l => l.IzdelUpId == izdel.Id);// Получаем все связи, где текущее изделие является родительским

            foreach (var link in relatedLinks)// Для каждой связи рекурсивно добавляем подизделие в Excel
            {
                cost += RecursiveAdd(link.Izdel, links.Where(l => l.IzdelUpId == link.IzdelId).ToList(), worksheet, ref row, level + 1, link.kol);
            }

            worksheet.Cells[currentRow, 3].Value = cost; // Обновляем стоимость в Excel
            return cost;// Возвращаем итоговую стоимость
        }

        [HttpPost]
        public ActionResult AddIzdelAndLink(IzdelLinkViewModel obj)
        {
            if (!string.IsNullOrEmpty(obj.NewIzdel.Name))
            {
                var id = _context.Izdels.Max(x => x.Id);
                obj.NewLink.IzdelId = id + 1;
                _context.Izdels.Add(obj.NewIzdel);
                if (obj.NewLink.kol != 0 && obj.NewLink.IzdelUpId != 0) _context.Links.Add(obj.NewLink);
                _context.SaveChanges();
            }
            return RedirectToAction("Index");
        }


        public ActionResult AddIzdelAndLink()
        {
            var izdels = _context.Izdels.ToList();
            var links = _context.Links.ToList();
            var viewModel = new IzdelLinkViewModel
            {
                Izdels = izdels,
                Links = links
            };

            return View(viewModel);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
