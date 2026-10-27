using KinemoPlatform.API.Maintenance.Domain.Model.ValueObjects;
namespace KinemoPlatform.API.Maintenance.Interfaces.REST.Resources;
public record EquipmentTestResource(int Id, DateTimeOffset TestDate, ETestResult Result, string Observations, IEnumerable<ChannelTestResource> ChannelTests);