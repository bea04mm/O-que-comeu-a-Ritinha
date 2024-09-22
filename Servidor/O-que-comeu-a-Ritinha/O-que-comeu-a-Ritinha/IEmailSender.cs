namespace O_que_comeu_a_Ritinha
{
	/// <summary>
	/// Interface para o envio de emails
	/// </summary>
	public interface IEmailSender
	{
		/// <summary>
		/// Metodo assincrono para enviar um email
		/// </summary>
		/// <param name="email">O endereço de email do destinatario</param>
		/// <param name="subject">O assunto do email</param>
		/// <param name="message">O conteudo da mensagem do email</param>
		Task SendEmailAsync(string email, string subject, string message);
	}
}