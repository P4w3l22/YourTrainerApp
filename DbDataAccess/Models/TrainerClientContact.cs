using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbDataAccess.Models;

public class TrainerClientContact
{
	public int Id { get; set; }
	public int SenderId { get; set; }
	public int ReceiverId { get; set; }
	public string MessageType { get; set; }
	public string MessageContent { get; set; }
	public int IsRead {  get; set; }
	public DateTime SendDateTime { get; set; }
}
