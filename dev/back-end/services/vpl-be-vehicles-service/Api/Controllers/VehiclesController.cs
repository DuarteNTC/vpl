﻿using AutoMapper;

using MediatR;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using VehiclesService.App.Commands.Vehicles;
using VehiclesService.App.Queries.Vehicles;
using VehiclesService.Domain.Contracts.Services;
using VehiclesService.Domain.ViewModels;
using VehiclesService.Domain.ViewModels.Vehicles;

namespace VehiclesService.Api.Controllers
{
    /// <summary>
    /// Rotas responsáveis pelo CRUD de veiculos
    /// </summary>
    [Route("[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class VehiclesController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        public VehiclesController(IMediator mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }

        /// <summary>
        /// Ação responsável por listar todas as veiculos cadastradas
        /// </summary>
        /// <returns>Lista de veiculos cadastradas</returns>
        [HttpGet]
        public async Task<IList<VehicleVm>?> List()
        {
            return await _mediator.Send(new ListVehicleQuery());
        }

        /// <summary>
        /// Ação responsável por listar todas os veiculos cadastrados pelos ids especificados
        /// </summary>
        /// <param name="ids">Ids dos veículos a serem listados</param>
        /// <returns>Lista de veiculos cadastradas</returns>
        [HttpPost("listByIds")]
        public async Task<IList<VehicleVm>?> ListByIds([FromBody] IList<long> ids)
        {
            return await _mediator.Send(new ListVehiclesByIdsQuery
            {
                Ids = ids
            });
        }

        /// <summary>
        /// Ação responsável por encontrar uma veiculos a partir de um Id
        /// </summary>
        /// <param name="id">Id da veiculos a ser encontrada</param>
        /// <returns>veiculo encontrada</returns>
        [HttpGet("{id}")]
        public async Task<VehicleVm?> Find(long id)
        {
            return await _mediator.Send(new FindVehicleByIdQuery
            {
                Id = id
            });
        }

        /// <summary>
        /// Ação responsável por encontrar os veiculos a partir do ano e referencia de preço.
        /// </summary>
        /// <param name="priceReferenceNumber">Preço de referência</param>
        /// <param name="year">Ano de referência</param>
        /// <returns>veiculo encontrada</returns>
        [HttpGet("{priceReferenceNumber}/{year}")]
        public async Task<IList<VehicleVm>?> listByPriceYearReference(long priceReferenceNumber, long year) 
            => await _mediator.Send(new ListVehicleByReferenceYearQuery
                {
                    PriceReferenceNumber = priceReferenceNumber,
                    Year = year,

                });

        /// <summary>
        /// Ação responsável por listar os veículos por uma marca especifica
        /// </summary>
        /// <param name="vehicleId">Id da marca</param>
        /// <returns>Lista de veículos por marca</returns>
        [HttpGet("vehicle/{vehicleId}")]
        public async Task<IList<VehicleVm>?> ListByBrand(long vehicleId)
        {
            return await _mediator.Send(new ListVehicleByBrandIdQuery
            {
                BrandId = vehicleId
            });
        }

        /// <summary>
        /// Ação responsável por listar os veículos por uma marca especifica
        /// </summary>
        /// <param name="modelId">Id da marca</param>
        /// <returns>Lista de veículos por marca</returns>
        [HttpGet("model/{modelId}")]
        public async Task<IList<VehicleVm>?> ListByModel(long modelId)
        {
            return await _mediator.Send(new ListVehicleByModelIdQuery
            {
                ModelId = modelId
            });
        }

        /// <summary>
        /// Método responsável pela criação de uma nova veiculos
        /// </summary>
        /// <param name="vehicle">veiculo a ser inserida</param>
        /// <returns>veiculo inserida</returns>
        [HttpPost]
        [Authorize]
        public async Task<VehicleVm> Create([FromBody] CreateVehicleVm vehicle)
        {
            var command = _mapper.Map<CreateVehicleCommand>(vehicle);

            return await _mediator.Send(command);
        }

        /// <summary>
        /// Método responsável pela edição de uma veiculos
        /// </summary>
        /// <param name="id">Id da veiculos a ser editada</param>
        /// <param name="vehicle">veiculo a ser editada</param>
        /// <returns>veiculo editada</returns>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(BadRequestResultVm))]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound, "text/plain")]
        [Authorize]
        public async Task<VehicleVm> Update(long id, [FromBody] CreateVehicleVm vehicle)
        {
            var command = _mapper.Map<UpdateVehicleCommand>(vehicle);
            command.Id = id;

            return await _mediator.Send(command);
        }

        /// <summary>
        /// Método responsável pela remoção de uma veiculos
        /// </summary>
        /// <param name="id">Id da veiculos a ser removida</param>
        /// <returns>Resultado de remoção</returns>
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<RemoveResultVm> Remove(long id)
        {
            return await _mediator.Send(new RemoveVehicleCommand
            {
                Id = id
            });
        }
    }
}
