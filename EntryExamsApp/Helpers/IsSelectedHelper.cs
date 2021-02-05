using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EntryExamsApp.Helpers
{
    public static class IsSelectedHelper
    {
        public static string IsSelected(this IHtmlHelper htmlHelper, string actions, string controllers = null, string cssClass = "active")
        {
            // текущее действие и контроллер
            string currentAction = htmlHelper.ViewContext.RouteData.Values["action"] as string;
            string currentController = htmlHelper.ViewContext.RouteData.Values["controller"] as string;

            // действия и контроллеры, при которых нужно выделять элемент(можно несколько, через запятую)
            IEnumerable<string> acceptedActions = (actions ?? currentAction)?.Split(',');
            IEnumerable<string> acceptedControllers = (controllers ?? currentController)?.Split(',');

            // если текущее дествие и контроллер являются допустимыми - возврат класса для активации, инчае - путой строки
            return acceptedActions!.Contains(currentAction) && acceptedControllers!.Contains(currentController) ? cssClass : string.Empty;
        }
    }
}
