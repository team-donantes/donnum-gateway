using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Donnum.Gateway.Application.Features.Metrics.Queries;
using Donnum.Gateway.Application.Models.Metrics;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Donnum.Gateway.Presentation.API.Controllers;

/// <summary>
/// Proporciona endpoints para obtener métricas agregadas de hospitales.
/// </summary>
[ApiController]
[Route("api/metrics/hospital")]
public class HospitalMetricsController : ControllerBase
{
    private readonly IMediator _mediator;

    public HospitalMetricsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Obtiene el resumen general para el dashboard del hospital.
    /// </summary>
    [HttpGet("summary")]
    [ProducesResponseType(typeof(HospitalSummaryDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
    public async Task<IActionResult> GetSummary(
        [FromQuery] string? period, 
        [FromQuery] DateOnly? from, 
        [FromQuery] DateOnly? to, 
        CancellationToken cancellationToken)
    {
        var query = new GetHospitalSummaryQuery(period, from, to);
        var result = await _mediator.Send(query, cancellationToken);
        return Ok(result);
    }

    /// <summary>
    /// Obtiene el rendimiento de las campañas asociadas al hospital.
    /// </summary>
    [HttpGet("campaign-performance")]
    [ProducesResponseType(typeof(IReadOnlyList<CampaignPerformanceDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
    public async Task<IActionResult> GetCampaignPerformance(
        [FromQuery] string? period, 
        [FromQuery] Guid? campaignId, 
        [FromQuery] int? limit, 
        CancellationToken cancellationToken)
    {
        var query = new GetCampaignPerformanceQuery(period, campaignId, limit);
        var result = await _mediator.Send(query, cancellationToken);
        return Ok(result);
    }

    /// <summary>
    /// Obtiene estadísticas agregadas de solicitudes de emergencia por estado e índices de cobertura.
    /// </summary>
    [HttpGet("request-performance")]
    [ProducesResponseType(typeof(RequestPerformanceDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
    public async Task<IActionResult> GetRequestPerformance(
        [FromQuery] string? period, 
        [FromQuery] int? limit, 
        CancellationToken cancellationToken)
    {
        var query = new GetRequestPerformanceQuery(period, limit);
        var result = await _mediator.Send(query, cancellationToken);
        return Ok(result);
    }

    /// <summary>
    /// Obtiene el nivel de demanda de unidades de sangre agrupadas por tipo sanguíneo.
    /// </summary>
    [HttpGet("blood-type-demand")]
    [ProducesResponseType(typeof(IReadOnlyList<BloodTypeDemandDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
    public async Task<IActionResult> GetBloodTypeDemand(
        [FromQuery] string? period, 
        [FromQuery] string? bloodType, 
        [FromQuery] int? limit, 
        CancellationToken cancellationToken)
    {
        var query = new GetBloodTypeDemandQuery(period, bloodType, limit);
        var result = await _mediator.Send(query, cancellationToken);
        return Ok(result);
    }

    /// <summary>
    /// Obtiene la evolución histórica de solicitudes solicitadas y cubiertas por períodos.
    /// </summary>
    [HttpGet("monthly-evolution")]
    [ProducesResponseType(typeof(IReadOnlyList<MonthlyEvolutionDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
    public async Task<IActionResult> GetMonthlyEvolution(
        [FromQuery] string? period, 
        [FromQuery] int? limit, 
        CancellationToken cancellationToken)
    {
        var query = new GetMonthlyEvolutionQuery(period, limit);
        var result = await _mediator.Send(query, cancellationToken);
        return Ok(result);
    }
}
