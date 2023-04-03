using MimeKit;

// *******************************
// Esta POC es para enviar correos y se debe instalar el paquete MimeKit,
// e implementar el metodo lineas abajo,
// Para correos Gmail, se debe habilitar la autentocacion de doble factor y
// generar una clave para app en la ruta https://myaccount.google.com/apppasswords
// *******************************

string Response = await SendEmail("jpertuzegea@hotmail.com", "Mi asunto", "mi mensaje");
Console.WriteLine(Response);
Console.WriteLine("Salio");


// Metodo para enviar correos 
static async Task<string> SendEmail(string receptor, string asunto, string mensaje)
{
    Console.WriteLine("POC envio de correo ENTRO");

    // ****************************************
    //string UserMail = "jpertuzegea@hotmail.com";
    //string PasswordMail = "xxx123";
    //string Host = "smtp-mail.outlook.com";
    //int Port = 587;

    string UserMail = @"yoryydavi@gmail.com"; //
    string PasswordMail = "esjmajvllzhvdlae";
    string Host = "smtp.gmail.com";
    int Port = 25;
    // ****************************************

    try
    {
        MimeMessage message = new MimeMessage { Subject = asunto };
        message.From.Add(new MailboxAddress("Sigecor", UserMail));

        message.To.Add(new MailboxAddress("Custom", receptor));

        BodyBuilder bodyBuilder = new BodyBuilder { HtmlBody = mensaje };
        message.Body = bodyBuilder.ToMessageBody();

        MailKit.Net.Smtp.SmtpClient client = new MailKit.Net.Smtp.SmtpClient { CheckCertificateRevocation = true };

        client.Connect(Host, Port);
        client.Authenticate(UserMail, PasswordMail);

        var Response = await client.SendAsync(message);
        Console.WriteLine("Email enviado");
        return Response;
    }
    catch (Exception error)
    {
        Console.WriteLine("Email NO enviado");
        return error.Message;
    }
}