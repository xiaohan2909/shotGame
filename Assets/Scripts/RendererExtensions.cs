using UnityEngine;
using System.Collections;
/// <summary>
/// Renderer extensions. C#类扩展的用法
/// </summary>
public static class RendererExtensions
{
	public static bool IsVisibleFrom(this Renderer renderer, Camera camera)
	{
		Plane[] planes = GeometryUtility.CalculateFrustumPlanes(camera);
		return GeometryUtility.TestPlanesAABB(planes, renderer.bounds);
	}
}
