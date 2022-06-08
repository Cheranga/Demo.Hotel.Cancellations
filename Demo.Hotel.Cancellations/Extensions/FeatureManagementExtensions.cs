using Microsoft.FeatureManagement;

namespace Demo.Hotel.Cancellations.Extensions;

public static class FeatureManagementExtensions
{
    public static Task<bool> IsFeatureEnabled(this IFeatureManager featureManager, Features.Features feature)
    {
        return featureManager.IsEnabledAsync(feature.ToString());
    }
}