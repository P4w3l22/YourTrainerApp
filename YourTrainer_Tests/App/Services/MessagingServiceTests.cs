using Moq;
using Newtonsoft.Json;
using NUnit.Framework;
using System.Net;
using Xunit.Abstractions;
using YourTrainer_App.Services.APIServices.IServices;
using YourTrainer_App.Services.DataServices;
using YourTrainerApp.Models;
using static YourTrainer_Utility.StaticDetails;
using Assert = Xunit.Assert;

namespace YourTrainer_Tests.App.Services;

public class MessagingServiceTests
{
	private readonly ITestOutputHelper _output;

	public MessagingServiceTests(ITestOutputHelper output)
	{
		_output = output;
	}

	[Fact]
	public async Task SendMessage_ValidData_CallsSendMessage()
	{
		// Arrange
		string newMessage = "Wiadomość";
		int senderId = 1;
		int receiverId = 2;

		Mock<ITrainerClientContactService> mockTrainerClientContactService = new();
		IMessagingService service =
			new MessagingService(mockTrainerClientContactService.Object);

		// Act
		await service.SendMessage(newMessage, senderId, receiverId, MessageType.Text.ToString());

		// Assert
		mockTrainerClientContactService.Verify(
			x => x.SendMessageAsync<APIResponse>(It.Is<TrainerClientContact>(msg =>
				msg.SenderId == senderId &&
				msg.ReceiverId == receiverId &&
				msg.MessageType == MessageType.Text.ToString() &&
				msg.MessageContent == newMessage
			)),
			Times.Once
		);

	}

	[Fact]
	public async Task SendMessage_EmptyMessage_CallsSendMessage()
	{
		// Arrange
		string newMessage = "";
		int senderId = 1;
		int receiverId = 2;

		Mock<ITrainerClientContactService> mockTrainerClientContactService = new();
		IMessagingService service =
			new MessagingService(mockTrainerClientContactService.Object);

		// Act
		await service.SendMessage(newMessage, senderId, receiverId, MessageType.Text.ToString());

		// Assert
		mockTrainerClientContactService.Verify(
			x => x.SendMessageAsync<APIResponse>(It.IsAny<TrainerClientContact>()),
			Times.Never
		);
	}

	[Fact]
	public async Task SendMessage_NullMessage_CallsSendMessage()
	{
		// Arrange
		string? newMessage = null;
		int senderId = 1;
		int receiverId = 2;

		Mock<ITrainerClientContactService> mockTrainerClientContactService = new();
		IMessagingService service =
			new MessagingService(mockTrainerClientContactService.Object);

		// Act
		await service.SendMessage(newMessage, senderId, receiverId, MessageType.Text.ToString());

		// Assert
		mockTrainerClientContactService.Verify(
			x => x.SendMessageAsync<APIResponse>(It.IsAny<TrainerClientContact>()),
			Times.Never
		);
	}
}
