﻿using System.Text;
using FluentEmail.Core;
using FluentEmail.Core.Interfaces;
using FluentEmail.Core.Models;
using MimeKit;

namespace Pertamina.SIMIT.Infrastructure.Email.FluentMailkit;

/// <summary>
/// Send emails with the MailKit Library.
/// </summary>
public class FluentMailKitSender : ISender
{
    private readonly FluentSmtpClientOptions _smtpClientOptions;

    /// <summary>
    /// Creates a sender that uses the given SmtpClientOptions when sending with MailKit. Since the client is internal this will dispose of the client.
    /// </summary>
    /// <param name="smtpClientOptions">The SmtpClientOptions to use to create the MailKit client</param>
    public FluentMailKitSender(FluentSmtpClientOptions smtpClientOptions)
    {
        _smtpClientOptions = smtpClientOptions;
    }

    /// <summary>
    /// Send the specified email.
    /// </summary>
    /// <returns>A response with any errors and a success boolean.</returns>
    /// <param name="email">Email.</param>
    /// <param name="token">Cancellation Token.</param>
    public SendResponse Send(IFluentEmail email, CancellationToken? token = null)
    {
        var response = new SendResponse();
        var message = CreateMailMessage(email);

        if (token?.IsCancellationRequested ?? false)
        {
            response.ErrorMessages.Add("Message was cancelled by cancellation token.");
            return response;
        }

        try
        {
            if (_smtpClientOptions.UsePickupDirectory)
            {
                SaveToPickupDirectory(message, _smtpClientOptions.MailPickupDirectory).Wait();
                return response;
            }

            using var client = new FluentSmtpClient();
            if (_smtpClientOptions.SocketOptions.HasValue)
            {
                client.Connect(
                    _smtpClientOptions.Server,
                    _smtpClientOptions.Port,
                    _smtpClientOptions.SocketOptions.Value,
                    token.GetValueOrDefault());
            }
            else
            {
                client.Connect(
                    _smtpClientOptions.Server,
                    _smtpClientOptions.Port,
                    _smtpClientOptions.UseSsl,
                    token.GetValueOrDefault());
            }

            // Note: only needed if the SMTP server requires authentication
            if (_smtpClientOptions.RequiresAuthentication)
            {
                client.Authenticate(_smtpClientOptions.User, _smtpClientOptions.Password, token.GetValueOrDefault());
            }

            client.Send(message, token.GetValueOrDefault());
            client.Disconnect(true, token.GetValueOrDefault());
        }
        catch (Exception ex)
        {
            response.ErrorMessages.Add(ex.Message);
        }

        return response;
    }

    /// <summary>
    /// Send the specified email.
    /// </summary>
    /// <returns>A response with any errors and a success boolean.</returns>
    /// <param name="email">Email.</param>
    /// <param name="token">Cancellation Token.</param>
    public async Task<SendResponse> SendAsync(IFluentEmail email, CancellationToken? token = null)
    {
        var response = new SendResponse();
        var message = CreateMailMessage(email);

        if (token?.IsCancellationRequested ?? false)
        {
            response.ErrorMessages.Add("Message was cancelled by cancellation token.");
            return response;
        }

        try
        {
            if (_smtpClientOptions.UsePickupDirectory)
            {
                await SaveToPickupDirectory(message, _smtpClientOptions.MailPickupDirectory);
                return response;
            }

            using var client = new FluentSmtpClient();

            client.CheckCertificateRevocation = false;
            client.ServerCertificateValidationCallback = (s, c, h, e) => true;

            client.DeliveryStatusNotificationType = MailKit.Net.Smtp.DeliveryStatusNotificationType.Full;

            if (_smtpClientOptions.SocketOptions.HasValue)
            {
                await client.ConnectAsync(
                    _smtpClientOptions.Server,
                    _smtpClientOptions.Port,
                    _smtpClientOptions.SocketOptions.Value,
                    token.GetValueOrDefault());
            }
            else
            {
                await client.ConnectAsync(
                    _smtpClientOptions.Server,
                    _smtpClientOptions.Port,
                    _smtpClientOptions.UseSsl,
                    token.GetValueOrDefault());
            }

            // Note: only needed if the SMTP server requires authentication
            if (_smtpClientOptions.RequiresAuthentication)
            {
                await client.AuthenticateAsync(_smtpClientOptions.User, _smtpClientOptions.Password, token.GetValueOrDefault());
            }

            response.MessageId = message.MessageId;

            await client.SendAsync(message, token.GetValueOrDefault());

            await client.DisconnectAsync(true, token.GetValueOrDefault());
        }
        catch (Exception ex)
        {
            response.ErrorMessages.Add(ex.Message);
        }

        return response;
    }

