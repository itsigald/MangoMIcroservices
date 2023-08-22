using Mango.Services.EmailAPI.Data;
using Mango.Services.EmailAPI.Models;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System.Text;

namespace Mango.Services.EmailAPI.Services
{
    public class EmailService
    {
        private readonly DbContextOptions<AppDbContext> _context;

        public EmailService(DbContextOptions<AppDbContext> context)
        {
            _context = context;
        }

        public async Task EmailCartAndLog(CartDto? cartDto)
        {
            StringBuilder sbMessage = new StringBuilder();

            if(cartDto != null )
            {
                sbMessage.AppendLine("<h1>Cart Email Requested</h1> ");
                sbMessage.AppendLine($"<h4>Date Request: {cartDto.CartHeader.CartInsert.ToString("dd/MM/yyyy HH:mm:ss") }</h4>");
                sbMessage.AppendLine("<h4>Details</h4>");

                sbMessage.AppendLine("<table>");
                sbMessage.AppendLine("<tr>");
                sbMessage.AppendLine("<td><b>Product</b></td>");
                sbMessage.AppendLine("<td><b>Price</b></td>");
                sbMessage.AppendLine("<td><b>Quantity</b></td>");
                sbMessage.AppendLine("</tr>");

                if (cartDto.CartDetails != null && cartDto.CartDetails.Count() > 0)
                {
                    foreach (var item in cartDto.CartDetails)
                    {
                        sbMessage.Append("<tr>");
                        sbMessage.Append($"<td>{ item.Product!.Name }</td>");
                        sbMessage.Append($"<td style='text-align:right'>{ string.Format("{0:c}", item.Product!.Price) }</td>");
                        sbMessage.Append($"<td style='text-align:right'>{ item.Quantity }</td>");
                        sbMessage.Append("</tr>");
                    }
                    sbMessage.Append("</table>");
                }
                else
                {
                    sbMessage.Append("<tr><td colspan='3' style='text-align:center;'><b>No details are available</b></td></tr>");
                }

                sbMessage.Append("<br />");
                sbMessage.Append("<table>");
                sbMessage.Append("<tr>");
                sbMessage.Append($"<td><b>Total:</b></td>");
                sbMessage.Append($"<td><b>{string.Format("{0:c}", cartDto.CartHeader.CartTotal) }</b></td>");
                sbMessage.Append("</tr>");

                sbMessage.Append("<tr>");
                sbMessage.Append($"<td><b>Discount:</b></td>");
                sbMessage.Append($"<td><b>{string.Format("{0:c}", cartDto.CartHeader.Discount)}</b></td>");
                sbMessage.Append("</tr>");
                sbMessage.Append("</table>");
            }
            else
            {
                sbMessage.Append("<p>Cart is empty</p>");
            }

            await LogAndEmail(sbMessage.ToString(), cartDto.EmailInfoDto);
        }

        private async Task<bool> LogAndEmail(string message, EmailInfoDto emailInfo)
        {
            try
            {
                EmailLogger emailLog = new()
                {
                    EmailTo = emailInfo.EmailTo,
                    EmailFrom = emailInfo.EmailFrom,
                    EmailSent = DateTime.Now,
                    Message = message
                };

                await using var _db = new AppDbContext(_context);
                await _db.EmailLoggers.AddAsync(emailLog);
                await _db.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
