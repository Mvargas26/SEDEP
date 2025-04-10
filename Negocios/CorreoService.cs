﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Negocios
{
    public class CorreoService
    {

        // envia codigo de seguridad
        public async Task EnviarCodigoSeguridad(string correo, string codigo)
        {
            try
            {
                var clienteCorreo = new SmtpClient("smtp.gmail.com")
                {
                    Port = 587,  
                    Credentials = new NetworkCredential("sedep8099@gmail.com", "serm psmx uakd qdmo"),  // correo del remitente
                    EnableSsl = true
                };

                // crea el mensaje del correo
                var mensaje = new MailMessage
                {
                    From = new MailAddress("sedep8099@gmail.com"),  // correo del remitente
                    Subject = "Código de seguridad",
                    Body = $"Tu código de seguridad es {codigo}",
                    IsBodyHtml = false
                };

                mensaje.To.Add(correo);

                await clienteCorreo.SendMailAsync(mensaje);

                Console.WriteLine("Correo enviado exitosamente.");
            }
            catch (Exception ex)
            {
                throw new Exception("Error al enviar el correo de seguridad.", ex);
            }
        }
    }
}
