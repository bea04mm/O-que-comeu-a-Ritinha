using System.Net;
using System.Net.Mail;

namespace O_que_comeu_a_Ritinha
{
	/// <summary>
	/// Classe responsavel pelo envio de emails.
	/// Implementa a interface IEmailSender.
	/// </summary>
	public class EmailSender : IEmailSender
	{
		/// <summary>
		/// Envia um e-mail de forma assincrona
		/// </summary>
		/// <param name="email">O endereço de email do destinatario</param>
		/// <param name="subject">O assunto do email</param>
		/// <param name="message">O conteudo da mensagem do e-mail</param>
		public Task SendEmailAsync(string email, string subject, string message)
		{
			// Endereço de e-mail do remetente
			var mail = "oquecomeuaritinha@outlook.com";

			// Criacao do cliente SMTP com as configuracoes do servidor de email
			var client = new SmtpClient("smtp.office365.com", 587)
			{
				EnableSsl = true, // Habilita a criptografia SSL
				UseDefaultCredentials = false, // Nao usa as credenciais padrão do sistema
				Credentials = new NetworkCredential(mail, "Ritinha22!") // Credenciais do remetente
			};

			// Envio do e-mail assincrono
			return client.SendMailAsync(
				new MailMessage(from: mail, // Endereço de email do remetente
								to: email, // Endereço de email do destinatario
								subject, // Assunto do email
								message // Mensagem do email
								));
		}
	}

}