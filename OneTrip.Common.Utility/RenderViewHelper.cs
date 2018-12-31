using System.IO;
using System.Web.Mvc;

namespace OneTrip.Common.Utility
{
  public class RenderViewHelper
  {
    public static string RenderToString(string viewName, object model, ControllerBase controller)
    {
      if (string.IsNullOrEmpty(viewName))
        viewName = controller.ControllerContext.RouteData.GetRequiredString("action");

      controller.ViewData.Model = model;

      using (StringWriter sw = new StringWriter())
      {
        ViewEngineResult viewResult = ViewEngines.Engines.FindPartialView(controller.ControllerContext, viewName);
        ViewContext viewContext = new ViewContext(controller.ControllerContext, viewResult.View, controller.ViewData, controller.TempData, sw);
        viewResult.View.Render(viewContext, sw);

        return sw.GetStringBuilder().ToString();
      }
    }
  }
}