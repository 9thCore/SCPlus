using System.Collections.Generic;
using System.Linq;

namespace SCPlus.patch.game_event.tech_overlay
{
    internal class CTechRandomOverlay : CTechSelectionOverlay
    {
        public void Awake()
        {
            addNonTriggered = addTriggered;
        }

        public override void ClickedApply()
        {
            base.ClickedApply();

            List<string> randomTech = [];

            EventNode selectedTech = SyntaxTree.Children[0];

            foreach (EventNode child in selectedTech.Children)
            {
                randomTech.Add(child.Data);
            }

            ExtraFunctionality.Data data = ExtraFunctionality.GetDataOrDefault(EventHelper.eventScreen.CurrentEvent);
            data.randomTech = [.. randomTech];
        }

        public override void Enter()
        {
            base.Enter();

            if (!ExtraFunctionality.eventData.TryGetValue(EventHelper.eventScreen.CurrentEvent, out ExtraFunctionality.Data data)
                || data.randomTech is null)
            {
                SetData(new()
                {
                    parameterConditions = []
                });

                return;
            }

            List<ParameterCondition> conditions = [];

            foreach (string tech in data.randomTech)
            {
                conditions.Add(new()
                {
                    techTrigger = new()
                    {
                        triggered = true,
                        techID = tech
                    }
                });
            }

            SetData(new()
            {
                parameterConditions = [.. conditions]
            });
        }

        public override void AddEventDelegates()
        {
            base.AddEventDelegates();
            EventDelegate.Remove(addNonTriggered.onClick, addNonTriggered.onClick.Last());
        }

        public override void RemoveEventDelegates()
        {
            base.RemoveEventDelegates();
        }
    }
}
