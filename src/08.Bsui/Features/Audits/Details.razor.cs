﻿using Microsoft.AspNetCore.Components;
using MudBlazor;
using Pertamina.SIMIT.Bsui.Common.Constants;
using Pertamina.SIMIT.Bsui.Features.Audits.Constants;
using Pertamina.SIMIT.Shared.Audits.Queries.GetAudit;
using Pertamina.SIMIT.Shared.Common.Constants;
using Pertamina.SIMIT.Shared.Common.Extensions;
using Pertamina.SIMIT.Shared.Common.Responses;

namespace Pertamina.SIMIT.Bsui.Features.Audits;

public partial class Details
{
    [Parameter]
    public Guid AuditId { get; set; }

    private bool _isLoading;
    private ErrorResponse? _error;
    private List<BreadcrumbItem> _breadcrumbItems = new();
    private GetAuditResponse _audit = default!;

    protected override async Task OnParametersSetAsync()
    {
        await Reload();
    }

    private async Task Reload()
    {
        SetupBreadcrumb();

        await GetAudit();

        if (_audit is null)
        {
            _breadcrumbItems.Add(CommonBreadcrumbFor.Active(CommonDisplayTextFor.Error));

            return;
        }

        _breadcrumbItems.Add(CommonBreadcrumbFor.Active($"{_audit.ActionType} {_audit.EntityName} on {_audit.Created.ToLongDateTimeDisplayText()}"));
    }

    private async Task GetAudit()
    {
        _isLoading = true;

        var response = await _auditService.GetAuditAsync(AuditId);

        _isLoading = false;

        if (response.Error is not null)
        {
            _error = response.Error;

            return;
        }

        _audit = response.Result!;
    }

    private void SetupBreadcrumb()
    {
        _breadcrumbItems = new()
        {
            CommonBreadcrumbFor.Home,
            BreadcrumbFor.Index
        };
    }
}
