using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Negocios
{
    public class ReportesNegocio
    {
        //crear pdf
        //descomentar despues que este el sp, para agregar las librerias y que funcione 

        /*
         public byte[] GenerarPdfReporteGenerico(string titulo, List<string> lineas)
         {
             using var stream = new MemoryStream();
             var writer = new PdfWriter(stream);
             var pdf = new PdfDocument(writer);
             var doc = new Document(pdf);

             doc.Add(new Paragraph(titulo).SetBold().SetFontSize(16));
             doc.Add(new Paragraph($"Fecha: {DateTime.Now:dd/MM/yyyy}").SetFontSize(10));

             doc.Add(new Paragraph("\n"));

             foreach (var linea in lineas)
             {
                 doc.Add(new Paragraph(linea));
             }

             doc.Close();
             return stream.ToArray();
         }
        */

    }
}
