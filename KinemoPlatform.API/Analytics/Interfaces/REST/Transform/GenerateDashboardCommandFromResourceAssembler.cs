using KinemoPlatform.API.Analytics.Domain.Model.Commands;
using KinemoPlatform.API.Analytics.Interfaces.REST.Resources;
namespace KinemoPlatform.API.Analytics.Interfaces.REST.Transform;
public static class GenerateDashboardCommandFromResourceAssembler
{
    public static GenerateDashboardCommand ToCommandFromResource(GenerateDashboardResource r) => 
        new(r.PeriodStart, r.PeriodEnd);
}