    /// <summary>
    /// Saves email to a pickup directory.
    /// </summary>
    /// <param name="message">Message to save for pickup.</param>
    /// <param name="pickupDirectory">Pickup directory.</param>
    private static async Task SaveToPickupDirectory(MimeMessage message, string pickupDirectory)
    {
        // Note: this will require that you know where the specified pickup directory is.
        var path = Path.Combine(pickupDirectory, Guid.NewGuid().ToString() + ".eml");

        if (File.Exists(path))
        {
            return;
        }

        try
        {
            using var stream = new FileStream(path, FileMode.CreateNew);
            await message.WriteToAsync(stream);
            return;
        }
        catch (IOException)
        {
            // The file may have been created between our File.Exists() check and
            // our attempt to create the stream.
            throw;
        }
    }

    /// <summary>
    /// Create a MimMessage so MailKit can send it
    /// </summary>
    /// <returns>The mail message.</returns>
    /// <param name="email">Email data.</param>
    private static MimeMessage CreateMailMessage(IFluentEmail email)
    {
        var data = email.Data;

        var message = new MimeMessage();

        // fixes https://github.com/lukencode/FluentEmail/issues/228
        // if for any reason, subject header is not added, add it else update it.
        if (!message.Headers.Contains(HeaderId.Subject))
        {
            message.Headers.Add(HeaderId.Subject, Encoding.UTF8, data.Subject ?? string.Empty);
        }
        else
        {
            message.Headers[HeaderId.Subject] = data.Subject ?? string.Empty;
        }

        message.Headers.Add(HeaderId.Encoding, Encoding.UTF8.EncodingName);

        // Khusus untuk dapetin DSN (Delivery Status Notification)
        message.Headers.Add(HeaderId.ReturnReceiptTo, data.FromAddress.EmailAddress);

        // Khusus untuk dapetin Read Receipt atau email nya udah di baca atau belum ?
        //message.Headers.Add(HeaderId.DispositionNotificationTo, data.FromAddress.EmailAddress);

        message.From.Add(new MailboxAddress(data.FromAddress.Name, data.FromAddress.EmailAddress));

        var builder = new BodyBuilder();
        if (!string.IsNullOrEmpty(data.PlaintextAlternativeBody))
        {
            builder.TextBody = data.PlaintextAlternativeBody;
            builder.HtmlBody = data.Body;
        }
        else if (!data.IsHtml)
        {
            builder.TextBody = data.Body;
        }
        else
        {
            builder.HtmlBody = data.Body;
        }

        data.Attachments.ForEach(x =>
        {
            var attachment = builder.Attachments.Add(x.Filename, x.Data, ContentType.Parse(x.ContentType));
            attachment.ContentId = x.ContentId;
        });

        message.Body = builder.ToMessageBody();

        foreach (var header in data.Headers)
        {
            message.Headers.Add(header.Key, header.Value);
        }

        data.ToAddresses.ForEach(x => message.To.Add(new MailboxAddress(x.Name, x.EmailAddress)));
        data.CcAddresses.ForEach(x => message.Cc.Add(new MailboxAddress(x.Name, x.EmailAddress)));
        data.BccAddresses.ForEach(x => message.Bcc.Add(new MailboxAddress(x.Name, x.EmailAddress)));
        data.ReplyToAddresses.ForEach(x => message.ReplyTo.Add(new MailboxAddress(x.Name, x.EmailAddress)));

        switch (data.Priority)
        {
            case Priority.Low:
                message.Priority = MessagePriority.NonUrgent;
                break;
            case Priority.Normal:
                message.Priority = MessagePriority.Normal;
                break;
            case Priority.High:
                message.Priority = MessagePriority.Urgent;
                break;
        }

        return message;
    }
}
