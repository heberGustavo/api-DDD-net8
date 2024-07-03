﻿using AutoMapper;
using Domain.Interfaces;
using Entities.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Models;

namespace WebAPI.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class MessageController : ControllerBase
	{
		private readonly IMapper _mapper;
		private readonly IMessage _message;

		public MessageController(IMapper mapper, IMessage message)
		{
			_mapper = mapper;
			_message = message;
		}

		[Authorize]
		[Produces("application/json")]
		[HttpPost("/api/Add")]
		public async Task<List<Notifies>> Add(MessageViewModel messageViewModel)
		{
			messageViewModel.UserId = await RetornaIdUsuarioLogado();
			
			var messageMap = _mapper.Map<Message>(messageViewModel);
			await _message.Add(messageMap);
			return messageMap.Notificacoes;
		}

		[Authorize]
		[Produces("application/json")]
		[HttpPost("/api/Update")]
		public async Task<List<Notifies>> Update(MessageViewModel messageViewModel)
		{
			messageViewModel.UserId = await RetornaIdUsuarioLogado();

			var messageMap = _mapper.Map<Message>(messageViewModel);
			await _message.Update(messageMap);
			return messageMap.Notificacoes;
		}

		[Authorize]
		[Produces("application/json")]
		[HttpPost("/api/Delete")]
		public async Task<List<Notifies>> Delete(MessageViewModel messageViewModel)
		{
			messageViewModel.UserId = await RetornaIdUsuarioLogado();

			var messageMap = _mapper.Map<Message>(messageViewModel);
			await _message.Delete(messageMap);
			return messageMap.Notificacoes;
		}

		[Authorize]
		[Produces("application/json")]
		[HttpGet("/api/GetById")]
		public async Task<MessageViewModel> GetById(MessageViewModel messageViewModel)
		{
			messageViewModel = _mapper.Map<MessageViewModel>(await _message.GetEntityById(messageViewModel.Id));
			return messageViewModel;
		}

		[Authorize]
		[Produces("application/json")]
		[HttpGet("/api/GetAll")]
		public async Task<List<MessageViewModel>> GetAll()
		{
			return _mapper.Map<List<MessageViewModel>>(await _message.List());
		}


		#region Private Methods

		private async Task<string> RetornaIdUsuarioLogado()
		{
			if (User != null)
				return User.FindFirst("idUsuario").Value;

			return string.Empty;
		}

		#endregion
	}
}
