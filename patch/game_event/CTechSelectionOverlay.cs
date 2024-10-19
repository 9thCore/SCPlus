using System;
using SCPlus.patch.hierarchy;
using System.Collections.Generic;
using System.Linq;

namespace SCPlus.patch.game_event
{
    internal class CTechSelectionOverlay : CTechTriggerOverlay
    {
        public string tooltipPositiveText = "";
        public string tooltipNegativeText = "";

        public override void ClickedApply()
        {
            CUIManager.instance.HideOverlay(this);
            EventHelper.eventScreen.SaveCurrentEvent();
        }

        public override void ClickedClose()
        {
            CUIManager.instance.HideOverlay(this);
        }

        public override void NewTrigger(bool isTriggered, UITable table, ParameterCondition root, List<TriggerList> list)
        {
            base.NewTrigger(isTriggered, table, root, list);

            TriggerList component = list.Last();
            if (component == null)
            {
                Plugin.Logger.LogError($"Invalid: no last {nameof(TriggerList)} in list");
                return;
            }
            EditDescription(component, isTriggered);
        }

        public override void ExistingTrigger(bool isTriggered, UITable table, ParameterCondition cond, List<TriggerList> list)
        {
            base.ExistingTrigger(isTriggered, table, cond, list);

            TriggerList component = list.Last();
            if (component == null)
            {
                Plugin.Logger.LogError($"Invalid: no last {nameof(TriggerList)} in list");
                return;
            }
            EditDescription(component, isTriggered);
        }

        private void EditDescription(TriggerList component, bool isPositive)
        {
            if (!HierarchyHelper.TryFindComponentWithLogging(component.transform, out TooltipObject tooltip))
            {
                Plugin.Logger.LogError($"Could not edit description of {component}");
                return;
            }

            tooltip.localisationTag = isPositive ? tooltipPositiveText : tooltipNegativeText;
        }
    }
}
