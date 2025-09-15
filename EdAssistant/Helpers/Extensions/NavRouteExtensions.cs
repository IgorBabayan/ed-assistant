using StarSystem = EdAssistant.Models.Route.StarSystem;

namespace EdAssistant.Helpers.Extensions;

public static class NavRouteExtensions
{
    /// <summary>
    /// Calculates the total distance of the route
    /// </summary>
    /// <param name="route">The navigation route</param>
    /// <returns>Total distance in light years</returns>
    public static double TotalDistance(this NavRouteEvent route)
    {
        if (route?.Route == null || route.Route.Count < 2) 
            return 0;

        double totalDistance = 0;
        for (var index = 1; index < route.Route.Count; index++)
        {
            totalDistance += route.Route[index - 1].DistanceTo(route.Route[index]);
        }

        return totalDistance;
    }

    /// <summary>
    /// Gets the number of jumps in the route
    /// </summary>
    /// <param name="route">The navigation route</param>
    /// <returns>Number of jumps (systems - 1)</returns>
    public static int JumpCount(this NavRouteEvent route) => Math.Max(0, (route?.Route?.Count ?? 0) - 1);

    /// <summary>
    /// Gets the destination system (last system in route)
    /// </summary>
    /// <param name="route">The navigation route</param>
    /// <returns>Destination star system or null if route is empty</returns>
    public static StarSystem GetDestination(this NavRouteEvent route) => 
        route?.Route?.Count > 0 ? route.Route[^1] : null;

    /// <summary>
    /// Gets the origin system (first system in route)
    /// </summary>
    /// <param name="route">The navigation route</param>
    /// <returns>Origin star system or null if route is empty</returns>
    public static StarSystem GetOrigin(this NavRouteEvent route) => 
        route?.Route?.Count > 0 ? route.Route[0] : null;
}