using Microsoft.AspNetCore.Mvc;
using Modelos;
using Negocios;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using System.IO;

namespace SEDEP.Controllers
{
    public class ReportesController : Controller
    {

        ReportesNegocio Objeto = new ReportesNegocio();

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult ReporteJefaturas()
        {
            return View();
        }

        public IActionResult ReportesRRHH()
        {
            return View();
        }

        public IActionResult ReportePersonal()
        {
            try
            {
                var reportes = Objeto.ListarReportes();
                return View(reportes);
            }
            catch (Exception ex)
            {
                TempData["MensajeError"] = $"Error al obtener los reportes: {ex.Message}";
                return View(new List<ReporteMolde>());
            }
        }

        [HttpPost]
        public IActionResult ReporteID(string Cedula)
        {
            try
            {
                var reportes = Objeto.ListarReportesID(Cedula);
                return View("ReportePersonal", reportes);
            }
            catch (Exception ex)
            {
                TempData["MensajeError"] = $"Error al obtener los reportes: {ex.Message}";
                return View(new List<ReporteMolde>());
            }
        }

        // MÉTODO PARA CREAR EL PDF
        public IActionResult ReportePersonalPDF()
        {
            try
            {
                var reportes = Objeto.ListarReportes();

                var stream = new MemoryStream();

                var document = Document.Create(container =>
                {
                    container.Page(page =>
                    {
                        page.Margin(20);

                        page.Content()
                            .Column(col =>
                            {
                                col.Item().Text("Reporte de Evaluaciones").FontSize(20).Bold().AlignCenter();
                                col.Item().Text("");

                                foreach (var rep in reportes)
                                {
                                    col.Item().BorderBottom(1).Padding(5).Column(card =>
                                    {
                                        card.Item().Text($"Cédula: {rep.Cedula}").FontSize(12).Bold();
                                        card.Item().Text($"Nombre: {rep.Nombre}").FontSize(12);
                                        card.Item().Text($"ID Evaluación: {rep.IdEva}").FontSize(12);
                                        card.Item().Text($"Fecha de Creación: {rep.Fecha:dd/MM/yyyy}").FontSize(12);
                                        card.Item().Text($"Observaciones: {rep.Observaciones}").FontSize(12);
                                        card.Item().Text($"Objetivo: {rep.Objetivo}").FontSize(12);
                                        card.Item().Text($"Competencia: {rep.Competencia}").FontSize(12);
                                        card.Item().Text($"Valor Obtenido: {rep.Nota}").FontSize(12);
                                    });

                                    col.Item().Text(""); // Espaciado entre tarjetas
                                }
                            });
                    });
                });

                document.GeneratePdf(stream);
                stream.Position = 0;

                return File(stream, "application/pdf", "ReporteEvaluaciones.pdf");
            }
            catch (Exception ex)
            {
                return Content($"Error al generar el PDF: {ex.Message} \n\n {ex.StackTrace}");
            }
        }

    }
}
