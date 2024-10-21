using System;
using System.Collections.Generic;
using SCPlus.patch.game;

namespace SCPlus.patch.game_event
{
    internal class CTechLockOverlay : CTechSelectionOverlay
    {
        public override void ClickedApply()
        {
            base.ClickedApply();

            List<EventLockTech> eventLock = [];

            EventNode techLock = SyntaxTree.Children[0];
            EventNode techUnlock = SyntaxTree.Children[1];

            foreach (EventNode child in techLock.Children)
            {
                eventLock.Add(new()
                {
                    id = child.Data,
                    locked = true
                });
            }

            foreach (EventNode child in techUnlock.Children)
            {
                eventLock.Add(new()
                {
                    id = child.Data,
                    locked = false
                });
            }

            ExtraFunctionality.Data data = ExtraFunctionality.GetDataOrDefault(SetterHelper.eventScreen.CurrentEvent);
            data.eventLockTech = [.. eventLock];
        }

        public override void Enter()
        {
            base.Enter();

            if (!ExtraFunctionality.eventData.TryGetValue(SetterHelper.eventScreen.CurrentEvent, out ExtraFunctionality.Data data)
                || data.eventLockTech is null)
            {
                SetData(new()
                {
                    parameterConditions = []
                });

                return;
            }

            List<ParameterCondition> conditions = [];

            foreach (EventLockTech lockTech in data.eventLockTech)
            {
                conditions.Add(new()
                {
                    techTrigger = new()
                    {
                        triggered = lockTech.locked,
                        techID = lockTech.id
                    }
                });
            }

            SetData(new()
            {
                parameterConditions = [.. conditions]
            });
        }
    }
}
